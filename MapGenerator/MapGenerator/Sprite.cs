using System;
using System.Collections.Generic;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;


namespace MapGenerator
{
  public class Sprite
  {
    public Texture2D texture;

    public Vector2 position = Vector2.Zero;

    public Sprite(Texture2D texture, Vector2 position)
    {
      this.texture = texture;
      this.position = position;
    }

    public void Update(GameTime gameTime)
    {

    }

    public void Draw(SpriteBatch spriteBatch)
    {

    }
  }
}
