using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarCroft.Entities.Items;

namespace WarCroft.Entities.Inventory
{
    public abstract class Bag : IBag
    {
        private List<Item> items;

        protected  Bag(int capacity)
        {
            Capacity = capacity;
            items = new List<Item>();
        }
        public int Capacity { get; set; }

        public int Load { get => items.Sum(x => x.Weight); }

        public IReadOnlyCollection<Item> Items
        {
            get => items;
        }

        public void AddItem(Item item)
        {
            if (Load + item.Weight > Capacity)
            {
                throw new InvalidOperationException("Bag is full!");
            }

            items.Add(item);
        }

        public Item GetItem(string name)
        {
            if (!items.Any())
            {
                throw new InvalidOperationException("Bag is empty!");
            }

            Item item = items.FirstOrDefault(x => x.GetType().Name == name);

            if (item == null)
            {
                throw new ArgumentException("No item with name {name} in bag!");
            }

            items.Remove(item);
            return item;
        }
    }
}
