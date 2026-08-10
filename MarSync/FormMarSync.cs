using IDEALSoftware.VpeCommunity;
using MarSync.Classes.Ademico;
using MarSync.MarioMenu.Actions;
using MarSync.MarioMenu.Admin;
using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MarSync
{
    public partial class FormMarSync : Form
    {
        private readonly VpeControl AutoPageBreak;

        // public Form FormDataGridJsonPopUp { get; set; }

        public FormMarSync()
        {
            InitializeComponent();

            Text = "MarSync";
            AutoPageBreak = new VpeControl();
        }

        private void MenuItemCloseApp_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MenuItemMdvToSql_Click(object sender, EventArgs e)
        {
            var form = new FormMarSqlTools();
            form.ShowDialog(this);
        }

        private void FormMario_Load(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.IsUpgraded)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.IsUpgraded = true;
                Properties.Settings.Default.Save();
            }

            if (Properties.Settings.Default.MainTop <= 0)
            {
                this.Width = 816;
                this.Height = 489;
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.Top = Properties.Settings.Default.MainTop;
                this.Left = Properties.Settings.Default.MainLeft;
                this.Width = Properties.Settings.Default.MainWidth;
                this.Height = Properties.Settings.Default.MainHeight;
            }

            if (Properties.Settings.Default.TestGuidToControl == MyApiSecrets.guidTestAccess)
            {
                SharedGlobals.UserTestGuid = Properties.Settings.Default.TestGuidToControl;
            }
            else
            {
                SharedGlobals.UserTestGuid = "";
                Properties.Settings.Default.TestGuidToControl = SharedGlobals.UserTestGuid;
                Properties.Settings.Default.Save();
            }

            if (Properties.Settings.Default.ProdGuidToControl == MyApiSecrets.guidProdAccess)
            {
                SharedGlobals.UserProdGuid = Properties.Settings.Default.ProdGuidToControl;
            }
            else
            {
                SharedGlobals.UserProdGuid = "";
                Properties.Settings.Default.ProdGuidToControl = SharedGlobals.UserProdGuid;
                Properties.Settings.Default.Save();
            }

            SharedGlobals.UserCanToggleTabs = Properties.Settings.Default.UserCanToggleTabs;

            SharedGlobals.MdvSettings2025 = Properties.Settings.Default.MdvSetting2025;
            SharedGlobals.LocalSQLSetting2025 = Properties.Settings.Default.LocalSQLSetting2025;
            SharedGlobals.HostedSQLSetting2025 = Properties.Settings.Default.HostedSQLSetting2025;

            bool apiMode = Properties.Settings.Default.PeppolTestMode;

            if (apiMode)
            {
                MarHelpers.SetApiMode("TESTMODE");
            }
            else
            {
                MarHelpers.SetApiMode("PRODUCTIE");
            }

            if (Properties.Settings.Default.LastSelectedCompany != "")
            {
                SharedGlobals.LastSelectedCompany = Properties.Settings.Default.LastSelectedCompany;
            }
        }

        private void FormMario_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.MainTop = this.Top;
            Properties.Settings.Default.MainLeft = this.Left;
            Properties.Settings.Default.MainWidth = this.Width;
            Properties.Settings.Default.MainHeight = this.Height;

            Properties.Settings.Default.MdvSetting2025 = SharedGlobals.MdvSettings2025;
            Properties.Settings.Default.LocalSQLSetting2025 = SharedGlobals.LocalSQLSetting2025;
            Properties.Settings.Default.HostedSQLSetting2025 = SharedGlobals.HostedSQLSetting2025;
            Properties.Settings.Default.Save();
        }

        private void MenuItemUserSettings_Click(object sender, EventArgs e)
        {
            //if (SharedGlobals.LastSelectedCompany != "")
            //{
            //    SharedGlobals.ActiveCompany = SharedGlobals.LastSelectedCompany; 
            //    string confirmMessage =
            //    "Verder werken met laatst geselecteerd bedrijf?\n\n " +
            //    SharedGlobals.LastSelectedCompany + "\n\n" +
            //    "Kies neen om een ander bedrijf te selecteren.";
            //    var confirmResult = MessageBox.Show(confirmMessage, "Bedrijft kiezen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    if (confirmResult != DialogResult.Yes)
            //    {
            //        var form = new FormChooseCompany();
            //        form.ShowDialog(this);
            //    }
            //}

            var form = new FormChooseCompany();
            form.ShowDialog(this);

            if (SharedGlobals.ActiveCompany != "")
            {
                if (SharedGlobals.ApiModus == "PRODUCTIE")
                {
                    Text = "MarSync - [" + SharedGlobals.ActiveCompany + "] " + SharedGlobals.CompanyName;
                }
                else if (SharedGlobals.ApiModus == "TESTMODE")
                {
                    Text = "MarSync - [" + SharedGlobals.ActiveCompany + "] " + SharedGlobals.CompanyName + " [" + SharedGlobals.ApiModus + "]";
                }
                else
                {
                    MessageBox.Show("Onbekende API modus: " + SharedGlobals.ApiModus, "Foutmelding", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                MenuItemZipCompany.Text = "Zip [" + SharedGlobals.CompanyName + "] naar Marnt Cloud";
            }
            else
            {
                Text = "MarSync" + " [" + SharedGlobals.ApiModus + "]";
                MenuItemZipCompany.Text = "Zip bedrijf naar Marnt Cloud";
            }
        }

        private void FormMario_Shown(object sender, EventArgs e)
        {
            bool isAdmin = Properties.Settings.Default.IsAdmin;
            AdminMenuItem.Visible = isAdmin; // Show or hide the Admin menu based on the IsAdmin setting             

            // Load MimDataLocation from marIntegraal settings
            // Value must contains "\marnt\data"
            string valuePath = Interaction.GetSetting(
                "marINTEGRAAL",       // AppName
                "marIntegraal",     // Section
                "Bedrijfsinhoudsopgave2025",
                "" // Default if not found
                ) ?? ""; // Ensure null-coalescing operator to handle possible null value.

            bool containsPath = valuePath.ToLower().Contains(@"\marnt\data".ToLower());
            if (!containsPath)
            {
                MessageBox.Show("De locatie van de bedrijfsinhoudsopgave is niet correct ingesteld.\n\nDuidt in marIntegraal een correcte locatie aan a.u.b.", "Fout in locatie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
            if (!Directory.Exists(valuePath))
            {
                MessageBox.Show($"marnt\\data niet gevonden:\n\n {valuePath}");
            }
            SharedGlobals.SetMimDataLocation(valuePath);

            // Load MarNT CloudLocation from settings
            valuePath = Interaction.GetSetting(
                "marINTEGRAAL",       // AppName
                "dnnInstellingen",     // Section
                "Cloud",
                "" // Default if not found
                ) ?? ""; // Ensure null-coalescing operator to handle possible null value.

            // A marNT Clouddrive Location must ends with  "\marnt"                                    
            if (!Directory.Exists(valuePath))
            {
                MessageBox.Show($"inhoudsopgave voor cloud niet gevonden:\n\n {valuePath}");
            }
            SharedGlobals.MarntCloudLocation = valuePath;

            // Load MarNT Archive CloudLocation from settings
            valuePath = Interaction.GetSetting(
                "marINTEGRAAL",       // AppName
                "dnnInstellingen",     // Section
                "Archief",
                "" // Default if not found
                ) ?? ""; // Ensure null-coalescing operator to handle possible null value.

            // A Clouddrive marNT archive Location
            if (!Directory.Exists(valuePath))
            {
                MessageBox.Show($"inhoudsopgave archief niet gevonden:\n\n {valuePath}");
            }
            SharedGlobals.MarntCLoudArchiveLocation = valuePath;

            // A Clouddrive MarNT Mario Location must end with "\marnt\mario"                        
            valuePath = Interaction.GetSetting(
                "marINTEGRAAL",       // AppName
                "dnnInstellingen",     // Section
                "Mario",
                "" // Default if not found
                ) ?? ""; // Ensure null-coalescing operator to handle possible null value.

            // A Clouddrive marNT Location must end with "\marnt"                        
            if (!Directory.Exists(valuePath))
            {
                MessageBox.Show($"inhoudsopgave voor manueel niet gevonden:\n\n {valuePath}");
            }
            SharedGlobals.MarntCloudMarioLocation = valuePath;

            // Trigger the user settings to set the company if needed
            MenuItemUserSettings_Click(string.Empty, EventArgs.Empty);
        }

        private void MenuItemPeppolActions_Click(object sender, EventArgs e)
        {
            if (SharedGlobals.CompanyKBONumber == "")
            {
                MessageBox.Show("Eerst een bedrijf activeren a.u.b.", "Kies eerst een bedrijf", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (SharedGlobals.ApiModus == "TESTMODE")
            {

                if (SharedGlobals.UserTestGuid != MyApiSecrets.guidTestAccess)
                {
                    MessageBox.Show("Eerst minstens uw TEST sleutel activeren", "Toegangsleutel ontbreekt of onjuist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            Hide();
            var form = new FormSyncPeppol();
            form.ShowDialog(this);
            Show();
        }

        private void MenuItemPeppolTesting_Click(object sender, EventArgs e)
        {
            var form = new FormAdminPeppolActions();
            form.ShowDialog(this);
        }

        private void MenuItemPeppolSettings_Click(object sender, EventArgs e)
        {
            var form = new FormAdminPeppolSettings();
            form.ShowDialog(this);

        }

        public static void ExportDocument(string FileName, VpeControl VpeDoc)
        {
            if (MessageBox.Show("Create PDF file?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                FileName = Path.GetTempPath() + FileName;
                if (VpeDoc.WriteDoc(FileName))
                {
                    MessageBox.Show("PDF file \"" + FileName + "\" created successfully.");
                    Process process = new Process();
                    process.StartInfo.FileName = FileName;
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                }
                else
                {
                    MessageBox.Show("Error creating PDF file \"" + FileName + "\"\n" +
                        "Possible reasons:\n" +
                        " - the file is open in Acrobat Reader\n" +
                        " - hard disk full\n" +
                        " - no access rights to the folder\n" +
                        " - out of memory\n" +
                        " - export module missing");
                }
            }
        }

        private void MenuItemZipCompany_Click(object sender, EventArgs e)
        {
            if (SharedGlobals.ActiveCompany == "")
            {
                MessageBox.Show("Eerst een bedrijf activeren a.u.b.", "Kies eerst een bedrijf", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if marnt.mdv file is locked by another process                    
            string directoryPath = SharedGlobals.MimDataLocation + "\\" + SharedGlobals.ActiveCompany;
            string fileName = "marnt.ldb"; // Replace with the file name you want to check

            // Combine the directory path and file name
            string filePath = Path.Combine(directoryPath, fileName);

            // Check if the file exists
            if (File.Exists(filePath))
            {
                MessageBox.Show("Dît bedrijf wordt nog bewerkt in een marIntegraal sessie.\n\n" +
                "Gelieve het bedrijf in marIntegraal te sluiten en probeer daarna opnieuw.",
                "Bedrijf: " + SharedGlobals.ActiveCompany,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                // MarHelpers.ResetCompanyGlobals();
                return;
            }

            string sourceFolderPath = SharedGlobals.MimDataLocation + "\\" + SharedGlobals.ActiveCompany;
            string cloudFolderPath = SharedGlobals.MarntCLoudArchiveLocation + "\\" + SharedGlobals.ActiveCompany; ;

            string confirmMessage = "Weet u zeker dat u bedrijf " + SharedGlobals.ActiveCompany + " wilt zippen naar Marnt Cloud?\n\n" +
                "De map wordt gezipt naar:\n" + cloudFolderPath + "\n\n" +
                "De naam van het zip-bestand is: " + SharedGlobals.ActiveCompany + "{Datum:Uur}.zip";
            var confirmResult = MessageBox.Show(confirmMessage, "Bevestig zip naar Marnt Cloud", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.No) return;

            try
            {
                Application.UseWaitCursor = true;
                Cursor.Current = Cursors.WaitCursor;
                ToolStripStatusLabel.Text = "Bezig...";
                Application.DoEvents();

                string zipResult = MarDataFilesHelper.ZipFolderToCloudDrive(sourceFolderPath, cloudFolderPath);

                var answer = MessageBox.Show(
                    zipResult + " met succes.\n\nFolder openen?", "Zip naar Marnt Cloud",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (answer == DialogResult.Yes)
                    OpenFolder(cloudFolderPath);
            }
            catch (Exception)
            {
                MessageBox.Show("Fout bij het zippen van de map naar Marnt Cloud. Controleer of de Marnt Cloud locatie correct is ingesteld.", "Foutmelding", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Application.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;
                ToolStripStatusLabel.Text = "Klaar";
                Application.DoEvents();
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

        private void VATSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new MdiForms.FormCheckEUVat
            {
                MdiParent = this // Set MDI parent
            };
            form.Show();
        }

        private void MenuItemPepolArchive_Click(object sender, EventArgs e)
        {
            if (SharedGlobals.ActiveCompany == "")
            {
                MessageBox.Show("Eerst een bedrijf activeren a.u.b.", "Kies eerst een bedrijf", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SharedGlobals.CloudCompanyPeppolArchiveLocation =
                SharedGlobals.MarntCLoudArchiveLocation +
                "\\" + SharedGlobals.ActiveCompany +
                "\\peppol\\archive"; // year//week01 till week52

            // using (var dlg = new MarSync.MarioMenu.Actions.FormPeppolArchiveBrowser())
            using (var dlg = new MarSync.MenuPeppolClient.FormPeppolArchiveExplorer())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK &&
                    !string.IsNullOrEmpty(dlg.SelectedFilePath))
                {
                    // Do whatever you want with the chosen XML file
                    MessageBox.Show(dlg.SelectedFilePath, "Gekozen archiefbestand", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void MenuItemAutoPageBreak_Click(object sender, EventArgs e)
        {
            // =====================================================================
            //                             Auto Page Break
            // =====================================================================
            OpenFileDialog SPPathFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"c:\",
                Filter = "c# txt files (*.cs)|*.cs|All files (*.txt)|*.txt",
                Multiselect = true,
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (SPPathFileDialog.ShowDialog() == DialogResult.OK)
            {
                // string fileContent = string.Empty;
                string firstFullPath; // = string.Empty;

                //Get the path of specified file
                firstFullPath = SPPathFileDialog.FileName;
                // var arrayFileNames = SPPathFileDialog.FileNames;

                string sLine, block = "";
                StreamReader stream;

                FileInfo fi = new FileInfo(firstFullPath);
                if (!fi.Exists)
                {
                    MessageBox.Show("File \"" + firstFullPath + "\" not found.");
                    return;
                }

                stream = new StreamReader(firstFullPath);
                MenuItemAutoPageBreak.Enabled = false;
                while (stream.Peek() > -1)    // not EOF
                {
                    sLine = stream.ReadLine();

                    // Replace all TAB characters with blanks, since the plain text object created with
                    // VPE.Print[Box]() or VPE.Write[Box]() does not handle TAB's
                    // NOTE: The RTF (Rich Text Format) object of the VPE Professional Edition DOES handle
                    //       tabs and also hanging indents, but this demo is intended for the
                    //       VPE Standard Edition.
                    sLine = sLine.Replace("\t", "   ");
                    block = block + "\n" + sLine;
                }
                stream.Close();

                MessageBox.Show("We loaded the file\n \"" + firstFullPath + "\"\n\n" +
                    "into memory using the slow \"ReadLine\" method.\n" +
                    "We also replaced all tab-characters with blanks.\n" +
                    "NOW we call VPE and create a document from the data.\n" +
                    "VPE will create the page breaks itself. This will work very fast!");

                AutoPageBreak.OpenDoc();
                AutoPageBreak.SelectFont("Courier New", 8);

                // Set the bottom margin, so the report will fit
                // onto A4 as well as onto US-Letter paper:
                // =============================================
                AutoPageBreak.SetOutRect(2, 2, 19, 26.5);

                // Header will be placed outside default output rectangle:
                AutoPageBreak.NoPen();
                AutoPageBreak.TextUnderline = true;
                AutoPageBreak.DefineHeader(1, 1, -7, -0.5, "Auto Text Break Demo - Page @PAGE");

                // On every intial page:
                // VLEFT   = VLEFTMARGIN
                // VTOP    = VTOPMARGIN
                // VRIGHT  = VRIGHTMARGIN
                // VBOTTOM = VTOPMARGIN !!!!!!!!!!
                AutoPageBreak.TextUnderline = false;
                AutoPageBreak.SetPen(0.03, PenStyle.Solid, Color.Black);
                AutoPageBreak.WriteBox(AutoPageBreak.nLeft, AutoPageBreak.nBottom, AutoPageBreak.nRight, AutoPageBreak.nFree, "[N TO BC LtGray CE S 12 B]Start of Listing");
                AutoPageBreak.WriteBox(AutoPageBreak.nLeft, AutoPageBreak.nBottom, AutoPageBreak.nRight, AutoPageBreak.nFree, block);
                AutoPageBreak.WriteBox(AutoPageBreak.nLeft, AutoPageBreak.nBottom, AutoPageBreak.nRight, AutoPageBreak.nFree, "[N TO BC LtGray CE S 12 B]End of Listing");

                AutoPageBreak.Preview();

                ExportDocument("Auto Break.pdf", AutoPageBreak);
                MenuItemAutoPageBreak.Enabled = true;
            }
        }
    }
}
