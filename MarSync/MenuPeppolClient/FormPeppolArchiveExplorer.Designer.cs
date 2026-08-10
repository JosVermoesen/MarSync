namespace MarSync.MenuPeppolClient
{
    partial class FormPeppolArchiveExplorer
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
            this.TreeView = new System.Windows.Forms.TreeView();
            this.Panel = new System.Windows.Forms.Panel();
            this.ButtonXML = new System.Windows.Forms.Button();
            this.ButtonPDF = new System.Windows.Forms.Button();
            this.LabelFileName = new System.Windows.Forms.Label();
            this.RichTextBoxQuickInfo = new System.Windows.Forms.RichTextBox();
            this.LabelFullPath = new System.Windows.Forms.Label();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreeView
            // 
            this.TreeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.TreeView.Location = new System.Drawing.Point(0, 0);
            this.TreeView.Name = "TreeView";
            this.TreeView.Size = new System.Drawing.Size(226, 331);
            this.TreeView.TabIndex = 0;
            // 
            // Panel
            // 
            this.Panel.Controls.Add(this.ButtonXML);
            this.Panel.Controls.Add(this.ButtonPDF);
            this.Panel.Controls.Add(this.LabelFileName);
            this.Panel.Controls.Add(this.RichTextBoxQuickInfo);
            this.Panel.Controls.Add(this.LabelFullPath);
            this.Panel.Dock = System.Windows.Forms.DockStyle.Right;
            this.Panel.Location = new System.Drawing.Point(229, 0);
            this.Panel.Name = "Panel";
            this.Panel.Size = new System.Drawing.Size(449, 331);
            this.Panel.TabIndex = 1;
            // 
            // ButtonXML
            // 
            this.ButtonXML.Location = new System.Drawing.Point(2, 12);
            this.ButtonXML.Name = "ButtonXML";
            this.ButtonXML.Size = new System.Drawing.Size(75, 23);
            this.ButtonXML.TabIndex = 4;
            this.ButtonXML.Text = "XML";
            this.ButtonXML.UseVisualStyleBackColor = true;
            this.ButtonXML.Click += new System.EventHandler(this.ButtonXML_Click);
            // 
            // ButtonPDF
            // 
            this.ButtonPDF.Location = new System.Drawing.Point(83, 12);
            this.ButtonPDF.Name = "ButtonPDF";
            this.ButtonPDF.Size = new System.Drawing.Size(75, 23);
            this.ButtonPDF.TabIndex = 3;
            this.ButtonPDF.Text = "PDF";
            this.ButtonPDF.UseVisualStyleBackColor = true;
            this.ButtonPDF.Click += new System.EventHandler(this.ButtonPDF_Click);
            // 
            // LabelFileName
            // 
            this.LabelFileName.AutoSize = true;
            this.LabelFileName.Location = new System.Drawing.Point(164, 17);
            this.LabelFileName.Name = "LabelFileName";
            this.LabelFileName.Size = new System.Drawing.Size(35, 13);
            this.LabelFileName.TabIndex = 1;
            this.LabelFileName.Text = "label2";
            // 
            // RichTextBoxQuickInfo
            // 
            this.RichTextBoxQuickInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.RichTextBoxQuickInfo.Location = new System.Drawing.Point(0, 40);
            this.RichTextBoxQuickInfo.Name = "RichTextBoxQuickInfo";
            this.RichTextBoxQuickInfo.Size = new System.Drawing.Size(449, 291);
            this.RichTextBoxQuickInfo.TabIndex = 2;
            this.RichTextBoxQuickInfo.Text = "";
            // 
            // LabelFullPath
            // 
            this.LabelFullPath.AutoSize = true;
            this.LabelFullPath.Location = new System.Drawing.Point(53, 249);
            this.LabelFullPath.Name = "LabelFullPath";
            this.LabelFullPath.Size = new System.Drawing.Size(35, 13);
            this.LabelFullPath.TabIndex = 0;
            this.LabelFullPath.Text = "label1";
            this.LabelFullPath.Visible = false;
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(0, 0);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 2;
            this.ButtonClose.Text = "button1";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // FormPeppolArchiveExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(678, 331);
            this.Controls.Add(this.Panel);
            this.Controls.Add(this.TreeView);
            this.Controls.Add(this.ButtonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormPeppolArchiveExplorer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormPeppolArchiveExplorer";
            this.Panel.ResumeLayout(false);
            this.Panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView TreeView;
        private System.Windows.Forms.Panel Panel;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Label LabelFileName;
        private System.Windows.Forms.Label LabelFullPath;
        private System.Windows.Forms.RichTextBox RichTextBoxQuickInfo;
        private System.Windows.Forms.Button ButtonXML;
        private System.Windows.Forms.Button ButtonPDF;
    }
}