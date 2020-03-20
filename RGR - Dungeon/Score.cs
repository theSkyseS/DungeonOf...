using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR___Dungeon
{
    class Score
    {
        private Dictionary<string, int> scoreBoard = new Dictionary<string, int>();

        public static Score LoadScore()
        {
            using (StreamReader sr = new StreamReader(@"saves/scoreboard.json"))
            {
                Score score = new Score();
                while (!sr.EndOfStream)
                {
                    score.scoreBoard.Add(sr.ReadLine(), int.Parse(sr.ReadLine()));
                }
                sr.Close();
                return score;
            }
        }
        public void SaveScore()
        {
            using (StreamWriter sw = new StreamWriter(@"saves/scoreboard.json"))
            {
                for (int i = 0; i < scoreBoard.Count; i++)
                {
                    sw.WriteLine(scoreBoard.Keys.ElementAt(i));
                    sw.WriteLine(scoreBoard.Values.ElementAt(i));
                }
                sw.Close();
            }
        }
        public void AddToScoreboard(Player player)
        {
            scoreBoard.Add(player.name, player.score);
        }
        public void WriteScoreBoard()
        {
            for(int i = 0; i < scoreBoard.Count; i++)
                Console.WriteLine(scoreBoard.ElementAt(i));
        }

        
    }
}
