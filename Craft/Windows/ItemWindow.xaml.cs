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
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _item.Name = _tbName.Text;
                DialogResult = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                throw;
            }
        }
    }
}
