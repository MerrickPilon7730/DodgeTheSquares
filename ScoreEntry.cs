using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodgeTheSquares
{
    public class ScoreEntry
    {
        public string PlayerName { get; set; }
        public TimeSpan Time { get; set; }

        public ScoreEntry(string playerName, TimeSpan time)
        {
            PlayerName = playerName;
            Time = time;
        }
    }
}
