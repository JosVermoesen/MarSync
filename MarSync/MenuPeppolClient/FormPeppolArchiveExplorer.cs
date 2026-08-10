using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static MarSync.MarHelpers;

namespace MarSync.MenuPeppolClient
{
    public partial class FormPeppolArchiveExplorer : Form
    {
        public string SelectedFilePath { get; private set; }

        public FormPeppolArchiveExplorer()
        {
            InitializeComponent();
            Text = "Peppol Archief Explorer";

            TreeView.AfterSelect += TreeViewArchive_AfterSelect;
            LoadTree();
        }

        private void LoadTree()
        {
            TreeView.Nodes.Clear();

            string rootPath = SharedGlobals.CloudCompanyPeppolArchiveLocation;
            if (string.IsNullOrWhiteSpace(rootPath) || !Directory.Exists(rootPath))
            {
                MessageBox.Show(
                    "Archieflocatie niet gevonden:\n\n" + rootPath,
                    "Peppol archief",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Years (subfolders of root) as top-level nodes, ordered descending
            foreach (var yearDir in Directory.GetDirectories(rootPath)
                                             .OrderByDescending(d => d))
            {
                string yearName = Path.GetFileName(yearDir);
                var yearNode = new TreeNode(yearName)
                {
                    Tag = yearDir
                };

                // Weeks (WeekNN under year), ordered descending by week number
                var weekDirs = Directory.GetDirectories(yearDir, "Week*")
                                        .OrderByDescending(d =>
                                        {
                                            string name = Path.GetFileName(d); // e.g. "Week01"
                                            if (name.Length >= 6 &&
                                                int.TryParse(name.Substring(4, 2), out int weekNum))
                                            {
                                                return weekNum;
                                            }
                                            return 0;
                                        });

                foreach (var weekDir in weekDirs)
                {
                    string weekName = Path.GetFileName(weekDir);
                    var weekNode = new TreeNode(weekName)
                    {
                        Tag = weekDir
                    };

                    foreach (var xmlFile in Directory.GetFiles(weekDir, "*.xml")
                                                     .OrderBy(f => f))
                    {
                        string fileName = Path.GetFileName(xmlFile);
                        var fileNode = new TreeNode(fileName)
                        {
                            Tag = xmlFile
                        };
                        weekNode.Nodes.Add(fileNode);
                    }

                    yearNode.Nodes.Add(weekNode);
                }

                TreeView.Nodes.Add(yearNode);
                yearNode.Expand();
            }
        }

        private void TreeViewArchive_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Only treat leaf nodes that point to actual files as selectable
            string path = e.Node.Tag as string;
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                SelectedFilePath = path;
                RichTextBoxQuickInfo.Text = GetQuickInfo(path);
                ButtonPDF.Visible = true;
                ButtonXML.Visible = true;
            }
            else
            {
                SelectedFilePath = null;
                RichTextBoxQuickInfo.Text = "Selecteer een document";
                ButtonPDF.Visible = false;
                ButtonXML.Visible = false;
            }
            LabelFullPath.Text = SelectedFilePath ?? "Geen document geselecteerd";
            LabelFileName.Text = SelectedFilePath != null ? Path.GetFileName(SelectedFilePath) : "Geen document geselecteerd";
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private string GetQuickInfo(string pathLocation)
        {
            string documentId = 
                ReadUBLInvoice(
                    filePath: pathLocation,
                    messageBox: false,
                    justDocumentId: false,
                    justPayRef: false,
                    companyIdDocumentIdAndPayRef: false,
                    quickInfo: true);
            
            return documentId;
        }

        private void ButtonXML_Click(object sender, EventArgs e)
        {            
            var frm = new FormPeppolViewer();

            frm.LoadInvoiceFromFile(SelectedFilePath);
            frm.Show();
        }

        private void ButtonPDF_Click(object sender, EventArgs e)
        {
            ExtractPdfFromUblDocument(SelectedFilePath);
        }
    }
}
