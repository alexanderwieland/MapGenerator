using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;

namespace MapGenerator
{
  class Tile
  {
    public Vector2 position;

    public TILE_TYPE type;

    public TILE_ORIENTATION orientation;

    public Texture2D texture;

    public Tile(Texture2D texture, Vector2 position, TILE_TYPE type, TILE_ORIENTATION orientation)
    {
      this.texture = texture;
      this.position = position;
      this.orientation = orientation;
      this.type = type;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      
      spriteBatch.Draw(this.texture, this.position, Color.White);
    }
  }
}