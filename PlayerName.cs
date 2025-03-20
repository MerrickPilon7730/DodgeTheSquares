using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

public class PlayerName
{
    private SpriteFont _font;
    private Vector2 _position;
    private Rectangle _boxRectangle;
    private StringBuilder _inputText;
    private Color _textColor;
    private Color _boxColor;
    public bool _isSubmitted { get; private set; }
    private bool _isActive;
    private string _newHighScore = "Way to go, you got a new high score!";
    private string _inputSubmit = "Press ENTER to submit your name";

    public string Text => _inputText.ToString();

    public PlayerName(SpriteFont font, Vector2 position, int width, int height)
    {
        _font = font;
        _position = position;
        _boxRectangle = new Rectangle((int)position.X, (int)position.Y, width, height);
        _inputText = new StringBuilder();
        _textColor = Color.Black;
        _boxColor = Color.White;
        _isActive = true;
    }

    public void LoadContent(ContentManager content)
    {
        _font = content.Load<SpriteFont>("MenuFont");
    }

    public void Update(KeyboardState keyboardState, KeyboardState previousKeyboardState)
    {
        if (!_isActive) return;

        foreach (var key in keyboardState.GetPressedKeys())
        {
            if (previousKeyboardState.IsKeyUp(key))
            {
                if (key == Keys.Back && _inputText.Length > 0)
                {
                    _inputText.Remove(_inputText.Length - 1, 1);
                }
                else if (key == Keys.Enter)
                {
                    _isActive = false;
                    _isSubmitted = true; // Set flag when Enter is pressed
                }
                else if (_inputText.Length < 15) // Limit text length
                {
                    char keyChar = ConvertKeyToChar(key, keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift));
                    if (keyChar != '\0')
                        _inputText.Append(keyChar);
                }
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw the box
        Texture2D texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        texture.SetData(new Color[] { _boxColor });

        spriteBatch.Begin();
        spriteBatch.Draw(texture, _boxRectangle, _boxColor);

        // Draw the text
        spriteBatch.DrawString(_font, _newHighScore, _position + new Vector2(-150, -70), Color.OrangeRed, 0f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 1);
        spriteBatch.DrawString(_font, _inputText.ToString(), _position + new Vector2(5, 10), _textColor);
        spriteBatch.DrawString(_font, _inputSubmit, _position + new Vector2(-50, 60), Color.OrangeRed);
        spriteBatch.End();
    }

    private char ConvertKeyToChar(Keys key, bool shift)
    {
        if (key >= Keys.A && key <= Keys.Z)
        {
            char c = (char)('A' + (key - Keys.A));
            return shift ? c : char.ToLower(c);
        }
        if (key >= Keys.D0 && key <= Keys.D9)
        {
            char c = (char)('0' + (key - Keys.D0));
            return shift && key == Keys.D1 ? '!' : c; // Example for handling shifted number keys
        }

        return '\0'; // Return null character for unsupported keys
    }

    public void Activate()
    {
        _isActive = true;
        _isSubmitted = false; // Reset submission status
    }
    public void Deactivate() => _isActive = false;
}

