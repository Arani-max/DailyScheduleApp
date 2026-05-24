namespace DailyScheduleApp.Forms
{
    partial class AddEditTaskForm
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

            this.Text = "Задача";
            this.Size = new System.Drawing.Size(450, 480);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Padding = new System.Windows.Forms.Padding(15);

            int y = 15;
            int labelWidth = 120;
            int controlX = 140;
            int controlWidth = 270;

            // Название
            lblTitle = new System.Windows.Forms.Label();
            lblTitle.Text = "Название:";
            lblTitle.Location = new System.Drawing.Point(15, y + 3);
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            txtTitle = new System.Windows.Forms.TextBox();
            txtTitle.Location = new System.Drawing.Point(controlX, y);
            txtTitle.Width = controlWidth;
            txtTitle.MaxLength = 100;
            this.Controls.Add(txtTitle);
            y += 35;

            // Описание
            lblDescription = new System.Windows.Forms.Label();
            lblDescription.Text = "Описание:";
            lblDescription.Location = new System.Drawing.Point(15, y + 3);
            lblDescription.AutoSize = true;
            this.Controls.Add(lblDescription);

            txtDescription = new System.Windows.Forms.TextBox();
            txtDescription.Location = new System.Drawing.Point(controlX, y);
            txtDescription.Size = new System.Drawing.Size(controlWidth, 60);
            txtDescription.Multiline = true;
            txtDescription.MaxLength = 500;
            this.Controls.Add(txtDescription);
            y += 70;

            // Дата
            lblDate = new System.Windows.Forms.Label();
            lblDate.Text = "Дата:";
            lblDate.Location = new System.Drawing.Point(15, y + 3);
            lblDate.AutoSize = true;
            this.Controls.Add(lblDate);

            dtpDate = new System.Windows.Forms.DateTimePicker();
            dtpDate.Location = new System.Drawing.Point(controlX, y);
            dtpDate.Width = controlWidth;
            dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.Controls.Add(dtpDate);
            y += 35;

            // Время начала
            lblStartTime = new System.Windows.Forms.Label();
            lblStartTime.Text = "Начало:";
            lblStartTime.Location = new System.Drawing.Point(15, y + 3);
            lblStartTime.AutoSize = true;
            this.Controls.Add(lblStartTime);

            dtpStartTime = new System.Windows.Forms.DateTimePicker();
            dtpStartTime.Location = new System.Drawing.Point(controlX, y);
            dtpStartTime.Width = 120;
            dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            dtpStartTime.ShowUpDown = true;
            this.Controls.Add(dtpStartTime);

            // Время окончания
            lblEndTime = new System.Windows.Forms.Label();
            lblEndTime.Text = "Конец:";
            lblEndTime.Location = new System.Drawing.Point(280, y + 3);
            lblEndTime.AutoSize = true;
            this.Controls.Add(lblEndTime);

            dtpEndTime = new System.Windows.Forms.DateTimePicker();
            dtpEndTime.Location = new System.Drawing.Point(330, y);
            dtpEndTime.Width = 80;
            dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            dtpEndTime.ShowUpDown = true;
            this.Controls.Add(dtpEndTime);
            y += 35;

            // Категория
            lblCategory = new System.Windows.Forms.Label();
            lblCategory.Text = "Категория:";
            lblCategory.Location = new System.Drawing.Point(15, y + 3);
            lblCategory.AutoSize = true;
            this.Controls.Add(lblCategory);

            cmbCategory = new System.Windows.Forms.ComboBox();
            cmbCategory.Location = new System.Drawing.Point(controlX, y);
            cmbCategory.Width = controlWidth;
            cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.Controls.Add(cmbCategory);
            y += 35;

            // Приоритет
            lblPriority = new System.Windows.Forms.Label();
            lblPriority.Text = "Приоритет:";
            lblPriority.Location = new System.Drawing.Point(15, y + 3);
            lblPriority.AutoSize = true;
            this.Controls.Add(lblPriority);

            cmbPriority = new System.Windows.Forms.ComboBox();
            cmbPriority.Location = new System.Drawing.Point(controlX, y);
            cmbPriority.Width = controlWidth;
            cmbPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbPriority.Items.Add("1 — Критический");
            cmbPriority.Items.Add("2 — Высокий");
            cmbPriority.Items.Add("3 — Средний");
            cmbPriority.Items.Add("4 — Низкий");
            cmbPriority.Items.Add("5 — Минимальный");
            cmbPriority.SelectedIndex = 2; // Средний по умолчанию
            this.Controls.Add(cmbPriority);
            y += 45;

            // Метка ошибки
            lblError = new System.Windows.Forms.Label();
            lblError.Location = new System.Drawing.Point(15, y);
            lblError.Size = new System.Drawing.Size(400, 25);
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Text = "";
            this.Controls.Add(lblError);
            y += 30;

            // Кнопки
            btnOk = new System.Windows.Forms.Button();
            btnOk.Text = "Сохранить";
            btnOk.Size = new System.Drawing.Size(120, 38);
            btnOk.Location = new System.Drawing.Point(190, y);
            btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnOk.BackColor = System.Drawing.Color.FromArgb(70, 130, 70);
            btnOk.ForeColor = System.Drawing.Color.White;
            btnOk.Click += BtnOk_Click;
            this.Controls.Add(btnOk);

            btnCancel = new System.Windows.Forms.Button();
            btnCancel.Text = "Отмена";
            btnCancel.Size = new System.Drawing.Size(120, 38);
            btnCancel.Location = new System.Drawing.Point(320, y);
            btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCancel.Click += BtnCancel_Click;
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.Label lblEndTime;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblPriority;
        private System.Windows.Forms.ComboBox cmbPriority;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}