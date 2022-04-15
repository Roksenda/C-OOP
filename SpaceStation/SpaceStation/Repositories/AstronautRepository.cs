using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceStation.Models.Astronauts.Contracts;
using SpaceStation.Repositories.Contracts;

namespace SpaceStation.Repositories
{
    public class AstronautRepository : IRepository<IAstronaut>
    {
        private List<IAstronaut> models;

        public AstronautRepository()
        {
            models = new List<IAstronaut>();
        }

        public IReadOnlyCollection<IAstronaut> Models
            => models;

        public void Add(IAstronaut model)
        {
            models.Add(model);
        }

        public bool Remove(IAstronaut model)
            => models.Remove(model);

        public IAstronaut FindByName(string name)
            => models.FirstOrDefault(x => x.Name == name);
    }
}
