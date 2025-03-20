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
    public class HelpScreen : Screen
    {
        private ScreenManager _screenManager;
        private GraphicsDevice _graphicsDevice;
        private SoundManager _soundManager;

        private Texture2D _backgroundImage;

        private SpriteFont _font;
        private string _backNavigation = "Back";
        private Vector2 _backPosition = new Vector2(100, 100);
        private Color _backColor = Color.OrangeRed;

        private int _selectedOption = 0;

        private string _helpTitle = "Dodge The Squares!";
        private string _helpText = "How to play:\nThere is but one rule, Dodge The Squares!\n\nWith the mouse, control the movement of the yellow square to avoid the moving green squares." +
            "\nEvery 5 seconds they speed up and every 10 seconds another green square is added!";



        public HelpScreen(ScreenManager screenManager, GraphicsDevice graphicsDevice)
        {
            _screenManager = screenManager;
            _graphicsDevice = graphicsDevice;
            _soundManager = new SoundManager();
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

            int xPosition = screenWidth / 2;
            int yPosition = screenHeight / 8;

            spriteBatch.Begin();
            spriteBatch.Draw(_backgroundImage, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            spriteBatch.DrawString(_font, _helpTitle, new Vector2(xPosition - 150, yPosition), Color.OrangeRed);
            spriteBatch.DrawString(_font, _helpText, new Vector2(screenWidth / 5, screenHeight / 4), Color.OrangeRed);


            spriteBatch.DrawString(_font, _backNavigation, _backPosition, _backColor);
            spriteBatch.End();
        }
    }

}
