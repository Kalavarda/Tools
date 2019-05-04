using System;
using System.Linq;

namespace Craft.Model
{
    public class Project
    {
        public Item[] Items { get; set; }

        public Recipe[] Recipes { get; set; }

        public Project()
        {
            Items = new Item[0];
            Recipes = new Recipe[0];
        }

        public void Add(Item item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (item.Id == Guid.Empty)
                throw new ArgumentException(nameof(item.Id));
            if (Items.Any(i => i.Id == item.Id))
                throw new ArgumentException(nameof(item.Id));

            var list = Items.ToList();
            list.Add(item);
            Items = list.ToArray();
        }

        public void Add(Recipe recipe)
        {
            if (recipe == null) throw new ArgumentNullException(nameof(recipe));

            if (recipe.Id == Guid.Empty)
                throw new ArgumentException(nameof(recipe.Id));
            if (Recipes.Any(i => i.Id == recipe.Id))
                throw new ArgumentException(nameof(recipe.Id));

            var list = Recipes.ToList();
            list.Add(recipe);
            Recipes = list.ToArray();
        }
    }
}
