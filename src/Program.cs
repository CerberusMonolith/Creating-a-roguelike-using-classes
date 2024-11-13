using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xz
{
    public class Player
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public Aid Aid { get; set; }
        public Weapon Weapon { get; set; }
        public int Score { get; set; }

        public Player(string name, int maxHealth)
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = MaxHealth;
            Score = 0;
        }

        public void Heal(int amount)
        {
            Health = Math.Min(Health + amount, MaxHealth);
        }

        public void Attack(Enemy enemy)
        {
            if (Weapon != null)
            {
                enemy.TakeDamage(Weapon.Damage);
            }
        }
    }

    public class Enemy
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public Weapon Weapon { get; set; }

        public Enemy(string name, int maxHealth, Weapon weapon)
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = MaxHealth;
            Weapon = weapon;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
        }

        public void Attack(Player player)
        {
            if (Weapon != null)
            {
                player.Health -= Weapon.Damage;
            }
        }
    }

    public class Aid
    {
        public string Name { get; set; }
        public int HealAmount { get; set; }

        public Aid(string name, int healAmount)
        {
            Name = name;
            HealAmount = healAmount;
        }
    }

    public class Weapon
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int Durability { get; set; }

        public Weapon(string name, int damage, int durability)
        {
            Name = name;
            Damage = damage;
            Durability = durability;
        }
    }

    class Program
    {
        static Random random = new Random();

        static string GenerateRandomName()
        {
            string[] names = { "Варвар", "Гоблин", "Огр", "Скелет", "Зомби", "Тролль", "Прокажённый крсетьянин", "Обезумевший инквизитор" };
            return names[random.Next(names.Length)];
        }

       
        static Weapon GenerateRandomWeapon()
        {
            string[] weaponNames = { "Меч", "Топор", "Копье", "Лук", "Дубина", "Длинный меч", "Булава", "Длинный лук", "Колдовской посох" };
            return new Weapon(weaponNames[random.Next(weaponNames.Length)], random.Next(5, 21), random.Next(1, 11));
        }

        
        static Aid GenerateRandomAid()
        {
            string[] aidNames = { "Средняя", "Большая", "Маленькая" };
            return new Aid(aidNames[random.Next(aidNames.Length)], random.Next(5, 21));
        }

      
        static void Fight(Player player, Enemy enemy)
        {
            Console.WriteLine($"{player.Name} встречает врага {enemy.Name}, у врага на поясе сияет оружие {enemy.Weapon.Name} ({enemy.Weapon.Damage})");
            Console.WriteLine($"У вас {player.Health}hp, у врага {enemy.Health}hp");

            while (player.Health > 0 && enemy.Health > 0)
            {
                Console.WriteLine($"Что вы будете делать?");
                Console.WriteLine("1. Ударить");
                Console.WriteLine("2. Пропустить ход");
                Console.WriteLine("3. Использовать аптечку");

                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        player.Attack(enemy);
                        Console.WriteLine($"{player.Name} ударил противника {enemy.Name}");
                        Console.WriteLine($"У врага {enemy.Health}hp, у вас {player.Health}hp");
                        if (enemy.Health > 0)
                        {
                            enemy.Attack(player);
                            Console.WriteLine($"Противник {enemy.Name} ударил вас!");
                            Console.WriteLine($"У врага {enemy.Health}hp, у вас {player.Health}hp");
                        }
                        break;
                    case 2:
                        Console.WriteLine($"{player.Name} пропустил ход");
                        enemy.Attack(player);
                        Console.WriteLine($"Противник {enemy.Name} ударил вас!");
                        Console.WriteLine($"У врага {enemy.Health}hp, у вас {player.Health}hp");
                        break;
                    case 3:
                        if (player.Aid != null)
                        {
                            player.Heal(player.Aid.HealAmount);
                            Console.WriteLine($"{player.Name} использовал аптечку");
                            Console.WriteLine($"У врага {enemy.Health}hp, у вас {player.Health}hp");
                        }
                        else
                        {
                            Console.WriteLine("У вас нет аптечки!");
                        }
                        break;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }

            if (enemy.Health <= 0)
            {
                Console.WriteLine($"Вы победили {enemy.Name}! +10 очков");
                player.Score += 10;
            }
            else
            {
                Console.WriteLine("Вы погибли!");
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать!");
            Console.WriteLine("Кто ты воин?");
            Console.WriteLine("Назови себя:");
            string playerName = Console.ReadLine();

            Console.WriteLine($"Ваше имя **{playerName}**!");

           
            Player player = new Player(playerName, 100);

            
            player.Weapon = GenerateRandomWeapon();
            player.Aid = GenerateRandomAid();

            Console.WriteLine($"Вам был принесён  **{player.Weapon.Name} ({player.Weapon.Damage})**, а также **{player.Aid.Name}** аптечка ({player.Aid.HealAmount}hp).");

            
            while (player.Health > 0)
            {
                
                Enemy enemy = new Enemy(GenerateRandomName(), random.Next(20, 61), GenerateRandomWeapon());

               
                Fight(player, enemy);

                if (player.Health <= 0)
                {
                    break;
                }
            }

            Console.WriteLine($"Игра окончена. Ваш счет: {player.Score}");
            Console.ReadKey();
        }
    }
}