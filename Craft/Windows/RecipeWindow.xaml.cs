using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Craft.Model;

namespace Craft.Windows
{
    public partial class RecipeWindow
    {
        private readonly Recipe _recipe;
        private readonly Project _project;
        private readonly ObservableCollection<RecipeItem> _recipeItems = new ObservableCollection<RecipeItem>();

        private IReadOnlyCollection<RecipeItem> SelectedItems
        {
            get
            {
                var items = _lb.SelectedItems.OfType<RecipeItem>();
                return items.ToArray();
            }
        }

        public RecipeWindow()
        {
            InitializeComponent();

            _lb.ItemsSource = _recipeItems;
        }

        public RecipeWindow(Recipe recipe, Project project): this()
        {
            _recipe = recipe ?? throw new ArgumentNullException(nameof(recipe));
            _project = project ?? throw new ArgumentNullException(nameof(project));

            _tbId.Text = _recipe.Id.ToString();
            _tbName.Text = _recipe.Name;

            foreach (var item in _recipe.Items)
                _recipeItems.Add(item);

            _tbResult.ItemsSource = project.Items.OrderBy(i => i.Name);
            if (recipe.Result != null)
            {
                _tbResult.SelectedItem = project.Items.FirstOrDefault(i => i.Id == recipe.Result.ItemId);
                _tbCount.Text = recipe.Result.Count.ToString();
            }
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _recipe.Name = _tbName.Text;

                if (_tbResult.SelectedItem is Item resultItem)
                    _recipe.Result = new RecipeResult
                    {
                        ItemId = resultItem.Id,
                        Count = int.Parse(_tbCount.Text)
                    };
                else
                    _recipe.Result = null;

                DialogResult = true;
            }
            catch (Exception error)
            {
                App.ShowError(error);
            }
        }

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            var recipeItem = new RecipeItem
            {
                Count = 1
            };
            var window = new RecipeItemWindow(recipeItem, _project) { Owner = this };
            if (window.ShowDialog() == true)
            {
                _recipe.Add(recipeItem);
                _recipeItems.Add(recipeItem);
            }
        }

        private void ListBoxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Tune();
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            var recipeItem = SelectedItems.FirstOrDefault();
            if (recipeItem == null)
                return;

            var window = new RecipeItemWindow(recipeItem, _project) { Owner = this };
            if (window.ShowDialog() == true)
            {
                Tune();
            }
        }

        private void Tune()
        {
            _btnEdit.IsEnabled = SelectedItems.Count == 1;
        }
    }
}
