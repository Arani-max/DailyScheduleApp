using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DailyScheduleApp.Models
{
    /// <summary>
    /// Основной менеджер расписания. Управляет загрузкой, сохранением и доступом к задачам.
    /// </summary>
    public class ScheduleManager
    {
        private List<TimeSlot> _allTasks;
        private readonly string _filePath;

        // Все доступные категории по умолчанию.
        private static readonly List<string> DefaultCategories = new List<string>
        {
            "Работа", "Учёба", "Спорт", "Отдых", "Домашние дела",
            "Питание", "Сон", "Транспорт", "Хобби", "Общее"
        };

        public IReadOnlyList<TimeSlot> AllTasks => _allTasks.AsReadOnly();

        public ScheduleManager(string filePath = "schedule.json")
        {
            _filePath = filePath;
            _allTasks = new List<TimeSlot>();
            LoadFromFile();
        }

        /// <summary>
        /// Возвращает расписание на указанный день.
        /// </summary>
        public ScheduleDay GetDay(DateTime date)
        {
            var tasksForDay = _allTasks.Where(t => t.Date.Date == date.Date).ToList();
            return new ScheduleDay(date, tasksForDay);
        }

        /// <summary>
        /// Добавляет задачу. Возвращает список конфликтов.
        /// </summary>
        public List<TimeSlot> AddTask(TimeSlot task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            var conflicts = _allTasks
                .Where(t => t.Date.Date == task.Date.Date && t.OverlapsWith(task))
                .ToList();

            _allTasks.Add(task);
            _allTasks.Sort();
            SaveToFile();

            return conflicts;
        }

        /// <summary>
        /// Обновляет существующую задачу.
        /// </summary>
        public List<TimeSlot> UpdateTask(Guid taskId, string title, string description,
            TimeSpan startTime, TimeSpan endTime, string category, int priority, DateTime date)
        {
            var task = _allTasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new ArgumentException("Задача не найдена.");

            // Удаляем старую и создаём обновлённую для проверки конфликтов
            _allTasks.Remove(task);

            var updatedTask = new TimeSlot(taskId, title, description, startTime,
                endTime, category, priority, task.IsCompleted, date);

            var conflicts = _allTasks
                .Where(t => t.Date.Date == updatedTask.Date.Date && t.OverlapsWith(updatedTask))
                .ToList();

            _allTasks.Add(updatedTask);
            _allTasks.Sort();
            SaveToFile();

            return conflicts;
        }

        /// <summary>
        /// Удаляет задачу по идентификатору.
        /// </summary>
        public bool RemoveTask(Guid taskId)
        {
            bool removed = _allTasks.RemoveAll(t => t.Id == taskId) > 0;
            if (removed) SaveToFile();
            return removed;
        }

        /// <summary>
        /// Отмечает задачу как выполненную/невыполненную.
        /// </summary>
        public void ToggleTaskCompletion(Guid taskId)
        {
            var task = _allTasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                task.IsCompleted = !task.IsCompleted;
                SaveToFile();
            }
        }

        /// <summary>
        /// Перемещает задачу на другой день (аналог перемещения прямоугольника из лаб. работы).
        /// </summary>
        public void MoveTask(Guid taskId, DateTime newDate)
        {
            var task = _allTasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new ArgumentException("Задача не найдена.");

            _allTasks.Remove(task);

            var movedTask = new TimeSlot(task.Id, task.Title, task.Description,
                task.StartTime, task.EndTime, task.Category, task.Priority,
                task.IsCompleted, newDate.Date);

            _allTasks.Add(movedTask);
            _allTasks.Sort();
            SaveToFile();
        }

        /// <summary>
        /// Сдвигает задачу по времени (аналог сдвига по осям).
        /// </summary>
        public void ShiftTaskTime(Guid taskId, TimeSpan shift)
        {
            var task = _allTasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new ArgumentException("Задача не найдена.");

            var newStart = task.StartTime + shift;
            var newEnd = task.EndTime + shift;

            if (newStart < TimeSpan.Zero || newEnd > TimeSpan.FromHours(24))
                throw new ArgumentException("Сдвиг выходит за пределы суток.");

            _allTasks.Remove(task);

            var shiftedTask = new TimeSlot(task.Id, task.Title, task.Description,
                newStart, newEnd, task.Category, task.Priority,
                task.IsCompleted, task.Date);

            _allTasks.Add(shiftedTask);
            _allTasks.Sort();
            SaveToFile();
        }

        /// <summary>
        /// Возвращает задачи по предикату (аналог из части A лаб. работы).
        /// </summary>
        public List<TimeSlot> GetTasksByPredicate(Func<TimeSlot, bool> predicate)
        {
            return _allTasks.Where(predicate).ToList();
        }

        /// <summary>
        /// Возвращает все уникальные категории.
        /// </summary>
        public List<string> GetAllCategories()
        {
            var usedCategories = _allTasks.Select(t => t.Category).Distinct();
            return DefaultCategories.Union(usedCategories).Distinct().OrderBy(c => c).ToList();
        }

        /// <summary>
        /// Возвращает объект статистики.
        /// </summary>
        public Statistics GetStatistics()
        {
            return new Statistics(_allTasks);
        }

        /// <summary>
        /// Самая длинная задача (аналог «наиболее удалённого прямоугольника»).
        /// </summary>
        public TimeSlot GetLongestTask()
        {
            return _allTasks.OrderByDescending(t => t.Duration).FirstOrDefault();
        }

        /// <summary>
        /// Задача с наивысшим приоритетом на сегодня.
        /// </summary>
        public TimeSlot GetHighestPriorityTaskToday()
        {
            return _allTasks
                .Where(t => t.Date.Date == DateTime.Today && !t.IsCompleted)
                .OrderBy(t => t.Priority)
                .ThenBy(t => t.StartTime)
                .FirstOrDefault();
        }

        /// <summary>
        /// Сохраняет данные в JSON-файл.
        /// </summary>
        private void SaveToFile()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                string json = JsonSerializer.Serialize(_allTasks, options);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                throw new IOException($"Ошибка при сохранении файла: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Загружает данные из JSON-файла.
        /// </summary>
        private void LoadFromFile()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        _allTasks = JsonSerializer.Deserialize<List<TimeSlot>>(json) ?? new List<TimeSlot>();
                        _allTasks.Sort();
                    }
                }
            }
            catch (Exception)
            {
                _allTasks = new List<TimeSlot>();
            }
        }

        /// <summary>
        /// Экспортирует расписание на день в текстовый файл.
        /// </summary>
        public void ExportDayToFile(DateTime date, string outputPath)
        {
            var day = GetDay(date);
            using var writer = new StreamWriter(outputPath);

            writer.WriteLine($"=== Расписание на {date:dd.MM.yyyy} ({date:dddd}) ===");
            writer.WriteLine();

            if (day.Tasks.Count == 0)
            {
                writer.WriteLine("Нет запланированных задач.");
            }
            else
            {
                foreach (var task in day.Tasks)
                {
                    writer.WriteLine(task.ToString());
                    if (!string.IsNullOrEmpty(task.Description))
                        writer.WriteLine($"   Описание: {task.Description}");
                    writer.WriteLine();
                }

                writer.WriteLine($"--- Итого ---");
                writer.WriteLine($"Всего задач: {day.Tasks.Count}");
                writer.WriteLine($"Выполнено: {day.Tasks.Count(t => t.IsCompleted)}");
                writer.WriteLine($"Запланированное время: {day.TotalPlannedTime:hh\\:mm}");
                writer.WriteLine($"Выполнение: {day.CompletionPercentage:F1}%");
            }
        }

        /// <summary>
        /// Импортирует задачи из текстового файла.
        /// Формат файла: каждая строка — "HH:mm-HH:mm|Название|Описание|Категория|Приоритет|ДД.ММ.ГГГГ"
        /// </summary>
        public (int imported, int errors, List<string> errorMessages) ImportFromTextFile(string inputPath)
        {
            if (!File.Exists(inputPath))
                throw new FileNotFoundException("Файл не найден.", inputPath);

            int imported = 0;
            int errors = 0;
            var errorMessages = new List<string>();

            var lines = File.ReadAllLines(inputPath);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                    continue;

                try
                {
                    var parts = line.Split('|');
                    if (parts.Length < 6)
                        throw new FormatException("Недостаточно полей.");

                    var timeParts = parts[0].Split('-');
                    var startTime = TimeSpan.Parse(timeParts[0].Trim());
                    var endTime = TimeSpan.Parse(timeParts[1].Trim());
                    string title = parts[1].Trim();
                    string description = parts[2].Trim();
                    string category = parts[3].Trim();
                    int priority = int.Parse(parts[4].Trim());
                    DateTime date = DateTime.ParseExact(parts[5].Trim(), "dd.MM.yyyy",
                        System.Globalization.CultureInfo.InvariantCulture);

                    var task = new TimeSlot(title, description, startTime, endTime, category, priority, date);
                    _allTasks.Add(task);
                    imported++;
                }
                catch (Exception ex)
                {
                    errors++;
                    errorMessages.Add($"Строка {i + 1}: {ex.Message}");
                }
            }

            if (imported > 0)
            {
                _allTasks.Sort();
                SaveToFile();
            }

            return (imported, errors, errorMessages);
        }
    }
}