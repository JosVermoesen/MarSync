namespace MarSync.MarioMenu.Actions
{
    partial class FormChooseCompany
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
            this.ListBoxCompanies = new System.Windows.Forms.ListBox();
            this.LabelMimDataLocation = new System.Windows.Forms.Label();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.RadioButtonProductionMode = new System.Windows.Forms.RadioButton();
            this.RadioButtonTestMode = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // ListBoxCompanies
            // 
            this.ListBoxCompanies.FormattingEnabled = true;
            this.ListBoxCompanies.Location = new System.Drawing.Point(8, 36);
            this.ListBoxCompanies.Name = "ListBoxCompanies";
            this.ListBoxCompanies.Size = new System.Drawing.Size(472, 69);
            this.ListBoxCompanies.TabIndex = 0;
            this.ListBoxCompanies.Click += new System.EventHandler(this.ListBoxCompanies_Click);
            this.ListBoxCompanies.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ListBoxCompanies_KeyPress);
            // 
            // LabelMimDataLocation
            // 
            this.LabelMimDataLocation.AutoSize = true;
            this.LabelMimDataLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelMimDataLocation.Location = new System.Drawing.Point(9, 9);
            this.LabelMimDataLocation.Name = "LabelMimDataLocation";
            this.LabelMimDataLocation.Size = new System.Drawing.Size(0, 16);
            this.LabelMimDataLocation.TabIndex = 1;
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(399, 108);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(81, 23);
            this.ButtonClose.TabIndex = 5;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // RadioButtonProductionMode
            // 
            this.RadioButtonProductionMode.AutoSize = true;
            this.RadioButtonProductionMode.Location = new System.Drawing.Point(96, 111);
            this.RadioButtonProductionMode.Name = "RadioButtonProductionMode";
            this.RadioButtonProductionMode.Size = new System.Drawing.Size(105, 17);
            this.RadioButtonProductionMode.TabIndex = 13;
            this.RadioButtonProductionMode.TabStop = true;
            this.RadioButtonProductionMode.Text = "Productie Modus";
            this.RadioButtonProductionMode.UseVisualStyleBackColor = true;
            this.RadioButtonProductionMode.CheckedChanged += new System.EventHandler(this.RadioButtonProductionMode_CheckedChanged);
            this.RadioButtonProductionMode.Click += new System.EventHandler(this.RadioButtonProductionMode_Click);
            // 
            // RadioButtonTestMode
            // 
            this.RadioButtonTestMode.AutoSize = true;
            this.RadioButtonTestMode.Location = new System.Drawing.Point(9, 111);
            this.RadioButtonTestMode.Name = "RadioButtonTestMode";
            this.RadioButtonTestMode.Size = new System.Drawing.Size(81, 17);
            this.RadioButtonTestMode.TabIndex = 12;
            this.RadioButtonTestMode.TabStop = true;
            this.RadioButtonTestMode.Text = "Test Modus";
            this.RadioButtonTestMode.UseVisualStyleBackColor = true;
            this.RadioButtonTestMode.CheckedChanged += new System.EventHandler(this.RadioButtonTestMode_CheckedChanged);
            this.RadioButtonTestMode.Click += new System.EventHandler(this.RadioButtonTestMode_Click);            
            // 
            // FormChooseCompany
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(494, 331);
            this.Controls.Add(this.RadioButtonProductionMode);
            this.Controls.Add(this.RadioButtonTestMode);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.LabelMimDataLocation);
            this.Controls.Add(this.ListBoxCompanies);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormChooseCompany";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormUserSettings";
            this.Load += new System.EventHandler(this.FormChooseCompany_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ListBoxCompanies;
        private System.Windows.Forms.Label LabelMimDataLocation;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.RadioButton RadioButtonProductionMode;
        private System.Windows.Forms.RadioButton RadioButtonTestMode;        
    }
}