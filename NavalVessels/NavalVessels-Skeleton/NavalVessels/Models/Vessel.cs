using System;
using System.Collections.Generic;
using System.Text;
using NavalVessels.Models.Contracts;
using NavalVessels.Utilities.Messages;

namespace NavalVessels.Models
{
    public abstract class Vessel : IVessel
    {
        private string name;
        private ICaptain captain;
        private readonly List<string> targets;
        private double speed;
        private double mainWeaponCaliber;
        private double armorThickness;
        public Vessel(string name, double mainWeaponCaliber, double speed, double armorThickness)
        {
            Name = name;
            MainWeaponCaliber = mainWeaponCaliber;
            Speed = speed;
            ArmorThickness = armorThickness;
            targets = new List<string>();
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(ExceptionMessages.InvalidVesselName);
                }

                name = value;
            }
        }

        public ICaptain Captain
        {
            get => captain;
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException(ExceptionMessages.InvalidCaptainToVessel);
                }

                captain = value;
            }
        }
        public double ArmorThickness { get; set; }
        public double MainWeaponCaliber { get; protected set; }
        public double Speed { get; protected set; }

        public ICollection<string> Targets
            => this.targets;
        public void Attack(IVessel target)
        {
            if (target == null)
            {
                throw new NullReferenceException(ExceptionMessages.InvalidTarget);
            }
            if (target.ArmorThickness - this.MainWeaponCaliber < 0)
            {
                target.ArmorThickness = 0;
            }
            else
            {
                target.ArmorThickness -= this.MainWeaponCaliber;
            }

            targets.Add(target.Name);
        }

        public abstract void RepairVessel();
        

        public override string ToString()
        {
            //"- {vessel name}
            //    * Type: { vessel type name}
            //*Armor thickness: { vessel armor thickness points}
            //*Main weapon caliber: { vessel main weapon caliber points}
            //*Speed: { vessel speed points}
            //knots
            //    * Targets: " – if there are no targets "None" Otherwise print "{ target1}, { target2}, { target3}, { targetN}
            //"
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"- {Name}");
            sb.AppendLine($" *Type: {GetType().Name}");
            sb.AppendLine($" *Armor thickness: {ArmorThickness}");
            sb.AppendLine($" *Main weapon caliber: {MainWeaponCaliber}");
            sb.AppendLine($" *Speed: {Speed} knots");
            sb.Append($" *Targets: ");
            if (targets.Count == 0)
            {
                sb.AppendLine("None");
            }
            else
            {
                sb.Append(string.Join(", ", targets));
            }

            return sb.ToString().TrimEnd();
        }
    }
}
