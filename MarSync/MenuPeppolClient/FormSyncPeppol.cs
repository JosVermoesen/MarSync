using ADODB;
using MarSync.Classes.Ademico;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Vsoft.Mail;
using ZXing;
using static MarSync.AdemicoModels;
using static MarSync.MarHelpers;

using Timer = System.Windows.Forms.Timer;

namespace MarSync.MarioMenu.Actions
{
    public partial class FormSyncPeppol : Form
    {
        private readonly Timer _timer;
        private readonly HttpClient httpCheck;

        private readonly Timer _countdownTimer; // Countdown timer
        private int _countdownSeconds; // Countdown seconds

        public int titanicDeckChairs = 0; // Just to check changes on documents in the listboxes

        public int totalInMapOut = 0;
        public int totalInMapIn = 0;

        public int totalInMapOutSent = 0;
        public int totalInMapOutSentConfirmed = 0;
        public int totalInMapOutAcceptedOrRejected = 0;

        public int totalInMapInToBook = 0;
                
        public bool firstCycle = false;
        public bool badRequestsOccurred = false;

        public FormSyncPeppol()
        {
            InitializeComponent();
                        
            _timer = new Timer
            {
                Interval = 1 * 60 * 1000 // 1 minuut in milliseconds
            };
            _timer.Tick += Timer_Tick;
            _timer.Stop();

            if (SharedGlobals.ApiModus == "PRODUCTIE")
            {
                Text = "MarSync v4.0.0.0 [" + SharedGlobals.ActiveCompany + "]";
            }
            else
            {
                Text = "MarSync v4.0.0.0 [" + SharedGlobals.ActiveCompany + "] " + SharedGlobals.ApiModus;
            }
            LabelActiveCompanyName.Text = SharedGlobals.CompanyName;

            httpCheck = new HttpClient(); // Initialize here

            // Configure countdown timer once (subscribe Tick here to avoid duplicate subscriptions)
            _countdownTimer = new Timer
            {
                Interval = 1000 // 1 second
            };
            _countdownTimer.Tick += CountdownTimer_Tick;
            _countdownTimer.Stop();

            InitializeControls();
            ShowOnlyFirstTwoTabs();
            RefreshMonitor(false);
        }

        private void ShowOnlyFirstTwoTabs()
        {
            // Keep references if you ever want to restore them later
            var tabsToKeep = new[]
            {
                TabControlVariousActions.TabPages[0],
                TabControlVariousActions.TabPages[1]
            };

            // Remove all tabs
            TabControlVariousActions.TabPages.Clear();

            // Re-add only the first two
            TabControlVariousActions.TabPages.AddRange(tabsToKeep);

            // Ensure selection is valid
            TabControlVariousActions.SelectedIndex = 0;
        }

        private async void InitializeControls()
        {
            string marVersion = Interaction.GetSetting(
                "marINTEGRAAL",       // AppName
                "marIntegraal",       // Section
                "VersionNumber",
                ""                    // Default if not found
                ) ?? string.Empty;


            // Be defensive: user settings may be empty, missing or non‑numeric
            if (!int.TryParse(marVersion, out int valOfVersion))
            {
                // Either skip the version check, or treat as “too old”
                // Here: treat unknown as “too old” but don’t crash
                MessageBox.Show(
                    "Versienummer van MarIntegraal kon niet gelezen worden.\n" +
                    "Gelieve een recentere versie te installeren.",
                    "Onbekende MarIntegraal versie",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);                
            }
            else if (valOfVersion < 115024)
            {
                MessageBox.Show(
                    "Installeer recentere versie van MarIntegraal a.u.b.",
                    "Versie MarIntegraal",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);                
            }

            //if (valOfVersion < 115007)
            //    MessageBox.Show("Installeer recentere versie van MarIntegraal a.u.b.");

            // Focus on LabelResponseCode.Text = "AP"; // Acceptatie
            // Focus on LabelResponseCode.Text = "RE"; // Weigering
            // Code Description
            // AP   Accepted:
            //      Status is used only when the Buyer has given a final approval of the invoice
            //      and the next step is payment.
            // RE   Rejected:
            //      MLR reject or Invoice reject.Status is used only when the Buyer will not
            //      process the referenced Invoice any further.Buyer is rejecting this invoice
            //      but not necessarily the commercial transaction.Although it can be used also
            //      for rejection for commercial reasons (invoice not corresponding to delivery).
            // PD   Paid:
            //      Fully paid or partially paid.When partially paid:
            //      Status is used together with Clarification Reason code PPD,
            //      only when the Buyer has initiated the payment of the invoice
            //      without having paid the accepted amount in full.
            // AB   Buyer acknowledges:
            //      Status is used when Buyer has received a readable invoice message
            //      that can be understood and submitted for processing by the Buyer.            
            // IP   In process:
            //      Status is used when the processing of the Invoice has started in Buyers system.
            // UQ   Under query:
            //      Status is used when Buyer will not proceed to accept the Invoice
            //      without receiving additional information from the Seller.
            // CA   Conditionally accepted:
            //      Status is used when Buyer is accepting the Invoice under conditions stated
            //      in 'Status Reason' and proceed to pay accordingly unless disputed by Seller.            

            ComboBoxResponseCode.Items.Clear();
            ComboBoxResponseCode.Items.Add(new KeyValuePair<string, string>("AP", "Aanvaard (Accepted)"));
            ComboBoxResponseCode.Items.Add(new KeyValuePair<string, string>("PD", "Betaald (Paid)"));
            ComboBoxResponseCode.Items.Add(new KeyValuePair<string, string>("RE", "Verworpen (Rejected)"));
            ComboBoxResponseCode.Items.Add(new KeyValuePair<string, string>("CA", "Gedeeltelijk aanvaard (Conditionally accepted)"));

            ComboBoxResponseCode.Items.Add(new KeyValuePair<string, string>("AB", "Koper erkent (Buyer acknowledges)"));
            ComboBoxResponseCode.Items.Add(new KeyValuePair<string, string>("IP", "In verwerking (In process)"));
            ComboBoxResponseCode.Items.Add(new KeyValuePair<string, string>("UQ", "In onderzoek (Under query)"));            
            ComboBoxResponseCode.DisplayMember = "Value";
            ComboBoxResponseCode.ValueMember = "Key";
            ComboBoxResponseCode.SelectedIndex = 0;
            ComboBoxBuyerOrSeller.SelectedIndex = 1; // Default to Seller, Buyer is checked at invoicing
            ComboBoxBuyerOrSeller.Enabled = true; // Disable selection eventualy, as at moment only Seller is supported for updating supported documents

            // Code Description
            // REC  Receiver unknown
            // UNR  Not recognized
            // NO   No issue
            // REF  References incorrect
            // LEG  Legal information incorrect
            // QUA  Item quality insufficient
            // DEL  Delivery proposed or provided is not acceptable
            // PRI  Prices incorrect
            // QTY  Quantity incorrect
            // ITM  Items incorrect
            // PAY  Payment terms incorrect            
            // FIN  Finance incorrect
            // PPD  Partially Paid
            // OTH  Other
            ComboBoxClarificationCode.Items.Clear();
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("REC", "Ontvanger onbekend (Receiver unknown)"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("UNR", "Niet herkend (Not recognized)"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("NO", "Geen probleem (No issue)"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("REF", "Referenties onjuist (References incorrect)"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("LEG", "Juridische informatie onjuist (Legal information incorrect)"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("QUA", "Artikelkwaliteit onvoldoende (Item quality insufficient)"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("DEL", "Levering niet acceptabel (Delivery not acceptable)"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("PRI", "Prijzen onjuist (Prices incorrect)"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("QTY", "Hoeveelheid onjuist (Quantity incorrect)"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("ITM", "Stuks onjuist (Items incorrect)"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("PAY", "Betalingsvoorwaarden onjuist (Payment terms incorrect)"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("FIN", "Financiën onjuist (Finance incorrect)"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("PPD", "Gedeeltelijk betaald (Partially Paid)"));
            ComboBoxClarificationCode.Items.Add(new KeyValuePair<string, string>("OTH", "Andere (Other)"));
            ComboBoxClarificationCode.DisplayMember = "Value";
            ComboBoxClarificationCode.ValueMember = "Key";
            ComboBoxClarificationCode.SelectedIndex = 0;

            if (SharedGlobals.AccountingContactEmailAddress.TrimEnd() == "")
            {
                PanelReporting.Enabled = false;
            }
            else
            {
                PanelReporting.Enabled = true;
            }
                        
            if (SharedGlobals.UserCanToggleTabs)
            {
                
            }
            else
            {
                
            }

            SetLabelsBusy(false);
        }

        private void SetLabelsBusy(bool all)
        {
            if (all)
            {
                LabelFileOut.Text = "bezig";
                LabelFileIn.Text = "bezig";

                LabelTotalInMapOut.Text = "bezig";
                LabelTotalInMapIn.Text = "bezig";

                LabelTotalInMapOutSent.Text = "bezig";
                LabelTotalInMapOutSentConfirmed.Text = "bezig";
            }
            else
            {
                LabelFileOut.Text = "bezig";
                LabelFileIn.Text = "bezig";
            }
        }

        async private Task RefreshMonitor(bool withActions)
        {
            titanicDeckChairs = 0;
            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;
            ToolStripStatusLabel.Text = "Bezig...";
            Application.DoEvents();

            RefreshListPeppolReceive();
            RefreshListPeppolSend();

            if (withActions)
            {
                bool result = await GetAllNotificationsForCompany();
                if (!result)
                {
                    MessageBox.Show("Fout bij instellen van notificaties. Timer niet gestart.", "Foutmelding", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ToolStripStatusLabel.Text = "Fout bij instellen van notificaties. Timer niet gestart.";
                    return;
                }
                if (!firstCycle)
                {
                    // Handle non-first cycle logic here
                    firstCycle = true;
                    PanelReactions.Visible = true;
                    LabelForReactions.Visible = false;
                }
            }

            if (withActions)
            {
                if (totalInMapOut > 0)
                {
                    LabelTotalInMapOutSent.Text = totalInMapOutSent.ToString();
                    await RefreshCyclePeppolSent();
                }
                else
                {
                    // Even if no documents to process, update the label to show current count
                    LabelTotalInMapOutSentConfirmed.Text = totalInMapOutSentConfirmed.ToString();
                }
            }

            if (withActions)
            {
                await RefreshCyclePeppolReceive();
                // RefreshCyclePeppolReceive("0208:");
                // if (CheckBoxAccept9925.Checked) {
                //     RefreshCyclePeppolReceive("9925:BE");
                // }  
                LabelTotalToBookMapin.Text = totalInMapInToBook.ToString();
            }
            Application.DoEvents();
            Application.UseWaitCursor = false;
            Cursor.Current = Cursors.Default;

            if (!withActions)
                ToolStripStatusLabel.Text = "Klaar! Druk [Esc] om te sluiten.";

            Application.DoEvents();
        }

        private void FormPeppolClientActions_FormClosing(object sender, FormClosingEventArgs e)
        {
            _timer.Stop(); // Stop the timer when the form is closing
            _timer.Dispose(); // Dispose of the timer to free resources
            _countdownTimer.Stop(); // Stop countdown timer
            _countdownTimer.Dispose(); // Dispose of the countdown timer

            if (badRequestsOccurred)
            {
                MessageBox.Show(
                    "Er zijn fouten meegedeeld aan Vsoft voor verkoopdocumenten.\n\n" +
                    SharedGlobals.ApiModus + " omgeving.\n\n" +
                    "Gelieve de aanbevelingen van Vsoft af te wachten",
                    "Bad Request op verkoopdocument(en)",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        async private void ButtonCheckConnectivity_Click(object sender, EventArgs e)
        {
            if (await IsConnectionValid()) {             
                ToolStripStatusLabel.Text = "Connectie succesvol.";
            } else
            {
                ToolStripStatusLabel.Text = "Connectie mislukt, controleer uw verbinding.";             
            }

            //try
            //{
            //    var response = await AdemicoClient.CheckConnection(new HttpClient());
            //    if (!string.IsNullOrEmpty(response))
            //    {
            //        ToolStripStatusLabel.Text = response;
            //    }
            //    else
            //    {
            //        MessageBox.Show("No response from server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
            //catch (HttpRequestException ex)
            //{
            //    MessageBox.Show(
            //        "HTTP error:\n" + ex.Message +
            //        (ex.InnerException != null ? "\n\nInner: " + ex.InnerException.Message : string.Empty),
            //        "Connectivity error",
            //        MessageBoxButtons.OK,
            //        MessageBoxIcon.Error);

            //    ToolStripStatusLabel.Text = "HTTP error: " + ex.Message;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(
            //        "Unexpected error:\n" + ex.Message,
            //        "Error",
            //        MessageBoxButtons.OK,
            //        MessageBoxIcon.Error);

            //    ToolStripStatusLabel.Text = "Error: " + ex.Message;
            //}
        }

        async private Task<bool> IsConnectionValid()
        {
            try
            {
                var response = await AdemicoClient.CheckConnection(new HttpClient());
                if (!string.IsNullOrEmpty(response))
                {
                    if (response.Contains("HttpRequestException"))
                    {
                        return false;
                    }else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (HttpRequestException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }   

        private async void ButtonTimer_Click(object sender, EventArgs e)
        {
            if (await IsConnectionValid())
            {
                // CONTINUE
            }
            else
            {
                ToolStripStatusLabel.Text = "Connectie mislukt, controleer uw verbinding.";
                return; // Stop if connection is not valid
            }

            // Optional UI setup before starting
            if (totalInMapOut > 0)
            {
                SetLabelsBusy(true);
            }
            else
            {
                LabelTotalInMapOutSent.Text = "0";
                LabelTotalInMapOutSentConfirmed.Text = "0";
            }

            // Do an immediate refresh then start the scheduled loop            
            RefreshMonitor(true);

            // Start the main timer
            _timer.Start();

            // Reset and start per-second countdown
            _countdownSeconds = _timer.Interval / 1000; // initialize seconds from main timer interval
            UpdateCountdownStatus(); // immediate UI update
            _countdownTimer.Start();

            ToolStripStatusLabel.Text = $"Timer is gestart. Next refresh in {FormatSeconds(_countdownSeconds)}";
            
            ButtonTimer.Enabled = false;
            ButtonMyStatusReport.Enabled = false;

            ButtonUpdateBEPartiesSupported.Enabled = false;
            ComboBoxBuyerOrSeller.Enabled = false;
            ButtonClose.Enabled = false;

            ButtonTimer.BackColor = System.Drawing.Color.MistyRose;
            ButtonMyStatusReport.BackColor = System.Drawing.Color.MistyRose;
        }

        private void ButtonShowSharedGlobals_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                $"Naam Bedrijf (KBO):\n{SharedGlobals.CompanyName}\n\n" +
                $"Adres: {SharedGlobals.CompanyAddress}\n" +
                $"Postcode en Plaats: {SharedGlobals.CompanyPostalCodeAndCity}\n\n" +
                $"Telefoon: {SharedGlobals.CompanyPhoneNumber}\n" +
                $"KBO Nummer: {SharedGlobals.CompanyKBONumber}\n" +
                $"BTW Nummer (zonder 'BE'): {SharedGlobals.CompanyVATNumber}\n" +
                $"IBAN Rekening Nummer: {SharedGlobals.CompanyIBANNumber}\n" +
                $"BIC Nummer: {SharedGlobals.CompanyBICNumber}\n\n" +
                $"Email Adres Bedrijf: {SharedGlobals.CompanyEmailAddress}\n" +
                $"Contactpersoon: {SharedGlobals.CompanyContactPerson}\n" +
                $"Email Adres Contactpersoon: {SharedGlobals.CompanyContactEmailAddress}\n\n" +                                
                $"Inhoudsopgave Mar Mdv Bestand: {SharedGlobals.MarntMdvLocation}\n",
                "Variabele gegevens van actief bedrijf",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void ButtonMarVariables_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                $"Inhoudsopgave Mar Data:\n {SharedGlobals.MimDataLocation}\n\n" +
                $"Inhoudsopgave Marnt Cloud:\n {SharedGlobals.MarntCloudLocation}\n\n" +
                $"Inhoudsopgave Archief Cloud:\n {SharedGlobals.MarntCLoudArchiveLocation}\n\n" +
                $"Inhoudsopgave Manueel Cloud:\n {SharedGlobals.MarntCloudMarioLocation}\n",
                "Variabele gegevens van MarIntegraal op dit toestel",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        async private void ButtonGetUBLDocument_Click(object sender, EventArgs e)
        {
            if (TextBoxDocumentTransmissionId.Text.Length == 0)
            {
                MessageBox.Show("Gelieve een Transmission ID in te vullen.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ToolStripStatusLabel.Text = "Ready";
                return;
            }

            string confirmMessage = $"Weet u zeker dat u het UBL document met Transmission ID {TextBoxDocumentTransmissionId.Text} wilt ophalen?";
            var confirmResult = MessageBox.Show(confirmMessage, "Bevestig Ophalen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult != DialogResult.Yes)
            {
                ToolStripStatusLabel.Text = "Ophalen geannuleerd door gebruiker.";
                return; // User chose not to proceed
            }

            string myDocumentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string ademicoUrl = SharedGlobals.AdemicoApiUrl;
            string accessToken = SharedGlobals.AdemicoAccessToken;
            string username = SharedGlobals.AdemicoUsername;
            string password = SharedGlobals.AdemicoPassword;

            if (TextBoxDocumentTransmissionId.Text.Length == 0)
            {
                MessageBox.Show("Gelieve een Transmission ID in te vullen.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ToolStripStatusLabel.Text = "Ready";
                return;
            }
            string transmissionId = TextBoxDocumentTransmissionId.Text.Trim();
            string requestUrl = $"{ademicoUrl}/api/peppol/v1/invoices/{transmissionId}/ubl?accessToken={accessToken}";

            try
            {
                HttpClient client = new HttpClient();
                var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                // Accept XML
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                // Make the GET request
                HttpResponseMessage response = await client.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                // Read the content
                string invoiceXml = await response.Content.ReadAsStringAsync();

                // Save to file                
                // Combine folder path with the filename you want
                string outputFile = Path.Combine(myDocumentsFolderPath, "invoice.xml");

                // Save to file in My Documents
                await Task.Run(() => File.WriteAllText(outputFile, invoiceXml));

                string documentId = ReadUBLInvoice(outputFile, false, true, false, false, false).ToUpper();
                string destinationPath = MoveXmlDocumentReceivedToMarPeppolIn(documentId);
                ToolStripStatusLabel.Text = "UBL Document " + documentId + ".XML opgehaald en bewaard met succes.";

                MessageBox.Show(
                    "UBL Document XML opgehaald en bewaard met succes.\n\n" +
                    $"Document bewaard als: {documentId}",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ToolStripStatusLabel.Text = "Klaar. Druk [Esc] om te sluiten.";
            }
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        async private void ButtonPublicSearch_Click(object sender, EventArgs e)
        {
            string toSearch;

            if (CheckBoxReceiver.Checked)
            {
                toSearch = TbNotificationReceiver.Text.Trim();
            }
            else
            {
                toSearch = TbNotificationSender.Text.Trim();
            }

            string result = await MarHelpers.GetPublicPeppolRegistrationAsync(toSearch, true);
            if (result != null)
            {
                ToolStripStatusLabel.Text = "Notifications retrieved successfully.";
                var deserializedString = JsonConvert.DeserializeObject(result);
                RichTextBoxResponses.Text = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
                MessageBox.Show(
                RichTextBoxResponses.Text,
                "Public Peppol Registration Search Result",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            }
            else
            {
                ToolStripStatusLabel.Text = "Mislukte Peppol Registratie Zoekopdracht";
                RichTextBoxResponses.Text = "";
            }
        }

        async private void ButtonUpdateBESupportedDocuments_Click(object sender, EventArgs e)
        {
            string confirmMessage =
                "Voor het actieve bedrijf de fiches van alle " + ComboBoxBuyerOrSeller.Text + " " +
                "bijwerken met hun ondersteunde Peppol functies.\n\n" +
                "Dit kan enige tijd in beslag nemen.";

            var confirmResult = MessageBox.Show(
                confirmMessage,
                "Bevestig bijwerken",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (confirmResult != DialogResult.Yes)
                return;

            ToolStripStatusLabel.Text = "Bezig...";
            Application.UseWaitCursor = true;
            Application.DoEvents();

            ButtonTimer.Enabled = false;
            ButtonMyStatusReport.Enabled = false;
            
            ButtonUpdateBEPartiesSupported.Enabled = false;
            ComboBoxBuyerOrSeller.Enabled = false;

            int updated = 0;
            bool isBuyer = ComboBoxBuyerOrSeller.SelectedIndex == 0; // Assuming 0 = Buyer, 1 = Seller

            try
            {
                updated = await RefreshSupportedDocumentsPartiesRS(isBuyer);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                Application.UseWaitCursor = false;
                MessageBox.Show(
                    "Er is een fout opgetreden bij toegang tot de database (COMException).\n\n" +
                    $"Bericht: {ex.Message}\n\n" +
                    $"HResult: 0x{ex.HResult:X8}",
                    "Databasefout",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                ToolStripStatusLabel.Text = "Databasefout (COMException).";
                return;
            }
            finally
            {
                Application.UseWaitCursor = false;
            }

            if (updated > 0)
            {
                ToolStripStatusLabel.Text = updated + " " + ComboBoxBuyerOrSeller.Text + " Ondersteunde documenten bijgewerkt met succes.";
                MessageBox.Show(ComboBoxBuyerOrSeller.Text + "  Ondersteunde documenten bijgewerkt met succes.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ToolStripStatusLabel.Text = "Klaar. Druk [Esc] om te sluiten.";
                DataGridViewNotifications.Visible = false;
            }
            else
            {
                ToolStripStatusLabel.Text = "Mislukt bijwerken van Klanten Ondersteunde Documenten.";
                MessageBox.Show("Mislukt bijwerken van Klanten Ondersteunde Documenten.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ToolStripStatusLabel.Text = "Er zijn fouten. Klanten met dubbele ondernemingsnummer enz.";
            }

            ToolStripStatusLabel.Text = "Klaar. Druk [Esc] om te sluiten.";
            ButtonTimer.Enabled = true;
            ButtonMyStatusReport.Enabled = true;

            ButtonUpdateBEPartiesSupported.Enabled = true;
            ComboBoxBuyerOrSeller.Enabled = true;
        }

        async private void ButtonInvoiceReceivedResponse_Click(object sender, EventArgs e)
        {
            string selectedResponseCode =
                ((KeyValuePair<string, string>)ComboBoxResponseCode.SelectedItem).Key;
                        
            var clarifications = new List<InvoiceResponseClarification>();
            if (selectedResponseCode == "AP" || selectedResponseCode == "PD")
            {
                clarifications = null;
            }
            else
            {
                string selectedClarificationCode =
                ((KeyValuePair<string, string>)ComboBoxClarificationCode.SelectedItem).Key;

                // Example clarification for rejection
                var clarification = new InvoiceResponseClarification
                {
                    ClarificationType = "OPStatusReason",
                    ClarificationCode = selectedClarificationCode,
                    Clarification = TextBoxClarification.Text
                };
                clarifications.Add(clarification);
            }

            string confirmMessage =
                "Reactie verzenden naar leverancier\n\n" +
                    $"Transmission ID: {TextBoxToAcceptOrRejectTransId.Text}\n" +
                    $"Response Code: {selectedResponseCode} ({ComboBoxResponseCode.Text})\n" +
                    $"Note: {TextBoxNote.Text}\n" +
                        (clarifications != null && clarifications.Count > 0
                            ? $"Clarification Code: {clarifications[0].ClarificationCode} ({ComboBoxClarificationCode.Text})\nClarification: {clarifications[0].Clarification}\n"
                            : "") + "\n" +

                "Weet u zeker dat u deze actie wilt uitvoeren?";
            var confirmResult = MessageBox.Show(confirmMessage, "Bevestig bijwerken", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (confirmResult != DialogResult.Yes)
            {
                return; // User chose No, exit the method
            }
            
            try
            {
                var result = await AdemicoClient.SendInvoiceResponseAsync(
                    invoiceTransmissionId: TextBoxToAcceptOrRejectTransId.Text,  // "2278c90b96ea11f0a46406668988840f",
                    responseCode: selectedResponseCode, // "AP" = Accepted, see your code table
                    note: TextBoxNote.Text,
                    effectiveDate: DateTime.UtcNow.ToString("yyyy-MM-dd"), // Optional, can be empty
                    clarifications: clarifications  // or a list of InvoiceResponseClarification if needed
                );

                if (result != null)
                {
                    ToolStripStatusLabel.Text = "Document Response succesvol verzonden.";
                    var deserializedString = JsonConvert.DeserializeObject(result.ResponseBody);
                    RichTextBoxResponses.Text = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
                    MessageBox.Show(
                        RichTextBoxResponses.Text,
                        "Resultaat Verzenden Documentreactie",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    ToolStripStatusLabel.Text = "Mislukt verzenden Documentreactie.";
                    RichTextBoxResponses.Text = "";
                }
            }
            catch (Exception ex)
            {
                ToolStripStatusLabel.Text = $"Fout: {ex.Message}";
                MessageBox.Show($"Fout: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RadioButtonAccept_CheckedChanged(object sender, EventArgs e)
        {
            ButtonInvoiceReceivedResponse.Text = "Acceptatie Verzenden";
            LabelResponseCode.Text = "AP";
            // Factuur geaccepteerd voor betalingsverwerking.
        }

        private void RadiobuttonReject_CheckedChanged(object sender, EventArgs e)
        {
            ButtonInvoiceReceivedResponse.Text = "Weigering Verzenden";
            LabelResponseCode.Text = "RE";
        }

        // Notifications Tab
        private void CheckBoxSender_CheckedChanged(object sender, EventArgs e)
        {
            TbNotificationSender.Enabled = CheckBoxSender.Checked;
            if (!CheckBoxSender.Checked)
            {
                TbNotificationSender.Text = "";
            }
            else
            {
                CheckBoxEventType.Checked = false;
                TbNotificationSender.Text = "0208:" + SharedGlobals.CompanyKBONumber; // Default sender for sent documents                
                TbNotificationReceiver.Text = "";
                TbNotificationReceiver.Enabled = false;
                CheckBoxReceiver.Checked = false;
                CheckBoxEventType.Checked = true;
            }
        }

        private void CheckBoxReceiver_CheckedChanged(object sender, EventArgs e)
        {
            TbNotificationReceiver.Enabled = CheckBoxReceiver.Checked;
            if (!CheckBoxReceiver.Checked)
            {
                TbNotificationReceiver.Text = "";
            }
            else
            {
                CheckBoxEventType.Checked = false;
                TbNotificationReceiver.Text = "0208:" + SharedGlobals.CompanyKBONumber; // Default receiver for received documents                
                TbNotificationSender.Text = "";
                TbNotificationSender.Enabled = false;
                CheckBoxSender.Checked = false;
                CheckBoxEventType.Checked = true;
            }
        }

        async private void ButtonNotifications_Click(object sender, EventArgs e)
        {
            // check if Response contains Too many requests
            var jsonResponse = await AdemicoClient.GetNotificationsAsync(
                transmissionId: TbNotificationTransId.Text, // "f8a591c77b2211f0b1ed0af13d778bd4"
                documentId: TbNotificationDocumentId.Text,
                // documentStatus: TbNotificationDocumentStatus.Text, // "RECEIVED" or "SENT"
                eventType: TbNotificationEventType.Text, // "DOCUMENT_RECEIVED" or "DOCUMENT_SENT"
                peppolDocumentType: TbNotificationPeppolDocumentType.Text, // "INVOICE"
                sender: TbNotificationSender.Text, // "9925:BE0440058217",
                receiver: TbNotificationReceiver.Text, // "0208:0440058217",
                startDateTime: "", // "2023-07-25T11:03:26.688Z"
                endDateTime: "", // "2023-07-29T11:03:26.688Z"
                page: "",
                pageSize: "250"
            );

            if (jsonResponse != null)
            {
                ToolStripStatusLabel.Text = "Meldingen succesvol opgehaald.";
                var deserializedString = JsonConvert.DeserializeObject(jsonResponse);
                RichTextBoxResponses.Text = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
                // DoPopUpDataGridJsonData(RichTextBoxResponses.Text); // Show the result in a popup with JSON table view

                // jsonString is your JSON from the API
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                Root data = System.Text.Json.JsonSerializer.Deserialize<Root>(jsonResponse, options);
                // Assuming you have a DataGridView named dgvNotifications
                DataGridViewNotifications.AutoGenerateColumns = false;
                DataGridViewNotifications.Columns.Clear();

                // Add columns
                DataGridViewNotifications.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Transmissie ID",
                    DataPropertyName = "TransmissionId",
                    // Width = 280
                });
                DataGridViewNotifications.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Document ID",
                    DataPropertyName = "DocumentId",
                    // Width = 120
                });
                DataGridViewNotifications.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Verzender",
                    DataPropertyName = "sender",
                    // Width = 150
                });

                DataGridViewNotifications.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "MeldDatum",
                    DataPropertyName = "notificationDate",
                    Width = 150
                });

                DataGridViewNotifications.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Status Document",
                    DataPropertyName = "documentStatus",
                    Width = 150
                });
                // Bind
                DataGridViewNotifications.DataSource = data.Notifications;
                DataGridViewNotifications.Refresh();
                DataGridViewNotifications.Visible = true;
            }
            else
            {
                ToolStripStatusLabel.Text = "Mislukt ophalen van meldingen.";
                RichTextBoxResponses.Text = "";
            }
        }

        private void CheckBoxEventType_CheckedChanged(object sender, EventArgs e)
        {
            TbNotificationEventType.Enabled = CheckBoxEventType.Checked;
            if (!CheckBoxEventType.Checked)
            {
                TbNotificationEventType.Text = "";
            }
            else
            {
                string eventType = CheckBoxReceiver.Checked ? "DOCUMENT_RECEIVED" : "DOCUMENT_SENT";
                TbNotificationEventType.Text = eventType;
                TbNotificationEventType.Focus();
            }
        }

        private void CheckBoxPeppolDocumentType_CheckedChanged(object sender, EventArgs e)
        {
            TbNotificationPeppolDocumentType.Enabled = CheckBoxPeppolDocumentType.Checked;
            if (!CheckBoxPeppolDocumentType.Checked)
            {
                TbNotificationPeppolDocumentType.Text = "";
            }
            else
            {
                TbNotificationPeppolDocumentType.Text = "INVOICE"; // Default peppol document type for invoices
                TbNotificationPeppolDocumentType.Focus();
            }
        }

        private void CheckBoxDocumentId_CheckedChanged(object sender, EventArgs e)
        {
            TbNotificationDocumentId.Enabled = CheckBoxDocumentId.Checked;
            if (!CheckBoxDocumentId.Checked)
            {
                TbNotificationDocumentId.Text = "";
            }
            else
            {
                TbNotificationDocumentId.Text = ""; // No default, user must enter document ID
                TbNotificationDocumentId.Focus();
            }
        }

        private void CheckBoxTransmissionId_CheckedChanged(object sender, EventArgs e)
        {
            TbNotificationTransId.Enabled = CheckBoxTransmissionId.Checked;
            if (!CheckBoxTransmissionId.Checked)
            {
                TbNotificationTransId.Text = "";
            }
            else
            {
                TbNotificationTransId.Text = ""; // No default, user must enter transmission ID
                TbNotificationTransId.Focus();

            }
        }

        private void TextBoxDocToReceiveTransId_TextChanged(object sender, EventArgs e)
        {
            ButtonGetUBLDocument.Enabled = TextBoxDocumentTransmissionId.Text.Length > 0;
        }

        private void TextBoxToAcceptOrRejectTransId_TextChanged(object sender, EventArgs e)
        {
            // e91c2e8396f411f08fb302bb4e4747f9
            ButtonInvoiceReceivedResponse.Enabled = TextBoxToAcceptOrRejectTransId.Text.Length > 0;
        }

        private void ComboBoxResponseCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCode = ((KeyValuePair<string, string>)ComboBoxResponseCode.SelectedItem).Key;
            if (selectedCode == "AP" || selectedCode == "PD")
            {
                // Accepted or Pending - disable clarification fields
                TextBoxNote.Text = "Document geaccepteerd voor betalingsverwerking.";
                TextBoxClarification.Text = "";
                TextBoxClarification.Visible = false;
                ComboBoxClarificationCode.Visible = false;
                LabelClarification.Visible = false;
                ComboBoxClarificationCode.SelectedIndex = -1;
            }
            else if (selectedCode == "RE")
            {
                // Rejected or other - enable clarification fields
                ComboBoxClarificationCode.SelectedIndex = 0; // Default to first clarification code
                TextBoxNote.Text = "Document geweigerd.";
                TextBoxClarification.Text = "";
                TextBoxClarification.Visible = true;
                ComboBoxClarificationCode.Visible = true;
                LabelClarification.Visible = true;
            }
            else // Other codes
            {
                TextBoxNote.Text = "";
                TextBoxClarification.Text = "";
                TextBoxClarification.Visible = true;
                ComboBoxClarificationCode.Visible = true;
                LabelClarification.Visible = true;
            }
        }
        
        private void ButtonOpenPdfMapIn_Click(object sender, EventArgs e)
        {
            ExtractPdfFromUblDocument(LabelFileIn.Text);
        }

        private void ButtonOpenXmlMapIn_Click(object sender, EventArgs e)
        {
            string xmlFile = LabelFileIn.Text;
            string xmlFilePath = Path.Combine(
                SharedGlobals.MimDataLocation,
                SharedGlobals.ActiveCompany,
                "peppol",
                "in",
                xmlFile);

            var frm = new FormPeppolViewer();

            frm.LoadInvoiceFromFile(xmlFilePath);
            frm.Show();
        }
        private void ButtonOpenPdfMapOut_Click(object sender, EventArgs e)
        {
            ExtractPdfFromUblDocument(LabelFileOut.Text);
        }

        private void ButtonCheckUBLSentValidity_Click(object sender, EventArgs e)
        {
            MessageBox.Show(CheckUBLVersion(LabelFileOut.Text), "UBL Validity Check", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ButtonShowSentProof_Click(object sender, EventArgs e)
        {
            string documentId = ReadUBLInvoice(LabelFileOut.Text, false, false, false, false, false).ToUpper();
            string existingResult = GetSellersDocumentSentResultRS(documentId, true);
            if (existingResult != "")
            {
                RichTextBoxResponses.Text = existingResult;
                MessageBox.Show(existingResult);
            }
        }

        private void LabelFileOut_TextChanged(object sender, EventArgs e)
        {
            if (LabelFileOut.Text.Length > 0
                && File.Exists(LabelFileOut.Text)
                && Path.GetExtension(LabelFileOut.Text).ToLower() == ".xml")
            {
                ButtonOpenPdfMapOut.Visible = true;
                ButtonShowSentProof.Visible = true;
                ButtonCheckUBLSentValidity.Visible = true;
            }
            else
            {
                ButtonOpenPdfMapOut.Visible = false;
                ButtonShowSentProof.Visible = false;
                ButtonCheckUBLSentValidity.Visible = false;
            }
        }

        private void LabelFileIn_TextChanged(object sender, EventArgs e)
        {
            if (LabelFileIn.Text.Length > 0
                && File.Exists(LabelFileIn.Text)
                && Path.GetExtension(LabelFileIn.Text).ToLower() == ".xml")
            {
                ButtonOpenPdfMapIn.Visible = true;
                ButtonOpenXmlMapIn.Visible = true;
            }
            else
            {
                ButtonOpenPdfMapIn.Visible = false;
                ButtonOpenXmlMapIn.Visible = false;
            }
        }

        private void ButtonReporting_Click(object sender, EventArgs e)
        {
            string confirmMessage =
               "Voor het actieve bedrijf de documenten voor aangeduide week verzenden.\n\n" + "Dit kan enige tijd in beslag nemen.";
            var confirmResult = MessageBox.Show(confirmMessage, "Bevestig bijwerken", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult != DialogResult.Yes)
            {
                return; // User chose No, exit the method
            }

            string result =
                PeppolMailSender.SendWeeklyPeppolMail(
                    smtpServer: MyApiSecrets.smtpServer,
                    smtpPort: MyApiSecrets.smtpPort,
                    enableSsl: MyApiSecrets.smtpUseSsl,
                    smtpUser: MyApiSecrets.smtpUsername,
                    smtpPassword: MyApiSecrets.smtpPassword,
                    fromEmail: MyApiSecrets.marSyncAdminEmail,
                    fromName: MyApiSecrets.marSyncAdminName,
                    customerFileNumber: SharedGlobals.AccountingReference,
                    accountantEmail: SharedGlobals.AccountingContactEmailAddress,
                    accountantName: SharedGlobals.AccountingContactPerson,
                    selectedYear: ComboBoxYear.SelectedItem.ToString(),
                    selectedWeek: ComboBoxWeeks.SelectedItem.ToString(),
                    log: Console.WriteLine
                );

            string messageUI = "Het rapport voor week " + ComboBoxWeeks.SelectedItem.ToString() + "\n\nvan het jaar " + ComboBoxYear.SelectedItem.ToString() + " is verzonden.";
            MessageBox.Show(messageUI, "Wekelijks Peppol Rapport", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // After message reset selections
            ComboBoxYear.SelectedIndex = -1;
            ComboBoxWeeks.Items.Clear();
            ButtonReporting.Enabled = false;
        }

        private void ComboBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxWeeks.Items.Clear();
            if (ComboBoxYear.SelectedItem == null)
                return;

            string selectedYear = ComboBoxYear.SelectedItem.ToString();
            string basePath =
                SharedGlobals.MarntCLoudArchiveLocation + "\\" + SharedGlobals.ActiveCompany +
                "\\peppol\\archive\\" + selectedYear;

            if (!Directory.Exists(basePath))
                return;

            var weekItems = new List<string>();

            // Get only WeekXX folders that actually contain XML invoices
            foreach (string weekFolder in Directory.GetDirectories(basePath, "Week*"))
            {
                if (Directory.GetFiles(weekFolder, "*.xml").Length > 0)
                {
                    string weekFolderName = Path.GetFileName(weekFolder); // e.g. "Week01"
                    string itemText = weekFolderName + " (" +
                                      Directory.GetFiles(weekFolder, "*.xml").Length.ToString() + ")";
                    weekItems.Add(itemText);
                }
            }

            // Sort descending by week number extracted from "WeekNN (...)".
            weekItems.Sort((a, b) =>
            {
                int weekA = ExtractWeekNumber(a);
                int weekB = ExtractWeekNumber(b);
                return weekB.CompareTo(weekA); // descending
            });

            ComboBoxWeeks.Items.AddRange(weekItems.ToArray());

            if (ComboBoxWeeks.Items.Count > 0)
            {
                ComboBoxWeeks.SelectedIndex = 0;
                ComboBoxWeeks.Enabled = true;
            }
            else
            {
                ComboBoxWeeks.Enabled = false;
            }
        }

        private void ComboBoxWeeks_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Enable Reporting button only if both year and week are selected
            ButtonReporting.Enabled = ComboBoxYear.SelectedItem != null && ComboBoxWeeks.SelectedItem != null;
        }

        public static int[] GetNotificationIds(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return new int[0];
            }

            var ids = new List<int>();
            try
            {
                var jObj = JObject.Parse(json);
                var notifications = jObj["notifications"];
                if (notifications != null && notifications.Type == JTokenType.Array)
                {
                    foreach (var notification in notifications)
                    {
                        var idToken = notification["notificationId"];
                        if (idToken != null && idToken.Type == JTokenType.Integer)
                        {
                            ids.Add(idToken.Value<int>());
                        }
                    }
                }
            }
            catch (JsonReaderException)
            {
                // Return empty array if JSON parsing fails
                return new int[0];
            }
            return ids.ToArray();
        }

        private string GetDocumentStatus(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
            {
                return "";
            }

            var relevantStatuses = new HashSet<string>
            {
                "BUYER_ACKNOWLEDGE",
                "IN_PROCESS",
                "UNDER_QUERY",
                "CONDITIONALLY_ACCEPTED",
                "REJECTED",
                "ACCEPTED",
                "PARTIALLY_PAID",
                "FULLY_PAID"
            };

            try
            {
                var jObj = JObject.Parse(jsonString);
                var notifications = jObj["notifications"];
                if (notifications == null)
                    return "";

                foreach (var notification in notifications)
                {
                    var status = notification["documentStatus"]?.ToString();
                    if (status != null && relevantStatuses.Contains(status))
                    {
                        // howto return one of the relevantstatuses?
                        return status;

                    }
                }
            }
            catch (JsonReaderException)
            {
                // Return empty string if JSON parsing fails
                return "";
            }
            return "";
        }

        public struct NotificationInfo
        {
            public string TransmissionId;
            public string DocumentId;
        }

        public static NotificationInfo[] GetTransmissionAndDocumentIds(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
                return new NotificationInfo[0];

            var result = new List<NotificationInfo>();
            var jObj = JObject.Parse(jsonString);
            var notifications = jObj["notifications"];
            if (notifications != null && notifications.Type == JTokenType.Array)
            {
                foreach (var notification in notifications)
                {
                    var transmissionId = notification["transmissionId"]?.ToString();
                    var documentId = notification["documentId"]?.ToString();
                    if (!string.IsNullOrEmpty(transmissionId) && !string.IsNullOrEmpty(documentId))
                    {
                        result.Add(new NotificationInfo
                        {
                            TransmissionId = transmissionId,
                            DocumentId = documentId
                        });
                    }
                }
            }
            return result.ToArray();
        }

        bool IsDocumentSent(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return false;
            }

            try
            {
                var jObj = JObject.Parse(jsonString);
                if (!(jObj["notifications"] is JArray notifications))
                {
                    return false;
                }

                foreach (var notification in notifications)
                {
                    var status = notification["documentStatus"]?.ToString();
                    if (string.Equals(status, "SENT", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine("Error parsing JSON in IsDocumentSent: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error in IsDocumentSent: " + ex.Message);
                return false;
            }
        }
        
        private void OpenFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = folderPath,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
            else
            {
                MessageBox.Show("The specified folder does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            ToolStripStatusLabel.Text = "Lijsten worden vernieuwd...";
            await RefreshMonitor(true); // Call with 'true' to perform full refresh with actions
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            _countdownSeconds--;
            UpdateCountdownStatus();

            if (_countdownSeconds <= 0)
            {
                _countdownTimer.Stop();
                _timer.Stop();
                ToolStripStatusLabel.Text = "De tijd is om!";
                totalInMapInToBook = 0;
                RefreshMonitor(true); // Refresh second time when countdown reaches zero

                // Re-enable the Buttons so the user can start the cycle again
                ButtonTimer.Enabled = true;
                ButtonMyStatusReport.Enabled = true;

                ButtonUpdateBEPartiesSupported.Enabled = true;
                ComboBoxBuyerOrSeller.Enabled = true;
                ButtonClose.Enabled = true;

                ButtonTimer.BackColor = System.Drawing.Color.Azure;
                ButtonMyStatusReport.BackColor = System.Drawing.Color.WhiteSmoke;

                // if (CheckBoxCloseOnFinish.Checked)
                //     Close();
            }
        }

        private void UpdateCountdownStatus()
        {
            ToolStripStatusLabel.Text = $"Overgebleven tijd: {FormatSeconds(_countdownSeconds)}";
        }

        private string FormatSeconds(int seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return string.Format("{0:D2}:{1:D2}", (int)time.TotalMinutes, time.Seconds);
        }

        private static string CheckUBLVersion(string filePath)
        {
            var xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(filePath);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            var ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            ns.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");

            // Detect UBL Version
            // var ublVersionNode = xmlDoc.SelectSingleNode("//cbc:UBLVersionID", ns);

            string stringToReturn;
            string listID;

            string customizationBE_UBL = "urn:cen.eu:en16931:2017#conformant#urn:UBL.BE:1.0.0.20180214";
            string customizationEU_UBL = "urn:cen.eu:en16931:2017#compliant#urn:fdc:peppol.eu:2017:poacc:billing:3.0";

            // CustomizationID
            var customizationIdNode = xmlDoc.SelectSingleNode("//cbc:CustomizationID", ns);
            string customizationId = customizationIdNode?.InnerText ?? "not found";

            // InvoiceTypeCode
            var invTypeNode = xmlDoc.SelectSingleNode("//cbc:InvoiceTypeCode", ns);
            if (invTypeNode != null)
            {
                listID = invTypeNode.InnerText;
            }
            else
            {
                listID = "not found";
            }

            if (customizationId == customizationEU_UBL && listID == "380")
            {
                stringToReturn = "Peppol BIS Billing UBL Invoice V3";
            }
            else if (customizationId == customizationEU_UBL && listID == "381")
            {
                stringToReturn = "Peppol BIS Billing UBL Credit Note V3";
            }
            else if (customizationId == customizationBE_UBL && listID == "380")
            {
                stringToReturn = "UBL.BE Invoice 3.0 - Belgium";
            }
            else if (customizationId == customizationBE_UBL && listID == "381")
            {
                stringToReturn = "UBL.BE Credit Note 3.0 - Belgium";
            }
            else
            {
                stringToReturn = "OTHER UBL DOC" + "|" + listID;
            }
            return stringToReturn;
        }
        
        private void ListBoxDocumentsIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListBoxDocumentsIn.Items.Count != 0)
            {
                if (ListBoxDocumentsIn.SelectedItem != null)
                {
                    LabelFileIn.Text = ListBoxDocumentsIn.SelectedItem.ToString().ToUpper();

                    // TODO: check if already booked in the system
                    // check

                    if (SharedGlobals.NotificationsReceiverALL == "")
                    {

                    }
                    else
                    {
                        string documentRef = MarHelpers.ReadUBLInvoice(LabelFileIn.Text, false, true, false, false, false);
                        string manyRef = MarHelpers.ReadUBLInvoice(LabelFileIn.Text, false, false, false, true, false);
                        string existingResult = GetBuyersBookStatusRS(manyRef);
                        LabelBuyersDocStatus.Text = existingResult;
                        string notificationResult = FilterJson(documentRef, false, "DOCUMENT_RECEIVED");
                        RichTextBoxResponses.Text = notificationResult;
                        var pairs = GetTransmissionAndDocumentIds(notificationResult);
                        if (pairs.Length == 1)
                        {
                            TextBoxToAcceptOrRejectTransId.Text = pairs[0].TransmissionId;
                        }
                        else
                        {
                            // MessageBox.Show("TransmissieId is reeds verwijderd");
                        }
                    }                    
                }
            }
        }
        private void ListBoxDocumentsOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ButtonSendUblDocument.Enabled = false; // Disable the button when selecting a new file
            LabelFileOut.Text = "";

            if (ListBoxDocumentsOut.Items.Count != 0)
            {
                if (ListBoxDocumentsOut.SelectedItem != null)
                {
                    LabelFileOut.Text = ListBoxDocumentsOut.SelectedItem.ToString().ToUpper();
                    string checkResult = ReadUBLInvoice(LabelFileOut.Text, false, false, false, false, false);
                    string existingResult = GetSellersDocumentSentResultRS(checkResult.ToUpper(), false);

                    if (existingResult == "")
                    {
                        ToolStripStatusLabel.Text = checkResult + " verkoopdocument nog te verzenden.";
                        // ButtonSendUblDocument.Enabled = true; // Enable the button if the file is valid and not yet sent                                        
                    }
                    else
                    {
                        // Check V405 field in marnt.mdv table for this document
                        // Check if the document result state in marnt.mdv table conforms that it was really sent
                        // If already sent but stated as error, refresh the database if needed
                        // With error, show the error message from the response body in the database

                        // notificationResult = await InvoiceNotificationState(checkResult); // Wait for the async task to complete

                        // ButtonSendUblDocument.Enabled = false; // Disable the button if the file was already sent                    
                        RichTextBoxResponses.Text = existingResult;

                        // MessageBox.Show("Verzendbewijs aanwezig in boekhouding:\n\n" + existingResult, checkResult + " reeds verzonden");
                        ToolStripStatusLabel.Text = checkResult + " verkoopdocument is reeds verzonden.";
                    }
                }
            }
        }

        async private Task RefreshCyclePeppolSent()
        {
            int localTotalOutSentConfirmed = 0;
            int localTotalInMapOutAcceptedOrRejected = 0;

            // Create a snapshot of the items to avoid modification during enumeration
            var itemsSnapshot = ListBoxDocumentsOut.Items.Cast<object>().ToList();

            foreach (var item in itemsSnapshot)
            {
                string documentId = MarHelpers.ReadUBLInvoice(item.ToString(), false, false, false, false, false).ToUpper();
                string existingResult = GetSellersDocumentSentResultRS(documentId, false);

                if (existingResult != "")
                {
                    // Check the notifications API for the SENT status of this document                    
                    string virtualResultSent = FilterJson(documentId, true, "DOCUMENT_SENT");

                    bool isSent = IsDocumentSent(virtualResultSent);

                    if (isSent == true)
                    {
                        bool updatedRS = SetDocumentResultRS(documentId, virtualResultSent, 1);
                        if (updatedRS)
                        {
                            localTotalOutSentConfirmed++;
                        }
                    }

                    string virtualResponses = FilterJson(documentId, false, "INVOICE_RESPONSE_RECEIVED");
                    if (virtualResponses != "")
                    {
                        bool updatedRS = SetDocumentResultRS(documentId, virtualResponses, 2);
                        if (updatedRS)
                        {
                            localTotalInMapOutAcceptedOrRejected++;
                        }
                    }

                    // check if paid already
                    bool isPaid = GetSellersPaidResultRS(documentId);
                    if (isPaid)
                    {
                        // Accepted and Paid

                        // 1) Delete all INVOICE_RESPONSE_RECEIVED notifications for this document
                        string virtualResponses2 = FilterJson(documentId, false, "INVOICE_RESPONSE_RECEIVED");
                        int[] notificationIdsStep1 = GetNotificationIds(virtualResponses2);

                        foreach (int id in notificationIdsStep1)
                        {
                            var result = await AdemicoClient.DeleteNotificationAsync(id).ConfigureAwait(true);
                            if (result.StatusCode != HttpStatusCode.OK)
                            {
                                MessageBox.Show("Something went wrong"); // Handle error, result.ResponseBody may contain error details
                            }
                        }

                        // 2) Then delete the DOCUMENT_SENT notification(s) (normally only one)
                        string notificationResultSent2 = FilterJson(documentId, true, "DOCUMENT_SENT");
                        int[] notificationIdsSent = GetNotificationIds(notificationResultSent2);

                        foreach (int id in notificationIdsSent)
                        {
                            var result = await AdemicoClient.DeleteNotificationAsync(id).ConfigureAwait(true);
                            if (result.StatusCode != HttpStatusCode.OK)
                            {
                                MessageBox.Show("Something went wrong"); // Handle error, result.ResponseBody may contain error details
                            }
                        }

                        titanicDeckChairs++;

                        // 3) Only after all deletes are awaited, move to archive folder
                        string resultMove =
                            MarDataFilesHelper.MovePeppolFileToCloudDrive(
                                item.ToString(),
                                SharedGlobals.MarntCLoudArchiveLocation + "\\" + SharedGlobals.ActiveCompany + "\\peppol\\out");
                        Console.WriteLine(resultMove);
                    }
                }
                else
                {
                    // send document
                    ToolStripStatusLabel.Text = "Bezig met verzenden verkoopdocument...";

                    string filePath = item.ToString(); // LabelFileOut.Text.Trim();
                    string activeSellerDocument = MarHelpers.ReadUBLInvoice(filePath, false, false, false, false, false).ToUpper();
                    
                    try
                    {
                        var result = await AdemicoClient.PeppolInvoiceSender.SendUblInvoiceAsync(
                            filePath: filePath,
                            xC5Reporting: false // or true if needed for Singapore reporting
                        );

                        ToolStripStatusLabel.Text = $"Status: {(int)result.StatusCode} {result.StatusCode}";

                        // result.statusCode should be "0K" if successful                        
                        int sentStatuscode = (int)result.StatusCode;
                        if (sentStatuscode == 400) // Bad Request
                        {
                            badRequestsOccurred = true;
                            // else such as result.statusCode "BadRequest" send document to Vsoft!

                            string sendBadRequest =
                                PeppolMailSender.SendBadRequestPeppolMail(
                                    smtpServer: MyApiSecrets.smtpServer,
                                    smtpPort: MyApiSecrets.smtpPort,
                                    enableSsl: MyApiSecrets.smtpUseSsl,
                                    smtpUser: MyApiSecrets.smtpUsername,
                                    smtpPassword: MyApiSecrets.smtpPassword,
                                    fromEmail: SharedGlobals.CompanyEmailAddress,
                                    fromName: SharedGlobals.CompanyName,
                                    vsoftAdminEmail: MyApiSecrets.marSyncAdminEmail,
                                    vsoftAdminName: MyApiSecrets.marSyncAdminName,
                                    selectedPeppolFile: filePath,
                                    responseBody: result.ResponseBody.ToString(),
                                    apiModus: SharedGlobals.ApiModus,
                                    log: Console.WriteLine
                                );

                            // string messageUI = "Een mail Bad Request werd verzonden naar Vsoft voor:\n\n" + filePath;
                            // MessageBox.Show(messageUI, "Melding Bad Request", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ToolStripStatusLabel.Text = "Een mail Bad Request werd verzonden naar Admin voor:\n\n" + filePath;
                        }

                        if (!string.IsNullOrEmpty(result.ResponseBody)) // Ensure ResponseBody is not null or empty
                        {
                            var deserializedString = JsonConvert.DeserializeObject(value: result.ResponseBody);
                            RichTextBoxResponses.Text = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);

                            bool updatedRS = SetDocumentResultRS(activeSellerDocument, RichTextBoxResponses.Text, 0);

                            totalInMapOutSent++;
                            titanicDeckChairs++;

                            string resultWeekFolderCopy = CopyPeppolFileToWeekFolderOnCloudDrive(
                            isSeller: true,
                            sourceFolderPathAndFile: filePath,
                            cloudFolderPath: SharedGlobals.MarntCLoudArchiveLocation + "\\" + SharedGlobals.ActiveCompany + "\\peppol\\archive");
                        }
                        else
                        {
                            ToolStripStatusLabel.Text = "Geen response body.";
                        }
                    }
                    catch (Exception ex)
                    {
                        ToolStripStatusLabel.Text = $"Fout: {ex.Message}";
                        // MessageBox.Show($"Fout: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }                    
                }
            }
            totalInMapOutSentConfirmed = localTotalOutSentConfirmed;
            totalInMapOutAcceptedOrRejected = localTotalInMapOutAcceptedOrRejected;
            // Show documents that were successfully sent to Peppol (have transmissionId)
            LabelTotalInMapOutSentConfirmed.Text = totalInMapOutSent.ToString();
            await RefreshListPeppolSend();
        }

        async private Task RefreshCyclePeppolReceive()
        {
            totalInMapInToBook = 0;
            string folderInPath = SharedGlobals.MimDataLocation + "\\" + SharedGlobals.ActiveCompany + "\\peppol\\in";

            // Check the notifications API for the RECEIVED status of documents            
            string virtualNotificationResult = FilterJson("", false, "DOCUMENT_RECEIVED");
            var pairs = GetTransmissionAndDocumentIds(virtualNotificationResult);
            foreach (var info in pairs)
            {
                // For each TransmissionId and DocumentId pair, search if DocumentId
                // exists in the ListBoxDocumentsPeppolIn.Items (the files in the "in" folder)
                string sanitizedDocumentId = SanitizeFileName(info.DocumentId);
                string filePath = folderInPath + "\\" + sanitizedDocumentId + ".xml";
                if (!File.Exists(filePath))
                {
                    // If not found, call the API to retrieve the UBL invoice XML using the TransmissionId
                    // and save it in the "in" folder with the DocumentId as file name
                    // Note: DocumentId may contain characters that are not allowed in file names, so we sanitize it                    
                    string myDocumentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    string ademicoUrl = SharedGlobals.AdemicoApiUrl;
                    string accessToken = SharedGlobals.AdemicoAccessToken;
                    string username = SharedGlobals.AdemicoUsername;
                    string password = SharedGlobals.AdemicoPassword;

                    string transmissionId = info.TransmissionId;
                    string requestUrl = $"{ademicoUrl}/api/peppol/v1/invoices/{transmissionId}/ubl?accessToken={accessToken}";

                    try
                    {
                        HttpClient client = new HttpClient();
                        var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                        client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                        // Accept XML
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                        // Make the GET request
                        HttpResponseMessage response = await client.GetAsync(requestUrl);
                        response.EnsureSuccessStatusCode();
                        
                        // Read the content
                        string invoiceXml = await response.Content.ReadAsStringAsync();

                        // Parse documentId directly from the XML string (no temp file needed)
                        string tempFile = Path.GetTempFileName();
                        File.WriteAllText(tempFile, invoiceXml);
                        string documentId = MarHelpers.ReadUBLInvoice(tempFile, false, true, false, false, false).ToUpper();
                        File.Delete(tempFile);

                        string sanitizedDocumentIdForSaving = SanitizeFileName(documentId);

                        // Save directly to peppol\in
                        string destinationFolder = Path.Combine(
                            SharedGlobals.MimDataLocation,
                            SharedGlobals.ActiveCompany,
                            "peppol",
                            "in");

                        Directory.CreateDirectory(destinationFolder);

                        string destinationPath = Path.Combine(destinationFolder, sanitizedDocumentIdForSaving + ".xml");
                        File.WriteAllText(destinationPath, invoiceXml);

                        // Checking document hash and registering in the database to detect manual changes
                        // After destinationPath is determined and File.WriteAllText(destinationPath, invoiceXml) is done:
                        var repo = new PeppolDocumentRepository(SharedGlobals.MimDataLocation + SharedGlobals.MarntMdvLocation); // e.g., path to your Access‑MDB
                        // 1) First try: does the document already exist with the same hash?
                        repo.UpdateReceivedOnIfHashMatches(documentId, destinationPath);
                        // 2) If no existing hash or mismatch, simply (re)register
                        repo.RegisterIncomingDocument(documentId, destinationPath);

                        ToolStripStatusLabel.Text = "UBL Invoice " + sanitizedDocumentIdForSaving + ".XML succesvol opgehaald en opgeslagen.";
                        Console.WriteLine("UBL Invoice succesvol opgehaald en opgeslagen op: " + destinationPath);
                        titanicDeckChairs++;
                        totalInMapInToBook++;

                        string resultWeekFolderCopy = CopyPeppolFileToWeekFolderOnCloudDrive(
                        isSeller: false,
                        sourceFolderPathAndFile: destinationPath,
                        cloudFolderPath: SharedGlobals.MarntCLoudArchiveLocation + "\\" + SharedGlobals.ActiveCompany + "\\peppol\\archive");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        ToolStripStatusLabel.Text = "Klaar";
                    }
                }
                else
                {
                    // TODO: edit/cleanup the messages below to the user, currently it is more for debugging and can be improved for better user experience
                    // If found, check if the document is already marked as paid in the system (based on the DocumentId)
                    // If paid, delete the DOCUMENT_RECEIVED notification(s) for this document and move the XML file to the archive folder
                    // If not paid, keep the file in the "in" folder for further processing and mark it in the UI as "to be booked"
                    // Also check if already accepted or rejected and mark in the UI if so, this can be used to prioritize booking of accepted documents
                    // Note: the document may be marked as paid in the system but not yet have the paid status in the notifications API, so we check the paid status based on the DocumentId in the system (e.g. via GetBuyersBookStatusRS) instead of relying solely on the notifications API status
                    // Also if paid but still notifications exist, we can delete the notifications and move the file to archive, this can be a sign that the notifications were not yet processed in the system when the document was marked as paid, so we do not rely on the absence of notifications but really on the paid status in the system based on DocumentId
                    // For the marking in the UI, we can use different colors or icons in the ListBoxDocumentsIn to indicate the status of each document (e.g. red for rejected, green for accepted, yellow for to be booked, etc.)
                    // This logic can be implemented in the following way:
                    //  1) Check the paid status in the system based on DocumentId (e.g. via GetBuyersBookStatusRS)
                    //  2) If paid, delete the DOCUMENT_RECEIVED notification(s) for this document and move the XML file to the archive folder
                    //  3) If not paid, check the notifications API for accepted or rejected status and mark in the UI accordingly, and keep the file in the "in" folder for further processing
                    //  4) If not paid and no accepted or rejected status, mark in the UI as "to be booked" and keep the file in the "in" folder for further processing
                    // Note: the checking of accepted or rejected status in the notifications API can be done by filtering the notifications for this document and looking for specific statuses such as "ACCEPTED" or "REJECTED", and the marking in the UI can be done by changing the background color of the item in the ListBoxDocumentsIn based on the status (e.g. green for accepted, red for rejected, yellow for to be booked, etc.)
                    // Also note that the logic for checking the paid status in the system based on DocumentId can be implemented in the GetBuyersBookStatusRS method, which can return not only the paid status but also the accepted or rejected status, so that we can minimize the number of calls to the notifications API and rely more on the system's own status based on DocumentId
                    // For the documents that are marked as "to be booked", we can also implement a reminder mechanism in the UI to follow up on these documents after a certain period of time, to ensure that they are booked in a timely manner and do not get forgotten in the "in" folder
                    // Finally, for the documents that are moved to the archive folder, we can also implement a cleanup mechanism to delete the archived files after a certain period of time, to avoid cluttering the archive folder with old files and to ensure that we only keep the relevant files for a certain retention period based on our business needs and compliance requirements
                    // Check the paid status in the system based on DocumentId
                    // Note: we check the paid status based on DocumentId in the system (e.g. via GetBuyersBookStatusRS) instead of relying solely on the notifications API status, because the document may be marked as paid in the system but not yet have the paid status in the notifications API, so we want to rely on the system's own status based on DocumentId for this logic
                    // Also if paid but still notifications exist, we can delete the notifications and move the file to archive, this can be a sign that the notifications were not yet processed in the system when the document was marked as paid, so we do not rely on the absence of notifications but really on the paid status in the system based on DocumentId
                    // Check accepted or rejected status in the notifications API and mark in the UI if not paid, this can help to prioritize booking of accepted documents and also to easily identify rejected documents that may need follow-up with the supplier
                    // For the marking in the UI, we can use different colors or icons in the ListBoxDocumentsIn to indicate the status of each document (e.g. red for rejected, green for accepted, yellow for to be booked, etc.)
                    // This logic can be implemented in the following way:
                    // 1) Check the paid status in the system based on DocumentId (e.g. via GetBuyersBookStatusRS)
                    // 2) If paid, delete the DOCUMENT_RECEIVED notification(s) for this document and move the XML file to the archive folder
                    // 3) If not paid, check the notifications API for accepted or rejected status and mark in the UI accordingly, and keep the file in the "in" folder for further processing
                    // 4) If not paid and no accepted or rejected status, mark in the UI as "to be booked" and keep the file in the "in" folder for further processing

                    string documentRef = MarHelpers.ReadUBLInvoice(filePath, false, true, false, false, false);
                    string arrayRef = MarHelpers.ReadUBLInvoice(filePath, false, false, false, true, false);
                    string balansOfPayments = GetBuyersBookStatusRS(arrayRef);

                    if (balansOfPayments == "Volledig Betaald")
                    {
                        string reactions = FilterJson(documentRef, true, "INVOICE_RESPONSE_SENT");
                        int[] notificationIdsStep1 = GetNotificationIds(reactions);

                        // 1) Delete all INVOICE_RESPONSE_SENT notifications for this document
                        foreach (int id in notificationIdsStep1)
                        {
                            var result = await AdemicoClient.DeleteNotificationAsync(id).ConfigureAwait(true);
                            if (result.StatusCode != HttpStatusCode.OK)
                            {
                                MessageBox.Show("Er is iets misgegaan"); // Handle error, result.ResponseBody may contain error details
                            }                            
                        }

                        // 2) Then delete all DOCUMENT_RECEIVED notifications for this document
                        string resultReceived = FilterJson(documentRef, false, "DOCUMENT_RECEIVED");
                        int[] notificationIdsStep2 = GetNotificationIds(resultReceived);

                        foreach (int id in notificationIdsStep2)
                        {
                            var result = await AdemicoClient.DeleteNotificationAsync(id).ConfigureAwait(true);
                            if (result.StatusCode != HttpStatusCode.OK)
                            {
                                MessageBox.Show("Er is iets misgegaan"); // Handle error, result.ResponseBody may contain error details
                            }
                        }

                        // ToolStripStatusLabel.Text = balansOfPayments;
                        titanicDeckChairs++;

                        // 3) Only after all deletes are awaited, move the XML file to archive
                        string resultMove =
                            MarDataFilesHelper.MovePeppolFileToCloudDrive(
                                filePath,
                                SharedGlobals.MarntCLoudArchiveLocation + "\\" + SharedGlobals.ActiveCompany + "\\peppol\\in");
                        Console.WriteLine(resultMove);
                    }
                    else
                    {
                        string manyRef = MarHelpers.ReadUBLInvoice(filePath, false, false, false, true, false);

                        // Al ontvangen, controleer dan of het is geboekt  
                        string virtualResultJson = FilterJson(documentRef, false, "DOCUMENT_RECEIVED");
                        bool booked = SetBuyersBookStatusRS(manyRef, virtualResultJson, 0);
                        if (!booked)
                        {
                            totalInMapInToBook++;
                        }
                        // ToolStripStatusLabel.Text = "Document " + documentRef + " bewaard in database.";

                        string virtualReactJson = FilterJson(documentRef, true, "INVOICE_RESPONSE_SENT");

                        // If reactions exist, save them in the database
                        if (virtualReactJson != "")
                        {
                            bool saved = SetBuyersBookStatusRS(manyRef, virtualReactJson, 1);
                            if (saved)
                            {
                                ToolStripStatusLabel.Text = "Document " + documentRef + " reacties bewaard in database.";
                            }
                        }
                        else
                        {
                            // ToolStripStatusLabel.Text = "Document " + documentRef + " geen reacties om te bewaren.";
                        }
                    }
                }
            }
            await RefreshListPeppolReceive();

            // Create a snapshot of the items to avoid modification during enumeration
            var itemsSnapshot = ListBoxDocumentsIn.Items.Cast<object>().ToList();
            foreach (var item in itemsSnapshot)
            {
                // Controleer of de melding DOCUMENT_RECEIVED bestaat voor dit document
                string cleanUpDocument = MarHelpers.ReadUBLInvoice(item.ToString(), false, true, false, false, false);
                string resultReceived = FilterJson(cleanUpDocument, false, "DOCUMENT_RECEIVED");
                int[] notificationFound = GetNotificationIds(resultReceived);

                if (notificationFound.Length == 0)
                {
                    // Als we geen documentmelding hebben gevonden, kunnen we ze verwijderen
                    string arrayRef = MarHelpers.ReadUBLInvoice(item.ToString(), false, false, false, true, false);
                    string balansOfPayments = GetBuyersBookStatusRS(arrayRef);

                    if (balansOfPayments == "Volledig Betaald")
                    {                        
                        // 0. Betaald en meldingen moeten ook worden verwijderd, dus nu zeker naar archief verplaatsen
                        string resultMove =
                            MarDataFilesHelper.MovePeppolFileToCloudDrive(
                                item.ToString(),
                                SharedGlobals.MarntCLoudArchiveLocation + "\\" + SharedGlobals.ActiveCompany + "\\peppol\\in");
                        Console.WriteLine("Message from snapshot PeppolReceive\nOk, Should be removed: " + resultMove);

                        // Remove also response notifications if still any
                        string reactions = FilterJson(cleanUpDocument, true, "INVOICE_RESPONSE_SENT");
                        if (reactions == "")
                        {
                            // No reactions to delete
                            ToolStripStatusLabel.Text = "Geen reacties meer om te verwijderen voor document " + cleanUpDocument;
                            Console.WriteLine("Message from snapshot PeppolReceive\nOk, No reactions to delete for document: " + cleanUpDocument);
                        }
                        else
                        {
                            int[] notificationIdsStep1 = GetNotificationIds(reactions);

                            // 1) Delete all INVOICE_RESPONSE_SENT notifications for this document
                            foreach (int id in notificationIdsStep1)
                            {
                                var result = await AdemicoClient.DeleteNotificationAsync(id).ConfigureAwait(true);
                                if (result.StatusCode != HttpStatusCode.OK)
                                {
                                    MessageBox.Show("Something went wrong"); // Handle error, result.ResponseBody may contain error details
                                }
                                Console.WriteLine("Ok, Message from snapshot PeppolReceive\nOk, Deleted reaction notification with id: " + id + " for document: " + cleanUpDocument);
                            }
                        }                        
                    }
                }
            }
            await RefreshListPeppolReceive();
            ToolStripStatusLabel.Text = "Klaar. Druk [Esc] om te sluiten.";
        }

        async private Task RefreshListPeppolReceive()
        {
            string folderInPath = SharedGlobals.MimDataLocation + "\\" + SharedGlobals.ActiveCompany + "\\peppol\\in";
            totalInMapIn = 0;
            ListBoxDocumentsIn.Items.Clear();
            if (Directory.Exists(folderInPath))
            {
                string[] xmlFiles = Directory.GetFiles(folderInPath, "*.xml");
                ListBoxDocumentsIn.Items.Clear();
                ListBoxDocumentsIn.Items.AddRange(xmlFiles);
                LabelFileIn.Text = "";
            }
            else
            {
                // ToolStripStatusLabel.Text = "Geen ontvangen documenten voor " + SharedGlobals.ActiveCompany;
                ListBoxDocumentsIn.Visible = false;
            }
            totalInMapIn = ListBoxDocumentsIn.Items.Count;
            LabelTotalInMapIn.Text = totalInMapIn.ToString();
        }
        async private Task RefreshListPeppolSend()
        {
            totalInMapOut = 0;
            string folderOutPath = SharedGlobals.MimDataLocation + "\\" + SharedGlobals.ActiveCompany + "\\peppol\\out";
            ListBoxDocumentsOut.Items.Clear();

            if (Directory.Exists(folderOutPath))
            {
                string[] xmlFiles = Directory.GetFiles(folderOutPath, "*.xml");
                ListBoxDocumentsOut.Items.Clear();
                ListBoxDocumentsOut.Items.AddRange(xmlFiles);
                LabelFileOut.Text = "";
            }
            else
            {
                // ToolStripStatusLabel.Text = "Geen te verzenden documenten voor " + SharedGlobals.ActiveCompany;
                ListBoxDocumentsOut.Visible = false;
            }
            totalInMapOut = ListBoxDocumentsOut.Items.Count;
            LabelTotalInMapOut.Text = totalInMapOut.ToString();

            // Refresh the real number of total files to be sent
            int localTotalOutToEval = 0;
            int localTotalOutWithResults = 0;
            foreach (var item in ListBoxDocumentsOut.Items)
            {
                string documentId = ReadUBLInvoice(item.ToString(), false, false, false, false, false).ToUpper();
                string existingResult = GetSellersDocumentSentResultRS(documentId, false); // false for check SENT true for reactions?
                if (existingResult != "")
                {
                    localTotalOutWithResults++;
                    localTotalOutToEval++;
                }
            }
            totalInMapOut = ListBoxDocumentsOut.Items.Count;
            LabelTotalInMapOut.Text = totalInMapOut.ToString();
            totalInMapOutSent = localTotalOutToEval;
        }

        private static string SanitizeFileName(string fileName)
        {
            // Define invalid characters for file names
            char[] invalidChars = Path.GetInvalidFileNameChars();

            // Replace invalid characters with an underscore
            foreach (char c in invalidChars)
            {
                fileName = fileName.Replace(c.ToString(), "_");
            }

            // Optionally, trim whitespace
            return fileName.Trim();
        }
                
        private void TabControlVariousActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        public static string[] ExtractSupportedDocuments(string json)
        {
            var supportedDocuments = new List<string>();

            try
            {
                var jObject = JObject.Parse(json);

                // Navigate to the legalEntities array
                var legalEntities = jObject["legalEntities"];
                if (legalEntities != null && legalEntities.Type == JTokenType.Array)
                {
                    foreach (var legalEntity in legalEntities)
                    {
                        // Navigate to peppolRegistrations array
                        var peppolRegistrations = legalEntity["peppolRegistrations"];
                        if (peppolRegistrations != null && peppolRegistrations.Type == JTokenType.Array)
                        {
                            foreach (var registration in peppolRegistrations)
                            {
                                // Navigate to supportedDocuments array
                                var documents = registration["peppolRegistrationDetails"]?["supportedDocuments"];
                                if (documents != null && documents.Type == JTokenType.Array)
                                {
                                    foreach (var document in documents)
                                    {
                                        supportedDocuments.Add(document.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
            }

            return supportedDocuments.ToArray();
        }

        private static string BuildSupportedDocumentsReadableString(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return string.Empty;

            try
            {
                var jToken = JToken.Parse(json);
                string prettyJson = jToken.ToString(Newtonsoft.Json.Formatting.Indented);
                string[] supportedDocuments = ExtractSupportedDocuments(json);

                if (supportedDocuments == null || supportedDocuments.Length == 0)
                    return prettyJson;

                var sb = new StringBuilder(prettyJson.Length + 256);
                sb.Append(prettyJson);
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("SUPPORTED_DOCUMENTS:");
                foreach (var doc in supportedDocuments.Distinct().OrderBy(d => d, StringComparer.OrdinalIgnoreCase))
                {
                    sb.Append("- ");
                    sb.AppendLine(doc);
                }
                return sb.ToString();
            }
            catch
            {
                // If it's not valid JSON, just store the raw string.
                return json;
            }
        }
        private string MoveXmlDocumentReceivedToMarPeppolIn(string documentId)
        {
            // 1️⃣ Locate the original file in My Documents
            string documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string originalFile = Path.Combine(documentPath, "invoice.xml");

            // 2️⃣ Create a temporary renamed file path in the same folder
            string renamedFile = Path.Combine(documentPath, documentId + ".xml");

            // 3️⃣ Rename (move) the file
            if (File.Exists(originalFile))
            {
                File.Move(originalFile, renamedFile);
                // Console.WriteLine($"Renamed to: {renamedFile}");
            }
            else
            {
                Console.WriteLine("Original file not found.");
                return "";
            }

            // 4️⃣ Copy the renamed file to another location
            string destinationFolder = SharedGlobals.MimDataLocation + "\\" + SharedGlobals.ActiveCompany + "\\peppol\\in";
            // Directory.CreateDirectory(destinationFolder);    // ensure it exists
            string destinationPath = Path.Combine(destinationFolder, documentId + ".xml");

            File.Copy(renamedFile, destinationPath, overwrite: true);
            Console.WriteLine($"Copied to: {destinationPath}");
            // TODO: Remove the temporary renamed file if needed
            try
            {
                if (File.Exists(renamedFile))
                {
                    File.Delete(renamedFile);
                    // Console.WriteLine($"Deleted temporary file: {renamedFile}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting temporary file: {ex.Message}");
            }

            return destinationPath;
        }
        private async Task<string> CheckPeppolSupportedDocuments(string kbo)
        {
            string url;
            // URL for the Peppol directory search            
            url = "https://directory.peppol.eu/search/1.0/json?q=iso6523-actorid-upis:" + kbo;
            try
            {
                // Initialize HttpClient
                using (HttpClient httpClient = new HttpClient())
                {
                    // Send GET request
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    // Check if the response is successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string responseJson = await response.Content.ReadAsStringAsync();

                        RichTextBoxResponses.Text = responseJson;
                        return responseJson;
                    }
                    else
                    {
                        // Show HTTP error                        
                        RichTextBoxResponses.Text = $"HTTP Error: {response.StatusCode}";
                        return $"HTTP Error: {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                // Show exception message                
                RichTextBoxResponses.Text = $"Error: {ex.Message}";
                return $"Error: {ex.Message}";
            }
        }
        private async Task<bool> GetAllNotificationsForCompany()
        {
            string companyNumber = SharedGlobals.CompanyKBONumber.ToString();

            var jsonResponse = await AdemicoClient.GetNotificationsAsync(
                transmissionId: "",
                documentId: "",
                eventType: "",
                peppolDocumentType: "",
                sender: "",
                receiver: "",
                startDateTime: "",
                endDateTime: "",
                page: "",
                pageSize: "250"
            );

            if (jsonResponse != null)
            {
                var deserializedString = JsonConvert.DeserializeObject(jsonResponse);
                string result = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
                SharedGlobals.NotificationsSenderALL = result;
            }

            var jsonResponse2 = await AdemicoClient.GetNotificationsAsync(
                transmissionId: "",
                documentId: "",
                eventType: "",
                peppolDocumentType: "",
                sender: "",
                receiver: "0208:" + companyNumber,
                startDateTime: "",
                endDateTime: "",
                page: "",
                pageSize: "250"
            );

            if (jsonResponse2 != null)
            {
                var deserializedString = JsonConvert.DeserializeObject(jsonResponse2);
                string result = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
                SharedGlobals.NotificationsReceiverALL = result;
            }

            if (SharedGlobals.NotificationsSenderALL == "" && SharedGlobals.NotificationsReceiverALL == "")
            {
                return false;
            }
            return true;
        }

        private string FilterJson(
            string documentIdFilter,
            bool isSender,
            string eventTypeFilter)
        {
            string partyType;
            string json;
            string partyFilter;

            partyFilter = "0208:" + SharedGlobals.CompanyKBONumber;
            if (isSender)
            {
                // Filtering for sender notifications
                partyType = "sender";
                json = SharedGlobals.NotificationsSenderALL;
            }
            else
            {
                // Filtering for receiver notifications
                partyType = "receiver";
                json = SharedGlobals.NotificationsReceiverALL;
            }

            // 🔹 New: guard against empty or invalid JSON
            if (string.IsNullOrWhiteSpace(json))
            {
                return string.Empty;
            }

            JObject jObject;
            try
            {
                jObject = JObject.Parse(json);
            }
            catch (JsonReaderException)
            {
                // Log or show once if useful
                Console.WriteLine("Error parsing notifications JSON in FilterJson.");
                return string.Empty;
            }

            if (!(jObject["notifications"] is JArray notifications))
            {
                return string.Empty;
            }

            var filteredNotifications = new JArray();
            foreach (var notification in notifications)
            {
                string party = notification[partyType]?.ToString();
                string eventType = notification["eventType"]?.ToString();

                if (!string.Equals(party, partyFilter, StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(eventType, eventTypeFilter, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(documentIdFilter))
                {
                    string document = notification["documentId"]?.ToString();
                    if (string.Equals(document, documentIdFilter, StringComparison.OrdinalIgnoreCase))
                    {
                        filteredNotifications.Add(notification);
                    }
                }
                else
                {
                    filteredNotifications.Add(notification);
                }
            }

            if (filteredNotifications.Count == 0)
            {
                return string.Empty;
            }

            var result = new JObject
            {
                ["notifications"] = filteredNotifications
            };

            return result.ToString(Newtonsoft.Json.Formatting.Indented);
        }
                
        private static int ExtractWeekNumber(string itemText)
        {
            // itemText expected format: "WeekNN (..."
            // Fallback to 0 if parsing fails
            if (string.IsNullOrEmpty(itemText) || itemText.Length < 6)
                return 0;

            string numPart = itemText.Substring(4, 2); // "NN"
            return int.TryParse(numPart, out int week) ? week : 0;
        }
        
        private void ButtonCleanUpNotifications_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Deze functionaliteit is momenteel nog niet beschikbaar.");
        }

        private void LabelForReactions_Click(object sender, EventArgs e)
        {

        }
                        
        private static string SanitizePaymentRef(string paymentRef)
        {
            // Define invalid characters for payment references
            char[] invalidChars = new char[] { '+', '/' };

            // Replace invalid characters with an underscore
            foreach (char c in invalidChars)
            {
                paymentRef = paymentRef.Replace(c.ToString(), "");
            }

            // Optionally, trim whitespace
            return paymentRef.Trim();
        }

        private async void ButtonMyStatusReport_Click(object sender, EventArgs e)
        {
            ButtonMyStatusReport.Enabled = false;

            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            // Get the active company's KBO number
            string activeCompanyKBO = SharedGlobals.CompanyKBONumber;
            string activeCompanyName = SharedGlobals.CompanyName;

            if (string.IsNullOrWhiteSpace(activeCompanyKBO))
            {
                MessageBox.Show("Geen actieve onderneming geselecteerd.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Cursor.Current = Cursors.Default;
                return;
            }

            try
            {
                // Get all notifications for server account (from FormAdminPeppolActions logic)
                var jsonResponse = await AdemicoClient.GetNotificationsAsync(
                    transmissionId: "",
                    documentId: "",
                    eventType: "",
                    peppolDocumentType: "",
                    sender: "",
                    receiver: "",
                    startDateTime: "",
                    endDateTime: "",
                    page: "",
                    pageSize: "250"
                );

                if (jsonResponse == null || string.IsNullOrEmpty(jsonResponse))
                {
                    MessageBox.Show("Geen notificaties gevonden.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                var deserializedString = JsonConvert.DeserializeObject(jsonResponse);
                string allNotifications = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);

                // Generate HTML report for the active company
                string reportHtml = await GenerateCompanyStatusReport(
                    activeCompanyKBO,
                    activeCompanyName,
                    allNotifications
                );

                // Display the HTML in a browser window
                var viewer = new FormPeppolViewer
                {
                    Text = $"Status Rapport - {activeCompanyName}"
                };
                viewer.LoadHtmlContent(reportHtml);
                viewer.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij ophalen status rapport: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private async Task<string> GenerateCompanyStatusReport(string kboNumber, string companyName, string allNotifications)
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            var htmlBegin = new StringBuilder();
            var htmlReceiver = new StringBuilder();
            var htmlReactionsSent = new StringBuilder();
            var htmlSender = new StringBuilder();
            var htmlReactionsReceived = new StringBuilder();

            int totalCount = 0;
            string partyFilter = "0208:" + kboNumber;

            // First create heading with company info
            htmlBegin.Append("<!DOCTYPE html><html><head>");
            htmlBegin.Append("<meta charset='utf-8'>");
            htmlBegin.Append("<title>Status Rapport</title>");
            htmlBegin.Append("<style>");
            htmlBegin.Append("body { font-family: Arial, Helvetica, sans-serif; margin: 20px; }");
            htmlBegin.Append("h1 { color: #333; }");
            htmlBegin.Append("h3 { color: #555; margin-top: 30px; }");
            htmlBegin.Append("table { border-collapse: collapse; width: 100%; margin-top: 10px; }");
            htmlBegin.Append("th { background-color: #f0f0f0; font-weight: bold; text-align: left; padding: 8px; border-bottom: 2px solid #ccc; }");
            htmlBegin.Append("td { padding: 8px; border-bottom: 1px solid #eee; }");
            htmlBegin.Append("tr:nth-child(even) { background-color: #f9f9f9; }");
            htmlBegin.Append("tr:nth-child(odd) { background-color: #ffffff; }");
            htmlBegin.Append(".warning { font-size: 10px; color: #ff0000; margin-bottom: 4px; }");
            htmlBegin.Append(".info-list { font-size: 10px; color: #00008b; margin-top: 0; margin-bottom: 8px; padding-left: 18px; }");
            htmlBegin.Append("</style>");
            htmlBegin.Append("</head><body>");
            htmlBegin.Append($"<h1>{companyName} [{kboNumber}]</h1>");
            htmlBegin.Append($"<p><strong>Rapport Datum:</strong> {DateTime.Now:dd/MM/yyyy HH:mm}</p>");

            // 1. CUSTOMER RECEIVED DOCUMENTS
            string filteredJsonReceivedDocuments = FilterNotifications(
                json: allNotifications,
                isSender: false,
                "DOCUMENT_RECEIVED",
                partyFilter: partyFilter
            );

            var jObjDocumentsReceived = JObject.Parse(filteredJsonReceivedDocuments);
            var notificationsDocumentsReceived = jObjDocumentsReceived["notifications"] as JArray;
            int countDocumentsReceived = notificationsDocumentsReceived?.Count ?? 0;
            totalCount += countDocumentsReceived;

            if (countDocumentsReceived == 0)
            {
                htmlReceiver.Append("<h3>Ontvangen:</h3><p>Er zijn geen documenten te verwerken.</p>");
            }
            else
            {
                htmlReceiver.Append("<h3>Ontvangen:</h3>");
                htmlReceiver.Append("<p><i>Mogelijk</i> nog in te boeken <i>of</i> openstaand in betaalbalans.</p>");
                htmlReceiver.Append("<table>");
                htmlReceiver.Append("<tr><th>Verzender</th><th>Peppol Datum/Uur</th><th>Type</th><th>Document Id</th></tr>");

                foreach (var receive in notificationsDocumentsReceived)
                {
                    string date = receive["notificationDate"]?.ToString();
                    string peppolDocumentType = receive["peppolDocumentType"]?.ToString();
                    string documentId = receive["documentId"]?.ToString();
                    string kboSender = receive["sender"]?.ToString();

                    string kboSenderName = await ViesLookUpName(kboSender);
                    string kboNameResult = (kboSender == kboSenderName) ? kboSender : kboSenderName;

                    htmlReceiver.Append($"<tr><td>{kboNameResult}</td><td>{date}</td><td>{peppolDocumentType}</td><td>{documentId}</td></tr>");
                }
                htmlReceiver.Append("</table>");
            }

            // 2. SENT REACTIONS TO SUPPLIER
            string filteredJsonSentReactions = FilterNotifications(
                json: allNotifications,
                isSender: true,
                "INVOICE_RESPONSE_SENT",
                partyFilter: partyFilter
            );

            var jObjReactionsSent = JObject.Parse(filteredJsonSentReactions);
            var notificationsReactionsSent = jObjReactionsSent["notifications"] as JArray;
            int countReactionsSent = notificationsReactionsSent?.Count ?? 0;
            totalCount += countReactionsSent;

            if (countReactionsSent == 0)
            {
                htmlReactionsSent.Append("<p>Geen reacties verzonden op ontvangen documenten.</p>");
            }
            else
            {
                htmlReactionsSent.Append("<h3>Verzonden Reacties:</h3>");
                htmlReactionsSent.Append("<table>");
                htmlReactionsSent.Append("<tr><th>Gericht aan</th><th>Peppol Datum/Uur</th><th>Status</th><th>Document Id</th></tr>");

                foreach (var sent in notificationsReactionsSent)
                {
                    string date = sent["notificationDate"]?.ToString();
                    string documentStatus = sent["documentStatus"]?.ToString();
                    string documentId = sent["documentId"]?.ToString();
                    string kboReceiver = sent["receiver"]?.ToString();

                    string kboReceiverName = await ViesLookUpName(kboReceiver);
                    string kboNameResult = (kboReceiver == kboReceiverName) ? kboReceiver : kboReceiverName;

                    htmlReactionsSent.Append($"<tr><td>{kboNameResult}</td><td>{date}</td><td>{documentStatus}</td><td>{documentId}</td></tr>");
                }
                htmlReactionsSent.Append("</table>");
            }

            // 3. DOCUMENTS SENT
            string filteredSenderJson = FilterNotifications(
                json: allNotifications,
                isSender: true,
                "DOCUMENT_SENT",
                partyFilter: partyFilter
            );

            var jObjSender = JObject.Parse(filteredSenderJson);
            var senderNotifications = jObjSender["notifications"] as JArray;
            int countSender = senderNotifications?.Count ?? 0;
            totalCount += countSender;

            if (countSender == 0)
            {
                htmlSender.Append("<h3>Verzonden:</h3><p>Er zijn geen documenten te verwerken.</p>");
            }
            else
            {
                htmlSender.Append("<h3>Verzonden:</h3>");
                htmlSender.Append("<p>Openstaand in betaalbalans.</p>");
                htmlSender.Append("<table>");
                htmlSender.Append("<tr><th>Ontvanger</th><th>Peppol Datum/Uur</th><th>Type</th><th>Document Id</th></tr>");

                foreach (var receiverNotification in senderNotifications)
                {
                    var date = receiverNotification["notificationDate"]?.ToString();
                    var peppolDocumentType = receiverNotification["peppolDocumentType"]?.ToString();
                    var documentId = receiverNotification["documentId"]?.ToString();
                    var kboReceiver = receiverNotification["receiver"]?.ToString();

                    string kboReceiverName = await ViesLookUpName(kboReceiver);
                    string kboNameResult = (kboReceiver == kboReceiverName) ? kboReceiver : kboReceiverName;

                    htmlSender.Append($"<tr><td>{kboNameResult}</td><td>{date}</td><td>{peppolDocumentType}</td><td>{documentId}</td></tr>");
                }
                htmlSender.Append("</table>");
            }

            // 4. REACTIONS RECEIVED FROM CLIENTS
            string filteredJsonReceivedReactions = FilterNotifications(
                json: allNotifications,
                isSender: false,
                "INVOICE_RESPONSE_RECEIVED",
                partyFilter: partyFilter
            );

            var jObjReactionsReceived = JObject.Parse(filteredJsonReceivedReactions);
            var notificationsReactionsReceived = jObjReactionsReceived["notifications"] as JArray;
            int countReactionsReceived = notificationsReactionsReceived?.Count ?? 0;
            totalCount += countReactionsReceived;

            if (countReactionsReceived == 0)
            {
                htmlReactionsReceived.Append("<p>Geen reacties ontvangen op verzonden documenten.</p>");
            }
            else
            {
                htmlReactionsReceived.Append("<h3>Ontvangen Reacties:</h3>");
                htmlReactionsReceived.Append("<table>");
                htmlReactionsReceived.Append("<tr><th>Verzender</th><th>Peppol Datum/Uur</th><th>Status</th><th>Document Id</th></tr>");

                foreach (var received in notificationsReactionsReceived)
                {
                    string date = received["notificationDate"]?.ToString();
                    string documentStatus = received["documentStatus"]?.ToString();
                    string documentId = received["documentId"]?.ToString();
                    string kboSender = received["sender"]?.ToString();

                    string kboSenderName = await ViesLookUpName(kboSender);
                    string kboNameResult = (kboSender == kboSenderName) ? kboSender : kboSenderName;

                    htmlReactionsReceived.Append($"<tr><td>{kboNameResult}</td><td>{date}</td><td>{documentStatus}</td><td>{documentId}</td></tr>");
                }
                htmlReactionsReceived.Append("</table>");
            }

            // Combine all HTML parts
            var finalHtml = new StringBuilder();
            finalHtml.Append(htmlBegin);

            if (totalCount == 0)
            {
                finalHtml.Append("<h3>Geen notificaties gevonden</h3>");
                finalHtml.Append("<p>Er zijn momenteel geen Peppol notificaties voor deze onderneming.</p>");
            }
            else
            {
                finalHtml.Append(htmlReceiver);
                finalHtml.Append(htmlReactionsSent);
                finalHtml.Append(htmlSender);
                finalHtml.Append(htmlReactionsReceived);

                // Add footer notes
                finalHtml.Append("<p class='warning'>Let op:</p>");
                finalHtml.Append("<ul class='info-list'>");
                finalHtml.Append("<li>Peppol aankoop- en verkoopdocumenten met status betaald worden gecontroleerd verwijderd.</li>");
                finalHtml.Append("<li>Ingeboekte Peppol-documenten zonder betaalverrichtingen worden voorlopig niet verwijderd, Peppol-verzenddatum (zie kolom 2).</li>");
                finalHtml.Append("<li>Een kopij van alle Peppol aankoop- en verkoopdocumenten blijft beschikbaar via het archief (gesorteerd per jaar/week).</li>");
                finalHtml.Append("</ul>");
            }

            finalHtml.Append("</body></html>");

            return finalHtml.ToString();
        }

        private string FilterNotifications(string json, bool isSender, string eventTypeFilter, string partyFilter)
        {
            if (string.IsNullOrEmpty(json))
                return "{\"notifications\":[]}";

            string partyType = isSender ? "sender" : "receiver";

            var jObject = JObject.Parse(json);

            var filteredNotifications = new JArray();
            if (jObject["notifications"] is JArray notifications)
            {
                foreach (var notification in notifications)
                {
                    string party = notification[partyType]?.ToString();
                    string eventType = notification["eventType"]?.ToString();

                    if (party == partyFilter && eventType == eventTypeFilter)
                    {
                        filteredNotifications.Add(notification);
                    }
                }
            }

            var result = new JObject
            {
                ["notifications"] = filteredNotifications
            };

            return result.ToString(Newtonsoft.Json.Formatting.Indented);
        }

        private async Task<string> ViesLookUpName(string companyId)
        {
            if (string.IsNullOrWhiteSpace(companyId))
                return companyId;

            string countryCode;
            string vatNumberEU;

            if (companyId.Length > 5 && companyId.Substring(0, 5) == "0208:")
            {
                countryCode = "BE";
                vatNumberEU = countryCode + companyId.Substring(5);
            }
            else if (companyId.Length > 5 && companyId.Substring(0, 5) == "9925:")
            {
                countryCode = "BE";
                vatNumberEU = companyId.Substring(5);
            }
            else if (companyId.Length > 2)
            {
                countryCode = companyId.Substring(0, 2);
                vatNumberEU = companyId.Substring(2);
            }
            else
            {
                return companyId;
            }

            string url = "https://ec.europa.eu/taxation_customs/vies/rest-api/ms/" + countryCode + "/vat/" + vatNumberEU;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(5);
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(responseContent))
                        return companyId;

                    char firstNonWs = responseContent.SkipWhile(char.IsWhiteSpace).FirstOrDefault();
                    if (firstNonWs != '{' && firstNonWs != '[')
                        return companyId;

                    dynamic data = JsonConvert.DeserializeObject(responseContent);
                    if (data?.name != null)
                    {
                        return data.name.ToString();
                    }

                    return companyId;
                }
            }
            catch
            {
                return companyId;
            }
        }

        // OleDb Helper: Build connection string
        private string GetOleDbConnectionString()
        {
            return SharedGlobals.DbJetProvider + SharedGlobals.MimDataLocation + SharedGlobals.MarntMdvLocation;
        }

        public string GetSellersDocumentSentResultRS(
            string document,
            bool checkAcceptedOrRejected)
        {
            try
            {
                string documentId = document.Substring(0, 11);
                string connectionString = GetOleDbConnectionString();
                string fieldToCheck = checkAcceptedOrRejected ? "v406" : "v405";
                string fieldValue = string.Empty;

                ADODB.Connection conn = new ADODB.Connection();
                conn.Open(connectionString);

                try
                {
                    // First query: Get the field value using ADODB
                    string safeDocumentId = documentId.Replace("'", "''");
                    string sSQL = $"SELECT v033, v405, v406, dnnSync FROM Dokumenten WHERE v033 = '{safeDocumentId}'";
                    ADODB.Recordset rs = new ADODB.Recordset();
                    rs.Open(sSQL, conn, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic);

                    if (!rs.EOF)
                    {
                        object fieldObj = rs.Fields[fieldToCheck].Value;
                        if (fieldObj != null && fieldObj != DBNull.Value)
                        {
                            fieldValue = fieldObj.ToString();

                            // Clear transient/invalid states
                            if (!string.IsNullOrEmpty(fieldValue) &&
                                (fieldValue.Contains("VALIDATION_ERROR") || fieldValue.Contains("SCHEDULED")))
                            {
                                fieldValue = string.Empty;

                                // Update the record to clear the field using Recordset.Update()
                                try
                                {
                                    rs.Fields[fieldToCheck].Value = "";
                                    rs.Fields["dnnSync"].Value = "False";
                                    rs.Update();
                                }
                                catch
                                {
                                    // ignore update errors; just return empty
                                }
                            }
                        }
                    }
                    rs.Close();
                }
                finally
                {
                    conn.Close();
                }

                return fieldValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetSellersDocumentSentResultRS: " + ex.Message);
                throw;
            }
        }
        public bool GetSellersPaidResultRS(string document)
        {
            try
            {
                string documentId = document.Substring(0, 11);
                string connectionString = GetOleDbConnectionString();

                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();

                    string sSQL = "SELECT v033, v037, v249 FROM Dokumenten WHERE v033 = @documentId";
                    using (OleDbCommand cmd = new OleDbCommand(sSQL, conn))
                    {
                        // Explicitly set parameter type to VarChar to match database string field
                        cmd.Parameters.Add("@documentId", OleDbType.VarChar).Value = documentId;
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return false;
                            }

                            decimal openAmount = 0m;
                            decimal paidAmount = 0m;

                            if (!reader.IsDBNull(reader.GetOrdinal("v249")))
                            {
                                openAmount = Convert.ToDecimal(reader["v249"]);
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("v037")))
                            {
                                paidAmount = Convert.ToDecimal(reader["v037"]);
                            }

                            // Fully paid when remaining open amount is zero
                            return openAmount - paidAmount == 0m;
                        }
                    }
                }
            }
            catch (OleDbException ex)
            {
                Console.WriteLine("OleDbException in GetSellersPaidResultRS: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetSellersPaidResultRS: " + ex.Message);
                throw;
            }
        }
        public string GetBuyersBookStatusRS(string sellerDocument)
        {
            try
            {
                string connectionString = GetOleDbConnectionString();
                string sSQL;
                OleDbCommand cmd = null;

                // Build the appropriate query based on input format
                if (sellerDocument.Contains("|"))
                {
                    string[] parts = sellerDocument.Split('|');
                    if (parts.Length != 3)
                    {
                        return "Nog in te boeken";
                    }

                    string companyId = parts[0];
                    string documentId = parts[1];
                    string paymentId = parts[2];

                    if (companyId.StartsWith("BE"))
                    {
                        companyId = companyId.Substring(2);
                    }
                    if (string.IsNullOrEmpty(paymentId))
                    {
                        paymentId = SanitizePaymentRef(documentId);
                    }

                    // Split into two queries to avoid computed field in JOIN
                    // First get supplier ID
                    string sSQLGetSupplier = "SELECT A110 FROM Leveranciers WHERE v404 = @companyId";
                    string supplierId = null;

                    using (OleDbConnection conn = new OleDbConnection(connectionString))
                    {
                        conn.Open();
                        cmd = new OleDbCommand(sSQLGetSupplier, conn);
                        cmd.Parameters.AddWithValue("@companyId", companyId);
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return "Nog in te boeken";
                            }
                            supplierId = reader["A110"].ToString();
                        }

                        // Now query for document with the supplier ID
                        string expectedDocRef = "L" + supplierId;
                        sSQL = "SELECT Dokumenten.v037, Dokumenten.v039, Dokumenten.v249, Dokumenten.v409 " +
                               "FROM Dokumenten " +
                               "WHERE Dokumenten.v034 = @expectedDocRef " +
                               "AND Dokumenten.v409 = @documentId " +
                               "AND Dokumenten.v039 = @paymentId";

                        cmd = new OleDbCommand(sSQL, conn);
                        cmd.Parameters.AddWithValue("@expectedDocRef", expectedDocRef);
                        cmd.Parameters.AddWithValue("@documentId", documentId);
                        cmd.Parameters.AddWithValue("@paymentId", paymentId);

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return "Nog in te boeken";
                            }

                            decimal openAmount = 0m;
                            decimal paidAmount = 0m;

                            if (!reader.IsDBNull(reader.GetOrdinal("v249")))
                            {
                                openAmount = Convert.ToDecimal(reader["v249"]);
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("v037")))
                            {
                                paidAmount = Convert.ToDecimal(reader["v037"]);
                            }

                            if (openAmount - paidAmount == 0m)
                            {
                                return "Volledig Betaald";
                            }

                            if (paidAmount != 0m)
                            {
                                return "Deels Betaald";
                            }

                            return "Ingeboekt, nog te betalen";
                        }
                    }
                }
                else
                {
                    sSQL = "SELECT v037, v039, v249, v409 FROM Dokumenten WHERE v409 = @sellerDocument";

                    using (OleDbConnection conn = new OleDbConnection(connectionString))
                    {
                        conn.Open();
                        cmd = new OleDbCommand(sSQL, conn);
                        cmd.Parameters.AddWithValue("@sellerDocument", sellerDocument);

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return "Nog in te boeken";
                            }

                            decimal openAmount = 0m;
                            decimal paidAmount = 0m;

                            if (!reader.IsDBNull(reader.GetOrdinal("v249")))
                            {
                                openAmount = Convert.ToDecimal(reader["v249"]);
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("v037")))
                            {
                                paidAmount = Convert.ToDecimal(reader["v037"]);
                            }

                            if (openAmount - paidAmount == 0m)
                            {
                                return "Volledig Betaald";
                            }

                            if (paidAmount != 0m)
                            {
                                return "Deels Betaald";
                            }

                            return "Ingeboekt, nog te betalen";
                        }
                    }
                }
            }
            catch (OleDbException ex)
            {
                Console.WriteLine("OleDbException in GetBuyersBookStatusRS: " + ex.Message);
                return "Nog in te boeken";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetBuyersBookStatusRS: " + ex.Message);
                return "Nog in te boeken";
            }
        }
        public bool SetBuyersBookStatusRS(
            string sellerDocument,
            string jsonToSave,
            int asReceived)
        {
            string connectionString = GetOleDbConnectionString();
            int maxRetries = 5;
            int retryCount = 0;
            int delayMs = 150;

            // Parse parameters first
            string marDocumentId = null;
            if (sellerDocument.Contains("|"))
            {
                string[] parts = sellerDocument.Split('|');
                if (parts.Length != 3)
                {
                    return false;
                }

                string companyId = parts[0];
                string documentId = parts[1];
                string paymentId = parts[2];

                if (companyId.StartsWith("BE"))
                {
                    companyId = companyId.Substring(2);
                }

                if (string.IsNullOrEmpty(paymentId))
                {
                    paymentId = SanitizePaymentRef(documentId);
                }

                // Query to resolve v033 - simplified without computed field in JOIN using ADODB
                // First get the supplier ID from company ID
                string sSQLGetSupplierId = $"SELECT A110 FROM Leveranciers WHERE v404 = '{companyId.Replace("'", "''")}'";

                try
                {
                    ADODB.Connection conn = new ADODB.Connection();
                    conn.Open(connectionString);

                    try
                    {
                        // Get supplier ID first
                        string supplierId = null;
                        ADODB.Recordset rsSupplier = new ADODB.Recordset();
                        rsSupplier.Open(sSQLGetSupplierId, conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly);

                        if (rsSupplier.EOF)
                        {
                            Console.WriteLine($"SetBuyersBookStatusRS: No supplier found for companyId={companyId}");
                            rsSupplier.Close();
                            return false;
                        }
                        supplierId = rsSupplier.Fields["A110"].Value.ToString();
                        rsSupplier.Close();

                        // Now query for the document using the supplier ID
                        string expectedDocRef = "L" + supplierId;
                        string safDocumentId = documentId.Replace("'", "''");
                        string safePaymentId = paymentId.Replace("'", "''");
                        string sSQLResolve = $"SELECT v033 FROM Dokumenten WHERE v034 = '{expectedDocRef.Replace("'", "''")}' AND v409 = '{safDocumentId}' AND v039 = '{safePaymentId}'";

                        ADODB.Recordset rsDoc = new ADODB.Recordset();
                        rsDoc.Open(sSQLResolve, conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly);

                        if (rsDoc.EOF)
                        {
                            Console.WriteLine($"SetBuyersBookStatusRS: No document found - supplierId={supplierId}, docRef={expectedDocRef}, documentId={documentId}, paymentId={paymentId}");
                            rsDoc.Close();
                            return false;
                        }
                        marDocumentId = rsDoc.Fields["v033"].Value.ToString();
                        rsDoc.Close();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception in SetBuyersBookStatusRS (resolve phase): {ex.Message}");
                    return false;
                }
            }
            else
            {
                // For simple document ID, just use it directly
                marDocumentId = sellerDocument;
            }

            // Now retry the update with exponential backoff using ADODB
            while (retryCount < maxRetries)
            {
                try
                {
                    string dbField;
                    if (asReceived == 1)
                    {
                        dbField = "v408"; // responses JSON result
                    }
                    else if (asReceived == 0)
                    {
                        dbField = "v406"; // server received JSON result
                    }
                    else
                    {
                        return false;
                    }

                    ADODB.Connection conn = new ADODB.Connection();
                    conn.Open(connectionString);

                    try
                    {
                        // Use Recordset with Find() instead of WHERE clause to edit record directly
                        string tableSQL = "SELECT * FROM Dokumenten";
                        // Console.WriteLine($"SetBuyersBookStatusRS: Opening Dokumenten table for v033={marDocumentId}");
                        ADODB.Recordset rs = new ADODB.Recordset();
                        rs.Open(tableSQL, conn, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic);

                        // Find the record by v033 value (string field - use quotes in criteria)
                        rs.Find($"v033 = '{marDocumentId}'");

                        if (rs.EOF)
                        {
                            Console.WriteLine($"SetBuyersBookStatusRS: Record not found for v033={marDocumentId}");
                            rs.Close();
                            return false;
                        }

                        // Update the record directly through Recordset
                        // Console.WriteLine($"SetBuyersBookStatusRS: Updating record - dbField={dbField}, marDocumentId={marDocumentId}");
                        rs.Fields[dbField].Value = jsonToSave;
                        rs.Fields["dnnSync"].Value = "False";
                        rs.Update();
                        rs.Close();

                        return true;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                catch (UnauthorizedAccessException ex) when (ex.Message.IndexOf("locked", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    retryCount++;
                    if (retryCount < maxRetries)
                    {
                        System.Threading.Thread.Sleep(delayMs);
                        delayMs *= 2; // Exponential backoff
                    }
                    else
                    {
                        Console.WriteLine($"SetBuyersBookStatusRS: Database locked after {maxRetries} attempts");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception in SetBuyersBookStatusRS (update phase):");
                    Console.WriteLine($"  Type: {ex.GetType().Name}");
                    Console.WriteLine($"  Message: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"  Inner Exception: {ex.InnerException.Message}");
                    }
                    return false;
                }
            }

            return false;
        }
        public bool SetDocumentResultRS(
            string document,
            string jsonResult,
            int toComplete)
        {
            try
            {
                string documentId = document.Substring(0, 11);
                string connectionString = GetOleDbConnectionString();

                string dbField = "";
                if (toComplete == 1)
                {
                    dbField = "v406"; // document sent JSON result
                }
                else if (toComplete == 0)
                {
                    dbField = "v405"; // server/client accepted JSON result
                }
                else if (toComplete == 2)
                {
                    dbField = "v408"; // responses JSON result
                }
                else
                {
                    return false;
                }

                ADODB.Connection conn = new ADODB.Connection();
                conn.Open(connectionString);

                try
                {
                    // Use Recordset with Find() to edit record directly (v033 is a string field)
                    string tableSQL = "SELECT * FROM Dokumenten";
                    ADODB.Recordset rs = new ADODB.Recordset();
                    rs.Open(tableSQL, conn, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic);

                    // Find the record by v033 value (string field - use quotes in criteria)
                    rs.Find($"v033 = '{documentId}'");

                    if (rs.EOF)
                    {
                        rs.Close();
                        return false;
                    }

                    // Update the record directly through Recordset
                    rs.Fields[dbField].Value = jsonResult;
                    rs.Fields["dnnSync"].Value = "False";
                    rs.Update();
                    rs.Close();

                    return true;
                }
                finally
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in SetDocumentResultRS: " + ex.Message);
                return false;
            }
        }
        async public Task<int> RefreshSupportedDocumentsPartiesRS(bool isBuyer)
        {
            int numberUpdated = 0;
            string sSQL;
            string tableName = isBuyer ? "Klanten" : "Leveranciers";
            string supportedField = isBuyer ? "v407" : "v410";
            int totalRecordsToProcess = 0;

            if (isBuyer)
            {
                sSQL = "SELECT Klanten.A110, Klanten.A100, Klanten.v404, Klanten.v150, Klanten.v407, Klanten.dnnSync " +
                       "FROM Klanten " +
                       "WHERE trim(Klanten.v150) = 'BE' AND len(trim(Klanten.v404)) = 10;";
            }
            else
            {
                sSQL = "SELECT Leveranciers.A110, Leveranciers.A100, Leveranciers.v404, Leveranciers.v150, Leveranciers.v410, Leveranciers.dnnSync " +
                       "FROM Leveranciers " +
                       "WHERE trim(Leveranciers.v150) = 'BE' AND len(trim(Leveranciers.v404)) = 10;";
            }

            string connectionString = GetOleDbConnectionString();

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(sSQL, conn))
                    {
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                return 0;
                            }

                            // First pass: fill DataGridView
                            DataGridViewNotifications.Rows.Clear();
                            DataGridViewNotifications.Columns.Clear();
                            DataGridViewNotifications.Columns.Add("A110", "Id");
                            DataGridViewNotifications.Columns.Add("v404", "KBO");
                            DataGridViewNotifications.Columns.Add("A100", "Name");
                            if (isBuyer)
                            {
                                DataGridViewNotifications.Columns.Add("v407", "Ondersteund");
                            }
                            else
                            {
                                DataGridViewNotifications.Columns.Add("v410", "Ondersteund");
                            }

                            while (reader.Read())
                            {
                                string id = reader["A110"].ToString().Trim();
                                string kbo = reader["v404"].ToString().Trim();
                                string name = reader["A100"].ToString().Trim();
                                string supportedFieldLocal = isBuyer ? "v407" : "v410";
                                string supported = reader[supportedFieldLocal].ToString().Trim();
                                DataGridViewNotifications.Rows.Add(id, kbo, name, supported);
                            }

                            totalRecordsToProcess = DataGridViewNotifications.Rows.Count;
                            DataGridViewNotifications.AutoResizeColumns();
                            DataGridViewNotifications.Visible = true;
                        }
                    }

                    conn.Close();
                }

                // Second pass: update supported documents using ADODB with single open Recordset
                ADODB.Connection adoConn = new ADODB.Connection();
                ADODB.Recordset rs = new ADODB.Recordset();
                try
                {
                    adoConn.Open(connectionString, "", "", -1);

                    // Open Recordset with the filtered records
                    string filterSQL = isBuyer 
                        ? "SELECT * FROM Klanten WHERE v150 = 'BE' AND len(v404) = 10"
                        : "SELECT * FROM Leveranciers WHERE v150 = 'BE' AND len(v404) = 10";

                    rs.Open(filterSQL, adoConn, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic);

                    if (rs.EOF)
                    {
                        return 0;
                    }

                    rs.MoveFirst();

                    // Iterate through existing records and update them
                    while (!rs.EOF)
                    {
                        try
                        {
                            if (isBuyer)
                            {
                                string customerId = rs.Fields["A110"].Value.ToString().Trim();
                                string customerName = rs.Fields["A100"].Value.ToString().Trim();
                                string customerKbo = rs.Fields["v404"].Value.ToString().Trim();

                                if (customerKbo.Length == 10)
                                {
                                    string resultJson = await CheckPeppolSupportedDocuments(customerKbo);
                                    if (!string.IsNullOrEmpty(resultJson))
                                    {
                                        string readable = resultJson;

                                        // Update current record directly
                                        rs.Fields[supportedField].Value = readable;
                                        rs.Fields["dnnSync"].Value = "False";
                                        rs.Update();
                                        numberUpdated++;

                                        ToolStripStatusLabel.Text = $"Bezig... {numberUpdated} van {totalRecordsToProcess} - {customerName}";
                                        Application.DoEvents();
                                    }
                                }
                            }
                            else
                            {
                                string supplierId = rs.Fields["A110"].Value.ToString().Trim();
                                string supplierName = rs.Fields["A100"].Value.ToString().Trim();
                                string supplierKbo = rs.Fields["v404"].Value.ToString().Trim();

                                if (supplierKbo.Length == 10)
                                {
                                    string resultJson = await CheckPeppolSupportedDocuments(supplierKbo);
                                    if (!string.IsNullOrEmpty(resultJson))
                                    {
                                        string readable = resultJson;

                                        // Update current record directly
                                        rs.Fields[supportedField].Value = readable;
                                        rs.Fields["dnnSync"].Value = "False";
                                        rs.Update();
                                        numberUpdated++;

                                        ToolStripStatusLabel.Text = $"Bezig... {numberUpdated} van {totalRecordsToProcess} - {supplierName}";
                                        Application.DoEvents();
                                    }
                                }
                            }

                            rs.MoveNext();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error updating record: {ex.Message}");
                            rs.MoveNext();
                        }
                    }
                }
                finally
                {
                    if (rs.State == 1) // adStateOpen
                        rs.Close();
                    if (adoConn.State == 1) // adStateOpen
                        adoConn.Close();
                }

                return numberUpdated;
            }
            catch (OleDbException ex)
            {
                Console.WriteLine("OleDbException in RefreshSupportedDocumentsPartiesRS: " + ex.Message);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in RefreshSupportedDocumentsPartiesRS: " + ex.Message);
                return 0;
            }
        }

        private void ButtonGetDocument_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxTransmissionId.Text) || TextBoxTransmissionId.Text.Length != 32)
            {
                MessageBox.Show("Breng het transmissie ID van een document in bestaande uit 32 tekens.", "Invoer vereist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}