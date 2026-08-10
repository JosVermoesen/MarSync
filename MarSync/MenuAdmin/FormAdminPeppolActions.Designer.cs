namespace MarSync.MarioMenu.Admin
{
    partial class FormAdminPeppolActions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAdminPeppolActions));
            this.ButtonClose = new System.Windows.Forms.Button();
            this.TabControlVariousActions = new System.Windows.Forms.TabControl();
            this.TabActions = new System.Windows.Forms.TabPage();
            this.ButtonMailAllStatusReports = new System.Windows.Forms.Button();
            this.ButtonShowAllNotificationsAsReceiver = new System.Windows.Forms.Button();
            this.ButtonShowAllNotificationsAsSender = new System.Windows.Forms.Button();
            this.RadioButtonUseXml = new System.Windows.Forms.RadioButton();
            this.RadioButtonJson = new System.Windows.Forms.RadioButton();
            this.ButtonCheckPeppolRegistration = new System.Windows.Forms.Button();
            this.ButtonGetPeppolRegistrations = new System.Windows.Forms.Button();
            this.TextBoxCountryCode = new System.Windows.Forms.TextBox();
            this.TextBoxRegScheme = new System.Windows.Forms.TextBox();
            this.TextBoxRegIdentifier = new System.Windows.Forms.TextBox();
            this.TextBoxSupportedDocument = new System.Windows.Forms.TextBox();
            this.TextBoxLegalEntityId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ButtonCheckConnectivity = new System.Windows.Forms.Button();
            this.TabNotifications = new System.Windows.Forms.TabPage();
            this.ButtonTestFilter = new System.Windows.Forms.Button();
            this.RichTextBoxAllNotifications = new System.Windows.Forms.RichTextBox();
            this.TextBoxSender = new System.Windows.Forms.TextBox();
            this.TextBoxReceiver = new System.Windows.Forms.TextBox();
            this.RadioButtonGetSent = new System.Windows.Forms.RadioButton();
            this.RadioButtonGetReceived = new System.Windows.Forms.RadioButton();
            this.ButtonNotifications = new System.Windows.Forms.Button();
            this.TabSendDocument = new System.Windows.Forms.TabPage();
            this.LabelFile = new System.Windows.Forms.Label();
            this.ButtonSendUblDocument = new System.Windows.Forms.Button();
            this.ButtonCheckFile = new System.Windows.Forms.Button();
            this.TabReceiveDocument = new System.Windows.Forms.TabPage();
            this.ButtonGetUBLDocument = new System.Windows.Forms.Button();
            this.TextBoxTransmissionId = new System.Windows.Forms.TextBox();
            this.LabelTransmissionId = new System.Windows.Forms.Label();
            this.TabResponse = new System.Windows.Forms.TabPage();
            this.RichTextBoxResult = new System.Windows.Forms.RichTextBox();
            this.TabEntities = new System.Windows.Forms.TabPage();
            this.CheckBoxPublishMinimum = new System.Windows.Forms.CheckBox();
            this.RadioButtonBothSchemes = new System.Windows.Forms.RadioButton();
            this.RadioButtonOnlyScheme0208 = new System.Windows.Forms.RadioButton();
            this.TextBoxPhone = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.TextBoxContactEmail = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.TextBoxWebsiteUrl = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.TextBoxContactName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.TextBoxAdditionalInformation = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.TextBoxGeographicalInformation = new System.Windows.Forms.TextBox();
            this.TextBoxCompanyName = new System.Windows.Forms.TextBox();
            this.TextBoxIdentifier = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ButtonLookUp = new System.Windows.Forms.Button();
            this.ButtonEntityNew = new System.Windows.Forms.Button();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.TabControlVariousActions.SuspendLayout();
            this.TabActions.SuspendLayout();
            this.TabNotifications.SuspendLayout();
            this.TabSendDocument.SuspendLayout();
            this.TabReceiveDocument.SuspendLayout();
            this.TabResponse.SuspendLayout();
            this.TabEntities.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(428, 242);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 7;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // TabControlVariousActions
            // 
            this.TabControlVariousActions.Controls.Add(this.TabActions);
            this.TabControlVariousActions.Controls.Add(this.TabNotifications);
            this.TabControlVariousActions.Controls.Add(this.TabSendDocument);
            this.TabControlVariousActions.Controls.Add(this.TabReceiveDocument);
            this.TabControlVariousActions.Controls.Add(this.TabResponse);
            this.TabControlVariousActions.Controls.Add(this.TabEntities);
            this.TabControlVariousActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControlVariousActions.Location = new System.Drawing.Point(0, 0);
            this.TabControlVariousActions.Name = "TabControlVariousActions";
            this.TabControlVariousActions.SelectedIndex = 0;
            this.TabControlVariousActions.Size = new System.Drawing.Size(640, 342);
            this.TabControlVariousActions.TabIndex = 8;
            // 
            // TabActions
            // 
            this.TabActions.Controls.Add(this.ButtonMailAllStatusReports);
            this.TabActions.Controls.Add(this.ButtonShowAllNotificationsAsReceiver);
            this.TabActions.Controls.Add(this.ButtonShowAllNotificationsAsSender);
            this.TabActions.Controls.Add(this.RadioButtonUseXml);
            this.TabActions.Controls.Add(this.RadioButtonJson);
            this.TabActions.Controls.Add(this.ButtonCheckPeppolRegistration);
            this.TabActions.Controls.Add(this.ButtonGetPeppolRegistrations);
            this.TabActions.Controls.Add(this.TextBoxCountryCode);
            this.TabActions.Controls.Add(this.TextBoxRegScheme);
            this.TabActions.Controls.Add(this.TextBoxRegIdentifier);
            this.TabActions.Controls.Add(this.TextBoxSupportedDocument);
            this.TabActions.Controls.Add(this.TextBoxLegalEntityId);
            this.TabActions.Controls.Add(this.label5);
            this.TabActions.Controls.Add(this.label4);
            this.TabActions.Controls.Add(this.label3);
            this.TabActions.Controls.Add(this.label2);
            this.TabActions.Controls.Add(this.label1);
            this.TabActions.Controls.Add(this.ButtonCheckConnectivity);
            this.TabActions.Location = new System.Drawing.Point(4, 22);
            this.TabActions.Name = "TabActions";
            this.TabActions.Padding = new System.Windows.Forms.Padding(3);
            this.TabActions.Size = new System.Drawing.Size(632, 316);
            this.TabActions.TabIndex = 0;
            this.TabActions.Text = "Opzoekingen";
            this.TabActions.UseVisualStyleBackColor = true;
            // 
            // ButtonMailAllStatusReports
            // 
            this.ButtonMailAllStatusReports.Location = new System.Drawing.Point(485, 140);
            this.ButtonMailAllStatusReports.Name = "ButtonMailAllStatusReports";
            this.ButtonMailAllStatusReports.Size = new System.Drawing.Size(129, 39);
            this.ButtonMailAllStatusReports.TabIndex = 23;
            this.ButtonMailAllStatusReports.Text = "Inform legal entities";
            this.ButtonMailAllStatusReports.UseVisualStyleBackColor = true;
            this.ButtonMailAllStatusReports.Click += new System.EventHandler(this.ButtonMailAllStatusReports_Click);
            // 
            // ButtonShowAllNotificationsAsReceiver
            // 
            this.ButtonShowAllNotificationsAsReceiver.Location = new System.Drawing.Point(485, 65);
            this.ButtonShowAllNotificationsAsReceiver.Name = "ButtonShowAllNotificationsAsReceiver";
            this.ButtonShowAllNotificationsAsReceiver.Size = new System.Drawing.Size(129, 47);
            this.ButtonShowAllNotificationsAsReceiver.TabIndex = 20;
            this.ButtonShowAllNotificationsAsReceiver.Text = "Show All Notifications as Receiver";
            this.ButtonShowAllNotificationsAsReceiver.UseVisualStyleBackColor = true;
            this.ButtonShowAllNotificationsAsReceiver.Click += new System.EventHandler(this.ButtonShowAllNotificationsAsReceiver_Click);
            // 
            // ButtonShowAllNotificationsAsSender
            // 
            this.ButtonShowAllNotificationsAsSender.Location = new System.Drawing.Point(485, 15);
            this.ButtonShowAllNotificationsAsSender.Name = "ButtonShowAllNotificationsAsSender";
            this.ButtonShowAllNotificationsAsSender.Size = new System.Drawing.Size(129, 47);
            this.ButtonShowAllNotificationsAsSender.TabIndex = 19;
            this.ButtonShowAllNotificationsAsSender.Text = "Show All Notifications as Sender";
            this.ButtonShowAllNotificationsAsSender.UseVisualStyleBackColor = true;
            this.ButtonShowAllNotificationsAsSender.Click += new System.EventHandler(this.ButtonShowAllNotificationsAsSender_Click);
            // 
            // RadioButtonUseXml
            // 
            this.RadioButtonUseXml.AutoSize = true;
            this.RadioButtonUseXml.Location = new System.Drawing.Point(386, 215);
            this.RadioButtonUseXml.Name = "RadioButtonUseXml";
            this.RadioButtonUseXml.Size = new System.Drawing.Size(64, 17);
            this.RadioButtonUseXml.TabIndex = 18;
            this.RadioButtonUseXml.Text = "Use Xml";
            this.RadioButtonUseXml.UseVisualStyleBackColor = true;
            // 
            // RadioButtonJson
            // 
            this.RadioButtonJson.AutoSize = true;
            this.RadioButtonJson.Checked = true;
            this.RadioButtonJson.Location = new System.Drawing.Point(292, 215);
            this.RadioButtonJson.Name = "RadioButtonJson";
            this.RadioButtonJson.Size = new System.Drawing.Size(69, 17);
            this.RadioButtonJson.TabIndex = 17;
            this.RadioButtonJson.TabStop = true;
            this.RadioButtonJson.Text = "Use Json";
            this.RadioButtonJson.UseVisualStyleBackColor = true;
            // 
            // ButtonCheckPeppolRegistration
            // 
            this.ButtonCheckPeppolRegistration.Location = new System.Drawing.Point(292, 238);
            this.ButtonCheckPeppolRegistration.Name = "ButtonCheckPeppolRegistration";
            this.ButtonCheckPeppolRegistration.Size = new System.Drawing.Size(163, 23);
            this.ButtonCheckPeppolRegistration.TabIndex = 16;
            this.ButtonCheckPeppolRegistration.Text = "Check Peppol Registration";
            this.ButtonCheckPeppolRegistration.UseVisualStyleBackColor = true;
            this.ButtonCheckPeppolRegistration.Click += new System.EventHandler(this.ButtonCheckPeppolRegistration_Click);
            // 
            // ButtonGetPeppolRegistrations
            // 
            this.ButtonGetPeppolRegistrations.Location = new System.Drawing.Point(22, 238);
            this.ButtonGetPeppolRegistrations.Name = "ButtonGetPeppolRegistrations";
            this.ButtonGetPeppolRegistrations.Size = new System.Drawing.Size(158, 23);
            this.ButtonGetPeppolRegistrations.TabIndex = 15;
            this.ButtonGetPeppolRegistrations.Text = "Registratie(s) Opzoeken";
            this.ButtonGetPeppolRegistrations.UseVisualStyleBackColor = true;
            this.ButtonGetPeppolRegistrations.Click += new System.EventHandler(this.ButtonGetPeppolRegistrations_Click);
            // 
            // TextBoxCountryCode
            // 
            this.TextBoxCountryCode.Location = new System.Drawing.Point(103, 62);
            this.TextBoxCountryCode.Name = "TextBoxCountryCode";
            this.TextBoxCountryCode.Size = new System.Drawing.Size(77, 20);
            this.TextBoxCountryCode.TabIndex = 10;
            // 
            // TextBoxRegScheme
            // 
            this.TextBoxRegScheme.Location = new System.Drawing.Point(103, 96);
            this.TextBoxRegScheme.Name = "TextBoxRegScheme";
            this.TextBoxRegScheme.Size = new System.Drawing.Size(352, 20);
            this.TextBoxRegScheme.TabIndex = 11;
            // 
            // TextBoxRegIdentifier
            // 
            this.TextBoxRegIdentifier.Location = new System.Drawing.Point(103, 126);
            this.TextBoxRegIdentifier.Name = "TextBoxRegIdentifier";
            this.TextBoxRegIdentifier.Size = new System.Drawing.Size(352, 20);
            this.TextBoxRegIdentifier.TabIndex = 12;
            // 
            // TextBoxSupportedDocument
            // 
            this.TextBoxSupportedDocument.Location = new System.Drawing.Point(103, 159);
            this.TextBoxSupportedDocument.Name = "TextBoxSupportedDocument";
            this.TextBoxSupportedDocument.Size = new System.Drawing.Size(352, 20);
            this.TextBoxSupportedDocument.TabIndex = 13;
            // 
            // TextBoxLegalEntityId
            // 
            this.TextBoxLegalEntityId.Location = new System.Drawing.Point(103, 195);
            this.TextBoxLegalEntityId.Name = "TextBoxLegalEntityId";
            this.TextBoxLegalEntityId.Size = new System.Drawing.Size(77, 20);
            this.TextBoxLegalEntityId.TabIndex = 14;
            this.TextBoxLegalEntityId.Text = "whatever";
            this.TextBoxLegalEntityId.TextChanged += new System.EventHandler(this.TextBoxLegalEntityId_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 198);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "LegalEntityId";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "SupportedDoc";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "RegId";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "RegScheme";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "LandCode";
            // 
            // ButtonCheckConnectivity
            // 
            this.ButtonCheckConnectivity.Location = new System.Drawing.Point(22, 6);
            this.ButtonCheckConnectivity.Name = "ButtonCheckConnectivity";
            this.ButtonCheckConnectivity.Size = new System.Drawing.Size(129, 23);
            this.ButtonCheckConnectivity.TabIndex = 0;
            this.ButtonCheckConnectivity.Text = "Verbinding Testen";
            this.ButtonCheckConnectivity.UseVisualStyleBackColor = true;
            this.ButtonCheckConnectivity.Click += new System.EventHandler(this.ButtonCheckConnectivity_Click);
            // 
            // TabNotifications
            // 
            this.TabNotifications.Controls.Add(this.ButtonTestFilter);
            this.TabNotifications.Controls.Add(this.RichTextBoxAllNotifications);
            this.TabNotifications.Controls.Add(this.TextBoxSender);
            this.TabNotifications.Controls.Add(this.TextBoxReceiver);
            this.TabNotifications.Controls.Add(this.RadioButtonGetSent);
            this.TabNotifications.Controls.Add(this.RadioButtonGetReceived);
            this.TabNotifications.Controls.Add(this.ButtonNotifications);
            this.TabNotifications.Location = new System.Drawing.Point(4, 22);
            this.TabNotifications.Name = "TabNotifications";
            this.TabNotifications.Padding = new System.Windows.Forms.Padding(3);
            this.TabNotifications.Size = new System.Drawing.Size(632, 316);
            this.TabNotifications.TabIndex = 1;
            this.TabNotifications.Text = "Meldingen";
            this.TabNotifications.UseVisualStyleBackColor = true;
            // 
            // ButtonTestFilter
            // 
            this.ButtonTestFilter.Location = new System.Drawing.Point(522, 19);
            this.ButtonTestFilter.Name = "ButtonTestFilter";
            this.ButtonTestFilter.Size = new System.Drawing.Size(102, 54);
            this.ButtonTestFilter.TabIndex = 6;
            this.ButtonTestFilter.Text = "TestFilter";
            this.ButtonTestFilter.UseVisualStyleBackColor = true;
            // 
            // RichTextBoxAllNotifications
            // 
            this.RichTextBoxAllNotifications.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.RichTextBoxAllNotifications.Location = new System.Drawing.Point(3, 93);
            this.RichTextBoxAllNotifications.Name = "RichTextBoxAllNotifications";
            this.RichTextBoxAllNotifications.Size = new System.Drawing.Size(626, 220);
            this.RichTextBoxAllNotifications.TabIndex = 5;
            this.RichTextBoxAllNotifications.Text = "";
            // 
            // TextBoxSender
            // 
            this.TextBoxSender.Location = new System.Drawing.Point(226, 50);
            this.TextBoxSender.Name = "TextBoxSender";
            this.TextBoxSender.Size = new System.Drawing.Size(165, 20);
            this.TextBoxSender.TabIndex = 4;
            // 
            // TextBoxReceiver
            // 
            this.TextBoxReceiver.Location = new System.Drawing.Point(226, 19);
            this.TextBoxReceiver.Name = "TextBoxReceiver";
            this.TextBoxReceiver.Size = new System.Drawing.Size(165, 20);
            this.TextBoxReceiver.TabIndex = 3;
            // 
            // RadioButtonGetSent
            // 
            this.RadioButtonGetSent.AutoSize = true;
            this.RadioButtonGetSent.Location = new System.Drawing.Point(144, 53);
            this.RadioButtonGetSent.Name = "RadioButtonGetSent";
            this.RadioButtonGetSent.Size = new System.Drawing.Size(76, 17);
            this.RadioButtonGetSent.TabIndex = 2;
            this.RadioButtonGetSent.TabStop = true;
            this.RadioButtonGetSent.Text = "Verzonden";
            this.RadioButtonGetSent.UseVisualStyleBackColor = true;
            this.RadioButtonGetSent.CheckedChanged += new System.EventHandler(this.RadioButtonGetSent_CheckedChanged);
            // 
            // RadioButtonGetReceived
            // 
            this.RadioButtonGetReceived.AutoSize = true;
            this.RadioButtonGetReceived.Location = new System.Drawing.Point(144, 19);
            this.RadioButtonGetReceived.Name = "RadioButtonGetReceived";
            this.RadioButtonGetReceived.Size = new System.Drawing.Size(78, 17);
            this.RadioButtonGetReceived.TabIndex = 1;
            this.RadioButtonGetReceived.TabStop = true;
            this.RadioButtonGetReceived.Text = "Ontvangen";
            this.RadioButtonGetReceived.UseVisualStyleBackColor = true;
            this.RadioButtonGetReceived.CheckedChanged += new System.EventHandler(this.RadioButtonGetReceived_CheckedChanged);
            // 
            // ButtonNotifications
            // 
            this.ButtonNotifications.Location = new System.Drawing.Point(18, 16);
            this.ButtonNotifications.Name = "ButtonNotifications";
            this.ButtonNotifications.Size = new System.Drawing.Size(102, 54);
            this.ButtonNotifications.TabIndex = 0;
            this.ButtonNotifications.Text = "Meldingen Ophalen";
            this.ButtonNotifications.UseVisualStyleBackColor = true;
            this.ButtonNotifications.Click += new System.EventHandler(this.ButtonNotifications_Click);
            // 
            // TabSendDocument
            // 
            this.TabSendDocument.AllowDrop = true;
            this.TabSendDocument.Controls.Add(this.LabelFile);
            this.TabSendDocument.Controls.Add(this.ButtonSendUblDocument);
            this.TabSendDocument.Controls.Add(this.ButtonCheckFile);
            this.TabSendDocument.Location = new System.Drawing.Point(4, 22);
            this.TabSendDocument.Name = "TabSendDocument";
            this.TabSendDocument.Size = new System.Drawing.Size(632, 316);
            this.TabSendDocument.TabIndex = 2;
            this.TabSendDocument.Text = "Document Verzenden";
            this.TabSendDocument.UseVisualStyleBackColor = true;
            this.TabSendDocument.DragDrop += new System.Windows.Forms.DragEventHandler(this.TabSendDocument_DragDrop);
            this.TabSendDocument.DragEnter += new System.Windows.Forms.DragEventHandler(this.TabSendDocument_DragEnter);
            // 
            // LabelFile
            // 
            this.LabelFile.AutoSize = true;
            this.LabelFile.Location = new System.Drawing.Point(18, 51);
            this.LabelFile.Name = "LabelFile";
            this.LabelFile.Size = new System.Drawing.Size(352, 13);
            this.LabelFile.TabIndex = 2;
            this.LabelFile.Text = "Sleep het xml bestand naar deze ruimte en klik bestand controleren eerst.";
            this.LabelFile.TextChanged += new System.EventHandler(this.LabelFile_TextChanged);
            // 
            // ButtonSendUblDocument
            // 
            this.ButtonSendUblDocument.Enabled = false;
            this.ButtonSendUblDocument.Location = new System.Drawing.Point(21, 220);
            this.ButtonSendUblDocument.Name = "ButtonSendUblDocument";
            this.ButtonSendUblDocument.Size = new System.Drawing.Size(124, 46);
            this.ButtonSendUblDocument.TabIndex = 1;
            this.ButtonSendUblDocument.Text = "UBL Document Verzenden";
            this.ButtonSendUblDocument.UseVisualStyleBackColor = true;
            this.ButtonSendUblDocument.Click += new System.EventHandler(this.ButtonSendUblDocument_Click);
            // 
            // ButtonCheckFile
            // 
            this.ButtonCheckFile.Enabled = false;
            this.ButtonCheckFile.Location = new System.Drawing.Point(21, 13);
            this.ButtonCheckFile.Name = "ButtonCheckFile";
            this.ButtonCheckFile.Size = new System.Drawing.Size(111, 23);
            this.ButtonCheckFile.TabIndex = 0;
            this.ButtonCheckFile.Text = "Bestand Controleren";
            this.ButtonCheckFile.UseVisualStyleBackColor = true;
            this.ButtonCheckFile.Click += new System.EventHandler(this.ButtonCheckFile_Click);
            // 
            // TabReceiveDocument
            // 
            this.TabReceiveDocument.Controls.Add(this.ButtonGetUBLDocument);
            this.TabReceiveDocument.Controls.Add(this.TextBoxTransmissionId);
            this.TabReceiveDocument.Controls.Add(this.LabelTransmissionId);
            this.TabReceiveDocument.Location = new System.Drawing.Point(4, 22);
            this.TabReceiveDocument.Name = "TabReceiveDocument";
            this.TabReceiveDocument.Size = new System.Drawing.Size(632, 316);
            this.TabReceiveDocument.TabIndex = 3;
            this.TabReceiveDocument.Text = "Document Ontvangen";
            this.TabReceiveDocument.UseVisualStyleBackColor = true;
            // 
            // ButtonGetUBLDocument
            // 
            this.ButtonGetUBLDocument.Location = new System.Drawing.Point(370, 26);
            this.ButtonGetUBLDocument.Name = "ButtonGetUBLDocument";
            this.ButtonGetUBLDocument.Size = new System.Drawing.Size(119, 47);
            this.ButtonGetUBLDocument.TabIndex = 2;
            this.ButtonGetUBLDocument.Text = "Document Ophalen";
            this.ButtonGetUBLDocument.UseVisualStyleBackColor = true;
            this.ButtonGetUBLDocument.Click += new System.EventHandler(this.ButtonGetUBLDocument_Click);
            // 
            // TextBoxTransmissionId
            // 
            this.TextBoxTransmissionId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxTransmissionId.Location = new System.Drawing.Point(22, 42);
            this.TextBoxTransmissionId.Name = "TextBoxTransmissionId";
            this.TextBoxTransmissionId.Size = new System.Drawing.Size(333, 26);
            this.TextBoxTransmissionId.TabIndex = 1;
            this.TextBoxTransmissionId.Text = "f9ba35d67b2211f0bf1302bb4e4747f9";
            // 
            // LabelTransmissionId
            // 
            this.LabelTransmissionId.AutoSize = true;
            this.LabelTransmissionId.Location = new System.Drawing.Point(19, 26);
            this.LabelTransmissionId.Name = "LabelTransmissionId";
            this.LabelTransmissionId.Size = new System.Drawing.Size(74, 13);
            this.LabelTransmissionId.TabIndex = 0;
            this.LabelTransmissionId.Text = "Transmissie Id";
            // 
            // TabResponse
            // 
            this.TabResponse.Controls.Add(this.RichTextBoxResult);
            this.TabResponse.Location = new System.Drawing.Point(4, 22);
            this.TabResponse.Name = "TabResponse";
            this.TabResponse.Size = new System.Drawing.Size(632, 316);
            this.TabResponse.TabIndex = 4;
            this.TabResponse.Text = "Antwoorden";
            this.TabResponse.UseVisualStyleBackColor = true;
            // 
            // RichTextBoxResult
            // 
            this.RichTextBoxResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichTextBoxResult.Location = new System.Drawing.Point(0, 0);
            this.RichTextBoxResult.Name = "RichTextBoxResult";
            this.RichTextBoxResult.Size = new System.Drawing.Size(632, 316);
            this.RichTextBoxResult.TabIndex = 0;
            this.RichTextBoxResult.Text = "";
            // 
            // TabEntities
            // 
            this.TabEntities.Controls.Add(this.CheckBoxPublishMinimum);
            this.TabEntities.Controls.Add(this.RadioButtonBothSchemes);
            this.TabEntities.Controls.Add(this.RadioButtonOnlyScheme0208);
            this.TabEntities.Controls.Add(this.TextBoxPhone);
            this.TabEntities.Controls.Add(this.label13);
            this.TabEntities.Controls.Add(this.TextBoxContactEmail);
            this.TabEntities.Controls.Add(this.label12);
            this.TabEntities.Controls.Add(this.TextBoxWebsiteUrl);
            this.TabEntities.Controls.Add(this.label11);
            this.TabEntities.Controls.Add(this.TextBoxContactName);
            this.TabEntities.Controls.Add(this.label10);
            this.TabEntities.Controls.Add(this.TextBoxAdditionalInformation);
            this.TabEntities.Controls.Add(this.label9);
            this.TabEntities.Controls.Add(this.TextBoxGeographicalInformation);
            this.TabEntities.Controls.Add(this.TextBoxCompanyName);
            this.TabEntities.Controls.Add(this.TextBoxIdentifier);
            this.TabEntities.Controls.Add(this.label8);
            this.TabEntities.Controls.Add(this.label7);
            this.TabEntities.Controls.Add(this.label6);
            this.TabEntities.Controls.Add(this.ButtonLookUp);
            this.TabEntities.Controls.Add(this.ButtonEntityNew);
            this.TabEntities.Location = new System.Drawing.Point(4, 22);
            this.TabEntities.Name = "TabEntities";
            this.TabEntities.Size = new System.Drawing.Size(632, 316);
            this.TabEntities.TabIndex = 5;
            this.TabEntities.Text = "Entiteiten";
            this.TabEntities.UseVisualStyleBackColor = true;
            // 
            // CheckBoxPublishMinimum
            // 
            this.CheckBoxPublishMinimum.AutoSize = true;
            this.CheckBoxPublishMinimum.Location = new System.Drawing.Point(195, 250);
            this.CheckBoxPublishMinimum.Name = "CheckBoxPublishMinimum";
            this.CheckBoxPublishMinimum.Size = new System.Drawing.Size(120, 17);
            this.CheckBoxPublishMinimum.TabIndex = 21;
            this.CheckBoxPublishMinimum.Text = "Minimum Publiceren";
            this.CheckBoxPublishMinimum.UseVisualStyleBackColor = true;
            this.CheckBoxPublishMinimum.CheckedChanged += new System.EventHandler(this.CheckBoxPublishMinimum_CheckedChanged);
            // 
            // RadioButtonBothSchemes
            // 
            this.RadioButtonBothSchemes.AutoSize = true;
            this.RadioButtonBothSchemes.Location = new System.Drawing.Point(280, 222);
            this.RadioButtonBothSchemes.Name = "RadioButtonBothSchemes";
            this.RadioButtonBothSchemes.Size = new System.Drawing.Size(91, 17);
            this.RadioButtonBothSchemes.TabIndex = 20;
            this.RadioButtonBothSchemes.Text = "0208 en 9925";
            this.RadioButtonBothSchemes.UseVisualStyleBackColor = true;
            // 
            // RadioButtonOnlyScheme0208
            // 
            this.RadioButtonOnlyScheme0208.AutoSize = true;
            this.RadioButtonOnlyScheme0208.Checked = true;
            this.RadioButtonOnlyScheme0208.Location = new System.Drawing.Point(195, 222);
            this.RadioButtonOnlyScheme0208.Name = "RadioButtonOnlyScheme0208";
            this.RadioButtonOnlyScheme0208.Size = new System.Drawing.Size(79, 17);
            this.RadioButtonOnlyScheme0208.TabIndex = 19;
            this.RadioButtonOnlyScheme0208.TabStop = true;
            this.RadioButtonOnlyScheme0208.Text = "Enkel 0208";
            this.RadioButtonOnlyScheme0208.UseVisualStyleBackColor = true;
            // 
            // TextBoxPhone
            // 
            this.TextBoxPhone.Location = new System.Drawing.Point(195, 114);
            this.TextBoxPhone.Name = "TextBoxPhone";
            this.TextBoxPhone.Size = new System.Drawing.Size(429, 20);
            this.TextBoxPhone.TabIndex = 17;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(28, 121);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(49, 13);
            this.label13.TabIndex = 16;
            this.label13.Text = "Telefoon";
            // 
            // TextBoxContactEmail
            // 
            this.TextBoxContactEmail.Location = new System.Drawing.Point(195, 190);
            this.TextBoxContactEmail.Name = "TextBoxContactEmail";
            this.TextBoxContactEmail.Size = new System.Drawing.Size(429, 20);
            this.TextBoxContactEmail.TabIndex = 15;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(28, 197);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(26, 13);
            this.label12.TabIndex = 14;
            this.label12.Text = "Mail";
            // 
            // TextBoxWebsiteUrl
            // 
            this.TextBoxWebsiteUrl.Location = new System.Drawing.Point(195, 138);
            this.TextBoxWebsiteUrl.Name = "TextBoxWebsiteUrl";
            this.TextBoxWebsiteUrl.Size = new System.Drawing.Size(429, 20);
            this.TextBoxWebsiteUrl.TabIndex = 13;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(28, 145);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(46, 13);
            this.label11.TabIndex = 12;
            this.label11.Text = "Website";
            // 
            // TextBoxContactName
            // 
            this.TextBoxContactName.Location = new System.Drawing.Point(195, 164);
            this.TextBoxContactName.Name = "TextBoxContactName";
            this.TextBoxContactName.Size = new System.Drawing.Size(429, 20);
            this.TextBoxContactName.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(28, 171);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "Contact";
            // 
            // TextBoxAdditionalInformation
            // 
            this.TextBoxAdditionalInformation.Location = new System.Drawing.Point(195, 88);
            this.TextBoxAdditionalInformation.Name = "TextBoxAdditionalInformation";
            this.TextBoxAdditionalInformation.Size = new System.Drawing.Size(429, 20);
            this.TextBoxAdditionalInformation.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(28, 95);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Bijkomende Info";
            // 
            // TextBoxGeographicalInformation
            // 
            this.TextBoxGeographicalInformation.Location = new System.Drawing.Point(195, 58);
            this.TextBoxGeographicalInformation.Name = "TextBoxGeographicalInformation";
            this.TextBoxGeographicalInformation.Size = new System.Drawing.Size(429, 20);
            this.TextBoxGeographicalInformation.TabIndex = 7;
            // 
            // TextBoxCompanyName
            // 
            this.TextBoxCompanyName.Location = new System.Drawing.Point(195, 32);
            this.TextBoxCompanyName.Name = "TextBoxCompanyName";
            this.TextBoxCompanyName.ReadOnly = true;
            this.TextBoxCompanyName.Size = new System.Drawing.Size(429, 20);
            this.TextBoxCompanyName.TabIndex = 6;
            // 
            // TextBoxIdentifier
            // 
            this.TextBoxIdentifier.Location = new System.Drawing.Point(195, 6);
            this.TextBoxIdentifier.Name = "TextBoxIdentifier";
            this.TextBoxIdentifier.Size = new System.Drawing.Size(134, 20);
            this.TextBoxIdentifier.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(28, 65);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Volledig Adres ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Bedrijfsnaam (zie KBO)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "KBO Ondernemingsnummer";
            // 
            // ButtonLookUp
            // 
            this.ButtonLookUp.Location = new System.Drawing.Point(335, 3);
            this.ButtonLookUp.Name = "ButtonLookUp";
            this.ButtonLookUp.Size = new System.Drawing.Size(75, 23);
            this.ButtonLookUp.TabIndex = 1;
            this.ButtonLookUp.Text = "Opzoeken";
            this.ButtonLookUp.UseVisualStyleBackColor = true;
            this.ButtonLookUp.Click += new System.EventHandler(this.ButtonLookUp_Click);
            // 
            // ButtonEntityNew
            // 
            this.ButtonEntityNew.Enabled = false;
            this.ButtonEntityNew.Location = new System.Drawing.Point(493, 240);
            this.ButtonEntityNew.Name = "ButtonEntityNew";
            this.ButtonEntityNew.Size = new System.Drawing.Size(131, 27);
            this.ButtonEntityNew.TabIndex = 0;
            this.ButtonEntityNew.Text = "Registratie Starten";
            this.ButtonEntityNew.UseVisualStyleBackColor = true;
            this.ButtonEntityNew.Click += new System.EventHandler(this.ButtonEntityNew_Click);
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatusLabel});
            this.StatusStrip.Location = new System.Drawing.Point(0, 320);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(640, 22);
            this.StatusStrip.TabIndex = 9;
            this.StatusStrip.Text = "statusStrip1";
            // 
            // ToolStripStatusLabel
            // 
            this.ToolStripStatusLabel.Name = "ToolStripStatusLabel";
            this.ToolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.ToolStripStatusLabel.Text = "Ready";
            // 
            // FormAdminPeppolActions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(640, 342);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.TabControlVariousActions);
            this.Controls.Add(this.ButtonClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAdminPeppolActions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormPeppolTesting";
            this.TabControlVariousActions.ResumeLayout(false);
            this.TabActions.ResumeLayout(false);
            this.TabActions.PerformLayout();
            this.TabNotifications.ResumeLayout(false);
            this.TabNotifications.PerformLayout();
            this.TabSendDocument.ResumeLayout(false);
            this.TabSendDocument.PerformLayout();
            this.TabReceiveDocument.ResumeLayout(false);
            this.TabReceiveDocument.PerformLayout();
            this.TabResponse.ResumeLayout(false);
            this.TabEntities.ResumeLayout(false);
            this.TabEntities.PerformLayout();
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.TabControl TabControlVariousActions;
        private System.Windows.Forms.TabPage TabActions;
        private System.Windows.Forms.TabPage TabNotifications;
        private System.Windows.Forms.TabPage TabSendDocument;
        private System.Windows.Forms.TabPage TabReceiveDocument;
        private System.Windows.Forms.TabPage TabResponse;
        private System.Windows.Forms.TabPage TabEntities;
        private System.Windows.Forms.Button ButtonCheckConnectivity;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel;
        private System.Windows.Forms.TextBox TextBoxLegalEntityId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxCountryCode;
        private System.Windows.Forms.TextBox TextBoxRegScheme;
        private System.Windows.Forms.TextBox TextBoxRegIdentifier;
        private System.Windows.Forms.TextBox TextBoxSupportedDocument;
        private System.Windows.Forms.Button ButtonGetPeppolRegistrations;
        private System.Windows.Forms.RichTextBox RichTextBoxResult;
        private System.Windows.Forms.RadioButton RadioButtonGetSent;
        private System.Windows.Forms.RadioButton RadioButtonGetReceived;
        private System.Windows.Forms.Button ButtonNotifications;
        private System.Windows.Forms.TextBox TextBoxSender;
        private System.Windows.Forms.TextBox TextBoxReceiver;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button ButtonLookUp;
        private System.Windows.Forms.Button ButtonEntityNew;
        private System.Windows.Forms.TextBox TextBoxGeographicalInformation;
        private System.Windows.Forms.TextBox TextBoxCompanyName;
        private System.Windows.Forms.TextBox TextBoxIdentifier;
        private System.Windows.Forms.TextBox TextBoxAdditionalInformation;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox TextBoxContactName;
        private System.Windows.Forms.TextBox TextBoxWebsiteUrl;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox TextBoxContactEmail;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox TextBoxTransmissionId;
        private System.Windows.Forms.Label LabelTransmissionId;
        private System.Windows.Forms.Button ButtonGetUBLDocument;
        private System.Windows.Forms.Label LabelFile;
        private System.Windows.Forms.Button ButtonSendUblDocument;
        private System.Windows.Forms.Button ButtonCheckFile;
        private System.Windows.Forms.TextBox TextBoxPhone;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.RadioButton RadioButtonBothSchemes;
        private System.Windows.Forms.RadioButton RadioButtonOnlyScheme0208;
        private System.Windows.Forms.CheckBox CheckBoxPublishMinimum;
        private System.Windows.Forms.Button ButtonCheckPeppolRegistration;
        private System.Windows.Forms.RadioButton RadioButtonJson;
        private System.Windows.Forms.RadioButton RadioButtonUseXml;
        private System.Windows.Forms.Button ButtonShowAllNotificationsAsReceiver;
        private System.Windows.Forms.Button ButtonShowAllNotificationsAsSender;
        private System.Windows.Forms.RichTextBox RichTextBoxAllNotifications;
        private System.Windows.Forms.Button ButtonTestFilter;
        private System.Windows.Forms.Button ButtonMailAllStatusReports;
    }
}