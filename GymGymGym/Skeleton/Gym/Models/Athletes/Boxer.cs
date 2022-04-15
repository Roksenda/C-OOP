﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Gym.Models.Athletes
{
    public class Boxer : Athlete
    {
        private const int boxerStamina = 60;
        public Boxer(string fullName, string motivation, int numberOfMedals)
            : base(fullName, motivation, numberOfMedals, boxerStamina)
        {
        }

        public override void Exercise()
        {
            Stamina += 15;
            if (Stamina > 100)
            {
                Stamina = 100;
                throw new ArgumentException("Stamina cannot exceed 100 points.");
            }
        }
    }
}
