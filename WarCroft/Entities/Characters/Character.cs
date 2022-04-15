using System;

using WarCroft.Constants;
using WarCroft.Entities.Inventory;
using WarCroft.Entities.Items;

namespace WarCroft.Entities.Characters.Contracts
{
    public abstract class Character
    {
        // TODO: Implement the rest of the class.
        private string name;
        private double health;
        private double armor;
        private double baseHealth;
        private double baseArmor;

        public Character(string name, double health, double armor, double abilityPoints, Bag bag)
        {
            Name = name;
            BaseHealth = health;
            BaseArmor = armor;
            Health = health;
            Armor = armor;
            AbilityPoints = abilityPoints;
            Bag = bag;
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name cannot be null or whitespace!");
                }

                name = value;
            }
        }

        public double BaseHealth { get; }
        public double Health
        {
            get => health;
            internal set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > BaseHealth)
                {
                    value = BaseHealth;
                }

                health = value;
            }


        }

        public double BaseArmor { get; }

        public double Armor
        {
            get => armor;
            private set
            {
                if (value < 0)
                {
                    value = 0;
                }

                armor = value;
            }
        }

        public double AbilityPoints { get; }
        public Bag Bag { get; }
        public bool IsAlive { get; set; } = true;

        protected void EnsureAlive()
        {
            if (!this.IsAlive)
            {
                throw new InvalidOperationException(ExceptionMessages.AffectedCharacterDead);
            }
        }

        public void TakeDamage(double hitPoints)
        {
            EnsureAlive();
            if (Armor >= hitPoints)
            {
                Armor -= hitPoints;
            }
            else
            {
                double diff = hitPoints - Armor;
                Health -= diff;
                Armor = 0;
            }

            if (Health <= 0)
            {
                IsAlive = false;
            }
        }

        public void UseItem(Item item)
        {
            EnsureAlive();
            item.AffectCharacter(this);
        }

    }
}