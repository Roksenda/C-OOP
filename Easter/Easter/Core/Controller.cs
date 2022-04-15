using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easter.Core.Contracts;
using Easter.Models.Bunnies;
using Easter.Models.Bunnies.Contracts;
using Easter.Models.Dyes;
using Easter.Models.Dyes.Contracts;
using Easter.Models.Eggs;
using Easter.Models.Eggs.Contracts;
using Easter.Models.Workshops;
using Easter.Repositories;

namespace Easter.Core
{
    public class Controller : IController
    {
        private BunnyRepository bunnies;
        private EggRepository eggs;
        private Workshop workshop;
        private int countColorEggs;

        public Controller()
        {
            bunnies = new BunnyRepository();
            eggs = new EggRepository();
            workshop = new Workshop();
        }

        public string AddBunny(string bunnyType, string bunnyName)
        {
            IBunny bunny;
            if (bunnyType == "HappyBunny")
            {
                bunny = new HappyBunny(bunnyName);
            }
            else if (bunnyType == "SleepyBunny")
            {
                bunny = new SleepyBunny(bunnyName);
            }
            else
            {
                throw new InvalidOperationException("Invalid bunny type.");
            }
            bunnies.Add(bunny);

            return $"Successfully added {bunnyType} named {bunnyName}.";
        }

        public string AddDyeToBunny(string bunnyName, int power)
        {
            IBunny bunny = bunnies.FindByName(bunnyName);
            if (bunny == null)
            {
                throw new InvalidOperationException("The bunny you want to add a dye to doesn't exist!");
            }

            IDye dye = new Dye(power);
            bunny.Dyes.Add(dye);
            return $"Successfully added dye with power {power} to bunny {bunnyName}!";
        }

        public string AddEgg(string eggName, int energyRequired)
        {
            IEgg egg = new Egg(eggName, energyRequired);
            eggs.Add(egg);
            return $"Successfully added egg: {eggName}!";
        }

        public string ColorEgg(string eggName)
        {
            List<IBunny> result = bunnies.Models.Where(x => x.Energy >= 50).ToList();
            if (result.Count == 0)
            {
                throw new InvalidOperationException("There is no bunny ready to start coloring!");
            }

            result = result.OrderByDescending(x => x.Energy).ToList();
            IEgg egg = eggs.FindByName(eggName);
            foreach (var part in result)
            {
                workshop.Color(egg, part);
                if (part.Energy == 0)
                {
                    bunnies.Remove(part);
                }
            }
                

            if (egg.IsDone())
            {
                countColorEggs++;
                return $"Egg {egg.Name} is done.";
            }
            else
            {
                return $"Egg {egg.Name} is not done.";

            }
        }

        public string Report()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.AppendLine($"{countColorEggs} eggs are done!");
            sb.AppendLine($"Bunnies info:");
            foreach (var part in bunnies.Models)
            {
                var unfinished = part.Dyes.Where(x => !x.IsFinished()).ToList();
                sb.AppendLine($"Name: {part.Name}");
                sb.AppendLine($"Energy: {part.Energy}");
                sb.AppendLine($"Dyes: {unfinished.Count} not finished");

            }

            return sb.ToString().TrimEnd();
            //"{countColoredEggs} eggs are done!"
            //"Bunnies info:"
            //"Name: {bunnyName1}"
            //"Energy: {bunnyEnergy1}"
            //"Dyes: {countDyes} not finished"

        }
    }
}
