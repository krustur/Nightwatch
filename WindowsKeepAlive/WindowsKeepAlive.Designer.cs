namespace WindowsKeepAlive
{
    partial class WindowsKeepAlive
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowsKeepAlive));
            this.comboBoxSelectMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelCurrentMode = new System.Windows.Forms.Label();
            this.dateTimePickerAutomaticStartTime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerAutomaticStopTime = new System.Windows.Forms.DateTimePicker();
            this.groupBoxAutomaticSettings = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBoxAutomaticSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxSelectMode
            // 
            this.comboBoxSelectMode.FormattingEnabled = true;
            this.comboBoxSelectMode.Items.AddRange(new object[] {
            "Allow Sleep",
            "Prevent Sleep",
            "Prevent Sleep between ..."});
            this.comboBoxSelectMode.Location = new System.Drawing.Point(12, 25);
            this.comboBoxSelectMode.Name = "comboBoxSelectMode";
            this.comboBoxSelectMode.Size = new System.Drawing.Size(280, 21);
            this.comboBoxSelectMode.TabIndex = 3;
            this.comboBoxSelectMode.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSelectMode_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Select mode";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Current mode";
            // 
            // labelCurrentMode
            // 
            this.labelCurrentMode.AutoSize = true;
            this.labelCurrentMode.Location = new System.Drawing.Point(12, 173);
            this.labelCurrentMode.Name = "labelCurrentMode";
            this.labelCurrentMode.Size = new System.Drawing.Size(0, 13);
            this.labelCurrentMode.TabIndex = 6;
            // 
            // dateTimePickerAutomaticStartTime
            // 
            this.dateTimePickerAutomaticStartTime.Location = new System.Drawing.Point(6, 32);
            this.dateTimePickerAutomaticStartTime.Name = "dateTimePickerAutomaticStartTime";
            this.dateTimePickerAutomaticStartTime.Size = new System.Drawing.Size(118, 20);
            this.dateTimePickerAutomaticStartTime.TabIndex = 7;
            this.dateTimePickerAutomaticStartTime.ValueChanged += new System.EventHandler(this.DateTimePickerAutomaticStartTime_ValueChanged);
            // 
            // dateTimePickerAutomaticStopTime
            // 
            this.dateTimePickerAutomaticStopTime.Location = new System.Drawing.Point(6, 71);
            this.dateTimePickerAutomaticStopTime.Name = "dateTimePickerAutomaticStopTime";
            this.dateTimePickerAutomaticStopTime.Size = new System.Drawing.Size(121, 20);
            this.dateTimePickerAutomaticStopTime.TabIndex = 8;
            this.dateTimePickerAutomaticStopTime.ValueChanged += new System.EventHandler(this.DateTimePickerAutomaticStopTime_ValueChanged);
            // 
            // groupBoxAutomaticSettings
            // 
            this.groupBoxAutomaticSettings.Controls.Add(this.label4);
            this.groupBoxAutomaticSettings.Controls.Add(this.label3);
            this.groupBoxAutomaticSettings.Controls.Add(this.dateTimePickerAutomaticStopTime);
            this.groupBoxAutomaticSettings.Controls.Add(this.dateTimePickerAutomaticStartTime);
            this.groupBoxAutomaticSettings.Location = new System.Drawing.Point(12, 52);
            this.groupBoxAutomaticSettings.Name = "groupBoxAutomaticSettings";
            this.groupBoxAutomaticSettings.Size = new System.Drawing.Size(280, 105);
            this.groupBoxAutomaticSettings.TabIndex = 9;
            this.groupBoxAutomaticSettings.TabStop = false;
            this.groupBoxAutomaticSettings.Text = "Automatic settings";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Prevent sleep until";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Prevent sleep from";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 261);
            this.Controls.Add(this.groupBoxAutomaticSettings);
            this.Controls.Add(this.labelCurrentMode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxSelectMode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(320, 300);
            this.MinimumSize = new System.Drawing.Size(320, 300);
            this.Name = "Form1";
            this.Text = "Windows Prevent Sleep";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBoxAutomaticSettings.ResumeLayout(false);
            this.groupBoxAutomaticSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBoxSelectMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelCurrentMode;
        private System.Windows.Forms.DateTimePicker dateTimePickerAutomaticStartTime;
        private System.Windows.Forms.DateTimePicker dateTimePickerAutomaticStopTime;
        private System.Windows.Forms.GroupBox groupBoxAutomaticSettings;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}

