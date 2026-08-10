public partial class FormPeppolViewer : Form
{
    public FormPeppolViewer()
    {
        InitializeComponent();

        // Make sure this is true (also set in designer)
        this.KeyPreview = true;

        // Subscribe to KeyDown
        this.KeyDown += FormPeppolViewer_KeyDown;
    }

    private void FormPeppolViewer_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            this.Close();
        }
    }
}