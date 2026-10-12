using ADODB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace MarSync.SharedForms
{
    public partial class FormCheckDocument : Form
    {
        // private readonly string folderOutPath;

        public FormCheckDocument()
        {
            InitializeComponent();
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            // Validate document ID length
            if (TextBoxDocumentId.Text.Length < 11)
            {
                MessageBox.Show(
                    "Het documentnummer moet minstens 11 tekens lang zijn.",
                    "Ongeldig documentnummer",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                TextBoxDocumentId.Text = "V";
                TextBoxDocumentId.Focus();
                return;
            }

            string result = GetSellersDocumentSentResultRS(
                document: TextBoxDocumentId.Text,
                eraseContent: false);

            RichTextBoxSentId.Text = result;

            // Check if you can find document-codeClient.xml in the peppol\out folder
            RichTextBoxSentId.Focus();
        }

        private void TextBoxDocumentId_TextChanged(object sender, EventArgs e)
        {
            // Enable search button only if document ID is at least 11 characters
            if (TextBoxDocumentId.Text.Length < 11)
            {
                ButtonSearch.Enabled = false;
            }
            else
            {
                ButtonSearch.Enabled = true;
            }
        }

        private string GetSellersDocumentSentResultRS(
            string document,
            bool eraseContent)
        {
            string sSQL =
                "SELECT v033, v034, v405, v406, dnnSync FROM Dokumenten WHERE v033 = '" +
                document.Substring(0, 11) + "'";

            string connectionString =
                SharedGlobals.DbJetProvider + SharedGlobals.MimDataLocation + SharedGlobals.MarntMdvLocation;

            Recordset rs = null;

            try
            {
                rs = new Recordset
                {
                    CursorLocation = CursorLocationEnum.adUseClient
                };

                rs.Open(
                    sSQL,
                    connectionString,
                    CursorTypeEnum.adOpenDynamic,
                    LockTypeEnum.adLockOptimistic);

                if (rs.RecordCount != 1)
                {
                    return string.Empty;
                }
                                
                string fieldValue405 = string.Empty;
                string fieldValue406 = string.Empty;

                try
                {
                    if (!Convert.IsDBNull(rs.Fields["v405"].Value))
                    {
                        fieldValue405 = Convert.ToString(rs.Fields["v405"].Value);    
                        RichTextBoxSentId.Text = fieldValue405;
                    }

                    string v034 = rs.Fields["v034"].Value.ToString();
                    string v034WithoutFirstChar = string.IsNullOrEmpty(v034) ? string.Empty : v034.Substring(1).Trim();
                                                            

                    string fileName = rs.Fields["v033"].Value + "-" + v034WithoutFirstChar + ".xml";
                    string folderOutPath = Path.Combine(SharedGlobals.MimDataLocation, SharedGlobals.ActiveCompany, "peppol", "out");

                    string fullPath = Path.Combine(folderOutPath, fileName);
                    bool exists = File.Exists(fullPath);

                    if (exists)
                    {
                        LabelXmlFile.Text = fullPath;
                        TextBoxDocumentId.Enabled = false;
                        Buttoninitialize.Visible = true;
                        ButtonOpenPath.Visible = true;
                    }
                    else
                    {
                        LabelXmlFile.Text = fileName + " niet gevonden. Enkel resultaten worden weergegeven.";
                    }                    

                    if (!Convert.IsDBNull(rs.Fields["v406"].Value))
                    {
                        fieldValue406 = Convert.ToString(rs.Fields["v406"].Value);                        
                        RichTextBoxReceiveId.Text = fieldValue406;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Error retrieving field value in GetSellersDocumentSentResultRS.");
                    throw;
                }

                if (eraseContent)
                {
                    fieldValue405 = string.Empty;
                    fieldValue406 = string.Empty;

                    try
                    {
                        rs.Fields["v405"].Value = fieldValue405;
                        rs.Fields["v406"].Value = fieldValue406;
                        rs.Update();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error updating field value in GetSellersDocumentSentResultRS.");
                        throw;
                    }
                }
                return fieldValue405;
            }
            catch (Exception)
            {
                Console.WriteLine("Error opening recordset in GetSellersDocumentSentResultRS.");
                throw;
            }
            finally
            {
                if (rs != null)
                {
                    try
                    {
                        if (rs.State != 0)
                        {
                            rs.Close();
                        }
                    }
                    catch
                    {
                        // ignore close errors
                    }

                    try
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(rs);
                    }
                    catch
                    {
                        // ignore release errors
                    }
                }
            }
        }

        private void Buttoninitialize_Click(object sender, EventArgs e)
        {
            string MSG = "Weet u zeker dat u de inhoud van de velden v405 en v406 wilt wissen?";
            DialogResult result = MessageBox.Show(MSG, "Bevestiging", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string documentId = TextBoxDocumentId.Text;
                GetSellersDocumentSentResultRS(documentId, eraseContent: true);
                RichTextBoxSentId.Text = string.Empty;
                RichTextBoxReceiveId.Text = string.Empty;
                MessageBox.Show("De inhoud van de velden v405 en v406 is gewist. Vergeet niet het verbeterde xml bestand te plaatsen.", "Inhoud gewist", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }

        private void ButtonOpenPath_Click(object sender, EventArgs e)
        {
            // TODO : Open the folder in Windows Explorer   
            string folder = Path.GetDirectoryName(LabelXmlFile.Text);
            if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
            {
                Process.Start("explorer.exe", $"\"{folder}\"");
            }
        }
    }
}
