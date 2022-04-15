using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SpaceStation.Core.Contracts;
using SpaceStation.Models.Astronauts;
using SpaceStation.Models.Astronauts.Contracts;
using SpaceStation.Models.Mission;
using SpaceStation.Models.Mission.Contracts;
using SpaceStation.Models.Planets;
using SpaceStation.Models.Planets.Contracts;
using SpaceStation.Repositories;
using SpaceStation.Repositories.Contracts;

namespace SpaceStation.Core
{
    public class Controller : IController
    {
        private IRepository<IAstronaut> astRepository;
        private IRepository<IPlanet> planetRepository;
        private IMission mission;
        private int explorePlanet;

        public Controller()
        {
            astRepository = new AstronautRepository();
            planetRepository = new PlanetRepository();
            mission = new Mission();
        }
        public string AddAstronaut(string type, string astronautName)
        {
            IAstronaut astronaut;
            if (type == "Biologist")
            {
                astronaut = new Biologist(astronautName);
            }
            else if (type == "Geodesist")
            {
                astronaut = new Geodesist(astronautName);
            }
            else if (type == "Meteorologist")
            {
                astronaut = new Meteorologist(astronautName);
            }
            else
            {
                throw new InvalidOperationException("Astronaut type doesn't exists!");
            }

            astRepository.Add(astronaut);
            return $"Successfully added {type}: {astronautName}!";
        }

        public string AddPlanet(string planetName, params string[] items)
        {
            IPlanet planet = new Planet(planetName);
            foreach (var part in items)
            {
                planet.Items.Add(part);
            }
            planetRepository.Add(planet);
            return $"Successfully added Planet: {planetName}!";
        }

        public string RetireAstronaut(string astronautName)
        {
            IAstronaut astronaut = astRepository.FindByName(astronautName);
            if (astronaut == null)
            {
                throw new InvalidOperationException($"Astronaut {astronautName} doesn't exists!");
            }

            astRepository.Remove(astronaut);
            return $"Astronaut {astronautName} was retired!";
        }

        public string ExplorePlanet(string planetName)
        {
            var result = astRepository.Models.Where(x => x.Oxygen > 60).ToList();
            IPlanet planet = planetRepository.FindByName(planetName);
            if (result.Count == 0)
            {
                throw new InvalidOperationException($"You need at least one astronaut to explore the planet");
            }

            explorePlanet++;
            mission.Explore(planet, result);
            int deadAstro = 0;
            foreach (var part in result)
            {
                if (!part.CanBreath)
                {
                    deadAstro++;
                }
            }

            return $"Planet: {planetName} was explored! Exploration finished with {deadAstro} dead astronauts!";
        }

        public string Report()
        {
            //"{exploredPlanetsCount} planets were explored!
            //Astronauts info:
            //Name: { astronautName}
            //Oxygen: { astronautOxygen}
            //Bag items: { bagItem1, bagItem2, …, bagItemn} / none
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{explorePlanet} planets were explored!");
            sb.AppendLine($"Astronauts info:");
            foreach (var part in astRepository.Models)
            {
                sb.AppendLine($"Name: {part.Name}");
                sb.AppendLine($"Oxygen: {part.Oxygen}");
                sb.Append($"Bag items: ");
                if (part.Bag.Items.Count == 0)
                {
                    sb.AppendLine("none");
                }
                else
                {
                    sb.AppendLine(string.Join(", ", part.Bag.Items));
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
