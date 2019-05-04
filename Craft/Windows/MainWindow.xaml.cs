using System.IO;
using System.Windows;
using Craft.Windows;

namespace Craft
{
    public partial class MainWindow
    {
        private ItemsWindow _itemsWindow;
        private RecipesWindow _recipesWindow;
        private CraftWindow _craftWindow;

        public MainWindow()
        {
            InitializeComponent();

            ProjectManager.Instance.ProjectChanged += Tune;
            ProjectManager.Instance.FileChanged += Tune;

            ProjectManager.Instance.New();

            Tune();
        }

        private void OnItemsWindowClick(object sender, RoutedEventArgs e)
        {
            if (_itemsWindow == null)
            {
                _itemsWindow = new ItemsWindow(ProjectManager.Instance.Project) { Owner = this };
                _itemsWindow.Closed += (o, args) => { _itemsWindow = null; };
            }

            _itemsWindow.Show();
            _itemsWindow.Focus();
        }

        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnLoadClick(object sender, RoutedEventArgs e)
        {
            ProjectManager.Instance.Open();
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            ProjectManager.Instance.Save();
        }

        private void Tune()
        {
            if (ProjectManager.Instance.Project != null && ProjectManager.Instance.File != null)
                Title = Path.GetFileNameWithoutExtension(ProjectManager.Instance.File.Name) + " - Craft";
            else
                Title = "Craft";
        }

        private void OnRecipesWindowClick(object sender, RoutedEventArgs e)
        {
            if (_recipesWindow == null)
            {
                _recipesWindow = new RecipesWindow(ProjectManager.Instance.Project) { Owner = this };
                _recipesWindow.Closed += (o, args) => { _recipesWindow = null; };
            }

            _recipesWindow.Show();
            _recipesWindow.Focus();
        }

        private void OnCraftClick(object sender, RoutedEventArgs e)
        {
            if (_craftWindow == null)
            {
                _craftWindow = new CraftWindow(ProjectManager.Instance.Project) { Owner = this };
                _craftWindow.Closed += (o, args) => { _craftWindow = null; };
            }

            _craftWindow.Show();
            _craftWindow.Focus();
        }
    }
}
