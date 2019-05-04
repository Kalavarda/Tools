using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Craft.Model;

namespace Craft
{
    public class RecipeConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RecipeItem recipeItem)
            {
                if (targetType == typeof(string))
                {
                    var project = ProjectManager.Instance.Project;
                    if (project == null)
                        return null;

                    var item = project.Items.FirstOrDefault(i => i.Id == recipeItem.ItemId);
                    if (item == null)
                        return "<ПРЕДМЕТ НЕ НАЙДЕН>";

                    return item.Name + " / " + recipeItem.Count;
                }
            }

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
