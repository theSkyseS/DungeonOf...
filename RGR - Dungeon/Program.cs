using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RGR___Dungeon
{
    class Program
    {
        #region MainMethods
        static void Main()
        {
            //StartMusic();
            GameMethod();
        }
        private static void GameMethod()
        {
            Console.Clear();
            Player player = new Player();
            Score score = Score.LoadScore();
            try
            {
                Console.WriteLine("Введите номер действия:\n 1 - Новая игра \n 2 - Загрузить игру \n 3 - Доска Почёта \n 4 - Выход");
                int switchable = int.Parse(Console.ReadLine());
                switch (switchable)
                {
                    case 1:
                        Console.WriteLine("Введите имя персонажа");
                        player.name = Console.ReadLine();
                        if (player.name == "") player.name = "Игрок";
                        if (player.name == "Doom Guy")
                        {
                            Console.WriteLine("Видно, Вы опытный игрок. Сложность +5");
                            Player.difficulty += 5;                           
                        }
                        GameCycle(player, score); 
                        break;
                    case 2: GameCycle(LoadGame(), score); 
                        break;
                    case 3:
                        score.WriteScoreBoard();
                        Console.ReadKey();
                        break;
                    default: return;
                }
                GameMethod();
            }
            catch (Exception)
            {
               //Console.WriteLine(e.Message + "\n" + e.StackTrace + "\n" + e.Source + "\n" + e.InnerException);
                Console.WriteLine("Некорректный ввод");
                Console.ReadKey();
                GameMethod();
            }
        }

        static void GameCycle(Player player, Score score)
        {
            do
            {
                Console.Clear();
                player.CheckLevelUp();
                DifficultyChange(player);
                Console.Clear();
                Console.WriteLine(string.Format("Здоровье: {0}, Броня: {4}, Монеты: {1}, Зелья здоровья: {2}, Очков опыта: {3}",
                                                player.Health,
                                                player.score,
                                                player.healthPotions,
                                                player.expirience,
                                                player.currentArmor.durability));
                bool door = GenerateDoor();
                Console.WriteLine("Введите число: \n "
                                  + "1 или 2 - выбрать дверь \n "
                                  + "3 - использовать зелье \n "
                                  + "4 - сохранение(перезапишет старое) \n "
                                  + "5 - выход в меню");
                string selectedDoor = Console.ReadLine();
                switch (selectedDoor)
                {
                    case "1": Round(door, player, score); break;
                    case "2": Round(!door, player, score); break;
                    case "3": player.UsePotion(); Console.ReadKey(); break;
                    case "4": SaveGame(player); break;
                    case "5":score.SaveScore(); return;
                    default: Console.WriteLine("Неккоректный ввод"); break;
                }
            } while (player.Health > 0);
            score.SaveScore();
        }
        static void Round(bool door, Player player, Score score)
        {
            if (door)
            {
                Enemy enemy = GenerateEnemy();
                Console.WriteLine(string.Format("Вам встретился {0}! Приготовьтесь к битве!", enemy.name));
                Console.ReadKey();
                while (player.Health > 0 && enemy.Health > 0)
                {
                    Console.Clear();
                    Console.WriteLine(string.Format("Здоровье: {0}, Броня: {3}, Монеты: {1}, Зелья здоровья: {2}",
                                                    player.Health,
                                                    player.score,
                                                    player.healthPotions,
                                                    player.currentArmor.durability));
                    Console.WriteLine(string.Format("Враг: {0}, Здоровье врага: {1}",
                                                    enemy.name,
                                                    enemy.Health));
                    player.InflictAttack(enemy);
                    player.DamageWeapon();
                    enemy.InflictAttack(player);
                    Console.ReadKey();
                }

                if (player.Health <= 0)
                {
                    Console.WriteLine(string.Format("Поражение! =( Ваш счёт: {0}.", player.score));
                    score.AddToScoreboard(player);
                }
                else
                {
                    Console.WriteLine(string.Format("Вы одержали победу над {0} \n "
                                                    + "Получено опыта: {1}.",
                                                    enemy.name,
                                                    enemy.Exp));
                    Console.WriteLine();
                    Console.WriteLine(string.Format("Вы нашли {0} монет.", GenerateGold(player)));
                    player.expirience += enemy.Exp;
                    if (rnd.Next(101) < 30)
                    {
                        player.TakeNewWeapon(new RandomWeapon());
                    }
                    if (rnd.Next(101) < 25)
                    {
                        player.TakeNewArmor(GenerateArmor());
                    }
                }
                Console.ReadKey();
            }
            else
            {
                if (rnd.Next(101) < 15)
                {
                    player.TakeNewWeapon(new RandomWeapon());
                }
                if (rnd.Next(101) < 15)
                {
                    player.TakeNewArmor(GenerateArmor());
                }
                player.TakePotions(1);
                Console.WriteLine("Вы нашли немного монет и зелье здоровья");
                Console.WriteLine(string.Format("(+ {0} монет, + зелье здоровья)", GenerateGold(player)));
                Console.ReadKey();
            }
        }

        static void DifficultyChange(Player player)
        {
            if (player.Level >= 3 * Player.difficulty)
            {
                Player.difficulty += 1;
                Console.Clear();
                Console.WriteLine("Вы нашли дверь, ведущую на более глубокий уровень подземелья.\n"
                                  + "Враги станут сильнее, но и награда больше.");
                Console.ReadKey();
            }
        }
        #endregion

        #region Generators
        static bool GenerateDoor()
        {
            if (rnd.Next(100) <= 60)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static Enemy GenerateEnemy()
        {
            switch (rnd.Next(7))
            {
                case 0: return new Rat();
                case 1: return new DarkKnight();
                case 2: return new SlimeMonster();
                case 3: return new Zombie();
                case 4: return new SkeletonArcher();
                case 5: return new VengefulSpirit();
                case 6: return new Mage();
                default: return new Rat();
            }
        }

        static Armor GenerateArmor()
        {
            switch (rnd.Next(1, 5))
            {
                case 1:
                    return new ChitinArmor();
                case 2:
                    return new PlateArmor();
                case 3:
                    return new ClothArmor();
                case 4:
                    return new LeatherArmor();
                default:
                    return new LeatherArmor();
            }
        }
        static int GenerateGold(Player player)
        {
            int money = rnd.Next(10 * Player.difficulty, 50 * Player.difficulty);
            player.score += money;
            return money;
        }

        public static Random rnd = new Random();
        #endregion

        #region Save Load System
        static void SaveGame(Player player)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(@"saves/save.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, player);
            }
        }
        static Player LoadGame()
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(@"saves/save.dat", FileMode.OpenOrCreate))
                {
                    Player player = new Player();
                    player = (Player)formatter.Deserialize(fs);
                    Player.difficulty = player.Level / 3;
                    if (player.name == "Doom Guy")
                        Player.difficulty += 5;
                    return player;
                }
            }
            catch (IOException /*e*/)
            {
                //Console.WriteLine(e.Message + "\n" + e.StackTrace + "\n" + e.Source + "\n" + e.InnerException);
                Console.WriteLine("Не найдено сохранений. Начало новой игры.");
                Console.ReadKey();
                return new Player();
            }
        } 
            #endregion

        #region Music System
        static void StartMusic()
        {
            WMPLib.WindowsMediaPlayer musicPlayer = new WMPLib.WindowsMediaPlayer
            {
                URL = @"music/GameMusic.wpl"
            };
            musicPlayer.settings.volume = 6;
            musicPlayer.controls.play(); 
        }
        #endregion
    }
}
