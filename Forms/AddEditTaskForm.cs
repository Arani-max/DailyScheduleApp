using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DailyScheduleApp.Models;

namespace DailyScheduleApp.Forms
{
    public partial class AddEditTaskForm : Form
    {
        private readonly bool _isEditMode;

        /// <summary>
        /// Конструктор для создания новой задачи.
        /// </summary>
        public AddEditTaskForm(List<string> categories, DateTime defaultDate)
        {
            InitializeComponent();
            _isEditMode = false;
            this.Text = "Новая задача";

            foreach (var cat in categories)
                cmbCategory.Items.Add(cat);
            if (cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = 0;

            dtpDate.Value = defaultDate;
            dtpStartTime.Value = DateTime.Today.AddHours(9);
            dtpEndTime.Value = DateTime.Today.AddHours(10);
        }

        /// <summary>
        /// Конструктор для редактирования существующей задачи.
        /// </summary>
        public AddEditTaskForm(List<string> categories, TimeSlot existingTask)
        {
            InitializeComponent();
            _isEditMode = true;
            this.Text = "Редактирование задачи";

            foreach (var cat in categories)
                cmbCategory.Items.Add(cat);

            // Заполняем поля существующими данными
            txtTitle.Text = existingTask.Title;
            txtDescription.Text = existingTask.Description;
            dtpDate.Value = existingTask.Date;
            dtpStartTime.Value = DateTime.Today.Add(existingTask.StartTime);
            dtpEndTime.Value = DateTime.Today.Add(existingTask.EndTime);
            cmbPriority.SelectedIndex = existingTask.Priority - 1;

            // Выбираем категорию
            int catIndex = cmbCategory.Items.IndexOf(existingTask.Category);
            if (catIndex >= 0)
                cmbCategory.SelectedIndex = catIndex;
            else
            {
                cmbCategory.Items.Add(existingTask.Category);
                cmbCategory.SelectedItem = existingTask.Category;
            }
        }

        /// <summary>
        /// Возвращает задачу, созданную из введённых данных.
        /// </summary>
        public TimeSlot GetTimeSlot()
        {
            string title = txtTitle.Text.Trim();
            string description = txtDescription.Text.Trim();
            TimeSpan startTime = dtpStartTime.Value.TimeOfDay;
            TimeSpan endTime = dtpEndTime.Value.TimeOfDay;
            string category = cmbCategory.Text.Trim();
            int priority = cmbPriority.SelectedIndex + 1;
            DateTime date = dtpDate.Value.Date;

            return new TimeSlot(title, description, startTime, endTime, category, priority, date);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            // Валидация
            lblError.Text = "";

            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                lblError.Text = "Введите название задачи.";
                txtTitle.Focus();
                return;
            }

            if (dtpStartTime.Value.TimeOfDay >= dtpEndTime.Value.TimeOfDay)
            {
                lblError.Text = "Время начала должно быть раньше времени окончания.";
                dtpStartTime.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(cmbCategory.Text))
            {
                lblError.Text = "Выберите или введите категорию.";
                cmbCategory.Focus();
                return;
            }

            try
            {
                // Пробуем создать объект для проверки валидации
                GetTimeSlot();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}