using System;
using System.Windows.Forms;

namespace DbAutocadDemoNemetschek
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnLoadLines_Click(object sender, EventArgs e)
        {
            DBLoadUtil dbload = new DBLoadUtil();
            string result = dbload.LoadLines();
            lblInfo.Text = result;
        }

        private void btnDrawLines_Click(object sender, EventArgs e)
        {
            DBRetrieveUtil dbr = new DBRetrieveUtil();

            string result = dbr.RetrieveAndDrawLines();
            lblInfo.Text = result;
        }
    }
}
