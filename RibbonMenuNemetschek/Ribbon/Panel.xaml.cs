
using System.Windows;
using System.Windows.Controls;


namespace NemetschekRinnonMenuApp
{
    using Autodesk.AutoCAD.Internal;

    /// <summary>
    /// Interaction logic for SamplePanel.xaml
    /// </summary>
    public partial class Panel : UserControl
    {
        public Panel()
        {
            InitializeComponent();
        }

        private void CancelCommands()
        {
            if (((short)Autodesk.AutoCAD.ApplicationServices.Core.Application.GetSystemVariable("CMDACTIVE")) != 0)
            {
                Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.SendStringToExecute("\x001b\x001b", false, true, false);
                Utils.PostCommandPrompt();
            }
        }

        private void LineButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.CancelCommands();
            Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.SendStringToExecute("Line\n", false, true, true);
        }
    }
}
