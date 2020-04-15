using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RGR___Dungeon
{
    class Score
    {
        private List<int> scores = new List<int>();
        private List<string> playerNames = new List<string>();

        public static Score LoadScore()
        {
            using (StreamReader sr = new StreamReader(@"saves/scoreboard.json"))
            {
                Score score = new Score();
                while (!sr.EndOfStream)
                {
                    score.playerNames.Add(sr.ReadLine());
                    score.scores.Add(int.Parse(sr.ReadLine()));
                }
                sr.Close();
                return score;
            }
        }
        public void SaveScore()
        {
            using (StreamWriter sw = new StreamWriter(@"saves/scoreboard.json"))
            {
                for (int i = 0; i < scores.Count; i++)
                {
                    sw.WriteLine(playerNames.ElementAt(i));
                    sw.WriteLine(scores.ElementAt(i));
                }
                sw.Close();
            }
        }
        public void AddToScoreboard(Player player)
        {
            scores.Add(player.score);
            playerNames.Add(player.name);
        }
        public void WriteScoreBoard()
        {
            for(int i = 0; i < playerNames.Count; i++)
                Console.WriteLine(playerNames.ElementAt(i) + ", набрано очков: " + scores.ElementAt(i));
        }
    }
}
