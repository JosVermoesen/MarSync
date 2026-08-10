using ADODB;
using MarSync.Classes.Ademico;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Vsoft.Mail;
using static MarSync.AdemicoModels;
using System.Xml.Serialization;
using MarSync.Camt053;

namespace MarSync.MarioMenu.Admin
{
    public partial class FormAdminPeppolActions : Form
    {
        private readonly HttpClient httpCheck;

        public Recordset DocumentRS { get; private set; }

        public string senderALL = "";
        public string receiverALL = "";

        public FormAdminPeppolActions()
        {
            InitializeComponent();
            Text = "Admin - Peppol Acties" + " [" + SharedGlobals.ApiModus + "]";
            httpCheck = new HttpClient();

            RadioButtonGetReceived.Checked = true;
            TextBoxLegalEntityId.Text = ""; // Default to empty to enable country/scheme/identifier fields

            // Ensure events are hooked up if you didn't use the Designer
            DragEnter += TabSendDocument_DragEnter;
            DragDrop += TabSendDocument_DragDrop;
            RadioButtonGetReceived.Checked = true;
            CheckBoxPublishMinimum.Checked = true;
        }

        private void TabSendDocument_DragEnter(object sender, DragEventArgs e)
        {
            // Only allow file drops
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void TabSendDocument_DragDrop(object sender, DragEventArgs e)
        {
            // Use pattern matching to simplify the type check and cast
            if (e.Data is DataObject dataObject && dataObject.GetData(DataFormats.FileDrop) is string[] files && files.Length > 0)
            {
                // Example 1: Open each file with its default program
                // foreach (var file in files)
                //     Process.Start(new ProcessStartInfo(file) { UseShellExecute = true });

                // Example 2: Load the first file’s text into a TextBox
                if (File.Exists(files[0]))
                {
                    // Assuming you have a TextBox named LabelFile and LabelInvoiceId, LabelIssueDate
                    LabelFile.Text = files[0];
                }
            }
        }

        private void LabelFile_TextChanged(object sender, EventArgs e)
        {
            ButtonCheckFile.Enabled = !string.IsNullOrWhiteSpace(LabelFile.Text) && File.Exists(LabelFile.Text);
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        async private void ButtonCheckConnectivity_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabel.Text = "Bezig...";
            Application.DoEvents();

            var response = await AdemicoClient.CheckConnection(new HttpClient());
            if (response != null)
            {
                ToolStripStatusLabel.Text = response;
            }
            else
            {
                MessageBox.Show(response, "Foutmelding", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TextBoxLegalEntityId_TextChanged(object sender, EventArgs e)
        {
            if (TextBoxLegalEntityId.Text.Length > 0)
            {
                // If a Legal Entity ID is choosen, set specific values for Belgium
                TextBoxCountryCode.Text = "";
                TextBoxRegScheme.Text = "";
                TextBoxRegIdentifier.Text = "";
                TextBoxSupportedDocument.Text = "";
                TextBoxLegalEntityId.Enabled = true;
                TextBoxRegIdentifier.Enabled = false;
                TextBoxCountryCode.Enabled = false;
                TextBoxRegScheme.Enabled = false;
                TextBoxSupportedDocument.Enabled = false;
            }
            else
            {
                // If no Legal Entity ID is provided, set default values for Belgium
                TextBoxCountryCode.Text = "BE"; // Default to Belgium if no Legal Entity ID is provided                                
                TextBoxRegScheme.Text = "0208"; // Default to 0208 scheme for Belgium  
                TextBoxRegIdentifier.Text = "0440058217"; // Default to a common identifier for Belgium
                TextBoxSupportedDocument.Text = "PEPPOL_BIS_BILLING_UBL_INVOICE_V3"; // Default to UBL Invoice for Belgium            
                TextBoxLegalEntityId.Text = ""; // Default Legal Entity ID for Belgium

                TextBoxRegIdentifier.Enabled = true; // Enable the registration identifier field
                TextBoxCountryCode.Enabled = true; // Enable the country code field 
                TextBoxRegScheme.Enabled = true; // Enable the registration scheme field
                TextBoxSupportedDocument.Enabled = true; // Enable the supported document field
            }
        }

        async private void ButtonGetPeppolRegistrations_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabel.Text = "Bezig...";
            Application.DoEvents();

            string country = TextBoxCountryCode.Text; // Example country code
            string peppolRegistrationScheme = TextBoxRegScheme.Text; // Example scheme code, e.g., "0208"
            string peppolRegistrationIdentifier = TextBoxRegIdentifier.Text; // Example identifier, e.g., "0529835180"
            string peppolSupportedDocument = TextBoxSupportedDocument.Text; // Example supported document, e.g., "PEPPOL_BIS_BILLING_UBL_INVOICE_V3"
            string legalEntityId = TextBoxLegalEntityId.Text; // Example legal entity ID, if needed

            try
            {
                var respons = await AdemicoClient.GetPeppolRegistrationAsync(
                    country,
                    peppolRegistrationScheme,
                    peppolRegistrationIdentifier,
                    peppolSupportedDocument,
                    legalEntityId);

                if (respons.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ToolStripStatusLabel.Text = "Registration(s) found (200).";
                }
                else if (respons.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ToolStripStatusLabel.Text = "Unauthorized (401) — check credentials.";
                }
                else if (respons.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ToolStripStatusLabel.Text = "Not found (404) — no matching registration.";
                }
                else
                {
                    ToolStripStatusLabel.Text = $"Status: {(int)respons.StatusCode} {respons.StatusCode}";
                }

                if (!string.IsNullOrEmpty(respons.ResponseBody))
                {
                    var deserializedString = JsonConvert.DeserializeObject(respons.ResponseBody);
                    RichTextBoxResult.Text = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
                }
                else
                {
                    ToolStripStatusLabel.Text = "No response body.";
                }
            }
            catch (Exception ex)
            {
                ToolStripStatusLabel.Text = $"Request failed: {ex.Message}";
            }
        }

        private void RadioButtonGetSent_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButtonGetSent.Checked)
            {
                // Enable the fields for Sender documents                
                TextBoxSender.Enabled = true;
                TextBoxSender.Text = "0208:" + SharedGlobals.CompanyKBONumber; // Default receiver for received documents
                TextBoxReceiver.Enabled = false;
                TextBoxReceiver.Text = "";
            }
        }

        private void RadioButtonGetReceived_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButtonGetReceived.Checked)
            {
                // Enable the fields for Receiver documents                
                TextBoxReceiver.Enabled = true;
                TextBoxReceiver.Text = "0208:" + SharedGlobals.CompanyKBONumber; // Default receiver for received documents
                TextBoxSender.Enabled = false;
                TextBoxSender.Text = "";
            }
        }

        async private void ButtonNotifications_Click(object sender, EventArgs e)
        {
            RichTextBoxAllNotifications.Text = "";
            ToolStripStatusLabel.Text = "Bezig...";

            string eventType = ""; // RadioButtonGetReceived.Checked ? "DOCUMENT_RECEIVED" : "DOCUMENT_SENT";
            var jsonResponse = await AdemicoClient.GetNotificationsAsync(
                transmissionId: "", // "f8a591c77b2211f0b1ed0af13d778bd4"
                documentId: "",
                eventType: eventType, // "DOCUMENT_RECEIVED" or "DOCUMENT_SENT"
                peppolDocumentType: "", // "INVOICE"
                sender: TextBoxSender.Text, // "9925:BE0440058217",
                receiver: TextBoxReceiver.Text, // "0208:0440058217",
                startDateTime: "", // "2023-07-25T11:03:26.688Z"
                endDateTime: "", // "2023-07-29T11:03:26.688Z"
                page: "",
                pageSize: "250"
            );

            if (jsonResponse != null)
            {
                ToolStripStatusLabel.Text = "Notifications retrieved successfully.";
                var deserializedString = JsonConvert.DeserializeObject(jsonResponse);
                RichTextBoxResult.Text = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
                if (RadioButtonGetSent.Checked)
                {
                    senderALL = RichTextBoxResult.Text;
                    SharedGlobals.NotificationsSenderALL = senderALL;
                    RichTextBoxAllNotifications.Text = senderALL;
                }
                else if (RadioButtonGetReceived.Checked)
                {
                    receiverALL = RichTextBoxResult.Text;
                    SharedGlobals.NotificationsReceiverALL = receiverALL;
                    RichTextBoxAllNotifications.Text = receiverALL;
                }

            }
            else
            {
                ToolStripStatusLabel.Text = "Failed to retrieve notifications.";
                RichTextBoxResult.Text = "";
                senderALL = "";
                receiverALL = "";
                SharedGlobals.NotificationsReceiverALL = "";
                SharedGlobals.NotificationsSenderALL = "";
                RichTextBoxAllNotifications.Text = "Failed to retrieve notifications.";
            }
        }

        async private void ButtonLookUp_Click(object sender, EventArgs e)
        {
            if (TextBoxIdentifier.Text.Length != 12) // Check if the VAT number is exactly 12 characters long
            {
                ToolStripStatusLabel.Text = "Voer een geldig BTW-nummer in (12 tekens).";
                return;
            }
            ToolStripStatusLabel.Text = "Bezig...";
            Application.DoEvents();

            string vatNumber = TextBoxIdentifier.Text;
            string countryCode = vatNumber.Substring(0, 2); // Replace range operator with Substring
            string vat = vatNumber.Substring(2); // Replace range operator with Substring

            string url = "https://ec.europa.eu/taxation_customs/vies/rest-api/ms/" + countryCode + "/vat/" + vat;

            try
            {
                HttpResponseMessage response = await httpCheck.GetAsync(url); // Use the HttpClient instance
                response.EnsureSuccessStatusCode(); // Throws if not successful
                string responseContent = await response.Content.ReadAsStringAsync();

                var deserializedString = JsonConvert.DeserializeObject(responseContent);
                RichTextBoxResult.Text = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);

                // Deserialize into VatResponse object
                VatResponse data = JsonConvert.DeserializeObject<VatResponse>(responseContent);

                // Access fields
                if (data != null)
                {
                    string address = data.Address;

                    if (data?.IsValid == true)
                    {
                        TextBoxCompanyName.Text = ToTitleCase(data.Name ?? string.Empty);
                        TextBoxGeographicalInformation.Text = address?.Replace("\n", " - ");

                        ToolStripStatusLabel.Text = "Onderneming met geldig EU BTW nummer.";
                        ButtonEntityNew.Enabled = true;
                    }
                    else
                    {
                        TextBoxCompanyName.Clear();
                        TextBoxGeographicalInformation.Clear();

                        ToolStripStatusLabel.Text = "Ongeldig EU BTW nummer";
                        ButtonEntityNew.Enabled = false;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                ToolStripStatusLabel.Text = "Error: " + ex.Message;
            }
        }

        // Utility method to convert a string to title case
        public static string ToTitleCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            // step 1: bring everything to lowercase
            var lower = input.ToLower(CultureInfo.CurrentCulture);

            // step 2: capitalize the first letter of each word
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(lower);
        }

        async private void ButtonEntityNew_Click(object sender, EventArgs e)
        {
            if (!RadioButtonBothSchemes.Checked && !RadioButtonOnlyScheme0208.Checked)
            {
                ToolStripStatusLabel.Text = "Kies een registratie schema.";
                MessageBox.Show("Selecteer de nodige registratie schemas.", "Foutmelding", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ToolStripStatusLabel.Text = "Bezig...";
            string geographicalInformation;
            string websiteUrl;
            string contactType;
            string contactName;
            string contactPhoneNumber;
            string contactEmail;
            string additionalInformation;
            string name = TextBoxCompanyName.Text;
            string countryCode = "BE"; // Belgium
            string identifier9925 = TextBoxIdentifier.Text; // Full VAT number, e.g., "BE0421132626"
            string identifier0208 = TextBoxIdentifier.Text.Substring(2); // Remove country code for identifier, e.g., "0421132626"

            if (!CheckBoxPublishMinimum.Checked)
            {
                geographicalInformation = TextBoxGeographicalInformation.Text;
                websiteUrl = TextBoxWebsiteUrl.Text; // Optional
                contactType = "public"; // or "technical"
                contactName = TextBoxContactName.Text; // Example contact name
                contactPhoneNumber = TextBoxPhone.Text; // Example phone number
                contactEmail = TextBoxContactEmail.Text; // Example email
                additionalInformation = TextBoxAdditionalInformation.Text; // Optional

                if (RadioButtonBothSchemes.Checked)
                {
                    var entity = new CreateLegalEntityModel
                    {
                        LegalEntityDetails = new LegalEntityDetails
                        {
                            PublishInPeppolDirectory = CheckBoxPublishMinimum.Checked,
                            Name = name,
                            CountryCode = countryCode,
                            GeographicalInformation = geographicalInformation,
                            WebsiteUrl = websiteUrl,
                            Contacts = new List<Contact>
                    {
                        new Contact
                        {
                            ContactType = contactType,
                            Name = contactName,
                            PhoneNumber = contactPhoneNumber,
                            Email = contactEmail
                        }
                    },
                            AdditionalInformation = additionalInformation
                        },
                        PeppolRegistrations = new List<PeppolRegistration>
                {
                    new PeppolRegistration
                    {
                        PeppolIdentifier = new PeppolIdentifier
                        {
                            Scheme = "0208",
                                Identifier = identifier0208 // "0421132626"
                        },
                        SupportedDocuments = new List<string>
                        {
                            "UBL_BE_INVOICE_3_0",
                            "UBL_BE_CREDIT_NOTE_3_0",
                            "PEPPOL_BIS_BILLING_UBL_INVOICE_V3",
                            "PEPPOL_BIS_BILLING_UBL_CREDIT_NOTE_V3",
                            "PEPPOL_MESSAGE_LEVEL_RESPONSE_TRANSACTION_3_0",
                            "PEPPOL_INVOICE_RESPONSE_TRANSACTION_3_0"
                        },
                        PublishInSmp = true
                    },
                    new PeppolRegistration
                    {
                        PeppolIdentifier = new PeppolIdentifier
                        {
                            Scheme = "9925",
                            Identifier = identifier9925  // "BE0421132626"
                        },
                        SupportedDocuments = new List<string>
                        {
                            "UBL_BE_INVOICE_3_0",
                            "UBL_BE_CREDIT_NOTE_3_0",
                            "PEPPOL_BIS_BILLING_UBL_INVOICE_V3",
                            "PEPPOL_BIS_BILLING_UBL_CREDIT_NOTE_V3",
                            "PEPPOL_MESSAGE_LEVEL_RESPONSE_TRANSACTION_3_0",
                            "PEPPOL_INVOICE_RESPONSE_TRANSACTION_3_0"
                        },
                        PublishInSmp = true
                    }
                }
                    };

                    try
                    {
                        var result = await AdemicoClient.CreateOrRegisterLegalEntityAsync(request: entity);

                        ToolStripStatusLabel.Text = $"Status: {(int)result.StatusCode} {result.StatusCode}";
                        MessageBox.Show($"Response: {result.ResponseBody}", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception)
                    {
                        ToolStripStatusLabel.Text = "Error during entity creation.";
                    }

                    ButtonEntityNew.Enabled = false; // Disable the button after creating the entity
                    TextBoxCompanyName.Clear();
                    TextBoxGeographicalInformation.Clear();
                }
                else
                {
                    // Build the request using your Postman example values
                    var entity = new CreateLegalEntityModel
                    {
                        LegalEntityDetails = new LegalEntityDetails
                        {
                            PublishInPeppolDirectory = true,
                            Name = name,
                            CountryCode = countryCode,
                            GeographicalInformation = geographicalInformation,
                            WebsiteUrl = websiteUrl,
                            Contacts = new List<Contact>
                    {
                        new Contact
                        {
                            ContactType = contactType,
                            Name = contactName,
                            PhoneNumber = contactPhoneNumber,
                            Email = contactEmail
                        }
                    },
                            AdditionalInformation = additionalInformation
                        },
                        PeppolRegistrations = new List<PeppolRegistration>
                {
                    new PeppolRegistration
                    {
                        PeppolIdentifier = new PeppolIdentifier
                        {
                            Scheme = "0208",
                                Identifier = identifier0208 // "0421132626"
                        },
                        SupportedDocuments = new List<string>
                        {
                            "UBL_BE_INVOICE_3_0",
                            "UBL_BE_CREDIT_NOTE_3_0",
                            "PEPPOL_BIS_BILLING_UBL_INVOICE_V3",
                            "PEPPOL_BIS_BILLING_UBL_CREDIT_NOTE_V3",
                            "PEPPOL_MESSAGE_LEVEL_RESPONSE_TRANSACTION_3_0",
                            "PEPPOL_INVOICE_RESPONSE_TRANSACTION_3_0"
                        },
                        PublishInSmp = true
                    }
                }
                    };
                    try
                    {
                        var result = await AdemicoClient.CreateOrRegisterLegalEntityAsync(request: entity);

                        ToolStripStatusLabel.Text = $"Status: {(int)result.StatusCode} {result.StatusCode}";
                        MessageBox.Show($"Response: {result.ResponseBody}", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception)
                    {
                        ToolStripStatusLabel.Text = "Error during entity creation.";
                    }

                }
            }
            else
            {
                var entity = new CreateLegalEntityModel
                {
                    LegalEntityDetails = new LegalEntityDetails
                    {
                        PublishInPeppolDirectory = true,
                        Name = name,
                        CountryCode = countryCode,
                        GeographicalInformation = "",
                        WebsiteUrl = "",
                        AdditionalInformation = ""
                    },
                    PeppolRegistrations = new List<PeppolRegistration>
                {
                    new PeppolRegistration
                    {
                        PeppolIdentifier = new PeppolIdentifier
                        {
                            Scheme = "0208",
                                Identifier = identifier0208
                        },
                        SupportedDocuments = new List<string>
                        {
                            "UBL_BE_INVOICE_3_0",
                            "UBL_BE_CREDIT_NOTE_3_0",
                            "PEPPOL_BIS_BILLING_UBL_INVOICE_V3",
                            "PEPPOL_BIS_BILLING_UBL_CREDIT_NOTE_V3",
                            "PEPPOL_MESSAGE_LEVEL_RESPONSE_TRANSACTION_3_0",
                            "PEPPOL_INVOICE_RESPONSE_TRANSACTION_3_0"
                        },
                        PublishInSmp = true
                    }
                }
                };
                try
                {
                    var result = await AdemicoClient.CreateOrRegisterLegalEntityAsync(request: entity);

                    ToolStripStatusLabel.Text = $"Status: {(int)result.StatusCode} {result.StatusCode}";
                    MessageBox.Show($"Response: {result.ResponseBody}", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    ToolStripStatusLabel.Text = "Error during entity creation.";
                }


            }
            ButtonEntityNew.Enabled = false; // Disable the button after creating the entity
            TextBoxCompanyName.Clear();
            TextBoxGeographicalInformation.Clear();
        }


        async private void ButtonGetUBLDocument_Click(object sender, EventArgs e)
        {
            string ademicoUrl = SharedGlobals.AdemicoApiUrl;
            string accessToken = SharedGlobals.AdemicoAccessToken;
            string username = SharedGlobals.AdemicoUsername;
            string password = SharedGlobals.AdemicoPassword;

            string transmissionId = TextBoxTransmissionId.Text.Trim();

            string requestUrl = $"{ademicoUrl}/api/peppol/v1/invoices/{transmissionId}/ubl?accessToken={accessToken}";

            StatusStrip.Text = "Bezig...";
            try
            {
                using (var client = new HttpClient())
                {
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
                    string myDocumentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string outputFile = Path.Combine(myDocumentsFolderPath, "invoice.xml");

                    // Save to file in My Documents
                    using (var writer = new StreamWriter(outputFile, false, Encoding.UTF8))
                    {
                        await writer.WriteAsync(invoiceXml);
                    }

                    StatusStrip.Text = "UBL Invoice XML retrieved and saved successfully.";

                    MessageBox.Show(
                        "UBL Invoice XML retrieved and saved successfully.\n\n" +
                        $"Invoice saved to: {outputFile}",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                StatusStrip.Text = "Error: " + ex.Message;
            }
        }

        private void ButtonCheckFile_Click(object sender, EventArgs e)
        {
            string filePath = LabelFile.Text.Trim();
            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            if (extension != ".xml") // && extension != ".ubl")
            {
                // Show a message box if the file is not a valid UBL XML file
                ButtonSendUblDocument.Enabled = false; // Disable the button if the file is not valid
                return;
            }
            else
            {
                // HandleUblDocument(filePath);
                ReadUBLInvoice(filePath);


                // If the file is valid, enable the button
                ButtonSendUblDocument.Enabled = true;
                ToolStripStatusLabel.Text = "Bestand is een geldig UBL XML bestand.";
            }
        }

        async private void ButtonSendUblDocument_Click(object sender, EventArgs e)
        {
            string filePath = LabelFile.Text.Trim();

            try
            {
                var result = await AdemicoClient.PeppolInvoiceSender.SendUblInvoiceAsync(
                    filePath: filePath,
                    xC5Reporting: false // or true if needed for Singapore reporting
                );

                ToolStripStatusLabel.Text = $"Status: {(int)result.StatusCode} {result.StatusCode}";

                if (!string.IsNullOrEmpty(result.ResponseBody)) // Ensure ResponseBody is not null or empty
                {
                    var deserializedString = JsonConvert.DeserializeObject(value: result.ResponseBody);
                    RichTextBoxResult.Text = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
                    // Result is shown in the main form textbox now and can be copied from there
                    // Datagrid popup is not useful here
                    // DoPopUpDataGridJsonData(RichTextBoxResult.Text); // Show the result in a popup with JSON table view
                    MessageBox.Show($"Response: {result.ResponseBody}", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ToolStripStatusLabel.Text = "No response body.";
                }
            }
            catch (Exception ex)
            {
                ToolStripStatusLabel.Text = $"Error: {ex.Message}";
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckBoxPublishMinimum_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxPublishMinimum.Checked)
            {
                ToolStripStatusLabel.Text = "Minimum Bedrijfsgegevens worden gepubliceerd in de Peppol Directory.";
                // MessageBox.Show("Indien aangevinkt, worden de minimum bedrijfsgegevens gepubliceerd in de Peppol Directory.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RadioButtonBothSchemes.Visible = false;
                RadioButtonBothSchemes.Checked = false;
            }
            else
            {
                ToolStripStatusLabel.Text = "Alle ingevulde velden worden gepubliceerd in de Peppol Directory.";
                MessageBox.Show("Indien niet aangevinkt, worden alle ingevulde velden gepubliceerd in de Peppol Directory.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RadioButtonBothSchemes.Visible = true;
                RadioButtonBothSchemes.Enabled = true;
                RadioButtonOnlyScheme0208.Checked = true; // Default to only 0208 for more common Peppol registrations
            }

        }

        async private void ButtonCheckPeppolRegistration_Click(object sender, EventArgs e)
        {
            string jsonOrXml = RadioButtonJson.Checked ? "JSON" : "XML";

            // Set cursor to wait
            Cursor.Current = Cursors.WaitCursor;

            string peppolId = TextBoxRegIdentifier.Text.Trim();
            string url = "https://directory.peppol.eu/search/1.0/" + jsonOrXml + "?q=iso6523-actorid-upis:" + peppolId;

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        Cursor.Current = Cursors.Default;
                        string responseText = await response.Content.ReadAsStringAsync();
                        // MessageBox.Show(responseText, "Peppol Directory Response", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (jsonOrXml == "JSON")
                        {
                            ParsePeppolJson(responseText);
                            var deserializedString = JsonConvert.DeserializeObject(responseText);
                            RichTextBoxResult.Text = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
                            return;
                        }

                        var xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(responseText);

                        var docNodes = xmlDoc.SelectNodes("//match/docTypeID");
                        foreach (XmlNode docNode in docNodes)
                        {
                            MessageBox.Show(docNode.InnerText, "docTypeID", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        var entityNode = xmlDoc.SelectSingleNode("//match/entity");
                        if (entityNode != null)
                        {
                            var name = entityNode.SelectSingleNode("name")?.InnerText ?? "";
                            var country = entityNode.SelectSingleNode("countryCode")?.InnerText ?? "";
                            var email = entityNode.SelectSingleNode("contact")?.Attributes?["email"]?.Value ?? "";

                            MessageBox.Show("Naam: " + name, "Entity Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            MessageBox.Show("Land: " + country, "Country", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            MessageBox.Show("Email: " + email, "Email", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        RichTextBoxResult.Text = responseText;
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("HTTP Error: " + (int)response.StatusCode, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        RichTextBoxResult.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Exception: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RichTextBoxResult.Text = "";
            }
        }

        private void ParsePeppolJson(string responseText)
        {
            // Parse the JSON
            var jObj = JObject.Parse(responseText);

            // Get the first match (if any)
            var match = jObj["matches"]?.FirstOrDefault();
            if (match == null)
            {
                MessageBox.Show("No matches found.");
                return;
            }

            // Participant ID
            string participantId = match["participantID"]?["value"]?.ToString() ?? "";

            // Document Types
            var docTypes = match["docTypes"]?.Select(dt => dt["value"]?.ToString()).ToList();

            // Entity info
            var entity = match["entities"]?.FirstOrDefault();
            string entityName = entity?["name"]?.FirstOrDefault()?["name"]?.ToString() ?? "";
            string countryCode = entity?["countryCode"]?.ToString() ?? "";
            string regDate = entity?["regDate"]?.ToString() ?? "";

            // Show results (or assign to controls)
            MessageBox.Show("Participant ID: " + participantId);
            if (docTypes != null)
            {
                foreach (var docType in docTypes)
                    MessageBox.Show("DocType: " + docType);
            }
            MessageBox.Show("Naam: " + entityName);
            MessageBox.Show("Land: " + countryCode);
            MessageBox.Show("RegDate: " + regDate);
        }

        private static void ReadUBLInvoice(string filePath)
        {
            var xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(filePath);
            }
            catch (Exception)
            {
                return;
            }

            var ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            ns.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");

            var sb = new StringBuilder();

            // UBL Version
            var ublVersionNode = xmlDoc.SelectSingleNode("//cbc:UBLVersionID", ns);
            sb.AppendLine("UBL VersionID: " + (ublVersionNode?.InnerText ?? "not found"));

            // Document ID
            var invoiceIdNode = xmlDoc.SelectSingleNode("//cbc:ID", ns);
            sb.AppendLine("Document ID: " + (invoiceIdNode?.InnerText ?? "not found"));

            // IssueDate
            var issueDateNode = xmlDoc.SelectSingleNode("//cbc:IssueDate", ns);
            sb.AppendLine("IssueDate: " + (issueDateNode?.InnerText ?? "not found"));

            // DueDate
            var dueDateNode = xmlDoc.SelectSingleNode("//cbc:DueDate", ns);
            sb.AppendLine("DueDate: " + (dueDateNode?.InnerText ?? "not found"));

            // InvoiceTypeCode
            var invTypeNode = xmlDoc.SelectSingleNode("//cbc:InvoiceTypeCode", ns);
            if (invTypeNode != null)
            {
                var invoiceTypeCode = invTypeNode.InnerText;
                var listID = invTypeNode.Attributes?["listID"]?.Value ?? "";
                sb.AppendLine("invoiceTypeCode: " + invoiceTypeCode);
                sb.AppendLine("invoice listID: " + listID);
            }
            else
            {
                MessageBox.Show("InvoiceTypeCode element not found.");
            }

            // OrderReference
            var orderList = xmlDoc.SelectNodes("//cac:OrderReference", ns);
            if (orderList != null)
            {
                foreach (XmlNode ordNode in orderList)
                {
                    var orderId = ordNode.SelectSingleNode("cbc:ID", ns)?.InnerText ?? "Order ID: not available";
                    sb.AppendLine("Order ID: " + orderId);
                }
            }
            MessageBox.Show(sb.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Supplier info
            var supplierNode = xmlDoc.SelectSingleNode("//cac:AccountingSupplierParty/cac:Party", ns);
            if (supplierNode != null)
            {
                var msg = new StringBuilder();
                msg.AppendLine("Supplier info");
                msg.AppendLine("-------------");
                msg.AppendLine("endpointOndernemingsnummer " + NodeText(supplierNode, "cbc:EndpointID", ns));
                msg.AppendLine("supplierID: " + NodeText(supplierNode, "cac:PartyIdentification/cbc:ID", ns));
                msg.AppendLine("tradingName: " + NodeText(supplierNode, "cac:PartyName/cbc:Name", ns));
                msg.AppendLine("street: " + NodeText(supplierNode, "cac:PostalAddress/cbc:StreetName", ns));
                msg.AppendLine("city: " + NodeText(supplierNode, "cac:PostalAddress/cbc:CityName", ns));
                msg.AppendLine("postalZone: " + NodeText(supplierNode, "cac:PostalAddress/cbc:PostalZone", ns));
                msg.AppendLine("countryCode: " + NodeText(supplierNode, "cac:PostalAddress/cac:Country/cbc:IdentificationCode", ns));
                msg.AppendLine("vatNumber: " + NodeText(supplierNode, "cac:PartyTaxScheme/cbc:CompanyID", ns));
                MessageBox.Show(msg.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No AccountingSupplierParty element found.");
            }

            // Customer info
            var custNode = xmlDoc.SelectSingleNode("//cac:AccountingCustomerParty", ns);
            if (custNode != null)
            {
                var msg = new StringBuilder();
                msg.AppendLine("Customer info");
                msg.AppendLine("-------------");
                msg.AppendLine("custAssignedAccountID: " + NodeText(custNode, "cbc:CustomerAssignedAccountID", ns));
                msg.AppendLine("custEndpointID: " + NodeText(custNode, "cac:Party/cbc:EndpointID", ns));
                msg.AppendLine("custName: " + NodeText(custNode, "cac:Party/cac:PartyName/cbc:Name", ns));
                msg.AppendLine("custStreet: " + NodeText(custNode, "cac:Party/cac:PostalAddress/cbc:StreetName", ns));
                msg.AppendLine("custCity: " + NodeText(custNode, "cac:Party/cac:PostalAddress/cbc:CityName", ns));
                msg.AppendLine("custPostalZone: " + NodeText(custNode, "cac:Party/cac:PostalAddress/cbc:PostalZone", ns));
                msg.AppendLine("custCountryCode: " + NodeText(custNode, "cac:Party/cac:PostalAddress/cac:Country/cbc:IdentificationCode", ns));
                msg.AppendLine("custTaxID: " + NodeText(custNode, "cac:Party/cac:PartyTaxScheme/cbc:CompanyID", ns));
                msg.AppendLine("custTaxScheme: " + NodeText(custNode, "cac:Party/cac:PartyTaxScheme/cac:TaxScheme/cbc:ID", ns));
                MessageBox.Show(msg.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No AccountingCustomerParty element found.");
            }

            // PaymentMeans
            var pmNodes = xmlDoc.SelectNodes("//cac:PaymentMeans", ns);
            if (pmNodes != null && pmNodes.Count > 0)
            {
                var msg = new StringBuilder();
                msg.AppendLine("PaymentMeans");
                msg.AppendLine("------------");
                foreach (XmlNode pmNode in pmNodes)
                {
                    msg.AppendLine("PaymentMeansCode: " + NodeText(pmNode, "cbc:PaymentMeansCode", ns));
                    msg.AppendLine("PaymentID: " + NodeText(pmNode, "cbc:PaymentID", ns));
                    msg.AppendLine("Payee IBAN: " + NodeText(pmNode, "cac:PayeeFinancialAccount/cbc:ID", ns));
                    msg.AppendLine("Account Name: " + NodeText(pmNode, "cac:PayeeFinancialAccount/cbc:Name", ns));
                    msg.AppendLine("BIC/Branch ID: " + NodeText(pmNode, "cac:PayeeFinancialAccount/cac:FinancialInstitutionBranch/cbc:ID", ns));
                    msg.AppendLine();
                    msg.AppendLine("Card account (if present)");
                    msg.AppendLine("Card Account ID: " + NodeText(pmNode, "cac:CardAccount/cbc:ID", ns));
                    msg.AppendLine("Card Account Name: " + NodeText(pmNode, "cac:CardAccount/cbc:Name", ns));
                    msg.AppendLine();
                    msg.AppendLine("Direct debit mandate (if present)");
                    msg.AppendLine("Mandate ID: " + NodeText(pmNode, "cac:PaymentMandate/cbc:ID", ns));
                    msg.AppendLine("Mandate Date: " + NodeText(pmNode, "cac:PaymentMandate/cbc:PaymentMandateDate", ns));
                }
                MessageBox.Show(msg.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No PaymentMeans element found.");
            }

            // TaxTotal
            var msgTax = new StringBuilder();
            msgTax.AppendLine("TaxTotal");
            msgTax.AppendLine("--------");
            var taxAmountEl = xmlDoc.SelectSingleNode("//cac:TaxTotal/cbc:TaxAmount", ns);
            var currencyID = taxAmountEl?.Attributes?["currencyID"]?.Value ?? "";
            if (taxAmountEl != null && string.IsNullOrEmpty(currencyID))
            {
                MessageBox.Show("Attribute currencyID is missing on <cbc:TaxAmount>");
            }

            var taxTotals = xmlDoc.SelectNodes("//cac:TaxTotal", ns);
            if (taxTotals != null)
            {
                foreach (XmlNode taxTotalElem in taxTotals)
                {
                    var ttAmount = taxTotalElem.SelectSingleNode("cbc:TaxAmount", ns)?.InnerText ?? "";
                    msgTax.AppendLine($"TaxTotal: {ttAmount} {currencyID}");

                    var subtotals = taxTotalElem.SelectNodes("cac:TaxSubtotal", ns);
                    foreach (XmlNode subElem in subtotals)
                    {
                        msgTax.AppendLine();
                        msgTax.AppendLine("SubDetail");
                        msgTax.AppendLine("TaxableAmount: " + NodeText(subElem, "cbc:TaxableAmount", ns));
                        msgTax.AppendLine("TaxAmount: " + NodeText(subElem, "cbc:TaxAmount", ns));
                        msgTax.AppendLine("Percent: " + NodeText(subElem, "cac:TaxCategory/cbc:Percent", ns) + "%");
                        msgTax.AppendLine();
                    }
                }
            }

            // LegalMonetaryTotal
            var msgMoney = new StringBuilder();
            msgMoney.AppendLine("LegalMonetaryTotal");
            msgMoney.AppendLine("------------------");
            var moneyTotalEl = xmlDoc.SelectSingleNode("//cac:LegalMonetaryTotal", ns);
            if (moneyTotalEl != null)
            {
                msgMoney.AppendLine("LineExtensionAmount: " + NodeText(moneyTotalEl, "cbc:LineExtensionAmount", ns));
                msgMoney.AppendLine("TaxExclusiveAmount: " + NodeText(moneyTotalEl, "cbc:TaxExclusiveAmount", ns));
                msgMoney.AppendLine("TaxInclusiveAmount: " + NodeText(moneyTotalEl, "cbc:TaxInclusiveAmount", ns));
                msgMoney.AppendLine("PayableAmount: " + NodeText(moneyTotalEl, "cbc:PayableAmount", ns) + $" ({currencyID})");
                MessageBox.Show(msgMoney.ToString(), "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // InvoiceLines
            var msgLines = new StringBuilder();
            var invoiceLines = xmlDoc.SelectNodes("//cac:InvoiceLine", ns);
            if (invoiceLines != null)
            {
                foreach (XmlNode lineNode in invoiceLines)
                {
                    var desc = NodeText(lineNode, ".//cbc:Description", ns);
                    var qty = NodeText(lineNode, ".//cbc:InvoicedQuantity", ns);
                    var price = NodeText(lineNode, ".//cbc:PriceAmount", ns);
                    msgLines.AppendLine($"Item: {desc}, Quantity: {qty}, Price: {price}");
                }
            }
        }

        // Helper for safe node text extraction
        private static string NodeText(XmlNode parentNode, string xpath, XmlNamespaceManager ns)
        {
            var node = parentNode.SelectSingleNode(xpath, ns);
            return node?.InnerText.Trim() ?? "";
        }

        private void ButtonShowAllNotificationsAsSender_Click(object sender, EventArgs e)
        {
            RichTextBoxResult.Text = senderALL.ToString();

            // Automatically open the TabResponse
            if (RichTextBoxResult.Text != "")
            {
                TabControlVariousActions.SelectedTab = TabResponse;
            }
            else
            {
                MessageBox.Show("Eerst notificaties ophalen a.u.b.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ButtonShowAllNotificationsAsReceiver_Click(object sender, EventArgs e)
        {
            RichTextBoxResult.Text = receiverALL.ToString();

            // Automatically open the TabResponse
            if (RichTextBoxResult.Text != "")
            {
                TabControlVariousActions.SelectedTab = TabResponse;
            }
            else
            {
                MessageBox.Show("Eerst notificaties ophalen a.u.b.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string AdminFilterJson(
            string json,
            bool isSender,
            string eventTypeFilter,
            string partyFilter)
        {
            if (json == "")
                return "";

            string partyType;
            if (isSender)
            {
                // Filtering for sender notifications
                partyType = "sender";
            }
            else
            {
                // Filtering for receiver notifications
                partyType = "receiver";
            }

            // Parse the JSON
            var jObject = JObject.Parse(json);

            // Extract the notifications array
            var notifications = jObject["notifications"] as JArray;

            // Filter the notifications based on sender and eventType
            var filteredNotifications = new JArray();
            foreach (var notification in notifications)
            {
                string party = notification[partyType]?.ToString();
                string eventType = notification["eventType"]?.ToString();

                if (party == partyFilter && eventType == eventTypeFilter)
                {
                    filteredNotifications.Add(notification);
                }

                // if (party == partyFilter)
                // {
                //      filteredNotifications.Add(notification);
                // }
            }

            // Create the new JSON object
            var result = new JObject
            {
                ["notifications"] = filteredNotifications
            };

            // Output the new JSON
            return result.ToString(Newtonsoft.Json.Formatting.Indented);
        }
        
        private string GetLegalEntityEmail(string identifier)
        {
            // For testing purposes, return a fixed email address
            // return "josvermoesen@outlook.be";

            string adminMarntMdvLocation = "\\001\\marnt.mdv";
            string connectionString = SharedGlobals.DbJetProvider + SharedGlobals.MimDataLocation + adminMarntMdvLocation;
            string sSQL =
                "SELECT Klanten.v224, Klanten.v404 " +
                "FROM Klanten " +
                "WHERE Klanten.v404 = '" + identifier + "';";

            DocumentRS = new Recordset()
            {
                CursorLocation = CursorLocationEnum.adUseClient
            };
            DocumentRS.Open(sSQL, connectionString, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic);
            if (DocumentRS.RecordCount == 1)
            {
                string email = DocumentRS.Fields["v224"].Value.ToString();
                DocumentRS.Close();
                DocumentRS = null;
                return email;
            }
            else
            {
                DocumentRS.Close();
                DocumentRS = null;
                return "josvermoesen@outlook.be";
            }
        }

        public static List<(string Name, string Identifier)> ExtractNameIdentifierPairs(string json)
        {
            var result = new List<(string Name, string Identifier)>();
            var jObj = JObject.Parse(json);

            if (!(jObj["legalEntities"] is JArray legalEntities))
                return result;

            foreach (var entity in legalEntities)
            {
                var name = entity["legalEntityDetails"]?["name"]?.ToString();
                if (!(entity["peppolRegistrations"] is JArray peppolRegistrations))
                    continue;

                foreach (var reg in peppolRegistrations)
                {
                    var details = reg["peppolRegistrationDetails"];
                    var scheme = details?["peppolIdentifier"]?["scheme"]?.ToString();
                    var identifier = details?["peppolIdentifier"]?["identifier"]?.ToString();

                    // keep only scheme 0208, skip 9925 and everything else
                    if (!string.Equals(scheme, "0208", StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(identifier))
                    {
                        result.Add((name, identifier));
                    }
                }
            }
            return result;
        }

        private async Task<bool> GetAllNotificationsForServerAccount()
        {
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
                SharedGlobals.NotificationsServerALL = result;

                // Warn when pagination indicates more items than the requested pageSize
                try
                {
                    var jObject = JObject.Parse(jsonResponse);
                    if (jObject["pagination"] is JObject pagination)
                    {
                        int count = pagination.Value<int?>("count") ?? 0;
                        int pageSize = pagination.Value<int?>("pageSize") ?? 0;

                        if (pageSize > 0 && count > pageSize)
                        {
                            MessageBox.Show(
                                "Er zijn meer dan " + pageSize + " notificaties op de server. " +
                                "Gebruik filters of verwerk en verwijder oudere notificaties.",
                                "Waarschuwing notificaties",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return false;
                        }
                    }
                }
                catch
                {
                    // Ignore JSON errors in warning logic
                }
            }

            if (SharedGlobals.NotificationsServerALL == "")
            {
                return false;
            }
            return true;
        }

        private async Task<string> ViesLookUpName(string companyId)
        {
            string countryCode;
            string vatNumberEU;

            if (companyId.Substring(0, 5) == "0208:")
            {
                countryCode = "BE";
                vatNumberEU = countryCode + companyId.Substring(5);
            }
            else if (companyId.Substring(0, 5) == "9925:")
            {
                countryCode = "BE";
                vatNumberEU = companyId.Substring(5);
            }
            else
            {
                countryCode = companyId.Substring(0, 2);
                vatNumberEU = companyId.Substring(2);
            }

            string url = "https://ec.europa.eu/taxation_customs/vies/rest-api/ms/" + countryCode + "/vat/" + vatNumberEU;

            try
            {
                HttpResponseMessage response = await httpCheck.GetAsync(url); // Use the HttpClient instance
                response.EnsureSuccessStatusCode(); // Throws if not successful
                string responseContent = await response.Content.ReadAsStringAsync();

                // Work around HTML or non‑JSON error pages (e.g. "Server temporarily unavailable")
                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    return companyId;
                }

                // Quick check: must look like JSON (start with { or [)
                char firstNonWs = responseContent.SkipWhile(char.IsWhiteSpace).FirstOrDefault();
                if (firstNonWs != '{' && firstNonWs != '[')
                {
                    // Not JSON, probably HTML error page
                    Console.WriteLine("VIES returned non‑JSON content");
                    return companyId;
                }

                // Deserialize into VatResponse object
                VatResponse data;
                try
                {
                    data = JsonConvert.DeserializeObject<VatResponse>(responseContent);
                }
                catch (JsonException ex)
                {
                    // JSON is invalid or doesn't match VatResponse
                    Console.WriteLine("Error deserializing VIES response: " + ex.Message);
                    return companyId;
                }

                // Access fields
                if (data != null && data.IsValid)
                {
                    return ToTitleCase(data.Name ?? string.Empty);
                }

                return companyId;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                return companyId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected VIES error: {ex.Message}");
                return companyId;
            }
        }

        async private void ButtonMailAllStatusReports_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabel.Text = "Bezig...";
            string entitiesResult;
            try
            {
                var respons = await AdemicoClient.GetPeppolRegistrationAsync(
                    country: "BE",
                    peppolRegistrationScheme: "",
                    peppolRegistrationIdentifier: "",
                    peppolSupportedDocument: "",
                    legalEntityId: "",
                    page: 0,
                    pageSize: 100);

                if (respons.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ToolStripStatusLabel.Text = "Unauthorized (401) — check credentials.";
                    return;
                }
                else if (respons.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ToolStripStatusLabel.Text = "Not found (404) — no matching registration.";
                    return;
                }
                else if (respons.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ToolStripStatusLabel.Text = "Registration(s) found (200).";
                }
                else
                {
                    ToolStripStatusLabel.Text = $"Status: {(int)respons.StatusCode} {respons.StatusCode}";
                    return;
                }

                if (!string.IsNullOrEmpty(respons.ResponseBody))
                {
                    var deserializedString = JsonConvert.DeserializeObject(respons.ResponseBody);
                    entitiesResult = JsonConvert.SerializeObject(deserializedString, Newtonsoft.Json.Formatting.Indented);
                    RichTextBoxResult.Text = entitiesResult;
                }
                else
                {
                    ToolStripStatusLabel.Text = "No response body.";
                    return;
                }
            }
            catch (Exception ex)
            {
                ToolStripStatusLabel.Text = $"Request failed: {ex.Message}";
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            // Now do something with entitiesResult
            var pairs = ExtractNameIdentifierPairs(entitiesResult);
            if (pairs != null)
            {
                // Get all notification on our serveraccount
                bool result = await GetAllNotificationsForServerAccount();
                if (result)
                {
                    ToolStripStatusLabel.Text = "Notifications retrieved for server account.";
                }
                else
                {
                    ToolStripStatusLabel.Text = "No notifications found for server account.";
                    return;
                }
                foreach (var pair in pairs)
                {
                    var htmlBegin = new StringBuilder();
                    var htmlReceiver = new StringBuilder();
                    var htmlReactionsSent = new StringBuilder();
                    var htmlSender = new StringBuilder();
                    var htmlReactionsReceived = new StringBuilder();

                    int totalCount = 0;

                    // First create heading with companyID                    
                    htmlBegin.Append($"<h1>{pair.Name} [{pair.Identifier}]</h1>");
                    Console.WriteLine($"Name: {pair.Name}, Identifier: {pair.Identifier}");

                    // 1. CUSTOMER RECEIVED DOCUMENT:
                    //    Make an array of notifications where PROVED client received documents                    

                    string filteredJsonReceivedDocuments = AdminFilterJson(
                        json: SharedGlobals.NotificationsServerALL,
                        isSender: false,
                        "DOCUMENT_RECEIVED",
                        partyFilter: "0208:" + pair.Identifier
                    );

                    // Show messagebox only when there is a count of notifications is not empty
                    // Parse once and work with the notifications array
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
                        // Display HTML for received notifications                        
                        // First create heading for Receiver notifications
                        htmlReceiver.Append("<h3>Ontvangen:</h3>");
                        // Nog in te boeken of openstaand in betaalbalans
                        htmlReceiver.Append("<p><i>Mogelijk</i> nog in te boeken <p><i>of</i> openstaand in betaalbalans.</p>");
                        // Now create html table from the notifications for better readability                        
                        htmlReceiver.Append(
                            "<table border='0' cellpadding='6' cellspacing='0' " +
                            "style='border-collapse:collapse;font-family:Arial,Helvetica,sans-serif;font-size:12px;'>");

                        htmlReceiver.Append(
                            "<tr style='background-color:#f0f0f0;font-weight:bold;border-bottom:1px solid #cccccc;'>" +
                            "<th align='left' style='border-bottom:1px solid #cccccc;'>Verzender</th>" +
                            "<th align='left' style='border-bottom:1px solid #cccccc;'>Peppol Datum/Uur</th>" +
                            "<th align='left' style='border-bottom:1px solid #cccccc;'>Type</th>" +
                            "<th align='left' style='border-bottom:1px solid #cccccc;'>Document Id</th>" +
                            "</tr>");

                        bool isOdd = false;
                        foreach (var receive in notificationsDocumentsReceived)
                        {
                            string date = receive["notificationDate"]?.ToString();
                            // string eventType = receive["eventType"]?.ToString();
                            string peppolDocumentType = receive["peppolDocumentType"]?.ToString();
                            string documentId = receive["documentId"]?.ToString();
                            string kboSender = receive["sender"]?.ToString();

                            // try to get the company name of the sender
                            string kboSenderName = await ViesLookUpName(kboSender);
                            string kboNameResult = (kboSender == kboSenderName) ? kboSender : kboSenderName;

                            string rowColor = isOdd ? "#ffffff" : "#f9f9f9";
                            isOdd = !isOdd;

                            htmlReceiver.Append(
                                "<tr style='background-color:" + rowColor + ";'>" +
                                "<td style='border-bottom:1px solid #eeeeee;'>" + kboNameResult + "</td>" +
                                "<td style='border-bottom:1px solid #eeeeee;'>" + date + "</td>" +
                                "<td style='border-bottom:1px solid #eeeeee;'>" + peppolDocumentType + "</td>" +
                                "<td style='border-bottom:1px solid #eeeeee;'>" + documentId + "</td>" +
                                "</tr>");
                        }
                        htmlReceiver.Append("</table>");
                    }

                    // SENT REACTIONS To SUPPLIER:
                    // Make an array of notifications                    

                    string filteredJsonSentReactions = AdminFilterJson(
                        json: SharedGlobals.NotificationsServerALL,
                        isSender: true,
                        "INVOICE_RESPONSE_SENT",
                        partyFilter: "0208:" + pair.Identifier
                    );

                    // Show messagebox only when there is a count of notifications is not empty
                    // Parse once and work with the notifications array
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
                        // Display HTML for received notifications                        
                        // First create heading for Receiver notifications
                        htmlReactionsSent.Append("<h3>Verzonden Reacties:</h3>");
                        // Now create html table from the notifications for better readability                        
                        htmlReactionsSent.Append(
                            "<table border='0' cellpadding='6' cellspacing='0' " +
                            "style='border-collapse:collapse;font-family:Arial,Helvetica,sans-serif;font-size:12px;'>");

                        htmlReactionsSent.Append(
                            "<tr style='background-color:#f0f0f0;font-weight:bold;border-bottom:1px solid #cccccc;'>" +
                            "<th align='left' style='border-bottom:1px solid #cccccc;'>Gericht aan</th>" +
                            "<th align='left' style='border-bottom:1px solid #cccccc;'>Peppol Datum/Uur</th>" +
                            "<th align='left' style='border-bottom:1px solid #cccccc;'>Status</th>" +
                            "<th align='left' style='border-bottom:1px solid #cccccc;'>Document Id</th>" +
                            "</tr>");

                        bool isOdd = false;
                        foreach (var sent in notificationsReactionsSent)
                        {
                            string date = sent["notificationDate"]?.ToString();
                            // string eventType = receive["eventType"]?.ToString();
                            string documentStatus = sent["documentStatus"]?.ToString();
                            string documentId = sent["documentId"]?.ToString();
                            string kboReceiver = sent["receiver"]?.ToString();

                            // try to get the company name of the sender
                            string kboReceiverName = await ViesLookUpName(kboReceiver);
                            string kboNameResult = (kboReceiver == kboReceiverName) ? kboReceiver : kboReceiverName;

                            string rowColor = isOdd ? "#ffffff" : "#f9f9f9";
                            isOdd = !isOdd;

                            htmlReactionsSent.Append(
                                "<tr style='background-color:" + rowColor + ";'>" +
                                "<td style='border-bottom:1px solid #eeeeee;'>" + kboNameResult + "</td>" +
                                "<td style='border-bottom:1px solid #eeeeee;'>" + date + "</td>" +
                                "<td style='border-bottom:1px solid #eeeeee;'>" + documentStatus + "</td>" +
                                "<td style='border-bottom:1px solid #eeeeee;'>" + documentId + "</td>" +
                                "</tr>");
                        }
                        htmlReactionsSent.Append("</table>");
                    }

                    // Do the same for notifications as sender matching the pair.Identifier
                    string filteredSenderJson = AdminFilterJson(
                        json: SharedGlobals.NotificationsServerALL,
                        isSender: true,
                        "DOCUMENT_SENT",
                        partyFilter: "0208:" + pair.Identifier
                    );

                    // 3. Show messagebox only when there is a count of notifications is not empty
                    // Parse once and work with the notifications array
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
                        // Display HTML for received notifications                        
                        // First create heading for Receiver notifications
                        htmlSender.Append("<h3>Verzonden:</h3>");
                        // Nog openstaand in betaalbalans
                        htmlSender.Append("<p>Openstaand in betaalbalans.</p>");

                        // Now create html table from the notifications for better readability                        
                        htmlSender.Append(
                            "<table border='0' cellpadding='6' cellspacing='0' " +
                            "style='border-collapse:collapse;font-family:Arial,Helvetica,sans-serif;font-size:12px;'>");

                        htmlSender.Append(
                            "<tr style='background-color:#f0f0f0;font-weight:bold;border-bottom:1px solid #cccccc;'>" +
                            "<th align='left' style='border-bottom:1px solid #cccccc;'>Ontvanger</th>" +
                            "<th align='left' style='border-bottom:1px solid #cccccc;'>Peppol Datum/Uur</th>" +
                            "<th align='left' style='border-bottom:1px solid #cccccc;'>Type</th>" +
                            "<th align='left' style='border-bottom:1px solid #cccccc;'>Document Id</th>" +
                            "</tr>");

                        bool isOdd = false;
                        foreach (var receiverNotification in senderNotifications)
                        {
                            var date = receiverNotification["notificationDate"]?.ToString();
                            // var eventType = receiverNotification["eventType"]?.ToString();
                            var peppolDocumentType = receiverNotification["peppolDocumentType"]?.ToString();
                            var documentId = receiverNotification["documentId"]?.ToString();
                            var kboReceiver = receiverNotification["receiver"]?.ToString();

                            // try to get the company name of the sender
                            string kboReceiverName = await ViesLookUpName(kboReceiver);
                            string kboNameResult = (kboReceiver == kboReceiverName) ? kboReceiver : kboReceiverName;

                            string rowColor = isOdd ? "#ffffff" : "#f9f9f9";
                            isOdd = !isOdd;

                            htmlSender.Append(
                                "<tr style='background-color:" + rowColor + ";'>" +
                                "<td style='border-bottom:1px solid #eeeeee;'>" + kboNameResult + "</td>" +
                                "<td style='border-bottom:1px solid #eeeeee;'>" + date + "</td>" +
                                "<td style='border-bottom:1px solid #eeeeee;'>" + peppolDocumentType + "</td>" +
                                "<td style='border-bottom:1px solid #eeeeee;'>" + documentId + "</td>" +
                                "</tr>");
                        }
                        htmlSender.Append("</table>");
                    }

                    // REACTIONS received from client:
                    //    Make an array of notifications                    

                    string filteredJsonReceivedReactions = AdminFilterJson(
                        json: SharedGlobals.NotificationsServerALL,
                        isSender: false,
                        "INVOICE_RESPONSE_RECEIVED",
                        partyFilter: "0208:" + pair.Identifier
                    );

                    // Show messagebox only when there is a count of notifications is not empty
                    // Parse once and work with the notifications array
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
                        // Display HTML for received notifications                        
                        // First create heading for Receiver notifications
                        htmlReactionsReceived.Append("<h3>Ontvangen Reacties:</h3>");
                        // Now create html table from the notifications for better readability                        
                        htmlReactionsReceived.Append(
                            "<table border='0' cellpadding='6' cellspacing='0' " +
                            "style='border-collapse:collapse;font-family:Arial,Helvetica,sans-serif;font-size:12px;'>");

                        htmlReactionsReceived.Append(
                            "<tr style='background-color:#f0f0f0;font-weight:bold;border-bottom:1px solid #cccccc;'>" +
                            "<th align='left' style='border-bottom:1px solid #cccccc;'>Verzender</th>" +
                            "<th align='left' style='border-bottom:1px solid #cccccc;'>Peppol Datum/Uur</th>" +
                            "<th align='left' style='border-bottom:1px solid #cccccc;'>Status</th>" +
                            "<th align='left' style='border-bottom:1px solid #cccccc;'>Document Id</th>" +
                            "</tr>");

                        bool isOdd = false;
                        foreach (var received in notificationsReactionsReceived)
                        {
                            string date = received["notificationDate"]?.ToString();
                            // string eventType = receive["eventType"]?.ToString();
                            string documentStatus = received["documentStatus"]?.ToString();
                            string documentId = received["documentId"]?.ToString();
                            string kboSender = received["sender"]?.ToString();

                            // try to get the company name of the sender
                            string kboSenderName = await ViesLookUpName(kboSender);
                            string kboNameResult = (kboSender == kboSenderName) ? kboSender : kboSenderName;

                            string rowColor = isOdd ? "#ffffff" : "#f9f9f9";
                            isOdd = !isOdd;

                            htmlReactionsReceived.Append(
                                "<tr style='background-color:" + rowColor + ";'>" +
                                "<td style='border-bottom:1px solid #eeeeee;'>" + kboNameResult + "</td>" +
                                "<td style='border-bottom:1px solid #eeeeee;'>" + date + "</td>" +
                                "<td style='border-bottom:1px solid #eeeeee;'>" + documentStatus + "</td>" +
                                "<td style='border-bottom:1px solid #eeeeee;'>" + documentId + "</td>" +
                                "</tr>");
                        }
                        htmlReactionsReceived.Append("</table>");
                    }

                    if (totalCount != 0)
                    {
                        // Combine all HTML parts
                        var html = new StringBuilder();
                        html.Append(htmlBegin);
                        html.Append(htmlReceiver);
                        html.Append(htmlReactionsSent);
                        html.Append(htmlSender);
                        html.Append(htmlReactionsReceived);
                        // 

                        html.Append(
                            "<p style='font-size:10px;color:#ff0000;margin-bottom:4px;'>Let op:</p>" +
                            "<ul style='font-size:10px;color:#00008b;margin-top:0;margin-bottom:8px;padding-left:18px;'>" +
                            "<li>Peppol aankoop- en verkoopdocumenten met status betaald worden gecontroleerd verwijderd.</li>" +
                            "<li>Ingeboekte Peppol-documenten zonder betaalverrichtingen worden voorlopig niet verwijderd, Peppol-verzenddatum (zie kolom 2).</li>" +
                            "<li>Een kopij van alle Peppol aankoop- en verkoopdocumenten blijft beschikbaar via het archief (gesorteerd per jaar/week).</li>" +
                            "</ul>");

                        // Now get the email address of the legal entity from company 001
                        string clientEmail = GetLegalEntityEmail(pair.Identifier);

                        // Now send the email with the HTML content                    
                        string resultSendingMail =
                            PeppolMailSender.SendPeriodOverviewPeppolMail(
                                smtpServer: MyApiSecrets.smtpServer,
                                smtpPort: MyApiSecrets.smtpPort,
                                enableSsl: MyApiSecrets.smtpUseSsl,
                                smtpUser: MyApiSecrets.smtpUsername,
                                smtpPassword: MyApiSecrets.smtpPassword,
                                fromEmail: MyApiSecrets.marSyncAdminEmail,
                                fromName: MyApiSecrets.marSyncAdminName,
                                clientEmail: "josvermoesen@outlook.be", // debugging/testing always with josvermoesen, otherwise clientEmail,
                                reportHtml: html.ToString(),
                                log: Console.WriteLine
                            );
                        ToolStripStatusLabel.Text = resultSendingMail;
                    }
                }
            }
            ToolStripStatusLabel.Text = "Klaar";
            Cursor.Current = Cursors.Default;
        }
    }
}