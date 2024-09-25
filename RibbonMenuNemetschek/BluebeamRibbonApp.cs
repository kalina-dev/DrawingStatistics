#region Namespaces

using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Ribbon;
using Autodesk.Windows;

#endregion

namespace NemetschekRinnonMenuApp
{
    public class BluebeamRibbonApp : IExtensionApplication
    {
        #region IExtensionApplication Members

        public void Initialize()
        {
            if (ComponentManager.Ribbon == null)
            {
                ComponentManager.ItemInitialized += this.RibbonCompInitialized;
            }
            else
            {
                this.ApplicationSetup();
            }

        }

        public void ApplicationSetup()
        {
            RibbonNemetschek.SetupRibbon();
            RibbonServices.RibbonPaletteSet.WorkspaceLoaded += (this.RibbonPaletteLoaded);
            RibbonServices.RibbonPaletteSet.WorkspaceUnloading += (this.RibbonPaletteUnloaded);
        }

        private void RibbonPaletteLoaded(object sender, EventArgs e)
        {
            RibbonNemetschek.SetupRibbon();
        }

        private void RibbonPaletteUnloaded(object sender, EventArgs e)
        {
            RibbonServices.RibbonPaletteSet.RibbonControl.Tabs.Remove(RibbonServices.RibbonPaletteSet.RibbonControl.FindTab(RibbonNemetschek.SAMPLERIBBONSTABID));
        }


        private void RibbonCompInitialized(object sender, RibbonItemEventArgs e)
        {
            if (ComponentManager.Ribbon != null)
            {
                ComponentManager.ItemInitialized -= this.RibbonCompInitialized;
                this.ApplicationSetup();
            }
        }

        public void Terminate()
        {
        }

        #endregion
    }
}
