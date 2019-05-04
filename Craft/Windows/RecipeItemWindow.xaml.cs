using System;
using System.Linq;
using System.Windows;
using Craft.Model;

namespace Craft.Windows
{
    public partial class RecipeItemWindow
    {
        private readonly RecipeItem _recipeItem;
        private readonly Project _project;

        public RecipeItemWindow()
        {
            InitializeComponent();
        }

        public RecipeItemWindow(RecipeItem recipeItem, Project project): this()
        {
            _recipeItem = recipeItem ?? throw new ArgumentNullException(nameof(recipeItem));
            _project = project ?? throw new ArgumentNullException(nameof(project));

            _tbItem.ItemsSource = project.Items.OrderBy(i => i.Name);
            _tbItem.SelectedItem = project.Items.FirstOrDefault(i => i.Id == recipeItem.ItemId);
            _tbCount.Text = _recipeItem.Count.ToString();
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = _tbItem.SelectedItem as Item;
                if (item == null)
                    throw new Exception("Нужно выбрать предмет");

                _recipeItem.ItemId = item.Id;
                _recipeItem.Count = int.Parse(_tbCount.Text);
                DialogResult = true;
            }
            catch (Exception error)
            {
                App.ShowError(error);
            }
        }
    }
}
