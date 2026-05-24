using System;
using System.Collections.Generic;
using System.Linq;

namespace DailyScheduleApp.Models
{
    /// <summary>
    /// Класс для сбора и отображения статистики по расписанию.
    /// </summary>
    public class Statistics
    {
        private readonly List<TimeSlot> _allTasks;

        public Statistics(List<TimeSlot> allTasks)
        {
            _allTasks = allTasks ?? new List<TimeSlot>();
        }

        /// <summary>
        /// Статистика по категориям: количество задач, выполнено, среднее время.
        /// </summary>
        public Dictionary<string, CategoryStats> GetCategoryStatistics()
        {
            var result = new Dictionary<string, CategoryStats>();

            var groups = _allTasks.GroupBy(t => t.Category);
            foreach (var group in groups)
            {
                var tasks = group.ToList();
                result[group.Key] = new CategoryStats
                {
                    TotalTasks = tasks.Count,
                    CompletedTasks = tasks.Count(t => t.IsCompleted),
                    TotalDuration = TimeSpan.FromMinutes(tasks.Sum(t => t.Duration.TotalMinutes)),
                    AverageDuration = TimeSpan.FromMinutes(tasks.Average(t => t.Duration.TotalMinutes)),
                    AveragePriority = tasks.Average(t => t.Priority)
                };
            }

            return result;
        }

        /// <summary>
        /// Статистика за указанный период.
        /// </summary>
        public PeriodStats GetPeriodStatistics(DateTime startDate, DateTime endDate)
        {
            var tasksInPeriod = _allTasks
                .Where(t => t.Date.Date >= startDate.Date && t.Date.Date <= endDate.Date)
                .ToList();

            return new PeriodStats
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalTasks = tasksInPeriod.Count,
                CompletedTasks = tasksInPeriod.Count(t => t.IsCompleted),
                TotalPlannedTime = TimeSpan.FromMinutes(tasksInPeriod.Sum(t => t.Duration.TotalMinutes)),
                CompletedTime = TimeSpan.FromMinutes(
                    tasksInPeriod.Where(t => t.IsCompleted).Sum(t => t.Duration.TotalMinutes)),
                MostProductiveDay = tasksInPeriod
                    .GroupBy(t => t.Date.Date)
                    .OrderByDescending(g => g.Count(t => t.IsCompleted))
                    .FirstOrDefault()?.Key,
                MostPopularCategory = tasksInPeriod
                    .GroupBy(t => t.Category)
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault()?.Key ?? "Нет данных"
            };
        }

        /// <summary>
        /// Возвращает задачи с наибольшим числом невыполнений по категориям
        /// (аналог статистики неправильных ответов из лабораторной).
        /// </summary>
        public List<(string Category, int IncompleteCount)> GetIncompleteByCategory(int lastNDays = 7)
        {
            var cutoffDate = DateTime.Today.AddDays(-lastNDays);
            return _allTasks
                .Where(t => t.Date.Date >= cutoffDate && !t.IsCompleted)
                .GroupBy(t => t.Category)
                .Select(g => (Category: g.Key, IncompleteCount: g.Count()))
                .OrderByDescending(x => x.IncompleteCount)
                .ToList();
        }
    }

    /// <summary>
    /// Статистика по одной категории.
    /// </summary>
    public class CategoryStats
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public TimeSpan AverageDuration { get; set; }
        public double AveragePriority { get; set; }

        public double CompletionRate => TotalTasks == 0 ? 0 : (double)CompletedTasks / TotalTasks * 100;
    }

    /// <summary>
    /// Статистика за период.
    /// </summary>
    public class PeriodStats
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public TimeSpan TotalPlannedTime { get; set; }
        public TimeSpan CompletedTime { get; set; }
        public DateTime? MostProductiveDay { get; set; }
        public string MostPopularCategory { get; set; }

        public double CompletionRate => TotalTasks == 0 ? 0 : (double)CompletedTasks / TotalTasks * 100;
    }
}