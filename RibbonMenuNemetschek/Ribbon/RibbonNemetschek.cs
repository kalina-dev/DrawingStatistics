namespace NemetschekRinnonMenuApp
{
    using Autodesk.AutoCAD.Ribbon;
    using Autodesk.Windows;

    public class RibbonNemetschek
    {
        public const string SAMPLERIBBONSTABID = "RibbonTab";
        public const string SAMPLETABTITLE = "Nemetschek";

        public static void SetupRibbon()
        {
            var ribbonTab = RibbonServices.RibbonPaletteSet.RibbonControl.FindTab(SAMPLERIBBONSTABID);
            if (ribbonTab == null)
            {
                ribbonTab = new RibbonTab();
                RibbonServices.RibbonPaletteSet.RibbonControl.Tabs.Add(ribbonTab);
            }
            else
            {
                foreach (var panel in ribbonTab.Panels)
                {
                    panel.IsFloating = false;
                }
            }

            ribbonTab.Id = SAMPLERIBBONSTABID;
            ribbonTab.Title = SAMPLETABTITLE;
            ribbonTab.IsContextualTab = false;
            ribbonTab.IsEnabled = true;
            ribbonTab.IsVisible = true;

            var samplePanel = new Panel();
            var panel1 = new Ribbon.FirstRibbonPanel(samplePanel, "Nemetschek demo", samplePanel.Width);
            if (ribbonTab.Panels.Count > 0)
            {
                ribbonTab.Panels[0] = panel1;
            }
            else
            {
                ribbonTab.Panels.Add(panel1);
            }
        }
    }
}
