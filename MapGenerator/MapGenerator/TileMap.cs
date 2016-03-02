using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace MapGenerator
{
  class TileMap
  {



    public Tile[,] map;

    public int Width
    {
      get { return map.GetLength(0); }
    }

    public int Height
    {
      get { return map.GetLength(1); }
    }

    public TileMap(int width, int height)
    {
      map = new Tile[width, height];
    }    

  

    public void Draw(SpriteBatch spriteBatch, SpriteFont sf)
    {
      for (int x = 0; x < Width; x++)
      {
        for (int y = 0; y < Height; y++)
        {
          if( map[ x, y ] != null)
          map[x, y].Draw(spriteBatch,x,y,sf);
        }
      }
    }

    internal void add_array( Tile[,] map_to_add, Vector2 startpos )
    {
      //check if array has enough space
      for ( int i = 0; i < map_to_add.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < map_to_add.GetLength( 1 ); j++ )
        {
          if ( i + (int)startpos.X >= map.GetLength( 0 ) || j + (int)startpos.Y >= map.GetLength( 1 ) )
          {
            return;
          }
            if ( map[ i + (int)startpos.X, j + (int)startpos.Y ] != null )
          {
            return;
          }
        }
      }

      // Copy array in array
      for ( int i = 0; i < map_to_add.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < map_to_add.GetLength( 1 ); j++ )
        {
          if( map_to_add[ i, j ] != null )
          {
            map_to_add[ i, j ].position = new Vector2( ( i + (int)startpos.X ) * Global_Settings.tile_pixels, ( j + (int)startpos.Y ) * Global_Settings.tile_pixels );
            map[ i + (int)startpos.X, j + (int)startpos.Y ] = map_to_add[ i, j ];
          }
        }
      }
    }

    public bool is_within(int starta, int end, int value)
    {
      if (value >= starta && value <= end)
      {
        return true;
      }
      return false;
    }

  }
}
