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
  class DungenMap
  {
    
    public Tile[,] tiles;

    public int Width
    {
      get { return tiles.GetLength(0); }
    }

    public int Height
    {
      get { return tiles.GetLength(1); }
    }

    public DungenMap(int width, int height)
    {
      tiles = new Tile[width, height];
    }


    public void Draw( SpriteBatch spriteBatch, SpriteFont sf )
    {
      for ( int x = 0; x < Width; x++ )
      {
        for ( int y = 0; y < Height; y++ )
        {
          if ( tiles[ x, y ] != null )
            tiles[ x, y ].Draw( spriteBatch, x, y, sf );
        }
      }
    }

    public Tile search_for_tile( TILE_TYPE tile_type )
    {
      for ( int i = 0; i < this.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < this.tiles.GetLength( 1 ); j++ )
        {
          if ( this.tiles[ i, j ].type == tile_type )
          {
            return this.tiles[ i, j ];
          }
        }

      }
      return null;
    }

    public Tile search_for_tile_without_main_region( TILE_TYPE tile_type )
    {
      for ( int i = 0; i < this.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < this.tiles.GetLength( 1 ); j++ )
        {
          if ( this.tiles[ i, j ].type == tile_type && this.tiles[ i, j ].in_main_region == false )
          {
            return this.tiles[ i, j ];
          }
        }

      }
      return null;
    }


    public Tile search_for_tile_without_main_region_from_pos( TILE_TYPE tile_type, Tile from_tile )
    {
      int x = 0;
      int y = 0;
      if ( from_tile != null )
      {
        x = from_tile.X_Position;
        y = from_tile.Y_Position;
      }

      for ( int i = y; i < this.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = x; j < this.tiles.GetLength( 1 ); j++ )
        {
          if ( this.tiles[ i, j ].type == tile_type && this.tiles[ i, j ].in_main_region == false )
          {
            return this.tiles[ i, j ];
          }
        }

      }
      return null;
    }


    internal void add_array( Tile[,] map_to_add, Vector2 startpos )
    {
      //check if array has enough space
      for ( int i = 0; i < map_to_add.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < map_to_add.GetLength( 1 ); j++ )
        {
          if ( i + (int)startpos.X >= tiles.GetLength( 0 ) || j + (int)startpos.Y >= tiles.GetLength( 1 ) )
          {
            return;
          }
            if ( tiles[ i + (int)startpos.X, j + (int)startpos.Y ] != null )
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
            tiles[ i + (int)startpos.X, j + (int)startpos.Y ] = map_to_add[ i, j ];
          }
        }
      }
    }

    public bool is_within( int starta, int end, int value )
    {
      if ( value >= starta && value <= end )
      {
        return true;
      }
      return false;
    }

  }
}
