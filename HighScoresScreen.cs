using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodgeTheSquares
{
    public class HighScoresScreen : Screen
    {
        private ScreenManager _screenManager;
        private GraphicsDevice _graphicsDevice;
        private SoundManager _soundManager;
        private Scores _scores;
        private List<ScoreEntry> _highScores;

        private Texture2D _backgroundImage;

        private SpriteFont _font;
        private string _backNavigation = "Back";
        private Vector2 _backPosition = new Vector2(100, 100);
        private Color _backColor = Color.OrangeRed;

        private int _selectedOption = 0;



        public HighScoresScreen(ScreenManager screenManager, GraphicsDevice graphicsDevice)
        {
            _screenManager = screenManager;
            _graphicsDevice = graphicsDevice;
            _soundManager = new SoundManager();


            _scores = new Scores(_font, new Vector2(10, 10), graphicsDevice);
            _highScores = _scores.GetTopHighScores(_scores.GetScores());

        }

        public override void LoadContent(ContentManager content)
        {
            _backgroundImage = content.Load<Texture2D>("Background");
            _font = content.Load<SpriteFont>("MenuFont");
            _soundManager.LoadContent(content);


        }

        public override void Update(GameTime gameTime, MouseState mouseState, MouseState previousMouseState)
        {
            // Calculate the bounding rectangle for "Back"
            Vector2 backSize = _font.MeasureString(_backNavigation);
            Rectangle backRectangle = new Rectangle(_backPosition.ToPoint(), backSize.ToPoint());
            int previousSelectedOption = _selectedOption;


            // Check if the mouse is hovering over "Back"
            if (backRectangle.Contains(mouseState.Position))
            {
                _selectedOption = 1;
                if (_selectedOption != previousSelectedOption)
                {
                    _soundManager.PlayHoverSound(1);
                }
                _backColor = Color.Yellow; // Change color to yellow when hovering

                // Check if the user clicks on "Back"
                if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                {
                    _soundManager.PlayMenuClickSound(1);
                    _screenManager.SetScreen(new MenuScreen(_screenManager, _graphicsDevice));
                }
            }
            else
            {
                _selectedOption = 0;
                _backColor = Color.OrangeRed; // Reset color when not hovering
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int screenWidth = spriteBatch.GraphicsDevice.Viewport.Width;
            int screenHeight = spriteBatch.GraphicsDevice.Viewport.Height;


            spriteBatch.Begin();
            spriteBatch.Draw(_backgroundImage, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            spriteBatch.DrawString(_font, "High Scores!", new Vector2(screenWidth/2 -200, screenHeight/ 6 -50), Color.OrangeRed, 0f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 1);
            spriteBatch.DrawString(_font, "So Wow !", new Vector2(screenWidth / 2 + 200, screenHeight / 6 - 50), Color.OrangeRed, 1f, new Vector2(0, 0), 1.2f, SpriteEffects.None, 1);
            spriteBatch.DrawString(_font, "Get Good", new Vector2(screenWidth / 2 - 300, screenHeight / 6 + 200), Color.OrangeRed, -1.5f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 1);
            spriteBatch.DrawString(_font, "lolz", new Vector2(screenWidth / 2 +100, screenHeight / 6 + 500), Color.OrangeRed, 1.2f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 1);

            for (int i = 0; i < _highScores.Count; i++)
            {
                var entry = _highScores[i];
                string scoreText = $"{i + 1}. {entry.PlayerName}: {entry.Time.Minutes:00}:{entry.Time.Seconds:00}:{Math.Round(entry.Time.Milliseconds / 10.0):00}";
                spriteBatch.DrawString(_font, scoreText, new Vector2(screenWidth / 2 - 200, screenHeight / 5 + i * 100), Color.OrangeRed);
            }


            spriteBatch.DrawString(_font, _backNavigation, _backPosition, _backColor);
            spriteBatch.End();
        }
    }


}

