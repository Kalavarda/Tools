using System;
using System.Windows;
using Craft.Model;

namespace Craft.Windows
{
    public partial class ItemWindow
    {
        private readonly Item _item;

        public ItemWindow()
        {
            InitializeComponent();
        }

        public ItemWindow(Item item): this()
        {
            _item = item ?? throw new ArgumentNullException(nameof(item));

            _tbId.Text = item.Id.ToString();
            _tbName.Text = _item.Name;
            if (_item.Cost != null)
                _tbCost.Text = _item.Cost.Value.ToString();
            if (_item.Count != null)
                _tbCount.Text = _item.Count.Value.ToString();
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _item.Name = _tbName.Text;

                if (!string.IsNullOrWhiteSpace(_tbCost.Text))
                    _item.Cost = decimal.Parse(_tbCost.Text);
                else
                    _item.Cost = null;

                if (!string.IsNullOrWhiteSpace(_tbCount.Text))
                    _item.Count = int.Parse(_tbCount.Text);
                else
                    _item.Count = null;

                DialogResult = true;
            }
            catch (Exception error)
            {
                App.ShowError(error);
            }
        }
    }
}
