using System;
using System.Collections.Generic;
using System.Text;
using NavalVessels.Models.Contracts;

namespace NavalVessels.Models
{
    public class Battleship : Vessel, IBattleship
    {
        private const int armorRepear = 300;
        public Battleship(string name, double mainWeaponCaliber, double speed)
            : base(name, mainWeaponCaliber, speed, armorRepear)
        {
            SonarMode = false;
        }

        public bool SonarMode { get; private set; }
        public void ToggleSonarMode()
        {
            if (SonarMode == false)
            {
                SonarMode = true;
                MainWeaponCaliber += 40;
                Speed -= 5;
            }
            else
            {
                SonarMode = false;
                MainWeaponCaliber -= 40;
                Speed += 5;
            }
        }

        public override void RepairVessel()
        {
            if (ArmorThickness < 300)
            {
                ArmorThickness = 300;
            }
        }

        public override string ToString()
        {
            if (SonarMode == false)
            {
                return base.ToString() +
                       Environment.NewLine +
                       " *Sonar mode: OFF";
            }

            return base.ToString() +
                   Environment.NewLine +
                   " *Sonar mode: ON";
        }
    }
}
