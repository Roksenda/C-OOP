﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Easter.Models.Bunnies
{
    public class SleepyBunny : Bunny
    {
        public SleepyBunny(string name)
            : base(name, 50)
        {
        }

        public override void Work()
        {
            base.Work();
            if (Energy - 5 < 0)
            {
                Energy = 0;
            }
            else
            {
                Energy -= 5;
            }
        }
    }
}
