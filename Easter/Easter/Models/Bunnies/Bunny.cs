using System;
using System.Collections.Generic;
using System.Text;
using Easter.Models.Bunnies.Contracts;
using Easter.Models.Dyes.Contracts;

namespace Easter.Models.Bunnies
{
    public abstract class Bunny : IBunny
    {
        private string name;
        private int energy;
        private List<IDye> dyes;

        public Bunny(string name, int energy)
        {
            Name = name;
            Energy = energy;
            dyes = new List<IDye>();
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Bunny name cannot be null or empty.");
                }

                name = value;
            }
        }

        public int Energy
        {
            get => energy;
            protected set
            {
                if (value < 0)
                {
                    value = 0;
                }

                energy = value;
            }
        }

        public ICollection<IDye> Dyes
            => dyes;

        public virtual void Work()
        {
            if (Energy - 10 < 0)
            {
                Energy = 0;
            }
            else
            {
                Energy -= 10;
            }
        }

        public  void AddDye(IDye dye)
        {
            dyes.Add(dye);
        }
    }
}
