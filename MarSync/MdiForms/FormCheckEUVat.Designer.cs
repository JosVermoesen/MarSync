namespace MarSync.MdiForms
{
    partial class FormCheckEUVat
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
            this.TextBoxVatNumber = new System.Windows.Forms.TextBox();
            this.LabelResponseContent = new System.Windows.Forms.Label();
            this.LabelResponse = new System.Windows.Forms.Label();
            this.ButtonCheckVat = new System.Windows.Forms.Button();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextBoxVatNumber
            // 
            this.TextBoxVatNumber.Location = new System.Drawing.Point(117, 7);
            this.TextBoxVatNumber.Name = "TextBoxVatNumber";
            this.TextBoxVatNumber.Size = new System.Drawing.Size(148, 20);
            this.TextBoxVatNumber.TabIndex = 15;
            this.TextBoxVatNumber.Text = "LU20260743";
            // 
            // LabelResponseContent
            // 
            this.LabelResponseContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelResponseContent.Location = new System.Drawing.Point(271, 32);
            this.LabelResponseContent.Name = "LabelResponseContent";
            this.LabelResponseContent.Size = new System.Drawing.Size(353, 327);
            this.LabelResponseContent.TabIndex = 14;
            // 
            // LabelResponse
            // 
            this.LabelResponse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelResponse.Location = new System.Drawing.Point(2, 32);
            this.LabelResponse.Name = "LabelResponse";
            this.LabelResponse.Size = new System.Drawing.Size(263, 327);
            this.LabelResponse.TabIndex = 13;
            // 
            // ButtonCheckVat
            // 
            this.ButtonCheckVat.Location = new System.Drawing.Point(271, 5);
            this.ButtonCheckVat.Name = "ButtonCheckVat";
            this.ButtonCheckVat.Size = new System.Drawing.Size(155, 23);
            this.ButtonCheckVat.TabIndex = 12;
            this.ButtonCheckVat.Text = "BTW Nummer Opzoeken";
            this.ButtonCheckVat.UseVisualStyleBackColor = true;
            this.ButtonCheckVat.Click += new System.EventHandler(this.ButtonCheckVat_Click);
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(549, 4);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 16;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // FormCheckEUVat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(629, 367);
            this.ControlBox = false;
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.TextBoxVatNumber);
            this.Controls.Add(this.LabelResponseContent);
            this.Controls.Add(this.LabelResponse);
            this.Controls.Add(this.ButtonCheckVat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormCheckEUVat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormCheckEUVat";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxVatNumber;
        private System.Windows.Forms.Label LabelResponseContent;
        private System.Windows.Forms.Label LabelResponse;
        private System.Windows.Forms.Button ButtonCheckVat;
        private System.Windows.Forms.Button ButtonClose;
    }
}