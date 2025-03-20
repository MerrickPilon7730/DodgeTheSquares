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
    class PlayerSquare
    {
        public Vector2 _position { get; set; }
        public Texture2D _texture { get; set; }
        public int _size { get; set; }
        public int _screenWidth;
        public int _screenHeight;   
        public Rectangle _boundingBox => new Rectangle((int)_position.X, (int)_position.Y, _size, _size); // Get the bounding box for the square

        public PlayerSquare(int screenWidth, int screenHeight, int size)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;

            // Initialize position at the center of the screen
            _position = new Vector2(_screenWidth / 2 - size / 2, _screenHeight / 2 - size / 2);
            _size = size;
        }
        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            // Load the texture
            _texture = new Texture2D(graphicsDevice, 1, 1);
            _texture.SetData(new Color[] { Color.White }); // Simple white square texture
        }

        public void Update(MouseState mouseState)
        {
            float newX = mouseState.X - _size / 2;
            float newY = mouseState.Y - _size / 2;

            // Clamp the position to make sure the square stays within the screen bounds
            newX = MathHelper.Clamp(newX, 0, _screenWidth - _size);  // Ensures the square stays within the horizontal bounds
            newY = MathHelper.Clamp(newY, 0, _screenHeight - _size); // Ensures the square stays within the vertical bounds
                                                                    // Update the position of the square to follow the mouse
            _position = new Vector2(newX, newY); // Center the square on the mouse
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture,_position, null, Color.Yellow, 0f, Vector2.Zero, new Vector2(_size, _size), SpriteEffects.None, 0f);  // Draw the square at its position
        }


    }
}
