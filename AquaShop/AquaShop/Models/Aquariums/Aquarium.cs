using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using AquaShop.Models.Aquariums.Contracts;
using AquaShop.Models.Decorations.Contracts;
using AquaShop.Models.Fish.Contracts;

namespace AquaShop.Models.Aquariums
{
    public abstract class Aquarium : IAquarium
    {
        private string name;
        private List<IDecoration> decorations;
        private List<IFish> fishes;

        public Aquarium(string name, int capacity)
        {
            Name = name;
            Capacity = capacity;
            decorations = new List<IDecoration>();
            fishes = new List<IFish>();
        }
        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Aquarium name cannot be null or empty.");
                }

                name = value;
            }
        }
        public int Capacity { get; }

        public int Comfort
            => decorations.Sum(x => x.Comfort);

        public ICollection<IDecoration> Decorations
        {
            get => decorations;
        }

        public ICollection<IFish> Fish
        {
            get => fishes;
        }

        public void AddFish(IFish fish)
        {
            if (Capacity <= fishes.Count)
            {
                throw new InvalidOperationException("Not enough capacity.");
            }

            fishes.Add(fish);
        }

        public bool RemoveFish(IFish fish)
            => fishes.Remove(fish);

        public void AddDecoration(IDecoration decoration)
        {
            decorations.Add(decoration);
        }

        public void Feed()
        {
            foreach (var fish in fishes)
            {
                fish.Eat();
            }
        }

        public string GetInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{Name} ({GetType().Name}):");
            sb.Append("Fish: ");
            string result = fishes.Count > 0 ? string.Join(", ", fishes.Select(x => x.Name)) : "none";
            sb.AppendLine(result);
            sb.AppendLine($"Decorations: {Decorations.Count}");
            sb.AppendLine($"Comfort: {Comfort}");
            return sb.ToString().TrimEnd();
            //"{aquariumName} ({aquariumType}):
            //Fish: { fishName1}, { fishName2}, { fishName3} (…) / none
            //Decorations: { decorationsCount}
            //Comfort: { aquariumComfort}


        }
    }
}
