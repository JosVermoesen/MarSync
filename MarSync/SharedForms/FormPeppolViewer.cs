using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

public class FormPeppolViewer : Form
{
    private readonly WebView2 _webView;

    public FormPeppolViewer()
    {
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Peppol XML Document";
        Width = 900;
        Height = 700;        

        _webView = new WebView2
        {
            Dock = DockStyle.Fill
        };
        Controls.Add(_webView);

        Shown += async (sender, args) => await InitializeWebViewAsync();
    }

    private async Task InitializeWebViewAsync()
    {
        if (_webView.CoreWebView2 == null)
        {
            try
            {
                // Let WebView2 create / reuse its own environment
                await _webView.EnsureCoreWebView2Async();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "WebView2 kon niet worden geïnitialiseerd.\r\n" + ex.Message,
                    "WebView2 fout",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }

    public async void LoadInvoiceFromFile(string xmlPath)
    {
        if (string.IsNullOrWhiteSpace(xmlPath) || !File.Exists(xmlPath))
        {
            MessageBox.Show("XML bestand niet gevonden.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        string xsltPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XSLT", "peppol-invoice.xslt");
        string html = PeppolInvoiceRenderer.TransformXmlToHtml(xmlPath, xsltPath);

        await InitializeWebViewAsync();
        if (_webView.CoreWebView2 != null)
        {
            _webView.NavigateToString(html);
        }
    }

    public async void LoadHtmlContent(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            MessageBox.Show("Geen HTML inhoud beschikbaar.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        await InitializeWebViewAsync();
        if (_webView.CoreWebView2 != null)
        {
            _webView.NavigateToString(html);
        }
    }
}
