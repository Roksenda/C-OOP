using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NavalVessels.Core.Contracts;
using NavalVessels.Models;
using NavalVessels.Models.Contracts;
using NavalVessels.Repositories;
using NavalVessels.Repositories.Contracts;
using NavalVessels.Utilities.Messages;

namespace NavalVessels.Core
{
    public class Controller : IController
    {
        private VesselRepository vessels;
        private List<ICaptain> captains;

        public Controller()
        {
            vessels = new VesselRepository();
            captains = new List<ICaptain>();
        }

        public string HireCaptain(string fullName)
        {
            if (captains.Any(x => x.FullName == fullName))
            {
                return string.Format(OutputMessages.CaptainIsAlreadyHired, fullName);
            }
            ICaptain captain = new Captain(fullName);
            captains.Add(captain);
            return String.Format(OutputMessages.SuccessfullyAddedCaptain, fullName);
        }

        public string ProduceVessel(string name, string vesselType, double mainWeaponCaliber, double speed)
        {

            IVessel neededVessel = vessels.FindByName(name);
            if (neededVessel != null)
            {
                return string.Format(OutputMessages.VesselIsAlreadyManufactured, neededVessel.GetType().Name, name);
            }
            IVessel vessel;
            if (vesselType.ToLower() == "submarine")
            {
                vessel = new Submarine(name, mainWeaponCaliber, speed);
                vessels.Add(vessel);
            }
            else if (vesselType.ToLower() == "battleship")
            {
                vessel = new Battleship(name, mainWeaponCaliber, speed);
                vessels.Add(vessel);
            }
            else
            {
                return OutputMessages.InvalidVesselType;
            }
            vessels.Add(vessel);

            return string.Format(OutputMessages.SuccessfullyCreateVessel, vessel.GetType().Name, name, mainWeaponCaliber, speed);

        }

        public string AssignCaptain(string selectedCaptainName, string selectedVesselName)
        {
            ICaptain captain = captains.FirstOrDefault(x => x.FullName == selectedCaptainName);
            IVessel vessel = vessels.FindByName(selectedVesselName);
            if (captain == null)
            {
                return String.Format(OutputMessages.CaptainNotFound, selectedCaptainName);
            }

            if (vessel == null)
            {
                return String.Format(OutputMessages.VesselNotFound, selectedVesselName);
            }

            if (vessel.Captain != null)
            {
                return string.Format(OutputMessages.VesselOccupied, selectedVesselName);
            }

            vessel.Captain = captain;
            captain.Vessels.Add(vessel);
            return String.Format(OutputMessages.SuccessfullyAssignCaptain, selectedCaptainName, selectedVesselName);
        }

        public string CaptainReport(string captainFullName)
        {
            ICaptain captain = captains.FirstOrDefault(x => x.FullName == captainFullName);
            if (captain != null && captain.Vessels.Count > 0)
            {
                return captain.Report();
            }

            return "";
        }

        public string VesselReport(string vesselName)
        {
            IVessel vessel = vessels.FindByName(vesselName);
            if (vessel != null)
            {
                return vessel.ToString();
            }

            return "";
        }

        public string ToggleSpecialMode(string vesselName)
        {
            IVessel vessel = vessels.FindByName(vesselName);
            if (vessel == null)
            {
                return String.Format(OutputMessages.VesselNotFound, vesselName);

            }
            if (vessel.GetType().Name.ToLower() == "battleship")
            {
                Battleship battleship = vessel as Battleship;
                battleship.ToggleSonarMode();
                return String.Format(OutputMessages.ToggleBattleshipSonarMode, vesselName);
            }
            if (vessel.GetType().Name.ToLower() == "submarine")
            {
                Submarine submarine = vessel as Submarine;
                submarine.ToggleSubmergeMode();
                return String.Format(OutputMessages.ToggleSubmarineSubmergeMode, vesselName);
            }

            return String.Format(OutputMessages.VesselNotFound, vesselName);

        }

        public string AttackVessels(string attackingVesselName, string defendingVesselName)
        {
            IVessel vesselAttacker = vessels.FindByName(attackingVesselName);
            IVessel vesselDefender = vessels.FindByName(defendingVesselName);
            if (vesselAttacker == null)
            {
                return string.Format(OutputMessages.VesselNotFound,attackingVesselName);
                //return $"Vessel {attackingVesselName} could not be found.";
            }

            if (vesselDefender == null)
            {
                return string.Format(OutputMessages.VesselNotFound,defendingVesselName);
                //return $"Vessel {defendingVesselName} could not be found.";

            }

            if (vesselAttacker.ArmorThickness == 0)
            {
                return string.Format(OutputMessages.AttackVesselArmorThicknessZero, attackingVesselName);
                //return $"Unarmored vessel {attackingVesselName} cannot attack or be attacked.";
            }

            if (vesselDefender.ArmorThickness == 0)
            {
                return String.Format(OutputMessages.AttackVesselArmorThicknessZero,defendingVesselName);
                //return $"Unarmored vessel {defendingVesselName} cannot attack or be attacked.";
            }

            vesselAttacker.Attack(vesselDefender);
            vesselAttacker.Captain.IncreaseCombatExperience();
            vesselDefender.Captain.IncreaseCombatExperience();
            return
                $"Vessel {defendingVesselName} was attacked by vessel {attackingVesselName} - current armor thickness: " +
                $"{vesselDefender.ArmorThickness}.";
        }

        public string ServiceVessel(string vesselName)
        {
            IVessel vessel = vessels.FindByName(vesselName);
            if (vessel == null)
            {
                return $"Vessel {vesselName} could not be found.";
            }
            if (vessel.GetType().Name == "Battleship" && vessel.ArmorThickness < 300)
            {
                vessel.RepairVessel();
                return $"Vessel {vesselName} was repaired.";
            }
            else if (vessel.GetType().Name == "Submarine" && vessel.ArmorThickness < 200)
            {
                vessel.RepairVessel();
                return $"Vessel {vesselName} was repaired.";
            }

            return "";
        }
    }
}
