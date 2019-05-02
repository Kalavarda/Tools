using System.Windows;
using Craft.Model;
using Craft.Windows;

namespace Craft
{
    public partial class MainWindow
    {
        private ItemsWindow _itemsWindow;
        private Project _project;

        public MainWindow()
        {
            InitializeComponent();

            _project = new Project();
        }

        private void OnItemsWindowClick(object sender, RoutedEventArgs e)
        {
            if (_itemsWindow == null)
            {
                _itemsWindow = new ItemsWindow(_project) { Owner = this };
                _itemsWindow.Closed += (o, args) => { _itemsWindow = null; };
            }

            _itemsWindow.Show();
        }
    }
}
