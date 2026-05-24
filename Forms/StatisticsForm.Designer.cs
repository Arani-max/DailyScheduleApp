namespace DailyScheduleApp.Forms
{
    partial class StatisticsForm
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

            this.Text = "Статистика";
            this.Size = new System.Drawing.Size(700, 550);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.MinimizeBox = false;

            // Панель настроек периода
            panelSettings = new System.Windows.Forms.Panel();
            panelSettings.Dock = System.Windows.Forms.DockStyle.Top;
            panelSettings.Height = 50;
            panelSettings.Padding = new System.Windows.Forms.Padding(10);

            lblFrom = new System.Windows.Forms.Label();
            lblFrom.Text = "С:";
            lblFrom.AutoSize = true;
            lblFrom.Location = new System.Drawing.Point(10, 15);
            panelSettings.Controls.Add(lblFrom);

            dtpFrom = new System.Windows.Forms.DateTimePicker();
            dtpFrom.Location = new System.Drawing.Point(35, 10);
            dtpFrom.Width = 130;
            dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpFrom.Value = System.DateTime.Today.AddDays(-7);
            dtpFrom.ValueChanged += DtpRange_ValueChanged;
            panelSettings.Controls.Add(dtpFrom);

            lblTo = new System.Windows.Forms.Label();
            lblTo.Text = "По:";
            lblTo.AutoSize = true;
            lblTo.Location = new System.Drawing.Point(180, 15);
            panelSettings.Controls.Add(lblTo);

            dtpTo = new System.Windows.Forms.DateTimePicker();
            dtpTo.Location = new System.Drawing.Point(210, 10);
            dtpTo.Width = 130;
            dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpTo.Value = System.DateTime.Today;
            dtpTo.ValueChanged += DtpRange_ValueChanged;
            panelSettings.Controls.Add(dtpTo);

            // Быстрые кнопки периода
            btnWeek = new System.Windows.Forms.Button();
            btnWeek.Text = "Неделя";
            btnWeek.Location = new System.Drawing.Point(370, 8);
            btnWeek.Size = new System.Drawing.Size(80, 32);
            btnWeek.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnWeek.Click += BtnWeek_Click;
            panelSettings.Controls.Add(btnWeek);

            btnMonth = new System.Windows.Forms.Button();
            btnMonth.Text = "Месяц";
            btnMonth.Location = new System.Drawing.Point(460, 8);
            btnMonth.Size = new System.Drawing.Size(80, 32);
            btnMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnMonth.Click += BtnMonth_Click;
            panelSettings.Controls.Add(btnMonth);

            btnAllTime = new System.Windows.Forms.Button();
            btnAllTime.Text = "Всё время";
            btnAllTime.Location = new System.Drawing.Point(550, 8);
            btnAllTime.Size = new System.Drawing.Size(100, 32);
            btnAllTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAllTime.Click += BtnAllTime_Click;
            panelSettings.Controls.Add(btnAllTime);

            // TabControl для разных типов статистики
            tabControl = new System.Windows.Forms.TabControl();
            tabControl.Dock = System.Windows.Forms.DockStyle.Fill;

            // Вкладка «Общая»
            tabGeneral = new System.Windows.Forms.TabPage();
            tabGeneral.Text = "Общая";
            tabGeneral.Padding = new System.Windows.Forms.Padding(10);

            txtGeneral = new System.Windows.Forms.TextBox();
            txtGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            txtGeneral.Multiline = true;
            txtGeneral.ReadOnly = true;
            txtGeneral.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtGeneral.Font = new System.Drawing.Font("Consolas", 11F);
            txtGeneral.BackColor = System.Drawing.Color.White;
            tabGeneral.Controls.Add(txtGeneral);

            // Вкладка «По категориям»
            tabCategories = new System.Windows.Forms.TabPage();
            tabCategories.Text = "По категориям";

            dgvCategories = new System.Windows.Forms.DataGridView();
            dgvCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvCategories.ReadOnly = true;
            dgvCategories.AllowUserToAddRows = false;
            dgvCategories.AllowUserToDeleteRows = false;
            dgvCategories.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvCategories.RowHeadersVisible = false;
            dgvCategories.BackgroundColor = System.Drawing.Color.White;

            dgvCategories.Columns.Add("colCat", "Категория");
            dgvCategories.Columns.Add("colTotal", "Всего");
            dgvCategories.Columns.Add("colDone", "Выполнено");
            dgvCategories.Columns.Add("colRate", "% выполнения");
            dgvCategories.Columns.Add("colTime", "Общее время");
            dgvCategories.Columns.Add("colAvg", "Ср. время");
            dgvCategories.Columns.Add("colPrio", "Ср. приоритет");

            tabCategories.Controls.Add(dgvCategories);

            // Вкладка «Проблемные области»
            tabProblems = new System.Windows.Forms.TabPage();
            tabProblems.Text = "Невыполненные";
            tabProblems.Padding = new System.Windows.Forms.Padding(10);

            txtProblems = new System.Windows.Forms.TextBox();
            txtProblems.Dock = System.Windows.Forms.DockStyle.Fill;
            txtProblems.Multiline = true;
            txtProblems.ReadOnly = true;
            txtProblems.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtProblems.Font = new System.Drawing.Font("Consolas", 11F);
            txtProblems.BackColor = System.Drawing.Color.White;
            tabProblems.Controls.Add(txtProblems);

            tabControl.TabPages.Add(tabGeneral);
            tabControl.TabPages.Add(tabCategories);
            tabControl.TabPages.Add(tabProblems);

            this.Controls.Add(tabControl);
            this.Controls.Add(panelSettings);

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelSettings;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnWeek;
        private System.Windows.Forms.Button btnMonth;
        private System.Windows.Forms.Button btnAllTime;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TextBox txtGeneral;
        private System.Windows.Forms.TabPage tabCategories;
        private System.Windows.Forms.DataGridView dgvCategories;
        private System.Windows.Forms.TabPage tabProblems;
        private System.Windows.Forms.TextBox txtProblems;
    }
}