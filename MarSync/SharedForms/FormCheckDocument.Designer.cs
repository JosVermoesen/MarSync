namespace MarSync.SharedForms
{
    partial class FormCheckDocument
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
            this.LabelDocumentId = new System.Windows.Forms.Label();
            this.TextBoxDocumentId = new System.Windows.Forms.TextBox();
            this.ButtonSearch = new System.Windows.Forms.Button();
            this.LabelXmlFile = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.RichTextBoxSentId = new System.Windows.Forms.RichTextBox();
            this.RichTextBoxReceiveId = new System.Windows.Forms.RichTextBox();
            this.Buttoninitialize = new System.Windows.Forms.Button();
            this.ButtonOpenPath = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(277, 367);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(137, 23);
            this.ButtonClose.TabIndex = 0;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // LabelDocumentId
            // 
            this.LabelDocumentId.AutoSize = true;
            this.LabelDocumentId.Location = new System.Drawing.Point(12, 9);
            this.LabelDocumentId.Name = "LabelDocumentId";
            this.LabelDocumentId.Size = new System.Drawing.Size(98, 13);
            this.LabelDocumentId.TabIndex = 1;
            this.LabelDocumentId.Text = "Nummer Document";
            // 
            // TextBoxDocumentId
            // 
            this.TextBoxDocumentId.Location = new System.Drawing.Point(15, 25);
            this.TextBoxDocumentId.Name = "TextBoxDocumentId";
            this.TextBoxDocumentId.Size = new System.Drawing.Size(144, 20);
            this.TextBoxDocumentId.TabIndex = 2;
            this.TextBoxDocumentId.Text = "V";
            this.TextBoxDocumentId.TextChanged += new System.EventHandler(this.TextBoxDocumentId_TextChanged);
            // 
            // ButtonSearch
            // 
            this.ButtonSearch.Enabled = false;
            this.ButtonSearch.Location = new System.Drawing.Point(201, 23);
            this.ButtonSearch.Name = "ButtonSearch";
            this.ButtonSearch.Size = new System.Drawing.Size(137, 23);
            this.ButtonSearch.TabIndex = 4;
            this.ButtonSearch.Text = "Zoeken";
            this.ButtonSearch.UseVisualStyleBackColor = true;
            this.ButtonSearch.Click += new System.EventHandler(this.ButtonSearch_Click);
            // 
            // LabelXmlFile
            // 
            this.LabelXmlFile.Location = new System.Drawing.Point(52, 310);
            this.LabelXmlFile.Name = "LabelXmlFile";
            this.LabelXmlFile.Size = new System.Drawing.Size(355, 35);
            this.LabelXmlFile.TabIndex = 5;
            this.LabelXmlFile.Text = "?.xml";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 52);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(402, 255);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.RichTextBoxSentId);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(394, 229);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "VerzendID";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.RichTextBoxReceiveId);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(394, 229);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Ontvangstbewijs (Klant)";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // RichTextBoxSentId
            // 
            this.RichTextBoxSentId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichTextBoxSentId.Location = new System.Drawing.Point(3, 3);
            this.RichTextBoxSentId.Name = "RichTextBoxSentId";
            this.RichTextBoxSentId.Size = new System.Drawing.Size(388, 223);
            this.RichTextBoxSentId.TabIndex = 4;
            this.RichTextBoxSentId.Text = "";
            // 
            // RichTextBoxReceiveId
            // 
            this.RichTextBoxReceiveId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichTextBoxReceiveId.Location = new System.Drawing.Point(3, 3);
            this.RichTextBoxReceiveId.Name = "RichTextBoxReceiveId";
            this.RichTextBoxReceiveId.Size = new System.Drawing.Size(388, 223);
            this.RichTextBoxReceiveId.TabIndex = 5;
            this.RichTextBoxReceiveId.Text = "";
            // 
            // Buttoninitialize
            // 
            this.Buttoninitialize.Location = new System.Drawing.Point(19, 367);
            this.Buttoninitialize.Name = "Buttoninitialize";
            this.Buttoninitialize.Size = new System.Drawing.Size(75, 23);
            this.Buttoninitialize.TabIndex = 7;
            this.Buttoninitialize.Text = "Initialiseren";
            this.Buttoninitialize.UseVisualStyleBackColor = true;
            this.Buttoninitialize.Visible = false;
            this.Buttoninitialize.Click += new System.EventHandler(this.Buttoninitialize_Click);
            // 
            // ButtonOpenPath
            // 
            this.ButtonOpenPath.Location = new System.Drawing.Point(19, 305);
            this.ButtonOpenPath.Name = "ButtonOpenPath";
            this.ButtonOpenPath.Size = new System.Drawing.Size(34, 23);
            this.ButtonOpenPath.TabIndex = 8;
            this.ButtonOpenPath.Text = "...";
            this.ButtonOpenPath.UseVisualStyleBackColor = true;
            this.ButtonOpenPath.Visible = false;
            this.ButtonOpenPath.Click += new System.EventHandler(this.ButtonOpenPath_Click);
            // 
            // FormCheckDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(435, 402);
            this.Controls.Add(this.ButtonOpenPath);
            this.Controls.Add(this.Buttoninitialize);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.LabelXmlFile);
            this.Controls.Add(this.ButtonSearch);
            this.Controls.Add(this.TextBoxDocumentId);
            this.Controls.Add(this.LabelDocumentId);
            this.Controls.Add(this.ButtonClose);
            this.Name = "FormCheckDocument";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Verzendbewijs Verkoopdocument";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Label LabelDocumentId;
        private System.Windows.Forms.TextBox TextBoxDocumentId;
        private System.Windows.Forms.Button ButtonSearch;
        private System.Windows.Forms.Label LabelXmlFile;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox RichTextBoxSentId;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox RichTextBoxReceiveId;
        private System.Windows.Forms.Button Buttoninitialize;
        private System.Windows.Forms.Button ButtonOpenPath;
    }
}