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
    public class AboutScreen : Screen
    {

        private ScreenManager _screenManager;
        private GraphicsDevice _graphicsDevice;
        private SoundManager _soundManager;

        private Texture2D _backgroundImage;
        private Texture2D _christmassImage;

        private SpriteFont _font;
        private string _backNavigation = "Back";
        private string _aboutMe = "In rural Ontario, Merrick is the proud cat dad to his beloved \n" +
                                  "companions, Loki and Thor. Driven by a desire for new \n" +
                                  "challenges, he has embraced his passion for application \n" +
                                  "development, setting his sights on crafting interactive \n" +
                                  "games like \"Dodge The Squares!\" With a vision for the \n" +
                                  "future, he aims to grow his business, \"Pylon Games Inc,\" \n" +
                                  "to create diverse and thrilling games that inspire and \n" +
                                  "stimulate the mind.";
        private Vector2 _backPosition = new Vector2(100, 100);
        private Color _backColor = Color.OrangeRed;

        private int _selectedOption = 0;


        public AboutScreen(ScreenManager screenManager, GraphicsDevice graphicsDevice)
        {
            _screenManager = screenManager;
            _graphicsDevice = graphicsDevice;
            _soundManager = new SoundManager();
        }

        public override void LoadContent(ContentManager content)
        {
            _backgroundImage = content.Load<Texture2D>("Background");
            _christmassImage = content.Load<Texture2D>("ChristmassImage");
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
            spriteBatch.DrawString(_font, "About Me", new Vector2(screenWidth / 2 - 200, screenHeight / 6 - 50), Color.OrangeRed, 0f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 1);
            spriteBatch.Draw(_christmassImage, new Rectangle(200, 200, 700, 740), Color.White);
            spriteBatch.DrawString(_font, _aboutMe, new Vector2(950, 450), Color.OrangeRed);
            spriteBatch.DrawString(_font, _backNavigation, _backPosition, _backColor);
            spriteBatch.End();
        }
    }

}
