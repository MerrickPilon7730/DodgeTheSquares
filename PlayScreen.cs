using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection.Metadata;

namespace DodgeTheSquares
{
    public class PlayScreen : Screen
    {
        private ScreenManager _screenManager;
        private GraphicsDevice _graphicsDevice;
        private PlayerName _playerName;
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;
        private PlayerSquare _playerSquare;
        private Texture2D _playerTexture;
        private List<BouncingSquare> _squares;
        private Texture2D _squareTexture;
        private int _screenWidth;
        private int _screenHeight;
        private bool _isPaused;
        private SpriteFont _font;
        private GameOverScreen _gameOverScreen;
        private Scores _scores;
        private TimeSpan _finalGameTime;
        private bool _isNameSubmitted;
        private TimeSpan _elapsedTime;       // Track total elapsed time
        private TimeSpan _speedIncreaseInterval; // Interval for speed increase
        private float _speedMultiplier;
        private TimeSpan _squareAddInterval; // Interval for adding new squares
        private TimeSpan _squareAddElapsed; // Time elapsed since the last square was added
        private Texture2D _backgroundImage;
        private SoundManager _soundManager;

        public PlayScreen(int screenWidth, int screenHeight, GraphicsDevice graphicsDevice, ScreenManager screenManager)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _graphicsDevice = graphicsDevice;
            _screenManager = screenManager;
            _playerName = new PlayerName(_font, new Vector2(graphicsDevice.Viewport.Width /2 -150, graphicsDevice.Viewport.Height /2), 300, 50);

            _elapsedTime = TimeSpan.Zero;
            _speedIncreaseInterval = TimeSpan.FromSeconds(5); // Increase speed every 10 seconds
            _speedMultiplier = 1.2f;
            _squareAddInterval = TimeSpan.FromSeconds(10); // Add a new square every 10 seconds
            _squareAddElapsed = TimeSpan.Zero;
            _soundManager = new SoundManager();

            _isPaused = false;

            Game1.Instance.IsMouseVisible = false;
            _scores = new Scores(_font, new Vector2(10, 10), graphicsDevice);

            _squares = new List<BouncingSquare>();

            // Create some bouncing squares
            for (int i = 0; i < 3; i++)  // Let's create 5 squares for now
            {
                _squares.Add(new BouncingSquare(screenWidth, screenHeight));
            }
        }

        public override void LoadContent(ContentManager content)
        {

            _soundManager.LoadContent(content);
            _soundManager.PlayBackgroundMusic();
            _scores.LoadContent(content);
            _font = content.Load<SpriteFont>("MenuFont");
            _backgroundImage = content.Load<Texture2D>("PlayBackground");
            // Using the GraphicsDevice directly here
            _squareTexture = new Texture2D(_graphicsDevice, 1, 1);
            _squareTexture.SetData(new Color[] { Color.White }); // Simple white square texture
            _playerSquare = new PlayerSquare(_screenWidth, _screenHeight, 50);  // A 50x50 square

            // Load the square texture (a white square for simplicity)
            _playerTexture = new Texture2D(_graphicsDevice, 1, 1);
            _playerTexture.SetData(new Color[] { Color.White });

            _playerSquare.LoadContent(content, _graphicsDevice);

            _gameOverScreen = new GameOverScreen(_font, _screenWidth, _screenHeight, _screenManager, _graphicsDevice, content);


            _playerName.LoadContent(content);
            _isNameSubmitted = false;



        }

        public override void Update(GameTime gameTime, MouseState mouseState, MouseState previousMouseState)
        {

            base.Update(gameTime, mouseState, previousMouseState);

            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            _playerName.Update(_currentKeyboardState, _previousKeyboardState);

            _scores.Update(gameTime, _isPaused);
            _finalGameTime = _scores.GetGameTime();

            if (!_isPaused)
            {
                _squareAddElapsed += gameTime.ElapsedGameTime;

                // Check if it's time to add a new square
                if (_squareAddElapsed >= _squareAddInterval)
                {
                    _squares.Add(new BouncingSquare(_screenWidth, _screenHeight));
                    _squareAddElapsed -= _squareAddInterval; // Reset the timer
                }

                // Update elapsed time
                _elapsedTime += gameTime.ElapsedGameTime;

                // Check if it's time to increase speed
                if (_elapsedTime >= _speedIncreaseInterval)
                {
                    // Increase the speed of all bouncing squares
                    foreach (var square in _squares)
                    {
                        square.IncreaseSpeed(_speedMultiplier);
                    }

                    // Reset the timer
                    _elapsedTime -= _speedIncreaseInterval;
                }

                // Update all squares
                foreach (var square in _squares)
                {
                    square.Update(_screenWidth, _screenHeight);
                }
            }

            if (_isPaused)
            {
                
                _soundManager.StopBackgroundMusic();
                
                if (!_isNameSubmitted && _scores.IsNewHighScore(_finalGameTime))
                {


                    _playerName.Activate(); // Activate text box


                    // Handle player input for name submission
                    if (_currentKeyboardState.IsKeyDown(Keys.Enter) && _previousKeyboardState.IsKeyUp(Keys.Enter))
                    {
                        string playerName = _playerName.Text;

                        // Submit the name and the score
                        _scores.AddNewHighScore(playerName, _finalGameTime);

                        // Mark the name submission as completed
                        _isNameSubmitted = true;

                        // Deactivate the text box
                        _playerName.Deactivate();
                    }

                    return; // Wait for player to submit their name before proceeding
                }

                // If name submission is complete or not required, handle other paused logic
                _gameOverScreen.GetOptionsPosition(mouseState, previousMouseState);
                return; // Ensure game logic doesn't proceed while paused
            }

            // Update the square position based on the mouse
            _playerSquare.Update(mouseState);

            foreach (var square in _squares)
            {
                square.Update(_screenWidth, _screenHeight);
            }

            foreach (var bouncingSquare in _squares)
            {
                if (CollisionManager.IsColliding(_playerSquare._boundingBox, bouncingSquare._position))
                {
                    _soundManager.PlayCollisionSound(1);
                    _isPaused = true;
                    Game1.Instance.IsMouseVisible = true;
                }
            }


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(); // Start drawing
            spriteBatch.Draw(_backgroundImage, new Rectangle(0, 0, _screenWidth, _screenHeight), Color.White);

            // Always draw the current scene (player and squares)
            _playerSquare.Draw(spriteBatch);

            foreach (var square in _squares)
            {
                spriteBatch.Draw(_squareTexture, square._position, Color.Green); // Draw each square
            }
            _scores.Draw(spriteBatch);
            spriteBatch.End(); // Finish drawing the scene

            // If the game is paused, overlay the "Game Over" text on top of the scene
            if (_isPaused)
            {
                if (_scores.IsNewHighScore(_finalGameTime))
                {
                    if (!_isNameSubmitted)
                    {
                        // Draw the player name input box
                        _playerName.Draw(spriteBatch);
                    }
                    else
                    {
                        // After name submission, show the Game Over screen
                        _gameOverScreen.Draw(spriteBatch);
                    }
                }
                else
                {
                    // If not a new high score, directly show the Game Over screen
                    _gameOverScreen.Draw(spriteBatch);
                }
            }


        }
    }


}