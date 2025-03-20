using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BouncingSquare
{
    public Rectangle _position { get; private set; }
    public Vector2 _speed { get; set; }  // Use a setter for Speed

    private static Random _random = new Random();

    public BouncingSquare(int screenWidth, int screenHeight)
    {
        // Initialize position randomly within the screen bounds
        int size = _random.Next(20, 50); // Random size between 20 and 50
        _position = new Rectangle(_random.Next(0, screenWidth - size), _random.Next(0, screenHeight - size), size, size);

        // Initialize speed with random values
        _speed = new Vector2(_random.Next(-5, 6), _random.Next(-5, 6)); // Random speed between -5 and 5
    }

    public void Update(int screenWidth, int screenHeight)
    {
        // Update the position based on speed
        _position = new Rectangle(_position.X + (int)_speed.X, _position.Y + (int)_speed.Y, _position.Width, _position.Height);

        // Bounce off the edges
        if (_position.Left <= 0 || _position.Right >= screenWidth)
            _speed = new Vector2(-_speed.X, _speed.Y); // Update speed using the setter

        if (_position.Top <= 0 || _position.Bottom >= screenHeight)
            _speed = new Vector2(_speed.X, -_speed.Y); // Update speed using the setter
    }

    public void IncreaseSpeed(float multiplier)
    {
        // Multiply current speed by a factor
        _speed *= multiplier;
    }
}
