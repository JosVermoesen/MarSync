using System;
using System.Net.Http;
using System.Windows.Forms;

namespace MarSync.MdiForms
{
    public partial class FormCheckEUVat : Form
    {
        private HttpClient httpCheck;

        public FormCheckEUVat()
        {
            InitializeComponent();
            httpCheck = new HttpClient();
            Text = "Controleer EU BTW-nummer";
        }

        async private void ButtonCheckVat_Click(object sender, EventArgs e)
        {
            string vatNumber = TextBoxVatNumber.Text;
            string countryCode = vatNumber.Substring(0, 2);
            string vat = vatNumber.Substring(2);
            LabelResponse.Text = "Bezig...";
            LabelResponseContent.Text = "Bezig...";

            string url = "https://ec.europa.eu/taxation_customs/vies/rest-api/ms/" + countryCode + "/vat/" + vat;

            httpCheck = new HttpClient();

            HttpResponseMessage response = await httpCheck.GetAsync(url);
            string responseContent = await response.Content.ReadAsStringAsync();
            LabelResponse.Text = response.ToString();
            LabelResponseContent.Text = responseContent;
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
