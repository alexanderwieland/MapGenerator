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

  class DungeonGenerator : Generator
  {
    Camera cam = new Camera( );

    MouseState ms;
    KeyboardState ks;

    DungenMap map;
    FloodFiller floodfiller;
    RoomConnector roomconnector;

    protected override void LoadContent( )
    {
      this.loadTextures( );

      this.sf = SpriteFont.Load( this.GraphicsDevice, "Arial16.fnt" );

      cam.Pos = new Vector2( (GraphicsDevice.BackBuffer.Width /2), ( GraphicsDevice.BackBuffer.Height / 2 ) );
      map = new DungenMap( 150, 100 );

      this.add_rooms( 500 );
      this.fill_with_none( );

      floodfiller = new FloodFiller( map );
      roomconnector = new RoomConnector( map );
      floodfiller.defill( );

      base.LoadContent( );
    }

    public void fill_with_gravel( )
    {
      //Fill everything with gravel
      for ( int i = 1; i < map.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 1; j < map.tiles.GetLength( 1 ); j++ )
        {
          if ( map.tiles[ i, j ].type == TILE_TYPE.NONE )
          {
            map.tiles[ i, j ] = Tile.get_new_tile( TILE_TYPE.GRAVEL, i, j );
          }
        }
      }
    }   

    private void add_rooms( int v )
    {
      for ( int i = 0; i < v; i++ )
      {
        Room room = new Room( rand.Next( 2, 8 ) *2+1, rand.Next( 2, 8 ) * 2 + 1 );

        map.add_array( room.Room_map, new Vector2( rand.Next( 0, map.Width /2 -1 ) * 2 + 1, rand.Next( 0, map.Height/2 -1) ) * 2 + 1 );
      }
    }

    private void fill_with_none( )
    {
      for ( int i = 0; i < map.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < map.tiles.GetLength( 1 ); j++ )
        {
          if ( map.tiles[ i, j ] == null )
          {
            map.tiles[ i, j ] = Tile.get_new_tile( TILE_TYPE.NONE, i, j );
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
