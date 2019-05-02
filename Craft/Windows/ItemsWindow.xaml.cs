using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Craft.Model;

namespace Craft.Windows
{
    public partial class ItemsWindow
    {
        private readonly Project _project;
        private readonly ObservableCollection<Item> _items = new ObservableCollection<Item>();

        private IReadOnlyCollection<Item> SelectedItems
        {
            get
            {
                var items = _lb.SelectedItems.OfType<Item>();
                return items.ToArray();
            }
        }

        public ItemsWindow()
        {
            InitializeComponent();

            _lb.ItemsSource = _items;

            Tune();
        }

        private void Tune()
        {
            _btnEdit.IsEnabled = SelectedItems.Count == 1;
        }

        public ItemsWindow(Project project): this()
        {
            _project = project ?? throw new ArgumentNullException(nameof(project));

            foreach(var item in _project.Items)
                _items.Add(item);
        }

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            var item = new Item
            {
                Id = Guid.NewGuid()
            };
            var window = new ItemWindow(item) { Owner = this };
            if (window.ShowDialog() == true)
            {
                _project.Add(item);
                _items.Add(item);
            }
        }

        private void ListBoxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Tune();
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            var item = SelectedItems.FirstOrDefault();
            if (item == null)
                return;

            var window = new ItemWindow(item) { Owner = this };
            if (window.ShowDialog() == true)
            {
                Tune();
            }
        }
    }
}
