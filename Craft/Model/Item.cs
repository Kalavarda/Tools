using System;

namespace Craft.Model
{
    public class Item
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
