using System;
using System.Collections.Generic;
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
    }

    public static class ProjectExtensions
    {
        /// <summary>
        /// Сколько будет стоить, если всё крафтить
        /// </summary>
        public static decimal CalcFullCraftCost(this Project project, Recipe recipe)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            if (recipe == null) throw new ArgumentNullException(nameof(recipe));

            decimal sum = 0;

            foreach (var recipeItem in recipe.Items)
                sum += project.CalcFullCraftCost(recipeItem);

            return sum;
        }

        private static decimal CalcFullCraftCost(this Project project, RecipeItem recipeItem)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            if (recipeItem == null) throw new ArgumentNullException(nameof(recipeItem));

            var item = project.Items.FirstOrDefault(i => i.Id == recipeItem.ItemId);
            if (item == null)
                throw new Exception();

            var recipe = project.Recipes.FirstOrDefault(r => r.Result != null && r.Result.ItemId == item.Id);
            if (recipe != null)
            {
                var itemCraftCost = project.CalcFullCraftCost(recipe);
                return recipeItem.Count * itemCraftCost;
            }
            else
                if (item.Cost != null)
                    return recipeItem.Count * item.Cost.Value;

            return 0;
        }

        /// <summary>
        /// Вычисляет сколько каких предметов потребуется для полного крафта
        /// </summary>
        public static IReadOnlyDictionary<Guid, int> CalcFullCraftItemsCount(this Project project, Recipe recipe)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            if (recipe == null) throw new ArgumentNullException(nameof(recipe));

            var dict = new Dictionary<Guid, int>();

            foreach (var recipeItem in recipe.Items)
            {
                if (dict.ContainsKey(recipeItem.ItemId))
                    dict[recipeItem.ItemId] += recipeItem.Count;
                else
                    dict.Add(recipeItem.ItemId, recipeItem.Count);

                var rec = project.Recipes.FirstOrDefault(r => r.Result != null && r.Result.ItemId == recipeItem.ItemId);
                if (rec != null)
                {
                    var d = project.CalcFullCraftItemsCount(rec);
                    foreach (var pair in d)
                    {
                        if (dict.ContainsKey(pair.Key))
                            dict[pair.Key] += pair.Value * recipeItem.Count;
                        else
                            dict.Add(pair.Key, pair.Value * recipeItem.Count);
                    }

                    //dict.Remove(recipeItem.ItemId);
                }
            }

            return dict;
        }

        public static void Add(this Project project, Item item)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (item.Id == Guid.Empty)
                throw new ArgumentException(nameof(item.Id));
            if (project.Items.Any(i => i.Id == item.Id))
                throw new ArgumentException(nameof(item.Id));

            var list = project.Items.ToList();
            list.Add(item);
            project.Items = list.ToArray();
        }

        public static void Add(this Project project, Recipe recipe)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            if (recipe == null) throw new ArgumentNullException(nameof(recipe));

            if (recipe.Id == Guid.Empty)
                throw new ArgumentException(nameof(recipe.Id));
            if (project.Recipes.Any(i => i.Id == recipe.Id))
                throw new ArgumentException(nameof(recipe.Id));

            var list = project.Recipes.ToList();
            list.Add(recipe);
            project.Recipes = list.ToArray();
        }
    }
}
