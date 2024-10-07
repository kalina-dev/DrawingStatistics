
using System.Windows;
using System.Windows.Controls;
using Autodesk.AutoCAD.Internal;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace NemetschekRinnonMenuApp
{
    public partial class Panel : UserControl
    {
        public Panel()
        {
            InitializeComponent();
        }

        private void CancelCommands()
        {
            if (((short)Application.GetSystemVariable("CMDACTIVE")) != 0)
            {
                Application.DocumentManager.MdiActiveDocument.SendStringToExecute("\x001b\x001b", false, true, false);
                Utils.PostCommandPrompt();
            }
        }

        private void LineButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.CancelCommands();
            Application.DocumentManager.MdiActiveDocument.SendStringToExecute("Line\n", false, true, true);
        }
    }
}
