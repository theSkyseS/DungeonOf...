using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace RGR___Dungeon
{
    class Program
    {
        static List<string> LoadScore()
        {
            using (StreamReader sr = new StreamReader(@"scoreboard.json"))
            {
                List<string> score = new List<string>();
                while(!sr.EndOfStream)
                {
                    score.Add(sr.ReadLine());
                }
                sr.Close();
                sr.Dispose();
                return score;
            }
        }
        static void SaveScore(List<string> score)
        {
            using(StreamWriter sw = new StreamWriter(@"scoreboard.json"))
            {
                for(int i = 0; i < score.Count; i++)
                {
                    sw.WriteLine(score[i]);
                }
                sw.Close();
                sw.Dispose();
            }
        }
        static void AddToScoreboard(List<string> score, Player player)
        {
            score.Add(string.Format("{0}, Счёт: {1}", player.name, player.score));
        }
        static void Round(bool door, Player player, List<string> score)
        {
            if (door)
            {
                Enemy enemy = GenerateEnemy();
                Console.WriteLine(string.Format("Вам встретился {0}! Приготовьтесь к битве!", enemy.name));
                Console.ReadKey();
                while (player.health > 0 && enemy.health > 0)
                {
                    Console.Clear();
                    Console.WriteLine(string.Format("Здоровье: {0}, Монеты: {1}, Зелья здоровья: {2}",
                                                    player.health,
                                                    player.score,
                                                    player.healthPotions));
                    Console.WriteLine(string.Format("Враг: {0}, Здоровье врага: {1}",
                                                    enemy.name,
                                                    enemy.health));
                    player.InflictAttack(enemy);
                    enemy.InflictAttack(player);
                    Console.ReadKey();
                }

                if (player.health <= 0)
                {
                    Console.WriteLine(string.Format("Поражение! =( Ваш счёт: {0}", player.score));
                    AddToScoreboard(score, player);
                }
                else
                {
                    Console.WriteLine(string.Format("Вы одержали победу над {0}", enemy.name));

                }
                Console.ReadKey();
            }
            else
            {
                player.score += 10;
                player.TakePotions(1);
                Console.WriteLine("Вы нашли немного монет и зелье здоровья");
                Console.WriteLine("(+ 10 монет, + зелье здоровья)");
                Console.ReadKey();
            }
        }
        
        static bool GenerateDoor()
        {
            Random rnd = new Random();
            if (rnd.Next(2) == 1)
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
            Random rnd = new Random();
            switch(rnd.Next(3))
            {
                case 0: return new Rat();
                case 1: return new DarkKnight();
                //case 2: return new BFM();
                default: return new Rat();
            }
        }
        static void SaveGame(Player player)
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(@"save.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, player);
            }
        }
        static Player LoadGame()
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamReader sw = new StreamReader(@"save.json"))
            using (JsonReader rdr = new JsonTextReader(sw))
            {
                Player player = serializer.Deserialize<Player>(rdr);
                return player;
            }
        }
        static void GameCycle(Player player, List<string> score)
        {
            do
            {
                Console.Clear();
                Console.WriteLine(string.Format("Здоровье: {0}, Монеты: {1}, Зелья здоровья: {2}",
                                                player.health,
                                                player.score,
                                                player.healthPotions));
                bool door = GenerateDoor();
                Console.WriteLine("Введите число: \n 1 или 2 - выбрать дверь \n 3 - сохранение(перезапишет старое) \n 4 - выход в меню");
                string selectedDoor = Console.ReadLine();
                switch (selectedDoor)
                {
                    case "1":
                        {
                            Round(door, player, score);
                            break;
                        }

                    case "2":
                        {
                            Round(!door, player, score);
                            break;
                        }
                    case "3": SaveGame(player); break;
                    case "4": SaveScore(score);
                        return;
                    default:
                        Console.WriteLine("Неправильно");
                        break;
                }
            } while (player.health > 0);
            SaveScore(score);
            Main();
        }
        static void WriteScoreBoard(List<string> score)
        {
            for (int i = 0; i < score.Count; i++) Console.WriteLine(score[i]);
        }

        static void Main()
        {
            Console.Clear();
            Player player = new Player();
            List<string> score = new List<string>();
            score = LoadScore();
            try
            {
                Console.WriteLine("Введите номер действия:\n 1 - Новая игра \n 2 - Загрузить игру \n 3 - Доска Почёта \n 4 - Выход");
                int switchable = int.Parse(Console.ReadLine());
                switch (switchable)
                {
                    case 1: Console.WriteLine("Введите имя персонажа");
                        player.name = Console.ReadLine();
                        GameCycle(player, score); break;
                    case 2: GameCycle(LoadGame(), score); break;
                    case 3: WriteScoreBoard(score);
                        Console.ReadKey();
                            break;
                    case 4: return;
                    default: return;
                }
                Main();
            }catch(Exception)
            {
                Console.WriteLine("Некорректный ввод");
                Main();
            }
        }
    }
}
