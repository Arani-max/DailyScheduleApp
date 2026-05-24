using System;
using System.Collections.Generic;
using System.Linq;

namespace DailyScheduleApp.Models
{
    /// <summary>
    /// Представляет расписание на один день.
    /// </summary>
    public class ScheduleDay
    {
        // Дата дня.
        public DateTime Date { get; private set; }

        // Список задач на день.
        private List<TimeSlot> _tasks;
        public IReadOnlyList<TimeSlot> Tasks => _tasks.AsReadOnly();

        public ScheduleDay(DateTime date)
        {
            Date = date.Date;
            _tasks = new List<TimeSlot>();
        }

        public ScheduleDay(DateTime date, List<TimeSlot> tasks)
        {
            Date = date.Date;
            _tasks = tasks ?? new List<TimeSlot>();
        }

        /// <summary>
        /// Добавляет задачу в расписание дня.
        /// Возвращает список конфликтующих задач, если есть пересечения.
        /// </summary>
        public List<TimeSlot> AddTask(TimeSlot task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            if (task.Date.Date != Date)
                throw new ArgumentException("Дата задачи не совпадает с датой расписания.");

            var conflicts = _tasks.Where(t => t.OverlapsWith(task)).ToList();
            _tasks.Add(task);
            _tasks.Sort();

            return conflicts;
        }

        /// <summary>
        /// Удаляет задачу из расписания.
        /// </summary>
        public bool RemoveTask(Guid taskId)
        {
            return _tasks.RemoveAll(t => t.Id == taskId) > 0;
        }

        /// <summary>
        /// Находит задачу по идентификатору.
        /// </summary>
        public TimeSlot FindTask(Guid taskId)
        {
            return _tasks.FirstOrDefault(t => t.Id == taskId);
        }

        /// <summary>
        /// Возвращает задачи, отфильтрованные по категории.
        /// </summary>
        public List<TimeSlot> GetTasksByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return _tasks.ToList();

            return _tasks.Where(t => t.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// Возвращает задачи, отфильтрованные по предикату.
        /// </summary>
        public List<TimeSlot> GetTasksByPredicate(Func<TimeSlot, bool> predicate)
        {
            return _tasks.Where(predicate).ToList();
        }

        /// <summary>
        /// Общее запланированное время за день.
        /// </summary>
        public TimeSpan TotalPlannedTime => TimeSpan.FromMinutes(_tasks.Sum(t => t.Duration.TotalMinutes));

        /// <summary>
        /// Общее время выполненных задач.
        /// </summary>
        public TimeSpan CompletedTime => TimeSpan.FromMinutes(
            _tasks.Where(t => t.IsCompleted).Sum(t => t.Duration.TotalMinutes));

        /// <summary>
        /// Свободное время (из 24 часов).
        /// </summary>
        public TimeSpan FreeTime => TimeSpan.FromHours(24) - TotalPlannedTime;

        /// <summary>
        /// Процент выполнения задач.
        /// </summary>
        public double CompletionPercentage
        {
            get
            {
                if (_tasks.Count == 0) return 0;
                return (double)_tasks.Count(t => t.IsCompleted) / _tasks.Count * 100;
            }
        }

        /// <summary>
        /// Возвращает свободные промежутки времени за день.
        /// </summary>
        public List<(TimeSpan Start, TimeSpan End)> GetFreeSlots()
        {
            var freeSlots = new List<(TimeSpan, TimeSpan)>();
            var sortedTasks = _tasks.OrderBy(t => t.StartTime).ToList();

            TimeSpan current = TimeSpan.Zero;

            foreach (var task in sortedTasks)
            {
                if (task.StartTime > current)
                {
                    freeSlots.Add((current, task.StartTime));
                }
                if (task.EndTime > current)
                {
                    current = task.EndTime;
                }
            }

            if (current < TimeSpan.FromHours(24))
            {
                freeSlots.Add((current, TimeSpan.FromHours(24)));
            }

            return freeSlots;
        }

        /// <summary>
        /// Возвращает все уникальные категории задач за день.
        /// </summary>
        public List<string> GetCategories()
        {
            return _tasks.Select(t => t.Category).Distinct().OrderBy(c => c).ToList();
        }
    }
}