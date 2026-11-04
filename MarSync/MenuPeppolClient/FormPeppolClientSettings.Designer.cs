namespace MarSync.MenuPeppolClient
{
    partial class FormPeppolClientSettings
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
            this.ButtonValidateToggleTabs = new System.Windows.Forms.Button();
            this.TextBoxGuidToggleTabsToValidate = new System.Windows.Forms.TextBox();
            this.LabelToggleTabs = new System.Windows.Forms.Label();
            this.ButtonValidateProdGuid = new System.Windows.Forms.Button();
            this.TextBoxProdGuidToValidate = new System.Windows.Forms.TextBox();
            this.LabelProdGuid = new System.Windows.Forms.Label();
            this.ButtonValidateTestGuid = new System.Windows.Forms.Button();
            this.TextBoxTestGuidToValidate = new System.Windows.Forms.TextBox();
            this.LabelTestGuid = new System.Windows.Forms.Label();
            this.ButtonValidate = new System.Windows.Forms.Button();
            this.TextBoxIsAdminPassword = new System.Windows.Forms.TextBox();
            this.CheckBoxIsAdmin = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ButtonValidateToggleTabs
            // 
            this.ButtonValidateToggleTabs.Location = new System.Drawing.Point(284, 126);
            this.ButtonValidateToggleTabs.Name = "ButtonValidateToggleTabs";
            this.ButtonValidateToggleTabs.Size = new System.Drawing.Size(81, 20);
            this.ButtonValidateToggleTabs.TabIndex = 41;
            this.ButtonValidateToggleTabs.Text = "Valideren";
            this.ButtonValidateToggleTabs.UseVisualStyleBackColor = true;
            this.ButtonValidateToggleTabs.Click += new System.EventHandler(this.ButtonValidateToggleTabs_Click);
            // 
            // TextBoxGuidToggleTabsToValidate
            // 
            this.TextBoxGuidToggleTabsToValidate.Location = new System.Drawing.Point(9, 126);
            this.TextBoxGuidToggleTabsToValidate.Name = "TextBoxGuidToggleTabsToValidate";
            this.TextBoxGuidToggleTabsToValidate.Size = new System.Drawing.Size(269, 20);
            this.TextBoxGuidToggleTabsToValidate.TabIndex = 40;
            // 
            // LabelToggleTabs
            // 
            this.LabelToggleTabs.AutoSize = true;
            this.LabelToggleTabs.Location = new System.Drawing.Point(9, 110);
            this.LabelToggleTabs.Name = "LabelToggleTabs";
            this.LabelToggleTabs.Size = new System.Drawing.Size(142, 13);
            this.LabelToggleTabs.TabIndex = 39;
            this.LabelToggleTabs.Text = "Activeer ToggleTabs sleutel:";
            // 
            // ButtonValidateProdGuid
            // 
            this.ButtonValidateProdGuid.Location = new System.Drawing.Point(284, 78);
            this.ButtonValidateProdGuid.Name = "ButtonValidateProdGuid";
            this.ButtonValidateProdGuid.Size = new System.Drawing.Size(81, 20);
            this.ButtonValidateProdGuid.TabIndex = 38;
            this.ButtonValidateProdGuid.Text = "Valideren";
            this.ButtonValidateProdGuid.UseVisualStyleBackColor = true;
            this.ButtonValidateProdGuid.Click += new System.EventHandler(this.ButtonValidateProdGuid_Click);
            // 
            // TextBoxProdGuidToValidate
            // 
            this.TextBoxProdGuidToValidate.Location = new System.Drawing.Point(12, 79);
            this.TextBoxProdGuidToValidate.Name = "TextBoxProdGuidToValidate";
            this.TextBoxProdGuidToValidate.Size = new System.Drawing.Size(266, 20);
            this.TextBoxProdGuidToValidate.TabIndex = 37;
            // 
            // LabelProdGuid
            // 
            this.LabelProdGuid.AutoSize = true;
            this.LabelProdGuid.Location = new System.Drawing.Point(12, 63);
            this.LabelProdGuid.Name = "LabelProdGuid";
            this.LabelProdGuid.Size = new System.Drawing.Size(130, 13);
            this.LabelProdGuid.TabIndex = 36;
            this.LabelProdGuid.Text = "Activeer Productie sleutel:";
            // 
            // ButtonValidateTestGuid
            // 
            this.ButtonValidateTestGuid.Location = new System.Drawing.Point(284, 29);
            this.ButtonValidateTestGuid.Name = "ButtonValidateTestGuid";
            this.ButtonValidateTestGuid.Size = new System.Drawing.Size(81, 20);
            this.ButtonValidateTestGuid.TabIndex = 35;
            this.ButtonValidateTestGuid.Text = "Valideren";
            this.ButtonValidateTestGuid.UseVisualStyleBackColor = true;
            this.ButtonValidateTestGuid.Click += new System.EventHandler(this.ButtonValidateTestGuid_Click);
            // 
            // TextBoxTestGuidToValidate
            // 
            this.TextBoxTestGuidToValidate.Location = new System.Drawing.Point(12, 29);
            this.TextBoxTestGuidToValidate.Name = "TextBoxTestGuidToValidate";
            this.TextBoxTestGuidToValidate.Size = new System.Drawing.Size(266, 20);
            this.TextBoxTestGuidToValidate.TabIndex = 34;
            // 
            // LabelTestGuid
            // 
            this.LabelTestGuid.AutoSize = true;
            this.LabelTestGuid.Location = new System.Drawing.Point(12, 13);
            this.LabelTestGuid.Name = "LabelTestGuid";
            this.LabelTestGuid.Size = new System.Drawing.Size(103, 13);
            this.LabelTestGuid.TabIndex = 33;
            this.LabelTestGuid.Text = "Activeer Testsleutel:";
            // 
            // ButtonValidate
            // 
            this.ButtonValidate.Location = new System.Drawing.Point(284, 177);
            this.ButtonValidate.Name = "ButtonValidate";
            this.ButtonValidate.Size = new System.Drawing.Size(74, 23);
            this.ButtonValidate.TabIndex = 44;
            this.ButtonValidate.Text = "Valideren";
            this.ButtonValidate.UseVisualStyleBackColor = true;
            this.ButtonValidate.Visible = false;
            this.ButtonValidate.Click += new System.EventHandler(this.ButtonValidate_Click);
            // 
            // TextBoxIsAdminPassword
            // 
            this.TextBoxIsAdminPassword.Location = new System.Drawing.Point(9, 180);
            this.TextBoxIsAdminPassword.Name = "TextBoxIsAdminPassword";
            this.TextBoxIsAdminPassword.PasswordChar = '*';
            this.TextBoxIsAdminPassword.Size = new System.Drawing.Size(269, 20);
            this.TextBoxIsAdminPassword.TabIndex = 43;
            this.TextBoxIsAdminPassword.Visible = false;
            // 
            // CheckBoxIsAdmin
            // 
            this.CheckBoxIsAdmin.AutoSize = true;
            this.CheckBoxIsAdmin.Location = new System.Drawing.Point(12, 157);
            this.CheckBoxIsAdmin.Name = "CheckBoxIsAdmin";
            this.CheckBoxIsAdmin.Size = new System.Drawing.Size(133, 17);
            this.CheckBoxIsAdmin.TabIndex = 42;
            this.CheckBoxIsAdmin.Text = "Beheerder SQL Server";
            this.CheckBoxIsAdmin.UseVisualStyleBackColor = true;
            this.CheckBoxIsAdmin.CheckedChanged += new System.EventHandler(this.CheckBoxIsAdmin_CheckedChanged);
            // 
            // FormPeppolClientSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 212);
            this.Controls.Add(this.ButtonValidate);
            this.Controls.Add(this.TextBoxIsAdminPassword);
            this.Controls.Add(this.CheckBoxIsAdmin);
            this.Controls.Add(this.ButtonValidateToggleTabs);
            this.Controls.Add(this.TextBoxGuidToggleTabsToValidate);
            this.Controls.Add(this.LabelToggleTabs);
            this.Controls.Add(this.ButtonValidateProdGuid);
            this.Controls.Add(this.TextBoxProdGuidToValidate);
            this.Controls.Add(this.LabelProdGuid);
            this.Controls.Add(this.ButtonValidateTestGuid);
            this.Controls.Add(this.TextBoxTestGuidToValidate);
            this.Controls.Add(this.LabelTestGuid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormPeppolClientSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormSettings";
            this.Load += new System.EventHandler(this.FormPeppolClientSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ButtonValidateToggleTabs;
        private System.Windows.Forms.TextBox TextBoxGuidToggleTabsToValidate;
        private System.Windows.Forms.Label LabelToggleTabs;
        private System.Windows.Forms.Button ButtonValidateProdGuid;
        private System.Windows.Forms.TextBox TextBoxProdGuidToValidate;
        private System.Windows.Forms.Label LabelProdGuid;
        private System.Windows.Forms.Button ButtonValidateTestGuid;
        private System.Windows.Forms.TextBox TextBoxTestGuidToValidate;
        private System.Windows.Forms.Label LabelTestGuid;
        private System.Windows.Forms.Button ButtonValidate;
        private System.Windows.Forms.TextBox TextBoxIsAdminPassword;
        private System.Windows.Forms.CheckBox CheckBoxIsAdmin;
    }
}