namespace MarSync
{
    partial class FormMarSqlTools
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMarSqlTools));
            this.BtnUpdateMainFiles = new System.Windows.Forms.Button();
            this.lblMarDataLocatie = new System.Windows.Forms.Label();
            this.btnChangeMarNTLocation = new System.Windows.Forms.Button();
            this.tbMarLocatie = new System.Windows.Forms.TextBox();
            this.BtnResetMarntCCI = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCheckLocalSqlServer = new System.Windows.Forms.Button();
            this.tbLocalConnectionstring = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCheckHostedSqlServer = new System.Windows.Forms.Button();
            this.tbHostedConnectionstring = new System.Windows.Forms.TextBox();
            this.BtnCheckCustomers = new System.Windows.Forms.Button();
            this.BtnCheckLedgerAccounts = new System.Windows.Forms.Button();
            this.BtnUpdateInvoicesAndLedgers = new System.Windows.Forms.Button();
            this.rbLocal = new System.Windows.Forms.RadioButton();
            this.rbHosted = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabQR = new System.Windows.Forms.TabControl();
            this.tabPageMario = new System.Windows.Forms.TabPage();
            this.BtnResyncAll = new System.Windows.Forms.Button();
            this.BtnCheckContractsInvoices = new System.Windows.Forms.Button();
            this.BtnSetMarntDONE = new System.Windows.Forms.Button();
            this.BtnSPCreation = new System.Windows.Forms.Button();
            this.BtnUpdateContractsAndTB2Blocks = new System.Windows.Forms.Button();
            this.BtnCheckSuppliers = new System.Windows.Forms.Button();
            this.tabPageQRCodeGenerator = new System.Windows.Forms.TabPage();
            this.btnDemoContent = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtQRCode = new System.Windows.Forms.TextBox();
            this.pic = new System.Windows.Forms.PictureBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPageBarCodeReader = new System.Windows.Forms.TabPage();
            this.btnStopScan = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.cboCamera = new System.Windows.Forms.ComboBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBarcodeFromCamera = new System.Windows.Forms.TextBox();
            this.btnStartScan = new System.Windows.Forms.Button();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.tabQR.SuspendLayout();
            this.tabPageMario.SuspendLayout();
            this.tabPageQRCodeGenerator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).BeginInit();
            this.tabPageBarCodeReader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnUpdateMainFiles
            // 
            this.BtnUpdateMainFiles.Enabled = false;
            this.BtnUpdateMainFiles.Location = new System.Drawing.Point(241, 288);
            this.BtnUpdateMainFiles.Name = "BtnUpdateMainFiles";
            this.BtnUpdateMainFiles.Size = new System.Drawing.Size(125, 50);
            this.BtnUpdateMainFiles.TabIndex = 37;
            this.BtnUpdateMainFiles.Text = "Update Customers, Suppliers and LedgerAccounts";
            this.BtnUpdateMainFiles.UseVisualStyleBackColor = true;
            this.BtnUpdateMainFiles.Click += new System.EventHandler(this.BtnUpdateMainFiles_Click);
            // 
            // lblMarDataLocatie
            // 
            this.lblMarDataLocatie.AutoSize = true;
            this.lblMarDataLocatie.Location = new System.Drawing.Point(9, 10);
            this.lblMarDataLocatie.Name = "lblMarDataLocatie";
            this.lblMarDataLocatie.Size = new System.Drawing.Size(65, 13);
            this.lblMarDataLocatie.TabIndex = 74;
            this.lblMarDataLocatie.Text = "? marnt.mdv";
            this.lblMarDataLocatie.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnChangeMarNTLocation
            // 
            this.btnChangeMarNTLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeMarNTLocation.Location = new System.Drawing.Point(76, 9);
            this.btnChangeMarNTLocation.Name = "btnChangeMarNTLocation";
            this.btnChangeMarNTLocation.Size = new System.Drawing.Size(41, 22);
            this.btnChangeMarNTLocation.TabIndex = 73;
            this.btnChangeMarNTLocation.Text = "...";
            this.btnChangeMarNTLocation.UseVisualStyleBackColor = true;
            this.btnChangeMarNTLocation.Click += new System.EventHandler(this.BtnChangeMarNTLocation_Click);
            // 
            // tbMarLocatie
            // 
            this.tbMarLocatie.Location = new System.Drawing.Point(123, 6);
            this.tbMarLocatie.Multiline = true;
            this.tbMarLocatie.Name = "tbMarLocatie";
            this.tbMarLocatie.Size = new System.Drawing.Size(505, 30);
            this.tbMarLocatie.TabIndex = 72;
            // 
            // BtnResetMarntCCI
            // 
            this.BtnResetMarntCCI.Location = new System.Drawing.Point(12, 226);
            this.BtnResetMarntCCI.Name = "BtnResetMarntCCI";
            this.BtnResetMarntCCI.Size = new System.Drawing.Size(105, 32);
            this.BtnResetMarntCCI.TabIndex = 75;
            this.BtnResetMarntCCI.Text = "Reset marnt sync";
            this.BtnResetMarntCCI.UseVisualStyleBackColor = true;
            this.BtnResetMarntCCI.Click += new System.EventHandler(this.BtnResetMarntCCI_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 78;
            this.label3.Text = "? local sqlserver";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnCheckLocalSqlServer
            // 
            this.btnCheckLocalSqlServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckLocalSqlServer.Location = new System.Drawing.Point(41, 70);
            this.btnCheckLocalSqlServer.Name = "btnCheckLocalSqlServer";
            this.btnCheckLocalSqlServer.Size = new System.Drawing.Size(76, 22);
            this.btnCheckLocalSqlServer.TabIndex = 77;
            this.btnCheckLocalSqlServer.Text = "Remember";
            this.btnCheckLocalSqlServer.UseVisualStyleBackColor = true;
            this.btnCheckLocalSqlServer.Click += new System.EventHandler(this.BtnCheckLocalSqlServer_Click);
            // 
            // tbLocalConnectionstring
            // 
            this.tbLocalConnectionstring.Location = new System.Drawing.Point(123, 42);
            this.tbLocalConnectionstring.Multiline = true;
            this.tbLocalConnectionstring.Name = "tbLocalConnectionstring";
            this.tbLocalConnectionstring.Size = new System.Drawing.Size(505, 50);
            this.tbLocalConnectionstring.TabIndex = 76;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 81;
            this.label4.Text = "? hosted sqlserver";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnCheckHostedSqlServer
            // 
            this.btnCheckHostedSqlServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckHostedSqlServer.Location = new System.Drawing.Point(41, 140);
            this.btnCheckHostedSqlServer.Name = "btnCheckHostedSqlServer";
            this.btnCheckHostedSqlServer.Size = new System.Drawing.Size(76, 22);
            this.btnCheckHostedSqlServer.TabIndex = 80;
            this.btnCheckHostedSqlServer.Text = "Remember";
            this.btnCheckHostedSqlServer.UseVisualStyleBackColor = true;
            this.btnCheckHostedSqlServer.Click += new System.EventHandler(this.BtnCheckHostedSqlServer_Click);
            // 
            // tbHostedConnectionstring
            // 
            this.tbHostedConnectionstring.Location = new System.Drawing.Point(123, 100);
            this.tbHostedConnectionstring.Multiline = true;
            this.tbHostedConnectionstring.Name = "tbHostedConnectionstring";
            this.tbHostedConnectionstring.Size = new System.Drawing.Size(505, 62);
            this.tbHostedConnectionstring.TabIndex = 79;
            // 
            // BtnCheckCustomers
            // 
            this.BtnCheckCustomers.Enabled = false;
            this.BtnCheckCustomers.Location = new System.Drawing.Point(241, 168);
            this.BtnCheckCustomers.Name = "BtnCheckCustomers";
            this.BtnCheckCustomers.Size = new System.Drawing.Size(125, 32);
            this.BtnCheckCustomers.TabIndex = 82;
            this.BtnCheckCustomers.Text = "Check Customers";
            this.BtnCheckCustomers.UseVisualStyleBackColor = true;
            this.BtnCheckCustomers.Click += new System.EventHandler(this.BtnCheckCustomers_Click);
            // 
            // BtnCheckLedgerAccounts
            // 
            this.BtnCheckLedgerAccounts.Enabled = false;
            this.BtnCheckLedgerAccounts.Location = new System.Drawing.Point(372, 168);
            this.BtnCheckLedgerAccounts.Name = "BtnCheckLedgerAccounts";
            this.BtnCheckLedgerAccounts.Size = new System.Drawing.Size(125, 76);
            this.BtnCheckLedgerAccounts.TabIndex = 83;
            this.BtnCheckLedgerAccounts.Text = "Checking Ledger and LedgerAccounts";
            this.BtnCheckLedgerAccounts.UseVisualStyleBackColor = true;
            this.BtnCheckLedgerAccounts.Click += new System.EventHandler(this.BtnCheckLedgerAccounts_Click);
            // 
            // BtnUpdateInvoicesAndLedgers
            // 
            this.BtnUpdateInvoicesAndLedgers.Enabled = false;
            this.BtnUpdateInvoicesAndLedgers.Location = new System.Drawing.Point(372, 288);
            this.BtnUpdateInvoicesAndLedgers.Name = "BtnUpdateInvoicesAndLedgers";
            this.BtnUpdateInvoicesAndLedgers.Size = new System.Drawing.Size(125, 50);
            this.BtnUpdateInvoicesAndLedgers.TabIndex = 84;
            this.BtnUpdateInvoicesAndLedgers.Text = "Update Invoices and Ledgers";
            this.BtnUpdateInvoicesAndLedgers.UseVisualStyleBackColor = true;
            this.BtnUpdateInvoicesAndLedgers.Click += new System.EventHandler(this.BtnUpdateInvoicesAndLedgers_Click);
            // 
            // rbLocal
            // 
            this.rbLocal.AutoSize = true;
            this.rbLocal.Location = new System.Drawing.Point(17, 21);
            this.rbLocal.Name = "rbLocal";
            this.rbLocal.Size = new System.Drawing.Size(73, 17);
            this.rbLocal.TabIndex = 85;
            this.rbLocal.Text = "Use Local";
            this.rbLocal.UseVisualStyleBackColor = true;
            this.rbLocal.CheckedChanged += new System.EventHandler(this.RbLocal_CheckedChanged);
            // 
            // rbHosted
            // 
            this.rbHosted.AutoSize = true;
            this.rbHosted.Location = new System.Drawing.Point(96, 21);
            this.rbHosted.Name = "rbHosted";
            this.rbHosted.Size = new System.Drawing.Size(81, 17);
            this.rbHosted.TabIndex = 86;
            this.rbHosted.Text = "Use Hosted";
            this.rbHosted.UseVisualStyleBackColor = true;
            this.rbHosted.CheckedChanged += new System.EventHandler(this.RbHosted_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbLocal);
            this.groupBox1.Controls.Add(this.rbHosted);
            this.groupBox1.Location = new System.Drawing.Point(12, 168);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(178, 52);
            this.groupBox1.TabIndex = 87;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SQL Server";
            // 
            // tabQR
            // 
            this.tabQR.Controls.Add(this.tabPageMario);
            this.tabQR.Controls.Add(this.tabPageQRCodeGenerator);
            this.tabQR.Controls.Add(this.tabPageBarCodeReader);
            this.tabQR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabQR.Location = new System.Drawing.Point(0, 0);
            this.tabQR.Name = "tabQR";
            this.tabQR.SelectedIndex = 0;
            this.tabQR.Size = new System.Drawing.Size(656, 427);
            this.tabQR.TabIndex = 88;
            // 
            // tabPageMario
            // 
            this.tabPageMario.Controls.Add(this.BtnResyncAll);
            this.tabPageMario.Controls.Add(this.BtnCheckContractsInvoices);
            this.tabPageMario.Controls.Add(this.BtnSetMarntDONE);
            this.tabPageMario.Controls.Add(this.BtnSPCreation);
            this.tabPageMario.Controls.Add(this.BtnUpdateContractsAndTB2Blocks);
            this.tabPageMario.Controls.Add(this.BtnCheckSuppliers);
            this.tabPageMario.Controls.Add(this.tbHostedConnectionstring);
            this.tabPageMario.Controls.Add(this.groupBox1);
            this.tabPageMario.Controls.Add(this.BtnUpdateMainFiles);
            this.tabPageMario.Controls.Add(this.BtnUpdateInvoicesAndLedgers);
            this.tabPageMario.Controls.Add(this.tbMarLocatie);
            this.tabPageMario.Controls.Add(this.BtnCheckLedgerAccounts);
            this.tabPageMario.Controls.Add(this.btnChangeMarNTLocation);
            this.tabPageMario.Controls.Add(this.BtnCheckCustomers);
            this.tabPageMario.Controls.Add(this.lblMarDataLocatie);
            this.tabPageMario.Controls.Add(this.label4);
            this.tabPageMario.Controls.Add(this.BtnResetMarntCCI);
            this.tabPageMario.Controls.Add(this.btnCheckHostedSqlServer);
            this.tabPageMario.Controls.Add(this.tbLocalConnectionstring);
            this.tabPageMario.Controls.Add(this.btnCheckLocalSqlServer);
            this.tabPageMario.Controls.Add(this.label3);
            this.tabPageMario.Location = new System.Drawing.Point(4, 22);
            this.tabPageMario.Name = "tabPageMario";
            this.tabPageMario.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMario.Size = new System.Drawing.Size(648, 401);
            this.tabPageMario.TabIndex = 0;
            this.tabPageMario.Text = "Mario SQL Server";
            this.tabPageMario.UseVisualStyleBackColor = true;
            // 
            // BtnResyncAll
            // 
            this.BtnResyncAll.Enabled = false;
            this.BtnResyncAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnResyncAll.Location = new System.Drawing.Point(241, 250);
            this.BtnResyncAll.Name = "BtnResyncAll";
            this.BtnResyncAll.Size = new System.Drawing.Size(387, 31);
            this.BtnResyncAll.TabIndex = 96;
            this.BtnResyncAll.Text = "Resync ALL";
            this.BtnResyncAll.UseVisualStyleBackColor = true;
            this.BtnResyncAll.Click += new System.EventHandler(this.BtnResyncAll_Click);
            // 
            // BtnCheckContractsInvoices
            // 
            this.BtnCheckContractsInvoices.Enabled = false;
            this.BtnCheckContractsInvoices.Location = new System.Drawing.Point(241, 203);
            this.BtnCheckContractsInvoices.Name = "BtnCheckContractsInvoices";
            this.BtnCheckContractsInvoices.Size = new System.Drawing.Size(125, 41);
            this.BtnCheckContractsInvoices.TabIndex = 95;
            this.BtnCheckContractsInvoices.Text = "Checking Contracts/Invoices";
            this.BtnCheckContractsInvoices.UseVisualStyleBackColor = true;
            this.BtnCheckContractsInvoices.Click += new System.EventHandler(this.BtnCheckContractsInvoices_Click);
            // 
            // BtnSetMarntDONE
            // 
            this.BtnSetMarntDONE.Location = new System.Drawing.Point(12, 264);
            this.BtnSetMarntDONE.Name = "BtnSetMarntDONE";
            this.BtnSetMarntDONE.Size = new System.Drawing.Size(105, 45);
            this.BtnSetMarntDONE.TabIndex = 94;
            this.BtnSetMarntDONE.Text = "SET marnt sync DONE!";
            this.BtnSetMarntDONE.UseVisualStyleBackColor = true;
            this.BtnSetMarntDONE.Click += new System.EventHandler(this.BtnSetMarntDONE_Click);
            // 
            // BtnSPCreation
            // 
            this.BtnSPCreation.Enabled = false;
            this.BtnSPCreation.Location = new System.Drawing.Point(12, 315);
            this.BtnSPCreation.Name = "BtnSPCreation";
            this.BtnSPCreation.Size = new System.Drawing.Size(105, 32);
            this.BtnSPCreation.TabIndex = 93;
            this.BtnSPCreation.Text = "StoredProcedures";
            this.BtnSPCreation.UseVisualStyleBackColor = true;
            this.BtnSPCreation.Click += new System.EventHandler(this.BtnSPCreation_Click);
            // 
            // BtnUpdateContractsAndTB2Blocks
            // 
            this.BtnUpdateContractsAndTB2Blocks.Enabled = false;
            this.BtnUpdateContractsAndTB2Blocks.Location = new System.Drawing.Point(503, 288);
            this.BtnUpdateContractsAndTB2Blocks.Name = "BtnUpdateContractsAndTB2Blocks";
            this.BtnUpdateContractsAndTB2Blocks.Size = new System.Drawing.Size(125, 50);
            this.BtnUpdateContractsAndTB2Blocks.TabIndex = 89;
            this.BtnUpdateContractsAndTB2Blocks.Text = "Update Contracts and Telebib2 Retourblocks";
            this.BtnUpdateContractsAndTB2Blocks.UseVisualStyleBackColor = true;
            this.BtnUpdateContractsAndTB2Blocks.Click += new System.EventHandler(this.BtnUpdateContractsAndTB2Blocks_Click);
            // 
            // BtnCheckSuppliers
            // 
            this.BtnCheckSuppliers.Enabled = false;
            this.BtnCheckSuppliers.Location = new System.Drawing.Point(503, 168);
            this.BtnCheckSuppliers.Name = "BtnCheckSuppliers";
            this.BtnCheckSuppliers.Size = new System.Drawing.Size(125, 76);
            this.BtnCheckSuppliers.TabIndex = 88;
            this.BtnCheckSuppliers.Text = "Check Suppliers and Invoices";
            this.BtnCheckSuppliers.UseVisualStyleBackColor = true;
            this.BtnCheckSuppliers.Click += new System.EventHandler(this.BtnCheckSuppliers_Click);
            // 
            // tabPageQRCodeGenerator
            // 
            this.tabPageQRCodeGenerator.Controls.Add(this.btnDemoContent);
            this.tabPageQRCodeGenerator.Controls.Add(this.label6);
            this.tabPageQRCodeGenerator.Controls.Add(this.txtQRCode);
            this.tabPageQRCodeGenerator.Controls.Add(this.pic);
            this.tabPageQRCodeGenerator.Controls.Add(this.btnGenerate);
            this.tabPageQRCodeGenerator.Controls.Add(this.label5);
            this.tabPageQRCodeGenerator.Location = new System.Drawing.Point(4, 22);
            this.tabPageQRCodeGenerator.Name = "tabPageQRCodeGenerator";
            this.tabPageQRCodeGenerator.Size = new System.Drawing.Size(648, 401);
            this.tabPageQRCodeGenerator.TabIndex = 2;
            this.tabPageQRCodeGenerator.Text = "QR Code Generator";
            this.tabPageQRCodeGenerator.UseVisualStyleBackColor = true;
            // 
            // btnDemoContent
            // 
            this.btnDemoContent.Location = new System.Drawing.Point(8, 328);
            this.btnDemoContent.Name = "btnDemoContent";
            this.btnDemoContent.Size = new System.Drawing.Size(136, 23);
            this.btnDemoContent.TabIndex = 5;
            this.btnDemoContent.Text = "PaymentDemo Content";
            this.btnDemoContent.UseVisualStyleBackColor = true;
            this.btnDemoContent.Click += new System.EventHandler(this.BtnDemoContent_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(8, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(114, 22);
            this.label6.TabIndex = 4;
            this.label6.Text = "Text Content";
            // 
            // txtQRCode
            // 
            this.txtQRCode.Location = new System.Drawing.Point(8, 41);
            this.txtQRCode.Multiline = true;
            this.txtQRCode.Name = "txtQRCode";
            this.txtQRCode.Size = new System.Drawing.Size(280, 281);
            this.txtQRCode.TabIndex = 3;
            // 
            // pic
            // 
            this.pic.BackColor = System.Drawing.Color.White;
            this.pic.Location = new System.Drawing.Point(294, 41);
            this.pic.Name = "pic";
            this.pic.Size = new System.Drawing.Size(342, 281);
            this.pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pic.TabIndex = 2;
            this.pic.TabStop = false;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(213, 328);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 1;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(290, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 22);
            this.label5.TabIndex = 0;
            this.label5.Text = "QR Code";
            // 
            // tabPageBarCodeReader
            // 
            this.tabPageBarCodeReader.Controls.Add(this.btnStopScan);
            this.tabPageBarCodeReader.Controls.Add(this.label8);
            this.tabPageBarCodeReader.Controls.Add(this.cboCamera);
            this.tabPageBarCodeReader.Controls.Add(this.pictureBox);
            this.tabPageBarCodeReader.Controls.Add(this.label7);
            this.tabPageBarCodeReader.Controls.Add(this.txtBarcodeFromCamera);
            this.tabPageBarCodeReader.Controls.Add(this.btnStartScan);
            this.tabPageBarCodeReader.Location = new System.Drawing.Point(4, 22);
            this.tabPageBarCodeReader.Name = "tabPageBarCodeReader";
            this.tabPageBarCodeReader.Size = new System.Drawing.Size(648, 401);
            this.tabPageBarCodeReader.TabIndex = 3;
            this.tabPageBarCodeReader.Text = "Barcode Reader";
            this.tabPageBarCodeReader.UseVisualStyleBackColor = true;
            // 
            // btnStopScan
            // 
            this.btnStopScan.Location = new System.Drawing.Point(456, 344);
            this.btnStopScan.Name = "btnStopScan";
            this.btnStopScan.Size = new System.Drawing.Size(75, 23);
            this.btnStopScan.TabIndex = 6;
            this.btnStopScan.Text = "Stop";
            this.btnStopScan.UseVisualStyleBackColor = true;
            this.btnStopScan.Click += new System.EventHandler(this.BtnStopScan_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(23, 32);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Camera:";
            // 
            // cboCamera
            // 
            this.cboCamera.FormattingEnabled = true;
            this.cboCamera.Location = new System.Drawing.Point(75, 29);
            this.cboCamera.Name = "cboCamera";
            this.cboCamera.Size = new System.Drawing.Size(375, 21);
            this.cboCamera.TabIndex = 4;
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox.Location = new System.Drawing.Point(11, 56);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(439, 282);
            this.pictureBox.TabIndex = 3;
            this.pictureBox.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 347);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Barcode:";
            // 
            // txtBarcodeFromCamera
            // 
            this.txtBarcodeFromCamera.Location = new System.Drawing.Point(64, 344);
            this.txtBarcodeFromCamera.Name = "txtBarcodeFromCamera";
            this.txtBarcodeFromCamera.Size = new System.Drawing.Size(295, 20);
            this.txtBarcodeFromCamera.TabIndex = 1;
            // 
            // btnStartScan
            // 
            this.btnStartScan.Location = new System.Drawing.Point(375, 344);
            this.btnStartScan.Name = "btnStartScan";
            this.btnStartScan.Size = new System.Drawing.Size(75, 23);
            this.btnStartScan.TabIndex = 0;
            this.btnStartScan.Text = "Start";
            this.btnStartScan.UseVisualStyleBackColor = true;
            this.btnStartScan.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // StatusStrip
            // 
            this.StatusStrip.AutoSize = false;
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatusLabel});
            this.StatusStrip.Location = new System.Drawing.Point(0, 427);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(656, 22);
            this.StatusStrip.TabIndex = 89;
            // 
            // ToolStripStatusLabel
            // 
            this.ToolStripStatusLabel.Name = "ToolStripStatusLabel";
            this.ToolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.ToolStripStatusLabel.Text = "Ready";
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(23, 467);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 90;
            this.ButtonClose.Text = "Close";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // FormMarSqlTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(656, 449);
            this.Controls.Add(this.tabQR);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.ButtonClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMarSqlTools";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MarSync";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMarioApp_FormClosing);
            this.Load += new System.EventHandler(this.FrmMarioApp_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabQR.ResumeLayout(false);
            this.tabPageMario.ResumeLayout(false);
            this.tabPageMario.PerformLayout();
            this.tabPageQRCodeGenerator.ResumeLayout(false);
            this.tabPageQRCodeGenerator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).EndInit();
            this.tabPageBarCodeReader.ResumeLayout(false);
            this.tabPageBarCodeReader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button BtnUpdateMainFiles;
        internal System.Windows.Forms.Label lblMarDataLocatie;
        internal System.Windows.Forms.Button btnChangeMarNTLocation;
        internal System.Windows.Forms.TextBox tbMarLocatie;
        private System.Windows.Forms.Button BtnResetMarntCCI;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Button btnCheckLocalSqlServer;
        internal System.Windows.Forms.TextBox tbLocalConnectionstring;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.Button btnCheckHostedSqlServer;
        internal System.Windows.Forms.TextBox tbHostedConnectionstring;
        private System.Windows.Forms.Button BtnCheckCustomers;
        private System.Windows.Forms.Button BtnCheckLedgerAccounts;
        private System.Windows.Forms.Button BtnUpdateInvoicesAndLedgers;
        private System.Windows.Forms.RadioButton rbLocal;
        private System.Windows.Forms.RadioButton rbHosted;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabQR;
        private System.Windows.Forms.TabPage tabPageMario;
        private System.Windows.Forms.Button BtnCheckSuppliers;
        private System.Windows.Forms.Button BtnUpdateContractsAndTB2Blocks;
        private System.Windows.Forms.Button BtnSPCreation;
        private System.Windows.Forms.TabPage tabPageQRCodeGenerator;
        private System.Windows.Forms.TextBox txtQRCode;
        private System.Windows.Forms.PictureBox pic;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnDemoContent;
        private System.Windows.Forms.TabPage tabPageBarCodeReader;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBarcodeFromCamera;
        private System.Windows.Forms.Button btnStartScan;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboCamera;
        private System.Windows.Forms.Button btnStopScan;
        private System.Windows.Forms.Button BtnSetMarntDONE;
        private System.Windows.Forms.Button BtnCheckContractsInvoices;
        private System.Windows.Forms.Button BtnResyncAll;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel;
        private System.Windows.Forms.Button ButtonClose;
    }
}

