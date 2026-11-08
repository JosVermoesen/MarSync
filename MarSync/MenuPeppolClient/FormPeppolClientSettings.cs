using MarSync.Classes.Ademico;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarSync.MenuPeppolClient
{
    public partial class FormPeppolClientSettings : Form
    {
        bool _isAdmin;
        public FormPeppolClientSettings()
        {
            InitializeComponent();
        }
        
        private void ButtonValidateToggleTabs_Click(object sender, EventArgs e)
        {
            string guidToggleTabsInput = TextBoxGuidToggleTabsToValidate.Text;
            if (guidToggleTabsInput.Length != 36)
            {
                MessageBox.Show("De ingegeven GUID is niet correct.\nEen geldige GUID is 36 tekens lang.","Waarschuwing");
                TextBoxGuidToggleTabsToValidate.Text = "";
                return;
            }

            if (guidToggleTabsInput != MyApiSecrets.guidToggleTabs)
            {
                MessageBox.Show("De ingegeven GUID komt niet overeen met de verwachte GUID.\nControleer of de GUID correct is.","Waarschuwing");
                TextBoxGuidToggleTabsToValidate.Text = "";
                return;
            }
            SharedGlobals.UserCanToggleTabs = true;
            MessageBox.Show("De ingegeven GUID is correct.\nU krijgt toegang tot de TAB pagina's.","Waarschuwing");
            Properties.Settings.Default.UserCanToggleTabs = SharedGlobals.UserCanToggleTabs;
            Properties.Settings.Default.Save();

            LabelToggleTabs.Visible = false;
            TextBoxGuidToggleTabsToValidate.Visible = false;
            ButtonValidateToggleTabs.Visible = false;
        }

        private void ButtonValidate_Click(object sender, EventArgs e)
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

        private void ButtonValidateProdGuid_Click(object sender, EventArgs e)
        {
            string guidProdInput = TextBoxProdGuidToValidate.Text;
            if (guidProdInput.Length != 36)
            {
                MessageBox.Show("De ingegeven Productie GUID is niet correct.\nEen geldige GUID heeft 36 tekens.","Waarschuwing");
                TextBoxProdGuidToValidate.Text = "";
                return;
            }

            if (guidProdInput != MyApiSecrets.guidProdAccess)
            {
                MessageBox.Show("De ingegeven Productie GUID komt niet overeen met de verwachte GUID.\nControleer of de GUID correct is.","Waarschuwing");
                TextBoxProdGuidToValidate.Text = "";
                return;
            }
            SharedGlobals.UserProdGuid = TextBoxProdGuidToValidate.Text;
            MessageBox.Show("De ingegeven Productie GUID is correct.\nU kan nu aan de slag met Peppol voor marIntegraal.","Informatie");
            Properties.Settings.Default.ProdGuidToControl = SharedGlobals.UserProdGuid;
            Properties.Settings.Default.PeppolTestMode = false;
            Properties.Settings.Default.Save();


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

            LabelTestGuid.Enabled = false;
            TextBoxTestGuidToValidate.Enabled = false;
            ButtonValidateTestGuid.Enabled = false;
        }
        private void FormPeppolClientSettings_Load(object sender, EventArgs e)
        {
            _isAdmin = Properties.Settings.Default.IsAdmin;
            if (_isAdmin)
            {
                CheckBoxIsAdmin.Checked = true;
                CheckBoxIsAdmin.Enabled = false;
            }

            if (SharedGlobals.UserTestGuid != MyApiSecrets.guidTestAccess)
            {
                TextBoxTestGuidToValidate.Text = "";
                TextBoxTestGuidToValidate.Enabled = true;
                ButtonValidateTestGuid.Enabled = true;
            }
            else
            {
                TextBoxTestGuidToValidate.Text = SharedGlobals.UserTestGuid;
                TextBoxTestGuidToValidate.Enabled = false;
                ButtonValidateTestGuid.Enabled = false;
            }

            if (SharedGlobals.UserProdGuid != MyApiSecrets.guidProdAccess)
            {
                TextBoxProdGuidToValidate.Text = "";
                TextBoxProdGuidToValidate.Enabled = true;
                ButtonValidateProdGuid.Enabled = true;
            }
            else
            {
                TextBoxProdGuidToValidate.Text = SharedGlobals.UserProdGuid;
                TextBoxProdGuidToValidate.Enabled = false;
                ButtonValidateProdGuid.Enabled = false;
            }

            if (!SharedGlobals.UserCanToggleTabs)
            {
                TextBoxGuidToggleTabsToValidate.Text = "";
                TextBoxGuidToggleTabsToValidate.Enabled = true;
                ButtonValidateToggleTabs.Enabled = true;
            }
            else
            {
                TextBoxGuidToggleTabsToValidate.Text = SharedGlobals.UserCanToggleTabs.ToString();
                TextBoxGuidToggleTabsToValidate.Enabled = false;
                ButtonValidateToggleTabs.Enabled = false;
            }

        }
        private void CheckBoxIsAdmin_CheckedChanged(object sender, EventArgs e)
        {            
            if (CheckBoxIsAdmin.Checked)
            {
                TextBoxIsAdminPassword.Visible = true;
                if (_isAdmin)
                {
                    TextBoxIsAdminPassword.Enabled = false;                    
                    ButtonValidate.Enabled = false;
                    return;
                }
                TextBoxIsAdminPassword.Enabled = true;                
                ButtonValidate.Visible = true;
                ButtonValidate.Enabled = true;
            }
            else
            {                
                TextBoxIsAdminPassword.Visible = false;
                ButtonValidate.Visible = false;
            }            
        }
    }
}
