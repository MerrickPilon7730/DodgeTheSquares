
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using DodgeTheSquares;
using Microsoft.Xna.Framework.Audio;
using System.Reflection.Metadata;


public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private ScreenManager _screenManager;
    private MouseState _previousMouseState;
    private static Game1 _instance;
    private SoundManager _soundManager;




    public static Game1 Instance
    {
        get { return _instance ??= new Game1(); }
    }

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _soundManager = new SoundManager();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        _graphics.IsFullScreen = true;
        _instance = this;
        
    }

    protected override void Initialize()
    {

        _screenManager = new ScreenManager(Content, GraphicsDevice);
        _soundManager.LoadContent(Content);
        _soundManager.PlayMenuMusic();
        this.Window.Title = "DodgeTheSquares";
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _screenManager.LoadContent();


        // Set PlayScreen as the current screen and pass GraphicsDevice to it
        _screenManager.SetScreen(new MenuScreen(_screenManager, GraphicsDevice));
    }

    protected override void Update(GameTime gameTime)
    {
        MouseState currentMouseState = Mouse.GetState();
        _screenManager.Update(gameTime, currentMouseState, _previousMouseState);
        _previousMouseState = currentMouseState;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _screenManager.Draw(_spriteBatch);
        base.Draw(gameTime);
    }

}



