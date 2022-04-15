﻿using System;
using System.Collections.Generic;
using System.Text;
using SpaceStation.Models.Bags.Contracts;

namespace SpaceStation.Models.Bags
{
    public class Backpack : IBag
    {

        public Backpack()
        {
            Items = new List<string>();
        }

        public ICollection<string> Items { get; }
            
    }
}
