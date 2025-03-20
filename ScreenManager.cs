using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using DodgeTheSquares;
using Microsoft.Xna.Framework.Content;

public class ScreenManager
{
    private Screen _currentScreen;

    public ContentManager _contentManager { get; private set; }
    public GraphicsDevice _graphicsDevice { get; private set; }

    public ScreenManager(ContentManager contentManager, GraphicsDevice graphicsDevice)
    {
        // Initialize with the MenuScreen
        _contentManager = contentManager;
        _graphicsDevice = graphicsDevice;
        _currentScreen = new MenuScreen(this, graphicsDevice);
    }

    public void SetScreen(Screen newScreen)
    {
        _currentScreen?.UnloadContent(); // Unload the old screen's content
        _currentScreen = newScreen;
        _currentScreen.LoadContent(_contentManager); // Load content for the new screen
    }

    public void LoadContent()
    {
        _currentScreen.LoadContent(_contentManager); // Delegate loading content to the current screen
    }

    public void Update(GameTime gameTime, MouseState mouseState, MouseState previousMouseState)
    {
        _currentScreen.Update(gameTime, mouseState, previousMouseState);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _currentScreen.Draw(spriteBatch);
    }
}
