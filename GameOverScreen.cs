using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodgeTheSquares
{
    public class GameOverScreen : Screen
    {
        private ScreenManager _screenManager;
        private GraphicsDevice _graphicsDevice;
        private SoundManager _soundManager;

        private SpriteFont _font;
        private Vector2 _textSize;
        private Vector2 _textPosition;
        private int _screenWidth;
        private int _screenHeight;

        private string _backNavigation = "Back";
        private Vector2 _backPosition;
        private Color _backColor = Color.OrangeRed;


        private string _restartNavigation = "Restart";
        private Vector2 _restartPosition;
        private Color _restartColor = Color.OrangeRed;

        private int _selectedOption = 0;

        public GameOverScreen(SpriteFont font, int screenWidth, int screenHeight, ScreenManager screenManager, GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            _font = font;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _graphicsDevice = graphicsDevice;
            _screenManager = screenManager;

            // Initialize positions
            _textSize = _font.MeasureString("Game Over");
            _textPosition = new Vector2((_screenWidth - _textSize.X) / 2, (_screenHeight - _textSize.Y) / 2);

            _backPosition = new Vector2(_textPosition.X - 20, _textPosition.Y + 30);
            _restartPosition = new Vector2(_textPosition.X + 70, _textPosition.Y + 30);

            _soundManager = new SoundManager();
            _soundManager.LoadContent(contentManager);
        }



        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Begin();
            spriteBatch.DrawString(_font, "Game Over", _textPosition, Color.OrangeRed);
            spriteBatch.DrawString(_font, _backNavigation, _backPosition, _backColor);
            spriteBatch.DrawString(_font, _restartNavigation, _restartPosition, _restartColor);
            spriteBatch.End();
        }

        public void GetOptionsPosition(MouseState mouseState, MouseState previousMouseState) 
        {
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
                _backColor = Color.OrangeRed; // Reset color when not hovering
            }

            Vector2 restartSize = _font.MeasureString(_restartNavigation);
            Rectangle restartRectangle = new Rectangle(_restartPosition.ToPoint(), restartSize.ToPoint());

            // Check if the mouse is hovering over "Restart"
            if (restartRectangle.Contains(mouseState.Position))
            {
                _selectedOption = 2;
                if (_selectedOption != previousSelectedOption)
                {
                    _soundManager.PlayHoverSound(1);
                }
                _restartColor = Color.Yellow; // Change color to yellow when hovering

                // Check if the user clicks on "Back"
                if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                {
                    _soundManager.PlayMenuClickSound(1);
                    _screenManager.SetScreen(new PlayScreen(_graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height, _graphicsDevice, _screenManager));
                }
            }
            else
            {
                _restartColor = Color.OrangeRed; // Reset color when not hovering
            }

        }
    }
}
