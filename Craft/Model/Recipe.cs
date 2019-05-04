using System;
using System.Linq;

namespace Craft.Model
{
    public class Recipe
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public RecipeItem[] Items { get; set; }

        public RecipeResult Result { get; set; }

        public Recipe()
        {
            Items = new RecipeItem[0];
        }

        public override string ToString()
        {
            return Name;
        }

        public void Add(RecipeItem recipeItem)
        {
            if (recipeItem == null) throw new ArgumentNullException(nameof(recipeItem));

            if (recipeItem.Count < 1)
                throw new ArgumentException(nameof(recipeItem.Count));

            var list = Items.ToList();
            list.Add(recipeItem);
            Items = list.ToArray();
        }
    }

    public class RecipeItem
    {
        public Guid ItemId { get; set; }

        public int Count { get; set; }
    }

    public class RecipeResult
    {
        public Guid ItemId { get; set; }

        public int Count { get; set; }
    }
}
