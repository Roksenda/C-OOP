using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AquaShop.Models.Aquariums;
using AquaShop.Models.Aquariums.Contracts;
using AquaShop.Models.Decorations;
using AquaShop.Models.Decorations.Contracts;
using AquaShop.Models.Fish;
using AquaShop.Models.Fish.Contracts;
using AquaShop.Repositories;

namespace AquaShop.Core.Contracts
{
    public class Controller : IController
    {
        private DecorationRepository decorations;
        private List<IAquarium> aquariums;

        public Controller()
        {
            decorations = new DecorationRepository();
            aquariums = new List<IAquarium>();
        }


        public string AddAquarium(string aquariumType, string aquariumName)
        {
            if(aquariumType != nameof(FreshwaterAquarium) && aquariumType != nameof(SaltwaterAquarium))
            {
                throw new InvalidOperationException("Invalid aquarium type.");
            }

            IAquarium aquarium;
            if (aquariumType == nameof(FreshwaterAquarium))
            {
                aquarium = new FreshwaterAquarium(aquariumName);
            }
            else 
            {
                aquarium = new SaltwaterAquarium(aquariumName);
            }
           

            aquariums.Add(aquarium);
            return $"Successfully added {aquariumType}.";
        }

        public string AddDecoration(string decorationType)
        {
            if (decorationType != nameof(Ornament) && decorationType != nameof(Plant))
            {
                throw new InvalidOperationException("Invalid decoration type.");
            }

            IDecoration decoration;
            if (nameof(Ornament) == decorationType)
            {
                decoration = new Ornament();
            }
            else
            {
                decoration = new Plant();
            }
            

            decorations.Add(decoration);
            return $"Successfully added {decorationType}.";
        }

        public string InsertDecoration(string aquariumName, string decorationType)
        {
            IDecoration decoration = decorations.FindByType(decorationType);
            //if (decoration.GetType().Name != nameof(Ornament) && decoration.GetType().Name != nameof(Plant))
            //{
            //    throw new InvalidOperationException($"There isn't a decoration of type {decorationType}.");
            //}
            if (decoration == null)
            {
                throw new InvalidOperationException($"There isn't a decoration of type {decorationType}.");
            }

            IAquarium aquarium = aquariums.FirstOrDefault(x => x.Name == aquariumName);
            aquarium.AddDecoration(decoration);
            decorations.Remove(decoration);
            return $"Successfully added {decorationType} to {aquariumName}.";
        }

        public string AddFish(string aquariumName, string fishType, string fishName, string fishSpecies, decimal price)
        {
            IFish fish;
            if (fishType == nameof(FreshwaterFish))
            {
                fish = new FreshwaterFish(fishName, fishSpecies, price);
            }
            else if (fishType == nameof(SaltwaterFish))
            {
                fish = new SaltwaterFish(fishName, fishSpecies, price);
            }
            else
            {
                throw new InvalidOperationException("Invalid fish type.");
            }

            IAquarium aquarium = aquariums.FirstOrDefault(x => x.Name == aquariumName);
            string typeAquarium = aquarium.GetType().Name;

                if (typeAquarium == nameof(FreshwaterAquarium) && fishType == nameof(FreshwaterFish))
                {
                    aquarium.AddFish(fish);
                }
                else if (typeAquarium == nameof(SaltwaterAquarium) && fishType == nameof(SaltwaterFish))
                {
                    aquarium.AddFish(fish);
                }
                else
                {
                    return $"Water not suitable.";
                }

            return $"Successfully added {fishType} to {aquariumName}.";
        }

        public string FeedFish(string aquariumName)
        {
            IAquarium aquarium = aquariums.FirstOrDefault(x => x.Name == aquariumName);
            aquarium.Feed();
            return $"Fish fed: {aquarium.Fish.Count}";
        }

        public string CalculateValue(string aquariumName)
        {
            IAquarium aquarium = aquariums.FirstOrDefault(x => x.Name == aquariumName);
            decimal priceFishes = aquarium.Fish.Sum(x => x.Price);
            decimal priceDecoration = aquarium.Decorations.Sum(x => x.Price);
            decimal sum = priceFishes + priceDecoration;

            return $"The value of Aquarium {aquariumName} is {sum:f2}.";
        }

        public string Report()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var aquarium in aquariums)
            {
                sb.AppendLine(aquarium.GetInfo());
            }

            return sb.ToString().TrimEnd();
        }
    }
}
