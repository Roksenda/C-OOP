﻿using System;
using System.Collections.Generic;
using System.Text;
using SpaceStation.Models.Astronauts.Contracts;
using SpaceStation.Models.Bags;
using SpaceStation.Models.Bags.Contracts;

namespace SpaceStation.Models.Astronauts
{
    public abstract class Astronaut : IAstronaut
    {
        private string name;
        private double oxygen;
        private List<string> items;

        public Astronaut(string name, double oxygen)
        {
            Name = name;
            Oxygen = oxygen;
            Bag = new Backpack();
        }
        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Astronaut name cannot be null or empty.");
                }

                name = value;
            }
        }

        public double Oxygen
        {
            get => oxygen;
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Cannot create Astronaut with negative oxygen!");
                }

                oxygen = value;
            }
        }

        public bool CanBreath
            => Oxygen > 0;

        public IBag Bag { get; }

        public virtual void Breath()
        {
            if (Oxygen - 10 < 0)
            {
                Oxygen = 0;
            }
            else
            {
                Oxygen -= 10;
            }
        }

    }
}
