namespace DailyScheduleApp.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // === Основная разметка ===
            this.Text = "Распорядок дня";
            this.Size = new System.Drawing.Size(1100, 700);
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Font = new System.Drawing.Font("Segoe UI", 10F);

            // --- Верхняя панель ---
            panelTop = new System.Windows.Forms.Panel();
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Height = 60;
            panelTop.BackColor = System.Drawing.Color.FromArgb(50, 50, 80);

            lblTitle = new System.Windows.Forms.Label();
            lblTitle.Text = "📅 Распорядок дня";
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.White;
            lblTitle.AutoSize = true;
            lblTitle.Location = new System.Drawing.Point(15, 12);
            panelTop.Controls.Add(lblTitle);

            // --- Левая панель (навигация и фильтры) ---
            panelLeft = new System.Windows.Forms.Panel();
            panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            panelLeft.Width = 280;
            panelLeft.BackColor = System.Drawing.Color.FromArgb(240, 240, 245);
            panelLeft.Padding = new System.Windows.Forms.Padding(10);

            // Календарь
            calendar = new System.Windows.Forms.MonthCalendar();
            calendar.Location = new System.Drawing.Point(10, 10);
            calendar.MaxSelectionCount = 1;
            calendar.DateSelected += Calendar_DateSelected;
            panelLeft.Controls.Add(calendar);

            // Фильтр по категории
            lblCategoryFilter = new System.Windows.Forms.Label();
            lblCategoryFilter.Text = "Фильтр по категории:";
            lblCategoryFilter.Location = new System.Drawing.Point(10, 180);
            lblCategoryFilter.AutoSize = true;
            panelLeft.Controls.Add(lblCategoryFilter);

            cmbCategoryFilter = new System.Windows.Forms.ComboBox();
            cmbCategoryFilter.Location = new System.Drawing.Point(10, 205);
            cmbCategoryFilter.Width = 250;
            cmbCategoryFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbCategoryFilter.SelectedIndexChanged += CmbCategoryFilter_SelectedIndexChanged;
            panelLeft.Controls.Add(cmbCategoryFilter);

            // Фильтр по приоритету
            lblPriorityFilter = new System.Windows.Forms.Label();
            lblPriorityFilter.Text = "Фильтр по приоритету:";
            lblPriorityFilter.Location = new System.Drawing.Point(10, 245);
            lblPriorityFilter.AutoSize = true;
            panelLeft.Controls.Add(lblPriorityFilter);

            cmbPriorityFilter = new System.Windows.Forms.ComboBox();
            cmbPriorityFilter.Location = new System.Drawing.Point(10, 270);
            cmbPriorityFilter.Width = 250;
            cmbPriorityFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbPriorityFilter.SelectedIndexChanged += CmbPriorityFilter_SelectedIndexChanged;
            panelLeft.Controls.Add(cmbPriorityFilter);

            // Чекбокс «Показать только невыполненные»
            chkShowIncomplete = new System.Windows.Forms.CheckBox();
            chkShowIncomplete.Text = "Только невыполненные";
            chkShowIncomplete.Location = new System.Drawing.Point(10, 310);
            chkShowIncomplete.AutoSize = true;
            chkShowIncomplete.CheckedChanged += ChkShowIncomplete_CheckedChanged;
            panelLeft.Controls.Add(chkShowIncomplete);

            // Информация о дне
            lblDayInfo = new System.Windows.Forms.Label();
            lblDayInfo.Location = new System.Drawing.Point(10, 350);
            lblDayInfo.Size = new System.Drawing.Size(250, 150);
            lblDayInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            panelLeft.Controls.Add(lblDayInfo);

            // Кнопки
            btnStatistics = new System.Windows.Forms.Button();
            btnStatistics.Text = "📊 Статистика";
            btnStatistics.Location = new System.Drawing.Point(10, 510);
            btnStatistics.Size = new System.Drawing.Size(250, 35);
            btnStatistics.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnStatistics.Click += BtnStatistics_Click;
            panelLeft.Controls.Add(btnStatistics);

            btnExport = new System.Windows.Forms.Button();
            btnExport.Text = "💾 Экспорт дня";
            btnExport.Location = new System.Drawing.Point(10, 550);
            btnExport.Size = new System.Drawing.Size(120, 35);
            btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnExport.Click += BtnExport_Click;
            panelLeft.Controls.Add(btnExport);

            btnImport = new System.Windows.Forms.Button();
            btnImport.Text = "📂 Импорт";
            btnImport.Location = new System.Drawing.Point(140, 550);
            btnImport.Size = new System.Drawing.Size(120, 35);
            btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnImport.Click += BtnImport_Click;
            panelLeft.Controls.Add(btnImport);

            // --- Основная панель (список задач) ---
            panelMain = new System.Windows.Forms.Panel();
            panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            panelMain.Padding = new System.Windows.Forms.Padding(10);

            // Заголовок текущего дня
            lblCurrentDate = new System.Windows.Forms.Label();
            lblCurrentDate.Dock = System.Windows.Forms.DockStyle.Top;
            lblCurrentDate.Height = 40;
            lblCurrentDate.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            lblCurrentDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            panelMain.Controls.Add(lblCurrentDate);

            // Кнопки управления задачами
            panelTaskButtons = new System.Windows.Forms.Panel();
            panelTaskButtons.Dock = System.Windows.Forms.DockStyle.Top;
            panelTaskButtons.Height = 45;

            btnAddTask = new System.Windows.Forms.Button();
            btnAddTask.Text = "➕ Добавить";
            btnAddTask.Size = new System.Drawing.Size(130, 35);
            btnAddTask.Location = new System.Drawing.Point(0, 5);
            btnAddTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAddTask.BackColor = System.Drawing.Color.FromArgb(70, 130, 70);
            btnAddTask.ForeColor = System.Drawing.Color.White;
            btnAddTask.Click += BtnAddTask_Click;
            panelTaskButtons.Controls.Add(btnAddTask);

            btnEditTask = new System.Windows.Forms.Button();
            btnEditTask.Text = "✏️ Изменить";
            btnEditTask.Size = new System.Drawing.Size(130, 35);
            btnEditTask.Location = new System.Drawing.Point(140, 5);
            btnEditTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnEditTask.Click += BtnEditTask_Click;
            panelTaskButtons.Controls.Add(btnEditTask);

            btnDeleteTask = new System.Windows.Forms.Button();
            btnDeleteTask.Text = "🗑️ Удалить";
            btnDeleteTask.Size = new System.Drawing.Size(130, 35);
            btnDeleteTask.Location = new System.Drawing.Point(280, 5);
            btnDeleteTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnDeleteTask.BackColor = System.Drawing.Color.FromArgb(180, 60, 60);
            btnDeleteTask.ForeColor = System.Drawing.Color.White;
            btnDeleteTask.Click += BtnDeleteTask_Click;
            panelTaskButtons.Controls.Add(btnDeleteTask);

            btnToggleComplete = new System.Windows.Forms.Button();
            btnToggleComplete.Text = "✅ Выполнено";
            btnToggleComplete.Size = new System.Drawing.Size(140, 35);
            btnToggleComplete.Location = new System.Drawing.Point(420, 5);
            btnToggleComplete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnToggleComplete.Click += BtnToggleComplete_Click;
            panelTaskButtons.Controls.Add(btnToggleComplete);

            panelMain.Controls.Add(panelTaskButtons);

            // DataGridView для отображения задач
            dgvTasks = new System.Windows.Forms.DataGridView();
            dgvTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvTasks.ReadOnly = true;
            dgvTasks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvTasks.MultiSelect = false;
            dgvTasks.AllowUserToAddRows = false;
            dgvTasks.AllowUserToDeleteRows = false;
            dgvTasks.AllowUserToResizeRows = false;
            dgvTasks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvTasks.RowHeadersVisible = false;
            dgvTasks.BackgroundColor = System.Drawing.Color.White;
            dgvTasks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvTasks.CellDoubleClick += DgvTasks_CellDoubleClick;
            dgvTasks.SelectionChanged += DgvTasks_SelectionChanged;

            // Колонки
            dgvTasks.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "",
                Width = 30,
                AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            });
            dgvTasks.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
            {
                Name = "colTime",
                HeaderText = "Время",
                Width = 110,
                AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            });
            dgvTasks.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
            {
                Name = "colTitle",
                HeaderText = "Задача",
                FillWeight = 40
            });
            dgvTasks.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
            {
                Name = "colCategory",
                HeaderText = "Категория",
                Width = 120,
                AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            });
            dgvTasks.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
            {
                Name = "colPriority",
                HeaderText = "Приоритет",
                Width = 100,
                AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            });
            dgvTasks.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
            {
                Name = "colDuration",
                HeaderText = "Длительность",
                Width = 110,
                AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            });
            dgvTasks.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
            {
                Name = "colDescription",
                HeaderText = "Описание",
                FillWeight = 30
            });

            panelMain.Controls.Add(dgvTasks);

            // === Сборка формы ===
            // Порядок добавления важен для Dock
            this.Controls.Add(panelMain);
            this.Controls.Add(panelLeft);
            this.Controls.Add(panelTop);

            this.ResumeLayout(false);
        }

        // Элементы управления
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.MonthCalendar calendar;
        private System.Windows.Forms.Label lblCategoryFilter;
        private System.Windows.Forms.ComboBox cmbCategoryFilter;
        private System.Windows.Forms.Label lblPriorityFilter;
        private System.Windows.Forms.ComboBox cmbPriorityFilter;
        private System.Windows.Forms.CheckBox chkShowIncomplete;
        private System.Windows.Forms.Label lblDayInfo;
        private System.Windows.Forms.Button btnStatistics;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label lblCurrentDate;
        private System.Windows.Forms.Panel panelTaskButtons;
        private System.Windows.Forms.Button btnAddTask;
        private System.Windows.Forms.Button btnEditTask;
        private System.Windows.Forms.Button btnDeleteTask;
        private System.Windows.Forms.Button btnToggleComplete;
        private System.Windows.Forms.DataGridView dgvTasks;
    }
}