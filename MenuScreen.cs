using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using DodgeTheSquares;
using System;

public class MenuScreen : Screen
{
    private ScreenManager _screenManager;
    private GraphicsDevice _graphicsDevice;
    private SoundManager _soundManager;

    private Texture2D _backgroundImage;
    private SpriteFont _font;
    private string[] _menuOptions = { "Play", "High Scores", "Help", "About", "Exit" };
    private int _selectedOption = 0;
    private int _menuOptionsXPosition = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 3;
    private int _menuOptionsYPosition = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 3;

    private int _screenWidth;
    private int _screenHeight;

    public MenuScreen(ScreenManager screenManager, GraphicsDevice graphicsDevice)
    {
        _screenManager = screenManager;
        _graphicsDevice = graphicsDevice;
        _screenWidth = graphicsDevice.Viewport.Width;
        _screenHeight = graphicsDevice.Viewport.Height;
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
        Point mousePosition = new Point(mouseState.X, mouseState.Y);
        bool isMouseOverOption = false;
        int previousSelectedOption = _selectedOption;

        for (int i = 0; i < _menuOptions.Length; i++)
        {
            Vector2 optionPosition = new Vector2(_menuOptionsXPosition, _menuOptionsYPosition + i * 50);
            Vector2 optionSize = _font.MeasureString(_menuOptions[i]);
            Rectangle optionRectangle = new Rectangle(optionPosition.ToPoint(), optionSize.ToPoint());

            if (optionRectangle.Contains(mousePosition))
            {
                _selectedOption = i;
                isMouseOverOption = true;

                if (_selectedOption != previousSelectedOption)
                {
                    _soundManager.PlayHoverSound(1);
                }
            }
        }

        if (!isMouseOverOption)
        {
            _selectedOption = -1;
        }

        if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
        {
            _soundManager.PlayMenuClickSound(1);
            switch (_selectedOption)
            {
                case 0: // "Play"
                    _soundManager.StopBackgroundMusic();
                    _screenManager.SetScreen(new PlayScreen(_screenWidth, _screenHeight, _graphicsDevice, _screenManager)); // Switch to PlayScreen
                    break;
                case 1:
                    _screenManager.SetScreen(new HighScoresScreen(_screenManager, _graphicsDevice));
                    break;
                case 2: // "Help"
                    _screenManager.SetScreen(new HelpScreen(_screenManager, _graphicsDevice)); // Switch to HelpScreen
                    break;
                case 3: // "About"
                    _screenManager.SetScreen(new AboutScreen(_screenManager, _graphicsDevice)); // Switch to AboutScreen
                    break;
                case 4: // "Exit"
                    Environment.Exit(0);
                    break;
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_backgroundImage, new Rectangle(0, 0, spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height), Color.White);

        for (int i = 0; i < _menuOptions.Length; i++)
        {
            Color optionColor = (i == _selectedOption) ? Color.Yellow : Color.OrangeRed;
            spriteBatch.DrawString(_font, _menuOptions[i], new Vector2(_menuOptionsXPosition, _menuOptionsYPosition + i * 50), optionColor);
        }

        spriteBatch.End();
    }
}
