using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

namespace MapGenerator
{
  public enum Orientation
  {
    UP,
    DOWN,
    LEFT,
    RIGHT,
    NONE
  }

  class RoomGenerator : Generator
  {
    Camera cam = new Camera( );

    MouseState ms;
    KeyboardState ks;

    TileMap map;
    Random rand = new Random( );

    protected override void LoadContent( )
    {
      this.loadTextures( );

      this.sf = SpriteFont.Load( this.GraphicsDevice, "Arial16.fnt" );

      cam.Pos = new Vector2( (GraphicsDevice.BackBuffer.Width /2), ( GraphicsDevice.BackBuffer.Height / 2 ) );
      map = new TileMap( 100, 100 );

      this.add_rooms( 500 );

      base.LoadContent( );
    }

    public void init_flood_fill( )
    {
      for ( int i = 0; i < 10; i++ )
      {
        for ( int j = 0; j < 10; j++ )
        {
          if ( map.map[ i, j ].type == TILE_TYPE.NONE )
          {
            floodfill( map.map[ i, j ] );
          }
        }
      }
      // Fill everything with nix 
      for ( int i = 1; i < map.map.GetLength( 0 ); i++ )
      {
        for ( int j = 1; j < map.map.GetLength( 1 ); j++ )
        {
          if ( map.map[ i, j ].type == TILE_TYPE.NONE )
          {
            map.map[ i, j ] = Tile.get_new_tile( TILE_TYPE.GRAVEL, i, j );
          }
        }
      }
    }

    public void floodfill( Tile tile )
    {
      Stack<Tile> tiles = new Stack<Tile>( );

      Stack<Tile> backtrack = new Stack<Tile>( );

      // 1
      if ( tile.type != TILE_TYPE.NONE || !tile_is_valid( tile, Orientation.NONE ) )
      {
        return;
      }

      tiles.Push( tile );

      while ( tiles.Count > 0 )
      {
        Tile popped = tiles.Pop( );

        if ( popped.type == TILE_TYPE.NONE )
        {
          int x = (int)popped.X_Position;
          int y = (int)popped.Y_Position;

          map.map[ x, y ].change_biom( TILE_TYPE.GRASS );

          if ( tile.X_Position % 2 == 0 && tile.Y_Position % 2 == 0 )
          {
            backtrack.Push( popped );
          }

          switch ( rand.Next( 3 ) )
          {
            case 0:
              if ( floodfill_is_left_free( popped ) && tile_is_valid( map.map[ x - 1, y ], Orientation.RIGHT ) )
              {
                tiles.Push( map.map[ x - 1, y ] );
              }
              else if ( floodfill_is_right_free( popped ) && tile_is_valid( map.map[ x + 1, y ], Orientation.LEFT ) )
              {
                tiles.Push( map.map[ x + 1, y ] );
              }
              else if ( floodfill_is_up_free( popped ) && tile_is_valid( map.map[ x, y - 1 ], Orientation.DOWN ) )
              {
                tiles.Push( map.map[ x, y - 1 ] );
              }
              else if ( floodfill_is_down_free( popped ) && tile_is_valid( map.map[ x, y + 1 ], Orientation.UP ) )
              {
                tiles.Push( map.map[ x, y + 1 ] );
              }
              break;

            case 1:             
              if ( floodfill_is_right_free( popped ) && tile_is_valid( map.map[ x + 1, y ], Orientation.LEFT ) )
              {
                tiles.Push( map.map[ x + 1, y ] );
              }
              else if ( floodfill_is_up_free( popped ) && tile_is_valid( map.map[ x, y - 1 ], Orientation.DOWN ) )
              {
                tiles.Push( map.map[ x, y - 1 ] );
              }
              else if ( floodfill_is_down_free( popped ) && tile_is_valid( map.map[ x, y + 1 ], Orientation.UP ) )
              {
                tiles.Push( map.map[ x, y + 1 ] );
              }
              else if ( floodfill_is_left_free( popped ) && tile_is_valid( map.map[ x - 1, y ], Orientation.RIGHT ) )
              {
                tiles.Push( map.map[ x - 1, y ] );
              }
              break;

            case 2:
              if ( floodfill_is_up_free( popped ) && tile_is_valid( map.map[ x, y - 1 ], Orientation.DOWN ) )
              {
                tiles.Push( map.map[ x, y - 1 ] );
              }
              else if ( floodfill_is_down_free( popped ) && tile_is_valid( map.map[ x, y + 1 ], Orientation.UP ) )
              {
                tiles.Push( map.map[ x, y + 1 ] );
              }
              else if ( floodfill_is_left_free( popped ) && tile_is_valid( map.map[ x - 1, y ], Orientation.RIGHT ) )
              {
                tiles.Push( map.map[ x - 1, y ] );
              }
              else if ( floodfill_is_right_free( popped ) && tile_is_valid( map.map[ x + 1, y ], Orientation.LEFT ) )
              {
                tiles.Push( map.map[ x + 1, y ] );
              }              
                break;

            case 3:
              if ( floodfill_is_down_free( popped ) && tile_is_valid( map.map[ x, y + 1 ], Orientation.UP ) )
              {
                tiles.Push( map.map[ x, y + 1 ] );
              }
              else if ( floodfill_is_left_free( popped ) && tile_is_valid( map.map[ x - 1, y ], Orientation.RIGHT ) )
              {
                tiles.Push( map.map[ x - 1, y ] );
              }
              else if ( floodfill_is_right_free( popped ) && tile_is_valid( map.map[ x + 1, y ], Orientation.LEFT ) )
              {
                tiles.Push( map.map[ x + 1, y ] );
              }
              else if ( floodfill_is_up_free( popped ) && tile_is_valid( map.map[ x, y - 1 ], Orientation.DOWN ) )
              {
                tiles.Push( map.map[ x, y - 1 ] );
              }              
                break;
          }        
          
        }
      }

      while ( backtrack.Count > 0 )
      {
        Tile popped = backtrack.Pop( );

        int x = (int)popped.X_Position;
        int y = (int)popped.Y_Position;

        if ( popped.type == TILE_TYPE.NONE )
        {
          map.map[ x, y ].change_biom( TILE_TYPE.GRASS );
          if ( tile.X_Position % 2 == 0 && tile.Y_Position % 2 == 0 )
          {
            backtrack.Push( popped );
          }

        }
          switch ( rand.Next( 3 ) )
          {
            case 0:
              if ( floodfill_is_left_free( popped ) && tile_is_valid( map.map[ x - 1, y ], Orientation.RIGHT ) )
              {
                backtrack.Push( map.map[ x - 1, y ] );
              }
              else if ( floodfill_is_right_free( popped ) && tile_is_valid( map.map[ x + 1, y ], Orientation.LEFT ) )
              {
                backtrack.Push( map.map[ x + 1, y ] );
              }
              else if ( floodfill_is_up_free( popped ) && tile_is_valid( map.map[ x, y - 1 ], Orientation.DOWN ) )
              {
                backtrack.Push( map.map[ x, y - 1 ] );
              }
              else if ( floodfill_is_down_free( popped ) && tile_is_valid( map.map[ x, y + 1 ], Orientation.UP ) )
              {
                backtrack.Push( map.map[ x, y + 1 ] );
              }
              break;

            case 1:
              if ( floodfill_is_right_free( popped ) && tile_is_valid( map.map[ x + 1, y ], Orientation.LEFT ) )
              {
                backtrack.Push( map.map[ x + 1, y ] );
              }
              else if ( floodfill_is_up_free( popped ) && tile_is_valid( map.map[ x, y - 1 ], Orientation.DOWN ) )
              {
                backtrack.Push( map.map[ x, y - 1 ] );
              }
              else if ( floodfill_is_down_free( popped ) && tile_is_valid( map.map[ x, y + 1 ], Orientation.UP ) )
              {
                backtrack.Push( map.map[ x, y + 1 ] );
              }
              else if ( floodfill_is_left_free( popped ) && tile_is_valid( map.map[ x - 1, y ], Orientation.RIGHT ) )
              {
                backtrack.Push( map.map[ x - 1, y ] );
              }
              break;

            case 2:
              if ( floodfill_is_up_free( popped ) && tile_is_valid( map.map[ x, y - 1 ], Orientation.DOWN ) )
              {
                backtrack.Push( map.map[ x, y - 1 ] );
              }
              else if ( floodfill_is_down_free( popped ) && tile_is_valid( map.map[ x, y + 1 ], Orientation.UP ) )
              {
                backtrack.Push( map.map[ x, y + 1 ] );
              }
              else if ( floodfill_is_left_free( popped ) && tile_is_valid( map.map[ x - 1, y ], Orientation.RIGHT ) )
              {
                backtrack.Push( map.map[ x - 1, y ] );
              }
              else if ( floodfill_is_right_free( popped ) && tile_is_valid( map.map[ x + 1, y ], Orientation.LEFT ) )
              {
                backtrack.Push( map.map[ x + 1, y ] );
              }
              break;

            case 3:
              if ( floodfill_is_down_free( popped ) && tile_is_valid( map.map[ x, y + 1 ], Orientation.UP ) )
              {
                backtrack.Push( map.map[ x, y + 1 ] );
              }
              else if ( floodfill_is_left_free( popped ) && tile_is_valid( map.map[ x - 1, y ], Orientation.RIGHT ) )
              {
                backtrack.Push( map.map[ x - 1, y ] );
              }
              else if ( floodfill_is_right_free( popped ) && tile_is_valid( map.map[ x + 1, y ], Orientation.LEFT ) )
              {
                backtrack.Push( map.map[ x + 1, y ] );
              }
              else if ( floodfill_is_up_free( popped ) && tile_is_valid( map.map[ x, y - 1 ], Orientation.DOWN ) )
              {
                backtrack.Push( map.map[ x, y - 1 ] );
              }
              break;
          }
   

      }
    }
    

    public bool tile_is_valid( Tile tile, Orientation or )
    {

      if ( tile.X_Position % 2 == 1 && tile.Y_Position % 2 == 1 )
        return false;

      if ( or != Orientation.LEFT && !floodfill_is_left_free( tile ))
        return false;

      if ( or != Orientation.RIGHT && !floodfill_is_right_free( tile ) )
        return false;

      if ( or != Orientation.UP && !floodfill_is_up_free( tile ) )
        return false;

      if ( or != Orientation.DOWN && !floodfill_is_down_free( tile ) )
        return false;



      if ( or != Orientation.DOWN && or != Orientation.LEFT && !floodfill_is_down_left_free( tile ) )
        return false;

      if ( or != Orientation.DOWN && or != Orientation.RIGHT && !floodfill_is_down_right_free( tile ) )
        return false;

      if ( or != Orientation.UP && or != Orientation.LEFT && !floodfill_is_up_left_free( tile ) )
        return false;

      if ( or != Orientation.UP && or != Orientation.RIGHT && !floodfill_is_up_right_free( tile ) )
        return false;

      return true;
    }

    public bool floodfill_is_left_free( Tile tile )
    {
      if ( tile.X_Position - 1 > 0 && map.map[ tile.X_Position - 1, tile.Y_Position ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool floodfill_is_right_free( Tile tile )
    {
      if ( tile.X_Position + 1 < map.map.GetLength( 0 ) && map.map[ tile.X_Position + 1, tile.Y_Position ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool floodfill_is_up_free( Tile tile )
    {
      if ( tile.Y_Position - 1 > 0 && map.map[ tile.X_Position, tile.Y_Position - 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool floodfill_is_down_free( Tile tile )
    {
      if ( tile.Y_Position + 1 < map.map.GetLength( 1 ) && map.map[ tile.X_Position , tile.Y_Position + 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool floodfill_is_up_left_free( Tile tile )
    {
      if ( tile.Y_Position - 1 > 0 && tile.X_Position - 1 > 0 && map.map[ tile.X_Position - 1, tile.Y_Position - 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool floodfill_is_up_right_free( Tile tile )
    {
      if ( tile.Y_Position - 1 > 0 && tile.X_Position + 1 < map.map.GetLength( 0 ) && map.map[ tile.X_Position + 1, tile.Y_Position - 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool floodfill_is_down_right_free( Tile tile )
    {
      if ( tile.Y_Position + 1 < map.map.GetLength( 1 ) && tile.X_Position + 1 < map.map.GetLength( 0 ) && map.map[ tile.X_Position + 1, tile.Y_Position + 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }

    public bool floodfill_is_down_left_free( Tile tile )
    {
      if ( tile.Y_Position + 1 < map.map.GetLength( 1 ) && tile.X_Position - 1 > 0 && map.map[ tile.X_Position - 1, tile.Y_Position + 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }

    private void add_rooms( int v )
    {
      for ( int i = 0; i < v; i++ )
      {
        Room room = new Room( rand.Next( 3, 8 ) *2+1, rand.Next( 3, 8 ) * 2 + 1 );

        map.add_array( room.Room_map, new Vector2( rand.Next( 0, map.Width /2 -1 ) * 2 + 1, rand.Next( 0, map.Height/2 -1) ) * 2 + 1 );
      }

      // Fill everything with nix 
      for ( int i = 0; i < map.map.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < map.map.GetLength( 1 ); j++ )
        {
          if( map.map[ i,j] == null )
          {
            map.map[ i, j ] = Tile.get_new_tile( TILE_TYPE.NONE, i, j );
          }
        }
      }
    }

    protected override void Update( GameTime gameTime )
    {
      ms = mouseManager.GetState( );
      ks = keyBoardManager.GetState( );


      check_user_input( );


      if ( ks.IsKeyReleased( Keys.Escape ) )
      {
        this.Exit( );
        this.Dispose( );
        return;
      }

      GraphicsDevice.Flush( );
      base.Update( gameTime );
    }

    private void check_user_input( )
    {
      if ( ms.WheelDelta > 0 )
      {
        cam.Zoom += 0.05f;
      }
      if ( ms.WheelDelta < 0 )
      {
        cam.Zoom -= 0.05f;
      }
      if ( ks.IsKeyDown( Keys.Right ) || ks.IsKeyDown( Keys.D ) )
      {
        cam.Move( new Vector2( 25f, 0f ) );
      }
      if ( ks.IsKeyDown( Keys.Left ) || ks.IsKeyDown( Keys.A ) )
      {
        cam.Move( new Vector2( -25f, 0f ) );
      }
      if ( ks.IsKeyDown( Keys.Up ) || ks.IsKeyDown( Keys.W ) )
      {
        cam.Move( new Vector2( 0f, -25f ) );
      }
      if ( ks.IsKeyDown( Keys.Down ) || ks.IsKeyDown( Keys.S ) )
      {
        cam.Move( new Vector2( 0f, 25f ) );
      }
      if ( ms.LeftButton.Released )
      {

        init_flood_fill( );
      }
      if ( ms.RightButton.Released )
      {
      }

    }

    protected override void Draw( GameTime gameTime )
    {
      GraphicsDevice.Clear( Color.Black );

      spriteBatch.Begin( SpriteSortMode.Deferred, this.GraphicsDevice.BlendStates.NonPremultiplied, null, null, null, null, cam.get_transformation( GraphicsDevice ) );

      map.Draw( spriteBatch, sf );

      spriteBatch.End( );

      base.Draw( gameTime );
    }
  }
}
