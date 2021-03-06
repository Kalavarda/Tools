﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Craft.Model;

namespace Craft.Windows
{
    public partial class CraftWindow
    {
        private readonly Project _project;

        public Recipe SelectedRecipe => _cbRecipe.SelectedItem as Recipe;

        public CraftWindow()
        {
            InitializeComponent();
        }

        public CraftWindow(Project project):this()
        {
            _project = project ?? throw new ArgumentNullException(nameof(project));

            _cbRecipe.ItemsSource = _project.Recipes.OrderBy(r => r.Name);

            Tune();

            _tbCount.TextChanged += (sender, e) => Tune();
        }

        private void _cbRecipe_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Tune();
        }

        private void Tune()
        {
            if (SelectedRecipe != null)
            {
                if (int.TryParse(_tbCount.Text, out var count))
                {
                    _grdRecipeProcess.Visibility = Visibility.Visible;
                    _cbTotalCost.Text = (_project.CalcFullCraftCost(SelectedRecipe) * count).ToString();

                    if (SelectedRecipe.Result != null)
                        _tbResultCount.Text = (SelectedRecipe.Result.Count * count).ToString();
                    else
                        _tbResultCount.Text = string.Empty;

                    var itemsCount = _project.CalcFullCraftItemsCount(SelectedRecipe).OrderByDescending(p => p.Value);
                    var list = new List<string>();
                    foreach (var pair in itemsCount)
                    {
                        var item = _project.Items.FirstOrDefault(i => i.Id == pair.Key);
                        list.Add(item.Name + " / " + pair.Value * count);
                    }
                    _lbItemsCount.ItemsSource = list;
                }
            }
            else
            {
                _grdRecipeProcess.Visibility = Visibility.Collapsed;
                _cbTotalCost.Text = string.Empty;
                _lbItemsCount.ItemsSource = null;
            }
        }
    }
}
