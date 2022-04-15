using System;
using System.Collections.Generic;
using System.Text;
using Easter.Models.Dyes.Contracts;

namespace Easter.Models.Dyes
{
    public class Dye : IDye
    {
        private int power;
        public Dye(int power)
        {
            Power = power;
        }

        public int Power
        {
            get => power;
            private set
            {
                if (value < 0)
                {
                    value = 0;
                }

                power = value;
            }
        }
        public void Use()
        {
            if (Power - 10 < 0)
            {
                Power = 0;
            }
            else
            {
                Power -= 10;
            }
        }

        public bool IsFinished()
            => Power == 0;
    }
}
