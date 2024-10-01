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
            DBLoadUtility dbload = new DBLoadUtility();
            string result = dbload.LoadLines();
            lblInfo.Text = result;
        }

        private void BtnDrawLines_Click(object sender, EventArgs e)
        {
            DBRetrieveUtility dbr = new DBRetrieveUtility();
            string result = dbr.RetrieveAndDrawLines();
            lblInfo.Text = result;
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}
