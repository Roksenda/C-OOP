using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gym.Core.Contracts;
using Gym.Models.Athletes;
using Gym.Models.Athletes.Contracts;
using Gym.Models.Equipment;
using Gym.Models.Equipment.Contracts;
using Gym.Models.Gyms;
using Gym.Models.Gyms.Contracts;
using Gym.Repositories;

namespace Gym.Core
{
    public class Controller : IController
    {
        private EquipmentRepository equipment;

        private List<IGym> gyms;

        public Controller()
        {
            equipment = new EquipmentRepository();
            gyms = new List<IGym>();
        }

        public string AddGym(string gymType, string gymName)
        {
            IGym gym;
            if (gymType == "BoxingGym")
            {
                gym = new BoxingGym(gymName);
            }
            else if (gymType == "WeightliftingGym")
            {
                gym = new WeightliftingGym(gymName);
            }
            else
            {
                throw new InvalidOperationException("Invalid gym type.");
            }
            gyms.Add(gym);
            return $"Successfully added {gym.GetType().Name}.";
        }

        public string AddEquipment(string equipmentType)
        {
            IEquipment equipmentResult;
            if (equipmentType == "BoxingGloves")
            {
                equipmentResult = new BoxingGloves();
            }
            else if (equipmentType == "Kettlebell")
            {
                equipmentResult = new Kettlebell();
            }
            else
            {
                throw new InvalidOperationException("Invalid equipment type.");
            }

            equipment.Add(equipmentResult);
            return $"Successfully added {equipmentResult.GetType().Name}.";
        }

        public string InsertEquipment(string gymName, string equipmentType)
        {
            IGym gym = gyms.FirstOrDefault(x => x.Name == gymName);
            //IEquipment equipmentResult;
            //if (equipmentType == "BoxingGloves")
            //{
            //    equipmentResult = new BoxingGloves();
            //}
            //else if (equipmentType == "Kettlebell")
            //{
            //    equipmentResult = new Kettlebell();
            //}
            //else
            //{
            //    throw new InvalidOperationException($"There isn’t equipment of type {equipmentType}.");
            //}
            IEquipment equipmentResult = equipment.FindByType(equipmentType);
            if (equipmentResult == null)
            {
                throw new InvalidOperationException($"There isn’t equipment of type {equipmentType}.");
            }
            //if (equipmentType == "BoxingGloves" && gym.GetType().Name == "BoxingGym")
            //{
            //    gym.AddEquipment(equipmentResult);
            //    equipment.Remove(equipmentResult);
            //}
            //else if (equipmentType == "Kettlebell" && gym.GetType().Name == "WeightliftingGym")
            //{
            //    gym.AddEquipment(equipmentResult);
            //    equipment.Remove(equipmentResult);
            //}
            //else
            //{
            //    throw new InvalidOperationException($"There isn’t equipment of type {equipmentType}.");
            //}
            gym.AddEquipment(equipmentResult);
            equipment.Remove(equipmentResult);
            return $"Successfully added {equipmentType} to {gymName}.";
        }

        public string AddAthlete(string gymName, string athleteType, string athleteName, string motivation, int numberOfMedals)
        {
            IGym gym = gyms.FirstOrDefault(x => x.Name == gymName);
            IAthlete athlete;
            if (athleteType == "Boxer")
            {
                athlete = new Boxer(athleteName, motivation, numberOfMedals);
            }
            else if (athleteType == "Weightlifter")
            {
                athlete = new Weightlifter(athleteName, motivation, numberOfMedals);
            }
            else
            {
                throw new InvalidOperationException("Invalid athlete type.");
            }

            if (gym.GetType().Name == "BoxingGym" && athlete.GetType().Name == "Boxer")
            {
                gym.AddAthlete(athlete);
            }
            else if (gym.GetType().Name == "WeightliftingGym" && athlete.GetType().Name == "Weightlifter")
            {
                gym.AddAthlete(athlete);
            }
            else
            {
                return "The gym is not appropriate.";
            }

            return $"Successfully added {athleteType} to {gymName}.";
        }

        public string TrainAthletes(string gymName)
        {
            IGym gym = gyms.FirstOrDefault(x => x.Name == gymName);
            if (gym.Athletes.Count > 0)
            {
                foreach (var part in gym.Athletes)
                {
                    part.Exercise();
                }
            }
            return $"Exercise athletes: {gym.Athletes.Count}.";
        }

        public string EquipmentWeight(string gymName)
        {
            IGym gym = gyms.FirstOrDefault(x => x.Name == gymName);
            if (gym.Equipment.Count > 0)
            {
                double result = gym.EquipmentWeight;
                return $"The total weight of the equipment in the gym {gymName} is {result:f2} grams.";
            }

            return "";
        }

        public string Report()
        {
            StringBuilder sb = new StringBuilder();
            
                foreach (var part in gyms)
                {
                    sb.AppendLine(part.GymInfo());
                }

                return sb.ToString().TrimEnd();
            //foreach (var part in gyms)
            //{
            //    sb.AppendLine(part.GymInfo());
            //}
        }
    }
}
