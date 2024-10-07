using AutocadSQLPlugin;

namespace AutoCAD.SQL.Plugin
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void BtnLoadLines_Click(object sender, EventArgs e)
        {
            DBLoadUtility dbload = new();
            string result = dbload.LoadLines();
            lblInfo.Text = result;
        }

        private void BtnDrawLines_Click(object sender, EventArgs e)
        {
            DBRetrieveUtility dbr = new();
            string result = dbr.RetrieveAndDrawLines();
            lblInfo.Text = result;
        }
    }
}
