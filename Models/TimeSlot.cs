using System;
using System.Text.Json.Serialization;

namespace DailyScheduleApp.Models
{
    /// <summary>
    /// Представляет одну задачу/событие в расписании дня.
    /// </summary>
    public class TimeSlot : IComparable<TimeSlot>
    {
        // Уникальный идентификатор задачи.
        public Guid Id { get; private set; }

        // Название задачи.
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название задачи не может быть пустым.");
                _title = value.Trim();
            }
        }

        // Описание задачи.
        private string _description;
        public string Description
        {
            get => _description;
            set => _description = value?.Trim() ?? string.Empty;
        }

        // Время начала.
        private TimeSpan _startTime;
        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                if (value < TimeSpan.Zero || value >= TimeSpan.FromHours(24))
                    throw new ArgumentOutOfRangeException("Время начала должно быть в пределах 00:00–23:59.");
                _startTime = value;
            }
        }

        // Время окончания.
        private TimeSpan _endTime;
        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                if (value < TimeSpan.Zero || value > TimeSpan.FromHours(24))
                    throw new ArgumentOutOfRangeException("Время окончания должно быть в пределах 00:00–24:00.");
                _endTime = value;
            }
        }

        // Категория задачи.
        private string _category;
        public string Category
        {
            get => _category;
            set => _category = string.IsNullOrWhiteSpace(value) ? "Общее" : value.Trim();
        }

        // Приоритет задачи (1 — наивысший, 5 — наименьший).
        private int _priority;
        public int Priority
        {
            get => _priority;
            set
            {
                if (value < 1 || value > 5)
                    throw new ArgumentOutOfRangeException("Приоритет должен быть от 1 до 5.");
                _priority = value;
            }
        }

        // Выполнена ли задача.
        public bool IsCompleted { get; set; }

        // Дата, к которой привязана задача.
        public DateTime Date { get; set; }

        /// <summary>
        /// Конструктор для десериализации.
        /// </summary>
        [JsonConstructor]
        public TimeSlot(Guid id, string title, string description, TimeSpan startTime,
                        TimeSpan endTime, string category, int priority, bool isCompleted, DateTime date)
        {
            Id = id;
            Title = title;
            Description = description;
            Category = category;
            Priority = priority;
            IsCompleted = isCompleted;
            Date = date;

            // Устанавливаем время с валидацией
            StartTime = startTime;
            EndTime = endTime;

            ValidateTimeRange();
        }

        /// <summary>
        /// Основной конструктор для создания новой задачи.
        /// </summary>
        public TimeSlot(string title, string description, TimeSpan startTime,
                        TimeSpan endTime, string category, int priority, DateTime date)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
            Category = category;
            Priority = priority;
            IsCompleted = false;
            Date = date.Date;

            ValidateTimeRange();
        }

        /// <summary>
        /// Проверяет, что время начала раньше времени окончания.
        /// </summary>
        private void ValidateTimeRange()
        {
            if (_startTime >= _endTime)
                throw new ArgumentException("Время начала должно быть раньше времени окончания.");
        }

        /// <summary>
        /// Длительность задачи.
        /// </summary>
        [JsonIgnore]
        public TimeSpan Duration => EndTime - StartTime;

        /// <summary>
        /// Проверяет пересечение с другим временным слотом.
        /// </summary>
        public bool OverlapsWith(TimeSlot other)
        {
            if (other == null || other.Id == this.Id || other.Date.Date != this.Date.Date)
                return false;

            return StartTime < other.EndTime && EndTime > other.StartTime;
        }

        /// <summary>
        /// Возвращает строковое представление приоритета.
        /// </summary>
        public string GetPriorityText()
        {
            return Priority switch
            {
                1 => "Критический",
                2 => "Высокий",
                3 => "Средний",
                4 => "Низкий",
                5 => "Минимальный",
                _ => "Неизвестный"
            };
        }

        public int CompareTo(TimeSlot other)
        {
            if (other == null) return 1;
            int dateCompare = Date.CompareTo(other.Date);
            if (dateCompare != 0) return dateCompare;
            return StartTime.CompareTo(other.StartTime);
        }

        public override string ToString()
        {
            string status = IsCompleted ? "✓" : "○";
            return $"{status} [{StartTime:hh\\:mm}-{EndTime:hh\\:mm}] {Title} ({Category}, приоритет: {GetPriorityText()})";
        }
    }
}