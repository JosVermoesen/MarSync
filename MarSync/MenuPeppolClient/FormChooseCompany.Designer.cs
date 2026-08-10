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
            this.CheckBoxIsAdmin = new System.Windows.Forms.CheckBox();
            this.TextBoxIsAdminPassword = new System.Windows.Forms.TextBox();
            this.ButtonValidate = new System.Windows.Forms.Button();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.LabelTestGuid = new System.Windows.Forms.Label();
            this.TextBoxTestGuidToValidate = new System.Windows.Forms.TextBox();
            this.ButtonValidateTestGuid = new System.Windows.Forms.Button();
            this.RadioButtonProductionMode = new System.Windows.Forms.RadioButton();
            this.RadioButtonTestMode = new System.Windows.Forms.RadioButton();
            this.ButtonValidateProdGuid = new System.Windows.Forms.Button();
            this.TextBoxProdGuidToValidate = new System.Windows.Forms.TextBox();
            this.LabelProdGuid = new System.Windows.Forms.Label();
            this.TextBoxGuidToggleTabsToValidate = new System.Windows.Forms.TextBox();
            this.LabelToggleTabs = new System.Windows.Forms.Label();
            this.ButtonValidateToggleTabs = new System.Windows.Forms.Button();
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
            // CheckBoxIsAdmin
            // 
            this.CheckBoxIsAdmin.AutoSize = true;
            this.CheckBoxIsAdmin.Location = new System.Drawing.Point(8, 136);
            this.CheckBoxIsAdmin.Name = "CheckBoxIsAdmin";
            this.CheckBoxIsAdmin.Size = new System.Drawing.Size(81, 17);
            this.CheckBoxIsAdmin.TabIndex = 2;
            this.CheckBoxIsAdmin.Text = "Beheerder?";
            this.CheckBoxIsAdmin.UseVisualStyleBackColor = true;
            this.CheckBoxIsAdmin.CheckedChanged += new System.EventHandler(this.CheckBoxIsAdmin_CheckedChanged);
            // 
            // TextBoxIsAdminPassword
            // 
            this.TextBoxIsAdminPassword.Location = new System.Drawing.Point(106, 134);
            this.TextBoxIsAdminPassword.Name = "TextBoxIsAdminPassword";
            this.TextBoxIsAdminPassword.PasswordChar = '*';
            this.TextBoxIsAdminPassword.Size = new System.Drawing.Size(374, 20);
            this.TextBoxIsAdminPassword.TabIndex = 3;
            this.TextBoxIsAdminPassword.Visible = false;
            // 
            // ButtonValidate
            // 
            this.ButtonValidate.Location = new System.Drawing.Point(106, 160);
            this.ButtonValidate.Name = "ButtonValidate";
            this.ButtonValidate.Size = new System.Drawing.Size(74, 23);
            this.ButtonValidate.TabIndex = 4;
            this.ButtonValidate.Text = "Valideren";
            this.ButtonValidate.UseVisualStyleBackColor = true;
            this.ButtonValidate.Visible = false;
            this.ButtonValidate.Click += new System.EventHandler(this.ButtonValidate_Click);
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
            // LabelTestGuid
            // 
            this.LabelTestGuid.AutoSize = true;
            this.LabelTestGuid.Location = new System.Drawing.Point(12, 186);
            this.LabelTestGuid.Name = "LabelTestGuid";
            this.LabelTestGuid.Size = new System.Drawing.Size(103, 13);
            this.LabelTestGuid.TabIndex = 6;
            this.LabelTestGuid.Text = "Activeer Testsleutel:";
            this.LabelTestGuid.Visible = false;
            // 
            // TextBoxTestGuidToValidate
            // 
            this.TextBoxTestGuidToValidate.Location = new System.Drawing.Point(12, 202);
            this.TextBoxTestGuidToValidate.Name = "TextBoxTestGuidToValidate";
            this.TextBoxTestGuidToValidate.PasswordChar = '*';
            this.TextBoxTestGuidToValidate.Size = new System.Drawing.Size(381, 20);
            this.TextBoxTestGuidToValidate.TabIndex = 7;
            this.TextBoxTestGuidToValidate.Visible = false;
            // 
            // ButtonValidateTestGuid
            // 
            this.ButtonValidateTestGuid.Location = new System.Drawing.Point(399, 202);
            this.ButtonValidateTestGuid.Name = "ButtonValidateTestGuid";
            this.ButtonValidateTestGuid.Size = new System.Drawing.Size(81, 20);
            this.ButtonValidateTestGuid.TabIndex = 8;
            this.ButtonValidateTestGuid.Text = "Valideren";
            this.ButtonValidateTestGuid.UseVisualStyleBackColor = true;
            this.ButtonValidateTestGuid.Visible = false;
            this.ButtonValidateTestGuid.Click += new System.EventHandler(this.ButtonValidateTestGuid_Click);
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
            // ButtonValidateProdGuid
            // 
            this.ButtonValidateProdGuid.Location = new System.Drawing.Point(399, 251);
            this.ButtonValidateProdGuid.Name = "ButtonValidateProdGuid";
            this.ButtonValidateProdGuid.Size = new System.Drawing.Size(81, 20);
            this.ButtonValidateProdGuid.TabIndex = 16;
            this.ButtonValidateProdGuid.Text = "Valideren";
            this.ButtonValidateProdGuid.UseVisualStyleBackColor = true;
            this.ButtonValidateProdGuid.Visible = false;
            this.ButtonValidateProdGuid.Click += new System.EventHandler(this.ButtonValidateProdGuid_Click);
            // 
            // TextBoxProdGuidToValidate
            // 
            this.TextBoxProdGuidToValidate.Location = new System.Drawing.Point(12, 252);
            this.TextBoxProdGuidToValidate.Name = "TextBoxProdGuidToValidate";
            this.TextBoxProdGuidToValidate.PasswordChar = '*';
            this.TextBoxProdGuidToValidate.Size = new System.Drawing.Size(381, 20);
            this.TextBoxProdGuidToValidate.TabIndex = 15;
            this.TextBoxProdGuidToValidate.Visible = false;
            // 
            // LabelProdGuid
            // 
            this.LabelProdGuid.AutoSize = true;
            this.LabelProdGuid.Location = new System.Drawing.Point(12, 236);
            this.LabelProdGuid.Name = "LabelProdGuid";
            this.LabelProdGuid.Size = new System.Drawing.Size(130, 13);
            this.LabelProdGuid.TabIndex = 14;
            this.LabelProdGuid.Text = "Activeer Productie sleutel:";
            this.LabelProdGuid.Visible = false;
            // 
            // TextBoxGuidToggleTabsToValidate
            // 
            this.TextBoxGuidToggleTabsToValidate.Location = new System.Drawing.Point(9, 299);
            this.TextBoxGuidToggleTabsToValidate.Name = "TextBoxGuidToggleTabsToValidate";
            this.TextBoxGuidToggleTabsToValidate.PasswordChar = '*';
            this.TextBoxGuidToggleTabsToValidate.Size = new System.Drawing.Size(381, 20);
            this.TextBoxGuidToggleTabsToValidate.TabIndex = 18;
            this.TextBoxGuidToggleTabsToValidate.Visible = false;
            // 
            // LabelToggleTabs
            // 
            this.LabelToggleTabs.AutoSize = true;
            this.LabelToggleTabs.Location = new System.Drawing.Point(9, 283);
            this.LabelToggleTabs.Name = "LabelToggleTabs";
            this.LabelToggleTabs.Size = new System.Drawing.Size(142, 13);
            this.LabelToggleTabs.TabIndex = 17;
            this.LabelToggleTabs.Text = "Activeer ToggleTabs sleutel:";
            this.LabelToggleTabs.Visible = false;
            // 
            // ButtonValidateToggleTabs
            // 
            this.ButtonValidateToggleTabs.Location = new System.Drawing.Point(399, 299);
            this.ButtonValidateToggleTabs.Name = "ButtonValidateToggleTabs";
            this.ButtonValidateToggleTabs.Size = new System.Drawing.Size(81, 20);
            this.ButtonValidateToggleTabs.TabIndex = 19;
            this.ButtonValidateToggleTabs.Text = "Valideren";
            this.ButtonValidateToggleTabs.UseVisualStyleBackColor = true;
            this.ButtonValidateToggleTabs.Visible = false;
            this.ButtonValidateToggleTabs.Click += new System.EventHandler(this.ButtonValidateToggleTabs_Click);
            // 
            // FormChooseCompany
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(494, 331);
            this.Controls.Add(this.ButtonValidateToggleTabs);
            this.Controls.Add(this.TextBoxGuidToggleTabsToValidate);
            this.Controls.Add(this.LabelToggleTabs);
            this.Controls.Add(this.ButtonValidateProdGuid);
            this.Controls.Add(this.TextBoxProdGuidToValidate);
            this.Controls.Add(this.LabelProdGuid);
            this.Controls.Add(this.RadioButtonProductionMode);
            this.Controls.Add(this.RadioButtonTestMode);
            this.Controls.Add(this.ButtonValidateTestGuid);
            this.Controls.Add(this.TextBoxTestGuidToValidate);
            this.Controls.Add(this.LabelTestGuid);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.ButtonValidate);
            this.Controls.Add(this.TextBoxIsAdminPassword);
            this.Controls.Add(this.CheckBoxIsAdmin);
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
        private System.Windows.Forms.CheckBox CheckBoxIsAdmin;
        private System.Windows.Forms.TextBox TextBoxIsAdminPassword;
        private System.Windows.Forms.Button ButtonValidate;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Label LabelTestGuid;
        private System.Windows.Forms.TextBox TextBoxTestGuidToValidate;
        private System.Windows.Forms.Button ButtonValidateTestGuid;
        private System.Windows.Forms.RadioButton RadioButtonProductionMode;
        private System.Windows.Forms.RadioButton RadioButtonTestMode;
        private System.Windows.Forms.Button ButtonValidateProdGuid;
        private System.Windows.Forms.TextBox TextBoxProdGuidToValidate;
        private System.Windows.Forms.Label LabelProdGuid;
        private System.Windows.Forms.TextBox TextBoxGuidToggleTabsToValidate;
        private System.Windows.Forms.Label LabelToggleTabs;
        private System.Windows.Forms.Button ButtonValidateToggleTabs;
    }
}