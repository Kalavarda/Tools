using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Craft.Model;

namespace Craft.Windows
{
    public partial class RecipesWindow
    {
        private readonly Project _project;
        private readonly ObservableCollection<Recipe> _recipes = new ObservableCollection<Recipe>();

        private IReadOnlyCollection<Recipe> SelectedItems
        {
            get
            {
                var items = _lb.SelectedItems.OfType<Recipe>();
                return items.ToArray();
            }
        }

        public RecipesWindow()
        {
            InitializeComponent();

            _lb.ItemsSource = _recipes;
        }

        public RecipesWindow(Project project): this()
        {
            _project = project ?? throw new ArgumentNullException(nameof(project));

            foreach (var recipe in _project.Recipes)
                _recipes.Add(recipe);
        }

        private void Tune()
        {
            _btnEdit.IsEnabled = SelectedItems.Count == 1;
        }

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            var recipe = new Recipe
            {
                Id = Guid.NewGuid()
            };
            var window = new RecipeWindow(recipe, _project) { Owner = this };
            if (window.ShowDialog() == true)
            {
                _project.Add(recipe);
                _recipes.Add(recipe);
            }
        }

        private void ListBoxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Tune();
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            var recipe = SelectedItems.FirstOrDefault();
            if (recipe == null)
                return;

            var window = new RecipeWindow(recipe, _project) { Owner = this };
            if (window.ShowDialog() == true)
            {
                Tune();
            }
        }
    }
}
