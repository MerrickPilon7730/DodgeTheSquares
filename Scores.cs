using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace DodgeTheSquares
{
    class Scores
    {
        private TimeSpan _timePlayed;
        private List<ScoreEntry> _highScores;
        private SpriteFont _font;
        private Vector2 _position;
        private string _filePath;

        private int _screenWidth;
        private int _screenHeight;

        public Scores(SpriteFont font, Vector2 position, GraphicsDevice graphicsDevice)
        {
            _font = font;
            _position = position;
            _timePlayed = TimeSpan.Zero;
            _highScores = new List<ScoreEntry>();

            _screenWidth = graphicsDevice.Viewport.Width;
            _screenHeight = graphicsDevice.Viewport.Height;

        }

        public void Update(GameTime gameTime, bool isPaused)
        {
            if (!isPaused)
            {
                _timePlayed += gameTime.ElapsedGameTime;
            }
        }
        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("MenuFont");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            string timePlayedText = $"Time: {_timePlayed.Minutes:D2}:{_timePlayed.Seconds:D2}:{Math.Round(_timePlayed.Milliseconds / 10.0):00}";
            spriteBatch.DrawString(_font, timePlayedText, _position, Color.White);

        }

        public List<ScoreEntry> GetScores()
        {

            DirectoryInfo directory = new DirectoryInfo(Environment.CurrentDirectory);
            DirectoryInfo parentDirectory = directory.Parent.Parent.Parent;
            _filePath = Path.Combine(parentDirectory.FullName, "HighScores", "highScores.txt");

            Console.WriteLine("Current Directory: " + Environment.CurrentDirectory);
            Console.WriteLine("Parent Directory: " + parentDirectory.FullName);

            _highScores.Clear();

            if (!File.Exists(_filePath))
            {
                // Create a new file if it doesn't exist
                using (File.Create(_filePath)) { }

                // Seed the file with the required data
                string[] lines = new string[]
                {
                    "Merrick: 00:00:04.8333430",
                    "Merrick: 00:00:42.7500855",
                    "Merrick: 00:00:45.0500901",
                    "Colleen: 00:00:48.1167629",
                    "Merrick: 00:00:52.2001044"
                };

                // Write the data to the file
                File.WriteAllLines(_filePath, lines);
            }

            if (File.Exists(_filePath))
            {

                using (StreamReader reader = new StreamReader(_filePath))
                {
                    // Read the lines and parse the name and time
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split(": ");
                        if (parts.Length == 2)
                        {
                            string playerName = parts[0];
                            TimeSpan time = TimeSpan.Parse(parts[1]);

                            // Add the player and their time to the dictionary
                            _highScores.Add(new ScoreEntry(playerName, time));
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("File does not exist at path: " + _filePath);
            }

            return _highScores;
        }

        public List<ScoreEntry> GetTopHighScores(List<ScoreEntry> highScores)
        {
            // Sort the List by time in descending order and take the top 5
            return highScores
                .OrderByDescending(entry => entry.Time)
                .Take(5)
                .ToList();
        }

        public bool IsNewHighScore(TimeSpan time)
        {
            // Get the high scores from the file or current state
            var allHighScores = GetTopHighScores(GetScores());

            // If the list is not full, it's a new high score
            if (allHighScores.Count < 5)
                return true;

            // Check if the current time is better than the lowest time in the top 5
            return time > allHighScores.Last().Time;
        }

        public void AddNewHighScore(string playerName, TimeSpan time)
        {
            // Add the new high score
            _highScores.Add(new ScoreEntry(playerName, time));

            // Sort by time, keeping the best 5 scores
            _highScores = _highScores.OrderByDescending(entry => entry.Time).Take(5).ToList();

            // Write the new high scores to the file
            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                foreach (var score in _highScores.OrderBy(entry => entry.Time))
                {
                    writer.WriteLine($"{score.PlayerName}: {score.Time}");
                }
            }
        }

        public TimeSpan GetGameTime()
        {
            return _timePlayed;
        }
    }
}
