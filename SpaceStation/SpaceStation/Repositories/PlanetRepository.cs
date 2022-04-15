using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceStation.Models.Planets.Contracts;
using SpaceStation.Repositories.Contracts;

namespace SpaceStation.Repositories
{
    public class PlanetRepository : IRepository<IPlanet>
    {
        private List<IPlanet> models;

        public PlanetRepository()
        {
            models = new List<IPlanet>();
        }

        public IReadOnlyCollection<IPlanet> Models
            => models;
        public void Add(IPlanet model)
        {
            models.Add(model);
        }

        public bool Remove(IPlanet model)
            => models.Remove(model);

        public IPlanet FindByName(string name)
            => models.FirstOrDefault(x => x.Name == name);
    }
}
