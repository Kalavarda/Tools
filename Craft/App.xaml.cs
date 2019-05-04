using System;
using System.Windows;

namespace Craft
{
    public partial class App
    {
        public static void ShowError(Exception error)
        {
            MessageBox.Show(error.GetBaseException().Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
