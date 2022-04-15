using System;
using System.Collections.Generic;
using System.Text;
using NavalVessels.Models.Contracts;
using NavalVessels.Utilities.Messages;

namespace NavalVessels.Models
{
    public class Captain : ICaptain
    {
        private string fullName;
        private readonly List<IVessel> vessels;
        private int combatExperience;
        public Captain(string fullName)
        {
            FullName = fullName;
            CombatExperience = 0;
            vessels = new List<IVessel>();
        }

        public string FullName
        {
            get => fullName;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(ExceptionMessages.InvalidCaptainName);
                }

                fullName = value;
            }
        }

        public int CombatExperience
        {
            get => combatExperience
            ;
            private set
            {
                combatExperience = value;
            }
        }

        public ICollection<IVessel> Vessels
            => vessels;

        public void AddVessel(IVessel vessel)
        {
            if (vessel == null)
            {
                throw new NullReferenceException(ExceptionMessages.InvalidVesselForCaptain);
            }
            vessels.Add(vessel);
        }

        public void IncreaseCombatExperience()
        {
            CombatExperience += 10;
        }

        public string Report()
        {
            //"{FullName} has {CombatExperience} combat experience and commands {vessels count} vessels."
            //If the captain commands any vessel, return:
            //"- {vessel name}
            //    * Type: { vessel type name}
            //*Armor thickness: { vessel armor thickness points}
            //*Main weapon caliber: { vessel main weapon caliber points}
            //*Speed: { vessel speed points}
            //knots
            //    * Targets: None /{ targets}
            //*Sonar / Submerge mode: ON / OFF" 
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{FullName} has {CombatExperience} combat experience and commands {vessels.Count} vessels.");
            if (vessels.Count > 0)
            {
                foreach (var part in vessels)
                {
                    sb.AppendLine($"{part}");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
