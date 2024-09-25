namespace NemetschekRinnonMenuApp.Ribbon
{
    using System.Windows;
    using System.Windows.Controls;

    using Autodesk.Internal.Windows;
    using Autodesk.Windows;

    public class FirstRibbonPanel : RibbonPanel
    {
        private static int IdCount = 1;
        public UserControl MainControl;
        public ContentPresenter Presenter;

        public FirstRibbonPanel(UserControl initialControl, string panelTitle, double panelWidth)
        {
            var factory = new FrameworkElementFactory(typeof(StackPanel));
            factory.AddHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(this.StackLoaded));
            var template = new MyDataTemplate();
            var item = new RibbonCompositeItem
            {
                Id = "RIBBONPANEL" + IdCount.ToString().PadLeft(2, '0')
            };
            IdCount++;
            template.MainItem = item;
            template.VisualTree = factory;
            item.Content = template;
            var source = new RibbonPanelSource
            {
                Items = { item }
            };
            this.MainControl = initialControl;
            base.Source = source;
            base.Source.Title = panelTitle;
            base.Source.Items[0].Width = panelWidth;
            base.Source.Items[0].MinWidth = panelWidth;
        }

        private void StackLoaded(object sender, RoutedEventArgs e)
        {
            ContentPresenter templatedParent = (ContentPresenter)((StackPanel)sender).TemplatedParent;
            MyDataTemplate contentTemplate = (MyDataTemplate)templatedParent.ContentTemplate;
            templatedParent.Content = this.MainControl;
            this.Presenter = templatedParent;
            templatedParent.ContentTemplate = null;
        }
    }

    public class MyDataTemplate : DataTemplate
    {
        public UserControl MainControl;
        public RibbonCompositeItem MainItem;
    }
}
