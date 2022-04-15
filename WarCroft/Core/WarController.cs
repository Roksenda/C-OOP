using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarCroft.Constants;
using WarCroft.Entities.Characters;
using WarCroft.Entities.Characters.Contracts;
using WarCroft.Entities.Items;

namespace WarCroft.Core
{
    public class WarController
    {
        private List<Character> characters;
        private Stack<Item> items;
        public WarController()
        {
            characters = new List<Character>();
            items = new Stack<Item>();
        }

        public string JoinParty(string[] args)
        {
            if (nameof(Warrior) != args[0] && nameof(Priest) != args[0])
            {
                throw new ArgumentException(String.Format(ExceptionMessages.InvalidCharacterType, args[0]));
            }

            Character character;
            if (nameof(Warrior) == args[0])
            {
                character = new Warrior(args[1]);
            }
            else
            {
                character = new Priest(args[1]);
            }

            characters.Add(character);
            return $"{args[1]} joined the party!";

        }

        public string AddItemToPool(string[] args)
        {
            if (nameof(HealthPotion) != args[0] && nameof(FirePotion) != args[0])
            {
                throw new ArgumentException(String.Format(ExceptionMessages.InvalidItem));
            }

            Item item;
            if (nameof(HealthPotion) == args[0])
            {
                item = new HealthPotion();
            }
            else
            {
                item = new FirePotion();
            }
            items.Push(item);
            return $"{args[0]} added to pool.";
        }

        public string PickUpItem(string[] args)
        {
            Character character = characters.FirstOrDefault(x => x.Name == args[0]);
            if (character == null)
            {
                throw new ArgumentException($"Character {args[0]} not found!");
            }

            if (items.Count == 0)
            {
                throw new InvalidOperationException("No items left in pool!");
            }

            Item item = items.Pop();

            character.Bag.AddItem(item);

            return $"{args[0]} picked up {item.GetType().Name}!";
        }

        public string UseItem(string[] args)
        {
            Character character = characters.FirstOrDefault(x => x.Name == args[0]);
            if (character == null)
            {
                throw new ArgumentException($"Character {args[0]} not found!");
            }

            Item item = character.Bag.GetItem(args[1]);
            character.UseItem(item);
            return $"{character.Name} used {item.GetType().Name}.";
        }

        public string GetStats()
        {
            List<Character> result = characters.OrderByDescending(x => x.IsAlive)
                .ThenByDescending(x => x.Health).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var part in result)
            {
                sb.Append(
                    $"{part.Name} - HP: {part.Health}/{part.BaseHealth}, AP: {part.Armor}/{part.BaseArmor}, " +
                    $"Status: ");
                if (part.IsAlive)
                {
                    sb.AppendLine("Alive");
                }
                else
                {
                    sb.AppendLine("Dead");
                }

            }
            return sb.ToString().TrimEnd();
        }

        public string Attack(string[] args)
        {
            Character attacker = characters.FirstOrDefault(x => x.Name == args[0]);
            Character receiver = characters.FirstOrDefault(x => x.Name == args[1]);

            if (attacker == null)
            {
                throw new ArgumentException($"Character {args[0]} not found!");
            }
            if (receiver == null)
            {
                throw new ArgumentException($"Character {args[1]} not found!");
            }

            if (attacker.GetType().Name == nameof(Warrior))
            {
                Warrior warrior = attacker as Warrior;
                warrior.Attack(receiver);
            }
            else
            {
                throw new ArgumentException($"{attacker.Name} cannot attack!");
            }

            StringBuilder sb = new StringBuilder();
            sb.Append($"{attacker.Name} attacks {receiver.Name} for {attacker.AbilityPoints} hit points!");
            sb.AppendLine($"{receiver.Name} has {receiver.Health}/{receiver.BaseHealth} HP " +
                          $"and {receiver.Armor}/{receiver.BaseArmor} AP left!");
            if (receiver.IsAlive == false)
            {
                sb.AppendLine($"{receiver.Name} is dead!");
            }

            return sb.ToString().TrimEnd();
        }

        public string Heal(string[] args)
        {
            Character healerName = characters.FirstOrDefault(x => x.Name == args[0]);
            Character healingReceiverName = characters.FirstOrDefault(x => x.Name == args[1]);

            if (healerName == null)
            {
                throw new ArgumentException($"Character {args[0]} not found!");
            }
            if (healingReceiverName == null)
            {
                throw new ArgumentException($"Character {args[1]} not found!");
            }

            if (healerName.GetType().Name == nameof(Priest))
            {
                Priest priest = healerName as Priest;
                priest.Heal(healingReceiverName);
            }
            else
            {
                throw new ArgumentException($"{healerName.Name} cannot heal!");
            }

            return $"{healerName.Name} heals {healingReceiverName.Name} " +
                   $"for {healerName.AbilityPoints}! {healingReceiverName.Name} has {healingReceiverName.Health} health now!";
        }
    }
}
