using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Gym.Models.Athletes.Contracts;
using Gym.Models.Equipment.Contracts;
using Gym.Models.Gyms.Contracts;

namespace Gym.Models.Gyms
{
    public abstract class Gym : IGym
    {
        private string name;
        private List<IEquipment> equipments;
        private List<IAthlete> athletes;

        public Gym(string name, int capacity)
        {
            Name = name;
            Capacity = capacity;
            equipments = new List<IEquipment>();
            athletes = new List<IAthlete>();
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Gym name cannot be null or empty.");
                }

                name = value;
            }

        }
        public int Capacity { get; }

        public double EquipmentWeight
            => equipments.Sum(x => x.Weight);

        public ICollection<IEquipment> Equipment
            => equipments;

        public ICollection<IAthlete> Athletes
            => athletes;

        public void AddAthlete(IAthlete athlete)
        {
            if (Capacity <= athletes.Count)
            {
                throw new InvalidOperationException("Not enough space in the gym.");
            }

            athletes.Add(athlete);

        }

        public bool RemoveAthlete(IAthlete athlete)
        {
            return athletes.Remove(athlete);
        }

        public void AddEquipment(IEquipment equipment)
        {
            equipments.Add(equipment);
        }

        public void Exercise()
        {
            foreach (var part in athletes)
            {
                part.Exercise();
            }
        }

        public string GymInfo()
        {
            //StringBuilder sb = new StringBuilder();

            ////"{gymName} is a {gymType}:
            ////Athletes: { athleteName1}, { athleteName2}, { athleteName3} (…) / No athletes
            ////Equipment total count: { equipmentCount}
            ////Equipment total weight: { equipmentWeight}
            ////grams"
            ////Note: Do not use "\n\r" for a new line.

            //sb.AppendLine($"{Name} is a {GetType().Name}:");
            //sb.Append($"Athletes: ");
            //if (athletes.Count > 0)
            //{
            //    sb.Append($"{string.Join(", ", athletes)}");

            //}
            //else
            //{
            //    sb.Append($"No athletes");
            //}
            //Console.WriteLine();
            //sb.AppendLine($"Equipment total count: {equipments.Count}");
            //sb.AppendLine($"Equipment total weight: {EquipmentWeight} grams");

            //return sb.ToString().TrimEnd();
            string result = this.athletes.Count > 0 ? string.Join(", ", athletes.Select(x => x.FullName)) : "No athletes";
            //return $"{Name} is a {this.GetType().Name}" + Environment.NewLine +
            //       $"Athletes: {result}" + Environment.NewLine +
            //       $"Equipment total count: {equipments.Count}" + Environment.NewLine +
            //       $"Equipment total weight: {EquipmentWeight:f2} grams";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{Name} is a {this.GetType().Name}:");
            sb.AppendLine($"Athletes: {result}");
            sb.AppendLine($"Equipment total count: {equipments.Count}");
            sb.AppendLine($"Equipment total weight: {EquipmentWeight:f2} grams");
            return sb.ToString().TrimEnd();

        }
    }
}
