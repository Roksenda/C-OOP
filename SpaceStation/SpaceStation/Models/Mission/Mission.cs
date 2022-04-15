using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceStation.Models.Astronauts.Contracts;
using SpaceStation.Models.Mission.Contracts;
using SpaceStation.Models.Planets.Contracts;

namespace SpaceStation.Models.Mission
{
    public class Mission : IMission
    {
        public void Explore(IPlanet planet, ICollection<IAstronaut> astronauts)
        {
            List<IAstronaut> selectedAstronauts = astronauts.Where(x => x.Oxygen > 0).ToList();
            foreach (var part in selectedAstronauts)
            {
                while (planet.Items.Count > 0 && part.Oxygen > 0)
                {
                    string item = planet.Items.First();
                    part.Bag.Items.Add(item);
                    planet.Items.Remove(item);
                    part.Breath();
                }
            }
        }
    }
}
