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
    public abstract class Screen
    {
        public virtual void LoadContent(ContentManager content) { }
        public virtual void UnloadContent() { }
        public virtual void Update(GameTime gameTime, MouseState mouseState, MouseState previousMouseState) { }
        public virtual void Update(GameTime gameTime, MouseState mouseState, MouseState previousMouseState, SpriteBatch spriteBatch) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }

        public virtual void Draw(SpriteBatch spriteBatch, MouseState mouseState) { }
    }
}
