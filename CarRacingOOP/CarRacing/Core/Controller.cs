using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarRacing.Core.Contracts;
using CarRacing.Models.Cars;
using CarRacing.Models.Cars.Contracts;
using CarRacing.Models.Maps;
using CarRacing.Models.Maps.Contracts;
using CarRacing.Models.Racers;
using CarRacing.Models.Racers.Contracts;
using CarRacing.Repositories;
using CarRacing.Repositories.Contracts;

namespace CarRacing.Core
{
    public class Controller : IController
    {
        private IRepository<ICar> cars;
        private IRepository<IRacer> racers;
        private IMap map;

        public Controller()
        {
            cars = new CarRepository();
            racers = new RacerRepository();
            map = new Map();
        }

        public string AddCar(string type, string make, string model, string VIN, int horsePower)
        {
            ICar car;
            if (type == "SuperCar")
            {
                car = new SuperCar(make, model, VIN, horsePower);
            }
            else if (type == "TunedCar")
            {
                car = new TunedCar(make, model, VIN, horsePower);
            }
            else
            {
                throw new ArgumentException("Invalid car type!");
            }

            cars.Add(car);
            return $"Successfully added car {car.Make} {car.Model} ({VIN}).";
        }

        public string AddRacer(string type, string username, string carVIN)
        {
            ICar car = cars.FindBy(carVIN);
            if (car == null)
            {
                throw new ArgumentException("Car cannot be found!");
            }
            IRacer racer;
            if (type == "ProfessionalRacer")
            {
                racer = new ProfessionalRacer(username, car);
            }
            else if (type == "StreetRacer")
            {
                racer = new StreetRacer(username, car);
            }
            else
            {
                throw new ArgumentException("Invalid racer type!");
            }
            racers.Add(racer);
            return $"Successfully added racer {racer.Username}.";
        }

        public string BeginRace(string racerOneUsername, string racerTwoUsername)
        {
            IRacer racerFirst = racers.FindBy(racerOneUsername);
            IRacer racerSecond = racers.FindBy(racerTwoUsername);
            if (racerFirst == null)
            {
                throw new ArgumentException($"Racer {racerOneUsername} cannot be found!");
            }
            else if (racerSecond == null)
            {
                throw new ArgumentException($"Racer {racerTwoUsername} cannot be found!");
            }
            return map.StartRace(racerFirst, racerSecond);
        }

        public string Report()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var part in racers.Models.OrderByDescending(x => x.DrivingExperience)
                .ThenBy(x => x.Username))
            {
                sb.AppendLine($"{part.GetType().Name}: {part.Username}");
                sb.AppendLine($"--Driving behavior: {part.RacingBehavior}");
                sb.AppendLine($"--Driving experience: {part.DrivingExperience}");
                sb.AppendLine($"--Car: {part.Car.Make} {part.Car.Model} ({part.Car.VIN})");
                //"{racer type}: {racer username}"
                //"--Driving behavior: {racingBehavior}"
                //"--Driving experience: {drivingExperience}"
                //"--Car: {carMake} {carModel} ({carVIN})"

            }

            return sb.ToString().TrimEnd();
        }
    }
}
