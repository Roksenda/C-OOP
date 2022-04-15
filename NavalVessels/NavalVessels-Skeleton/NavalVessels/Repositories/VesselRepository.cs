using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NavalVessels.Models;
using NavalVessels.Models.Contracts;
using NavalVessels.Repositories.Contracts;

namespace NavalVessels.Repositories
{
    public class VesselRepository : IRepository<IVessel>
    {
        private readonly List<IVessel> models;

        public VesselRepository()
        {
            models = new List<IVessel>();
        }

        public IReadOnlyCollection<IVessel> Models
            => models;

        public void Add(IVessel model)
        {
            if (model != null)
            {
                models.Add(model);
            }
        }

        public bool Remove(IVessel model)
            => models.Remove(model);

        public IVessel FindByName(string name)
        {
            return models.FirstOrDefault(x => x.Name == name);
        }
    }
}
