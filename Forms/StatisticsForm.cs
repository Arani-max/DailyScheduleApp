using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DailyScheduleApp.Models;

namespace DailyScheduleApp.Forms
{
    public partial class StatisticsForm : Form
    {
        private readonly ScheduleManager _manager;

        public StatisticsForm(ScheduleManager manager)
        {
            InitializeComponent();
            _manager = manager;
            RefreshStatistics();
        }

        private void RefreshStatistics()
        {
            var stats = _manager.GetStatistics();
            var periodStats = stats.GetPeriodStatistics(dtpFrom.Value, dtpTo.Value);
            var categoryStats = stats.GetCategoryStatistics();
            int days = (int)(dtpTo.Value.Date - dtpFrom.Value.Date).TotalDays + 1;

            // === Вкладка «Общая» ===
            var sb = new StringBuilder();
            sb.AppendLine("╔══════════════════════════════════════════╗");
            sb.AppendLine("║         ОБЩАЯ СТАТИСТИКА                 ║");
            sb.AppendLine("╚══════════════════════════════════════════╝");
            sb.AppendLine();
            sb.AppendLine($"  Период: {dtpFrom.Value:dd.MM.yyyy} — {dtpTo.Value:dd.MM.yyyy} ({days} дн.)");
            sb.AppendLine();
            sb.AppendLine($"  📋 Всего задач:          {periodStats.TotalTasks}");
            sb.AppendLine($"  ✅ Выполнено:            {periodStats.CompletedTasks}");
            sb.AppendLine($"  ❌ Не выполнено:         {periodStats.TotalTasks - periodStats.CompletedTasks}");
            sb.AppendLine($"  📈 Процент выполнения:   {periodStats.CompletionRate:F1}%");
            sb.AppendLine();
            sb.AppendLine($"  ⏰ Запланировано:        {periodStats.TotalPlannedTime:hh\\:mm}");
            sb.AppendLine($"  ✅ Выполнено (время):    {periodStats.CompletedTime:hh\\:mm}");
            sb.AppendLine();

            if (periodStats.TotalTasks > 0)
            {
                sb.AppendLine($"  📊 Средн. задач/день:   {(double)periodStats.TotalTasks / days:F1}");
            }

            if (periodStats.MostProductiveDay.HasValue)
            {
                sb.AppendLine($"  🏆 Продуктивный день:   {periodStats.MostProductiveDay.Value:dd.MM.yyyy}");
            }
            sb.AppendLine($"  🏷️ Популярная категория: {periodStats.MostPopularCategory}");

            // Самая длинная задача
            var longest = _manager.GetLongestTask();
            if (longest != null)
            {
                sb.AppendLine();
                sb.AppendLine($"  ⏳ Самая длинная задача: \"{longest.Title}\"");
                sb.AppendLine($"     ({longest.Duration.TotalMinutes:F0} мин, {longest.Date:dd.MM.yyyy})");
            }

            txtGeneral.Text = sb.ToString();

            // === Вкладка «По категориям» ===
            dgvCategories.Rows.Clear();
            foreach (var kvp in categoryStats.OrderByDescending(x => x.Value.TotalTasks))
            {
                dgvCategories.Rows.Add(
                    kvp.Key,
                    kvp.Value.TotalTasks,
                    kvp.Value.CompletedTasks,
                    $"{kvp.Value.CompletionRate:F1}%",
                    $"{kvp.Value.TotalDuration.TotalHours:F1} ч",
                    $"{kvp.Value.AverageDuration.TotalMinutes:F0} мин",
                    $"{kvp.Value.AveragePriority:F1}"
                );

                // Цветовая индикация
                var row = dgvCategories.Rows[dgvCategories.Rows.Count - 1];
                if (kvp.Value.CompletionRate >= 80)
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(220, 245, 220);
                else if (kvp.Value.CompletionRate < 50)
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 235, 235);
            }

            // === Вкладка «Невыполненные» ===
            var sbProblems = new StringBuilder();
            sbProblems.AppendLine("Категории с наибольшим числом невыполненных задач:");
            sbProblems.AppendLine("(за последние 7/14/30 дней)");
            sbProblems.AppendLine();

            foreach (int lastDays in new[] { 7, 14, 30 })
            {
                var incomplete = stats.GetIncompleteByCategory(lastDays);
                sbProblems.AppendLine($"  === За последние {lastDays} дней ===");

                if (incomplete.Count == 0)
                {
                    sbProblems.AppendLine("    Нет невыполненных задач! 🎉");
                }
                else
                {
                    foreach (var item in incomplete)
                    {
                        string bar = new string('█', Math.Min(item.IncompleteCount, 20));
                        sbProblems.AppendLine($"    {item.Category,-20} {item.IncompleteCount,3} {bar}");
                    }
                }
                sbProblems.AppendLine();
            }

            // Свободные слоты на сегодня
            var today = _manager.GetDay(DateTime.Today);
            var freeSlots = today.GetFreeSlots();
            sbProblems.AppendLine("  === Свободное время сегодня ===");
            foreach (var slot in freeSlots)
            {
                var duration = slot.End - slot.Start;
                if (duration.TotalMinutes >= 15) // Показываем слоты от 15 минут
                {
                    sbProblems.AppendLine($"    {slot.Start:hh\\:mm} — {slot.End:hh\\:mm} ({duration.TotalMinutes:F0} мин)");
                }
            }

            txtProblems.Text = sbProblems.ToString();
        }

        private void DtpRange_ValueChanged(object sender, EventArgs e) => RefreshStatistics();

        private void BtnWeek_Click(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Today.AddDays(-7);
            dtpTo.Value = DateTime.Today;
        }

        private void BtnMonth_Click(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Today.AddDays(-30);
            dtpTo.Value = DateTime.Today;
        }

        private void BtnAllTime_Click(object sender, EventArgs e)
        {
            if (_manager.AllTasks.Count > 0)
            {
                dtpFrom.Value = _manager.AllTasks.Min(t => t.Date);
                dtpTo.Value = _manager.AllTasks.Max(t => t.Date);
            }
        }
    }
}