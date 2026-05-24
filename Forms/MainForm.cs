using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DailyScheduleApp.Models;

namespace DailyScheduleApp.Forms
{
    public partial class MainForm : Form
    {
        private readonly ScheduleManager _manager;
        private DateTime _selectedDate;
        private List<TimeSlot> _displayedTasks;

        public MainForm()
        {
            InitializeComponent();
            _manager = new ScheduleManager();
            _selectedDate = DateTime.Today;
            _displayedTasks = new List<TimeSlot>();

            InitializeFilters();
            RefreshView();
        }

        /// <summary>
        /// Инициализирует фильтры категорий и приоритетов.
        /// </summary>
        private void InitializeFilters()
        {
            // Категории
            cmbCategoryFilter.Items.Clear();
            cmbCategoryFilter.Items.Add("Все категории");
            foreach (var cat in _manager.GetAllCategories())
                cmbCategoryFilter.Items.Add(cat);
            cmbCategoryFilter.SelectedIndex = 0;

            // Приоритеты
            cmbPriorityFilter.Items.Clear();
            cmbPriorityFilter.Items.Add("Все приоритеты");
            cmbPriorityFilter.Items.Add("1 — Критический");
            cmbPriorityFilter.Items.Add("2 — Высокий");
            cmbPriorityFilter.Items.Add("3 — Средний");
            cmbPriorityFilter.Items.Add("4 — Низкий");
            cmbPriorityFilter.Items.Add("5 — Минимальный");
            cmbPriorityFilter.SelectedIndex = 0;
        }

        /// <summary>
        /// Обновляет всё отображение.
        /// </summary>
        private void RefreshView()
        {
            UpdateDateLabel();
            UpdateTaskList();
            UpdateDayInfo();
            UpdateButtonStates();
        }

        private void UpdateDateLabel()
        {
            string dayType = "";
            if (_selectedDate.Date == DateTime.Today)
                dayType = " (Сегодня)";
            else if (_selectedDate.Date == DateTime.Today.AddDays(1))
                dayType = " (Завтра)";
            else if (_selectedDate.Date == DateTime.Today.AddDays(-1))
                dayType = " (Вчера)";

            lblCurrentDate.Text = $"{_selectedDate:dd MMMM yyyy, dddd}{dayType}";
        }

        /// <summary>
        /// Обновляет список задач с учётом фильтров.
        /// </summary>
        private void UpdateTaskList()
        {
            var day = _manager.GetDay(_selectedDate);
            var tasks = day.Tasks.ToList();

            // Фильтр по категории
            if (cmbCategoryFilter.SelectedIndex > 0)
            {
                string category = cmbCategoryFilter.SelectedItem.ToString();
                tasks = tasks.Where(t => t.Category == category).ToList();
            }

            // Фильтр по приоритету
            if (cmbPriorityFilter.SelectedIndex > 0)
            {
                int priority = cmbPriorityFilter.SelectedIndex;
                tasks = tasks.Where(t => t.Priority == priority).ToList();
            }

            // Фильтр «только невыполненные»
            if (chkShowIncomplete.Checked)
            {
                tasks = tasks.Where(t => !t.IsCompleted).ToList();
            }

            _displayedTasks = tasks;

            // Заполнение DataGridView
            dgvTasks.Rows.Clear();
            foreach (var task in tasks)
            {
                int rowIndex = dgvTasks.Rows.Add(
                    task.IsCompleted ? "✓" : "○",
                    $"{task.StartTime:hh\\:mm} — {task.EndTime:hh\\:mm}",
                    task.Title,
                    task.Category,
                    task.GetPriorityText(),
                    $"{task.Duration.TotalMinutes:F0} мин",
                    task.Description
                );

                var row = dgvTasks.Rows[rowIndex];

                // Цветовая индикация
                if (task.IsCompleted)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(220, 245, 220);
                    row.DefaultCellStyle.ForeColor = Color.Gray;
                }
                else if (task.Priority == 1)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
                }
                else if (task.Priority == 2)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 245, 220);
                }

                // Подсветка текущей задачи
                if (_selectedDate.Date == DateTime.Today && !task.IsCompleted)
                {
                    var now = DateTime.Now.TimeOfDay;
                    if (task.StartTime <= now && task.EndTime > now)
                    {
                        row.DefaultCellStyle.Font = new Font(dgvTasks.Font, FontStyle.Bold);
                        row.DefaultCellStyle.BackColor = Color.FromArgb(220, 230, 255);
                    }
                }
            }
        }

        /// <summary>
        /// Обновляет информацию о дне в левой панели.
        /// </summary>
        private void UpdateDayInfo()
        {
            var day = _manager.GetDay(_selectedDate);
            var completedCount = day.Tasks.Count(t => t.IsCompleted);

            lblDayInfo.Text = $"📋 Задач: {day.Tasks.Count}\n" +
                              $"✅ Выполнено: {completedCount}\n" +
                              $"⏰ Занято: {day.TotalPlannedTime:hh\\:mm}\n" +
                              $"🕐 Свободно: {day.FreeTime:hh\\:mm}\n" +
                              $"📈 Выполнение: {day.CompletionPercentage:F1}%";

            var highPriority = _manager.GetHighestPriorityTaskToday();
            if (highPriority != null && _selectedDate.Date == DateTime.Today)
            {
                lblDayInfo.Text += $"\n\n🔥 Приоритет: {highPriority.Title}";
            }
        }

        private void UpdateButtonStates()
        {
            bool hasSelection = dgvTasks.SelectedRows.Count > 0;
            btnEditTask.Enabled = hasSelection;
            btnDeleteTask.Enabled = hasSelection;
            btnToggleComplete.Enabled = hasSelection;
        }

        /// <summary>
        /// Возвращает выбранную задачу.
        /// </summary>
        private TimeSlot GetSelectedTask()
        {
            if (dgvTasks.SelectedRows.Count == 0 || _displayedTasks.Count == 0)
                return null;

            int index = dgvTasks.SelectedRows[0].Index;
            if (index >= 0 && index < _displayedTasks.Count)
                return _displayedTasks[index];

            return null;
        }

        // === Обработчики событий ===

        private void Calendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            _selectedDate = e.Start.Date;
            RefreshView();
        }

        private void CmbCategoryFilter_SelectedIndexChanged(object sender, EventArgs e) => RefreshView();
        private void CmbPriorityFilter_SelectedIndexChanged(object sender, EventArgs e) => RefreshView();
        private void ChkShowIncomplete_CheckedChanged(object sender, EventArgs e) => RefreshView();
        private void DgvTasks_SelectionChanged(object sender, EventArgs e) => UpdateButtonStates();

        private void BtnAddTask_Click(object sender, EventArgs e)
        {
            using var form = new AddEditTaskForm(_manager.GetAllCategories(), _selectedDate);
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var task = form.GetTimeSlot();
                    var conflicts = _manager.AddTask(task);

                    if (conflicts.Count > 0)
                    {
                        string conflictList = string.Join("\n",
                            conflicts.Select(c => $"  • {c.StartTime:hh\\:mm}-{c.EndTime:hh\\:mm} {c.Title}"));
                        MessageBox.Show(
                            $"Задача добавлена, но пересекается с:\n{conflictList}",
                            "Предупреждение о пересечении",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    InitializeFilters();
                    RefreshView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnEditTask_Click(object sender, EventArgs e)
        {
            var task = GetSelectedTask();
            if (task == null)
            {
                MessageBox.Show("Выберите задачу для редактирования.",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var form = new AddEditTaskForm(_manager.GetAllCategories(), task);
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var edited = form.GetTimeSlot();
                    var conflicts = _manager.UpdateTask(task.Id, edited.Title, edited.Description,
                        edited.StartTime, edited.EndTime, edited.Category, edited.Priority, edited.Date);

                    if (conflicts.Count > 0)
                    {
                        string conflictList = string.Join("\n",
                            conflicts.Select(c => $"  • {c.StartTime:hh\\:mm}-{c.EndTime:hh\\:mm} {c.Title}"));
                        MessageBox.Show(
                            $"Задача обновлена, но пересекается с:\n{conflictList}",
                            "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    InitializeFilters();
                    RefreshView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDeleteTask_Click(object sender, EventArgs e)
        {
            var task = GetSelectedTask();
            if (task == null)
            {
                MessageBox.Show("Выберите задачу для удаления.",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Удалить задачу \"{task.Title}\"?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _manager.RemoveTask(task.Id);
                RefreshView();
            }
        }

        private void BtnToggleComplete_Click(object sender, EventArgs e)
        {
            var task = GetSelectedTask();
            if (task == null) return;

            _manager.ToggleTaskCompletion(task.Id);
            RefreshView();
        }

        private void DgvTasks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                BtnEditTask_Click(sender, e);
        }

        private void BtnStatistics_Click(object sender, EventArgs e)
        {
            using var form = new StatisticsForm(_manager);
            form.ShowDialog();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            using var dialog = new SaveFileDialog();
            dialog.Filter = "Текстовые файлы (*.txt)|*.txt";
            dialog.FileName = $"schedule_{_selectedDate:yyyy-MM-dd}.txt";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _manager.ExportDayToFile(_selectedDate, dialog.FileName);
                    MessageBox.Show("Расписание экспортировано!",
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка экспорта: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog();
            dialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var (imported, errors, errorMessages) = _manager.ImportFromTextFile(dialog.FileName);

                    string message = $"Импортировано: {imported} задач\nОшибок: {errors}";
                    if (errorMessages.Count > 0)
                    {
                        message += "\n\nОшибки:\n" + string.Join("\n", errorMessages.Take(10));
                        if (errorMessages.Count > 10)
                            message += $"\n... и ещё {errorMessages.Count - 10}";
                    }

                    MessageBox.Show(message, "Результат импорта",
                        MessageBoxButtons.OK,
                        errors > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                    InitializeFilters();
                    RefreshView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка импорта: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}