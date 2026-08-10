namespace MarSync.MarioMenu.Admin
{
    partial class FormAdminPeppolSettings
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
            this.ButtonClose = new System.Windows.Forms.Button();
            this.ButtonCreateGuid = new System.Windows.Forms.Button();
            this.TextBoxGuid = new System.Windows.Forms.TextBox();
            this.RadioButtonTestMode = new System.Windows.Forms.RadioButton();
            this.RadioButtonProductionMode = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(270, 221);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(100, 41);
            this.ButtonClose.TabIndex = 6;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // ButtonCreateGuid
            // 
            this.ButtonCreateGuid.Location = new System.Drawing.Point(15, 12);
            this.ButtonCreateGuid.Name = "ButtonCreateGuid";
            this.ButtonCreateGuid.Size = new System.Drawing.Size(111, 23);
            this.ButtonCreateGuid.TabIndex = 7;
            this.ButtonCreateGuid.Text = "Guid aanmaken";
            this.ButtonCreateGuid.UseVisualStyleBackColor = true;
            this.ButtonCreateGuid.Click += new System.EventHandler(this.ButtonCreateGuid_Click);
            // 
            // TextBoxGuid
            // 
            this.TextBoxGuid.Location = new System.Drawing.Point(15, 50);
            this.TextBoxGuid.Name = "TextBoxGuid";
            this.TextBoxGuid.ReadOnly = true;
            this.TextBoxGuid.Size = new System.Drawing.Size(271, 20);
            this.TextBoxGuid.TabIndex = 9;
            // 
            // RadioButtonTestMode
            // 
            this.RadioButtonTestMode.AutoSize = true;
            this.RadioButtonTestMode.Location = new System.Drawing.Point(15, 102);
            this.RadioButtonTestMode.Name = "RadioButtonTestMode";
            this.RadioButtonTestMode.Size = new System.Drawing.Size(81, 17);
            this.RadioButtonTestMode.TabIndex = 10;
            this.RadioButtonTestMode.TabStop = true;
            this.RadioButtonTestMode.Text = "Test Modus";
            this.RadioButtonTestMode.UseVisualStyleBackColor = true;
            this.RadioButtonTestMode.CheckedChanged += new System.EventHandler(this.RadioButtonTestMode_CheckedChanged);
            this.RadioButtonTestMode.Click += new System.EventHandler(this.RadioButtonTestMode_Click);
            // 
            // RadioButtonProductionMode
            // 
            this.RadioButtonProductionMode.AutoSize = true;
            this.RadioButtonProductionMode.Location = new System.Drawing.Point(15, 125);
            this.RadioButtonProductionMode.Name = "RadioButtonProductionMode";
            this.RadioButtonProductionMode.Size = new System.Drawing.Size(105, 17);
            this.RadioButtonProductionMode.TabIndex = 11;
            this.RadioButtonProductionMode.TabStop = true;
            this.RadioButtonProductionMode.Text = "Productie Modus";
            this.RadioButtonProductionMode.UseVisualStyleBackColor = true;
            this.RadioButtonProductionMode.CheckedChanged += new System.EventHandler(this.RadioButtonProductionMode_CheckedChanged);
            this.RadioButtonProductionMode.Click += new System.EventHandler(this.RadioButtonProductionMode_Click);
            // 
            // FormAdminSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(382, 274);
            this.Controls.Add(this.RadioButtonProductionMode);
            this.Controls.Add(this.RadioButtonTestMode);
            this.Controls.Add(this.TextBoxGuid);
            this.Controls.Add(this.ButtonCreateGuid);
            this.Controls.Add(this.ButtonClose);
            this.Name = "FormAdminSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormAdminSettings";
            this.Load += new System.EventHandler(this.FormAdminSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Button ButtonCreateGuid;
        private System.Windows.Forms.TextBox TextBoxGuid;
        private System.Windows.Forms.RadioButton RadioButtonTestMode;
        private System.Windows.Forms.RadioButton RadioButtonProductionMode;
    }
}