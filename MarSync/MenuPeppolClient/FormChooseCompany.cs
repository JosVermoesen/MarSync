using MarSync.Classes.Ademico;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vsoft.Mail;
using static QRCoder.PayloadGenerator;

namespace MarSync.MarioMenu.Actions
{
    public partial class FormChooseCompany : Form
    {        
        public FormChooseCompany()
        {
            InitializeComponent();
            LabelMimDataLocation.Text = SharedGlobals.MimDataLocation;
            Text = "MarSync - Bedrijf Activeren";
            if (LabelMimDataLocation.Text.Length > 0)
            {
                FillCompanyList();
            }
        }

        private void CheckBoxIsAdmin_CheckedChanged(object sender, System.EventArgs e)
        {
            TextBoxIsAdminPassword.Visible = CheckBoxIsAdmin.Checked;
            ButtonValidate.Visible = CheckBoxIsAdmin.Checked;

        }
        private void ButtonValidate_Click(object sender, System.EventArgs e)
        {
            if (TextBoxIsAdminPassword.Text == "Thequickbrownfoxjumpsoverthelazydog123!")
            {
                SharedGlobals.IsAdmin = true;
                Properties.Settings.Default.IsAdmin = true;
                Properties.Settings.Default.Save();
                MessageBox.Show("Admin rechten verkregen.\nHerstart het programma om de admin rechten beschikbaar te maken.");
                TextBoxIsAdminPassword.Visible = false;
                ButtonValidate.Visible = false;
                CheckBoxIsAdmin.Checked = false;
            }
            else
            {
                Properties.Settings.Default.IsAdmin = false;
                Properties.Settings.Default.Save();
                SharedGlobals.IsAdmin = false;
                MessageBox.Show("Foutief paswoord");
            }
            TextBoxIsAdminPassword.Text = "";

        }
        private void ButtonClose_Click(object sender, System.EventArgs e)
        {
            SharedGlobals.NotificationsReceiverALL = "";
            SharedGlobals.NotificationsSenderALL = "";
            Close();
        }
        private void ListBoxCompanies_Click(object sender, System.EventArgs e)
        {           
            if (ListBoxCompanies.SelectedItem != null)
            {
                bool forceClose = false;
                string selectedItem = ListBoxCompanies.SelectedItem.ToString();
                int separatorPos = selectedItem.IndexOf(" - ");
                

                if (separatorPos > 0)
                {
                    string selectedCompany = selectedItem.Substring(0, separatorPos);
                    // check if company is 098 to 099 (demo companies)
                    if (selectedCompany == "098" && SharedGlobals.ApiModus == "PRODUCTIE" || selectedCompany == "099" && SharedGlobals.ApiModus == "PRODUCTIE")
                    {
                        MessageBox.Show(
                            "Waarschuwing: De demo bedrijven 098 en 099 zijn niet geschikt voor productie gebruik.\n\n" +
                            "Gelieve een ander bedrijf te kiezen.",
                            SharedGlobals.ApiModus,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        return;
                    }

                    // check if company is 001 to 097 (real companies)  
                    if (int.TryParse(selectedCompany, out int companyNumber))
                    {
                        if (companyNumber >= 1 && companyNumber <= 97 && SharedGlobals.ApiModus == "TESTMODE")
                        {
                            MessageBox.Show(
                                "Waarschuwing: De echte bedrijven 001 tot en met 097 zijn niet geschikt voor test gebruik.\n\n" +
                                "Gelieve een ander bedrijf te kiezen.",
                                SharedGlobals.ApiModus,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            return;
                        }
                    }
                    MarHelpers.SetCompanyGlobals(selectedCompany);

                    // First check if both company email addresses available and valid
                    if (SharedGlobals.CompanyEmailAddress.Trim() == "")
                    {
                        MessageBox.Show("E-mailadres voor het bedrijf is niet ingevuld.\n\nControleer het e-mailadres. Een e-mailadres is verplicht.",
                                        "Email Adres bedrijf",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        forceClose = true;
                    }
                    else
                    {
                        if (!PeppolMailSender.IsValidEmail(SharedGlobals.CompanyEmailAddress))
                        {
                            MessageBox.Show("Ongeldig e-mailadres voor het bedrijf.\n\nControleer het e-mailadres. Een e-mailadres is verplicht.",
                                            "Email Adres bedrijf",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                            forceClose = true;
                        }
                    }

                    // Check if company contact email address is filled in
                    if (SharedGlobals.CompanyContactEmailAddress.Trim() == "")
                    {
                        MessageBox.Show("E-mailadres voor contactpersoon bedrijf is niet ingevuld.\n\nControleer het e-mailadres. Een e-mailadres is verplicht.",
                                        "Email Adres contactpersoon",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        forceClose = true;
                    }
                    else
                    {
                        if (!PeppolMailSender.IsValidEmail(SharedGlobals.CompanyContactEmailAddress))
                        {
                            MessageBox.Show("Ongeldig e-mailadres voor contactpersoon bedrijf.\n\nControleer het e-mailadres. Een e-mailadres is verplicht.",
                                            "Email Adres contactpersoon",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                            forceClose = true;
                        }
                    }

                    // Check if accounting contact email address is filled in
                    if (SharedGlobals.AccountingContactEmailAddress.Trim() == "")
                    {
                        // nothing entered so no accounting service to send
                    }
                    else
                    {
                        // Check if there is accounting contact email address
                        if (!PeppolMailSender.IsValidEmail(SharedGlobals.AccountingContactEmailAddress))
                        {
                            MessageBox.Show("Ongeldig e-mailadres voor de boekhoudcontactpersoon.\n\nControleer het e-mailadres of verwijder het adres en vul op met een ' ' spatie.",
                                            "Email Adres boekhoudcontactpersoon",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                            forceClose = true;
                        }
                    }    

                    try
                    {
                        string peppolOutDirectoryPath = SharedGlobals.MimDataLocation + "\\" + selectedCompany + "\\peppol\\out";
                        SharedGlobals.PeppolOutFiles = Directory.GetFiles(peppolOutDirectoryPath, "*.xml").Length;
                        Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Het lijkt alsof dit bedrijf belangrijke inhoudsopgaves ontbreekt\nOpen dit bedrijf eerst met marIntegraal 11.3.026 (of hoger)");
                        MarHelpers.ResetCompanyGlobals();
                        forceClose = false;
                    }

                    if (forceClose)
                    {
                        MessageBox.Show("Het bedrijf kan niet geactiveerd worden zolang de gemelde fouten niet opgelost zijn.\n\nBreng uw MarIntegraal omgeving in orde a.u.b.\n\nMarSync zal nu afgesloten worden.");
                        Application.Exit();
                    }

                    //// Check if marnt.mdv file is locked by another process                    
                    //string directoryPath = SharedGlobals.MimDataLocation + "\\" + selectedCompany;
                    //string fileName = "marnt.ldb"; // Replace with the file name you want to check

                    //// Combine the directory path and file name
                    //string filePath = Path.Combine(directoryPath, fileName);

                    //// Check if the file exists
                    //if (File.Exists(filePath))
                    //{
                    //    MessageBox.Show("Dît bedrijf wordt nog bewerkt in een marIntegraal sessie.\n\n" +
                    //    "Gelieve het bedrijf in marIntegraal te sluiten en probeer daarna opnieuw.",
                    //    "Bedrijf: " + selectedCompany,
                    //    MessageBoxButtons.OK,
                    //    MessageBoxIcon.Warning);
                    //    MarHelpers.ResetCompanyGlobals();
                    //    return;
                    //}

                    // End check save last selected company next session
                    Properties.Settings.Default.LastSelectedCompany = selectedCompany;
                    SharedGlobals.LastSelectedCompany = selectedCompany;

                    var repo = new PeppolDocumentRepository(SharedGlobals.MimDataLocation + SharedGlobals.MarntMdvLocation);
                    repo.EnsurePeppolTableExists(SharedGlobals.MimDataLocation + SharedGlobals.MarntMdvLocation);

                    Properties.Settings.Default.Save();
                }

            }
        }

        private void FillCompanyList()
        {
            ListBoxCompanies.Items.Clear();

            string MyPath = LabelMimDataLocation.Text;
            ResultExploringFileSystem(MyPath);
        }
        private bool ResultExploringFileSystem(string path)
        {
            if (path == "" || path == null)
            {
                return false;
            }

            if (Directory.Exists(path))
            {
                ProcessDirectory(path, "companyFolder");
                return true;
            }
            else
            {
                MessageBox.Show(path + " is not a valid directory");
                return false;
            }
        }
        private void ProcessDirectory(string targetDirectory, string listType)
        {
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                if (listType == "companyFolder")
                {
                    int subDirLength = subdirectory.Length;
                    string subDirMap = subdirectory.Substring(subDirLength - 3);
                    ProcessMarntTxtFile(subdirectory, subDirMap);
                }
            }
        }
        private void ProcessMarntTxtFile(string companyPath, string mapName)
        {
            string marntTxtPath = companyPath + @"\marnt.txt";
            if (File.Exists(marntTxtPath))
            {
                AddToMimList(marntTxtPath, mapName);
            }
            else
            {
                MessageBox.Show(marntTxtPath + " is not a valid file");
            }
        }
        private void AddToMimList(string marntTxtPath, string mapName)
        {
            // TODO add content to list view
            try
            {
                var stream = new StreamReader(marntTxtPath);
                if (stream.Peek() > -1)    // not EOF
                {
                    string line = stream.ReadLine();
                    ListBoxCompanies.Items.Add(mapName + " - " + line);
                }
                stream.Close();
            }
            catch (IOException e)
            {
                MessageBox.Show(marntTxtPath + " could not be read");
                MessageBox.Show(e.Message);
            }
        }
        
        private void FormChooseCompany_Load(object sender, EventArgs e)
        {
            if (SharedGlobals.UserTestGuid != MyApiSecrets.guidTestAccess)
            {
                LabelTestGuid.Visible = true;
                TextBoxTestGuidToValidate.Visible = true;
                ButtonValidateTestGuid.Visible = true;                
            }

            if (SharedGlobals.UserProdGuid != MyApiSecrets.guidProdAccess)
            {
                LabelProdGuid.Visible = true;
                TextBoxProdGuidToValidate.Visible = true;
                ButtonValidateProdGuid.Visible = true;
            }

            if (!SharedGlobals.UserCanToggleTabs)
            {
                LabelToggleTabs.Visible = true;
                TextBoxGuidToggleTabsToValidate.Visible = true;
                ButtonValidateToggleTabs.Visible = true;
            }

            if (SharedGlobals.ApiModus == "TESTMODE")
            {
                RadioButtonTestMode.Checked = true;
            }
            else
            {
                RadioButtonProductionMode.Checked = true;
            }
        }

        private void RadioButtonTestMode_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButtonTestMode.Checked)
            {
                MarHelpers.SetApiMode("TESTMODE");
                Properties.Settings.Default.PeppolTestMode = true;
                Properties.Settings.Default.Save();                
            }
        }

        private void RadioButtonProductionMode_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButtonProductionMode.Checked)
            {
                if (SharedGlobals.UserProdGuid.Trim() != "")
                {
                    MarHelpers.SetApiMode("PRODUCTIE");
                    Properties.Settings.Default.PeppolTestMode = false;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    MessageBox.Show("Waarschuwing: om de productie modus te kunnen gebruiken, moet u eerst een geldige Productie GUID ingeven.\n\n" +
                        "Gelieve een geldige Productie GUID in te geven.", SharedGlobals.ApiModus);
                    RadioButtonTestMode.Checked = true;
                    RadioButtonProductionMode.Checked = false;
                    RadioButtonProductionMode.Enabled = false;
                }
            }
        }

        private void RadioButtonTestMode_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void RadioButtonProductionMode_Click(object sender, EventArgs e)
        {            
            Application.Restart();
        }

        private void ListBoxCompanies_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ListBoxCompanies_Click (this, new EventArgs());
            }
        }

        private void ButtonValidateProdGuid_Click(object sender, EventArgs e)
        {
            string guidProdInput = TextBoxProdGuidToValidate.Text;
            if (guidProdInput.Length != 36)
            {
                MessageBox.Show("Waarschuwing: de ingegeven Productie GUID is niet correct.\nEen geldige GUID heeft 36 tekens.");
                TextBoxProdGuidToValidate.Text = "";
                return;
            }

            if (guidProdInput != MyApiSecrets.guidProdAccess)
            {
                MessageBox.Show("Waarschuwing: de ingegeven Productie GUID komt niet overeen met de verwachte GUID.\nControleer of de GUID correct is.");
                TextBoxProdGuidToValidate.Text = "";
                return;
            }
            SharedGlobals.UserProdGuid = TextBoxProdGuidToValidate.Text;
            MessageBox.Show("De ingegeven Productie GUID is correct.\nU kan nu aan de slag met Peppol voor marIntegraal.");
            Properties.Settings.Default.ProdGuidToControl = SharedGlobals.UserProdGuid;
            Properties.Settings.Default.PeppolTestMode = false;
            Properties.Settings.Default.Save();

            LabelProdGuid.Visible = false;
            TextBoxProdGuidToValidate.Visible = false;
            ButtonValidateProdGuid.Visible = false;

        }

        private void ButtonValidateTestGuid_Click(object sender, EventArgs e)
        {
            string guidTestInput = TextBoxTestGuidToValidate.Text;
            if (guidTestInput.Length != 36)
            {
                MessageBox.Show("Waarschuwing: de ingegeven GUID is niet correct.\nEen geldige GUID heeft 36 tekens.");
                TextBoxTestGuidToValidate.Text = "";
                return;
            }

            if (guidTestInput != MyApiSecrets.guidTestAccess)
            {
                MessageBox.Show("Waarschuwing: de ingegeven Test GUID komt niet overeen met de verwachte GUID.\nControleer of de GUID correct is.");
                TextBoxTestGuidToValidate.Text = "";
                return;
            }
            SharedGlobals.UserTestGuid = TextBoxTestGuidToValidate.Text;
            MessageBox.Show("De ingegeven Test GUID is correct.\nU kan nu aan de slag met Peppol voor marIntegraal.");
            Properties.Settings.Default.TestGuidToControl = SharedGlobals.UserTestGuid;
            Properties.Settings.Default.Save();

            LabelTestGuid.Visible = false;
            TextBoxTestGuidToValidate.Visible = false;
            ButtonValidateTestGuid.Visible = false;
        }

        private void ButtonValidateToggleTabs_Click(object sender, EventArgs e)
        {
            string guidToggleTabsInput = TextBoxGuidToggleTabsToValidate.Text;
            if (guidToggleTabsInput.Length != 36)
            {
                MessageBox.Show("Waarschuwing: de ingegeven GUID is niet correct.\nEen geldige GUID is 36 tekens lang.");
                TextBoxGuidToggleTabsToValidate.Text = "";
                return;
            }

            if (guidToggleTabsInput != MyApiSecrets.guidToggleTabs)
            {
                MessageBox.Show("Waarschuwing: de ingegeven GUID komt niet overeen met de verwachte GUID.\nControleer of de GUID correct is.");
                TextBoxGuidToggleTabsToValidate.Text = "";
                return;
            }
            SharedGlobals.UserCanToggleTabs = true;
            MessageBox.Show("De ingegeven GUID is correct.\nU krijgt toegang tot de TAB pagina's.");
            Properties.Settings.Default.UserCanToggleTabs = SharedGlobals.UserCanToggleTabs;            
            Properties.Settings.Default.Save();

            LabelToggleTabs.Visible = false;
            TextBoxGuidToggleTabsToValidate.Visible = false;
            ButtonValidateToggleTabs.Visible = false;
        }
    }
}
