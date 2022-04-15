using System;
using System.Collections.Generic;
using System.Text;
using NavalVessels.Models.Contracts;

namespace NavalVessels.Models
{
    public class Submarine : Vessel, ISubmarine
    {
        private const int armorRepear = 200;

        public Submarine(string name, double mainWeaponCaliber, double speed)
            : base(name, mainWeaponCaliber, speed, armorRepear)
        {
            SubmergeMode = false;
        }

        public bool SubmergeMode { get; private set; }
        public void ToggleSubmergeMode()
        {
            if (SubmergeMode == false)
            {
                SubmergeMode = true;
                MainWeaponCaliber += 40;
                Speed -= 4;
            }
            else
            {
                SubmergeMode = false;
                MainWeaponCaliber -= 40;
                Speed += 4;
            }
        }

        public override void RepairVessel()
        {
            if (ArmorThickness < 200)
            {
                ArmorThickness = 200;
            }
        }

        public override string ToString()
        {
            if (SubmergeMode == false)
            {
                return base.ToString() +
                       Environment.NewLine +
                       " *Submerge mode: OFF";
            }

            return base.ToString() +
                   Environment.NewLine +
                   " *Submerge mode: ON";
        }
    }
}
