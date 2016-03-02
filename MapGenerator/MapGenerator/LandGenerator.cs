using System.IO;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;
using System.Collections.Generic;

namespace MapGenerator
{

  class LandGenerator : Generator
  {
    public TileMap map_ground;

    public List<Tile[,]> overlay_maps = new List<Tile[,]>( );

    public LandGenerator( )
        {
           
        }    

        protected override void LoadContent( )
        {
            loadTextures( );

            sf = SpriteFont.Load( this.GraphicsDevice, "Arial16.fnt" );

            map_ground = new TileMap( Global_Settings.draw_width / Global_Settings.tile_pixels, Global_Settings.draw_height / Global_Settings.tile_pixels );
            this.generate_ground_map( );
      this.render_ground_map( );

            base.LoadContent( );
        }


        protected override void Update( GameTime gameTime )
        {
            MouseState ms = mouseManager.GetState( );
            KeyboardState ks = keyBoardManager.GetState( );

            if ( ms.LeftButton.Released )
            {
        //Console.Clear(); 
        //map_ground.generate_ground_map();

        this.render_ground_map( );
            }

            if ( ms.RightButton.Released )
            {
                Console.Clear( );
        this.generate_ground_map( );
            }

            base.Update( gameTime );

            if ( ks.IsKeyReleased( Keys.Escape ) )
            {
                this.Exit( );
                this.Dispose( );
            }
        }

        protected override void Draw( GameTime gameTime )
        {
            GraphicsDevice.Clear( Color.CornflowerBlue );


            spriteBatch.Begin( SpriteSortMode.Deferred, this.GraphicsDevice.BlendStates.NonPremultiplied );

            map_ground.Draw( spriteBatch, sf );


            spriteBatch.End( );

            base.Draw( gameTime );
        }

    public void generate_overlay_maps( )
    {
      Random rand = new Random( );


      int max_bioms = Enum.GetNames( typeof( TILE_TYPE ) ).Length - 1;

      int a = 256 / max_bioms;

      for ( int biom = max_bioms; biom >= 0; biom-- )
      {
        Tile[,] map = new Tile[ map_ground.Width, map_ground.Height ];

        for ( int i = 0; i < map_ground.Width; i++ )
        {
          for ( int j = 0; j < map_ground.Height; j++ )
          {

            map[ i, j ] = new Tile(
               Generator.tiles[ (TILE_TYPE)( biom ) ][ TILE_ORIENTATION.CENTER ],
               Generator.tiles[ (TILE_TYPE)( biom ) ][ TILE_ORIENTATION.CENTER ],
               new Vector2( i * Global_Settings.tile_pixels, j * Global_Settings.tile_pixels ),
               (TILE_TYPE)( biom ),
               TILE_ORIENTATION.CENTER
              );

          }
        }

        overlay_maps.Add( map );
      }



    }

    public void generate_ground_map( )
    {
      Random rand = new Random( );

      int randy_new = 128;
      int randy_old = 128;
      int max_bioms = Enum.GetNames( typeof( TILE_TYPE ) ).Length;

      int a = 256 / max_bioms;

      for ( int i = 0; i < map_ground.Width; i++ )
      {
        for ( int j = 0; j < map_ground.Height; j++ )
        {
          randy_new = rand.Next( randy_old - 20, randy_old + 20 );

          int start = 0;

          if ( randy_new < 0 )
          {
            randy_new = rand.Next( 0, a );
          }
          if ( randy_new > 255 )
          {
            randy_new = rand.Next( 255 - a, 255 );
          }

          while ( !map_ground.is_within( start, start + a, randy_new ) )
          {
            start += a;
          }

          map_ground.map[ i, j ] = new Tile(
             Generator.tiles[ (TILE_TYPE)( randy_new / a ) ][ TILE_ORIENTATION.CENTER ],
             Generator.tiles[ (TILE_TYPE)( randy_new / a ) ][ TILE_ORIENTATION.CENTER ],
             new Vector2( i * Global_Settings.tile_pixels, j * Global_Settings.tile_pixels ),
             (TILE_TYPE)( randy_new / a ),
             TILE_ORIENTATION.CENTER
            );

          randy_old = randy_new;
        }
      }

      for ( int j = 0; j < map_ground.Height; j++ )
      {
        for ( int i = 0; i < map_ground.Width; i++ )
        {
          randy_new = rand.Next( randy_old - 20, randy_old + 20 );

          int start = 0;

          if ( randy_new < 0 )
          {
            randy_new = rand.Next( 0, a );
          }
          if ( randy_new > 255 )
          {
            randy_new = rand.Next( 255 - a, 255 );
          }

          while ( !map_ground.is_within( start, start + a, randy_new ) )
          {
            start += a;
          }
          if ( randy_new % 2 == 0 )
          {
            map_ground.map[ i, j ] = new Tile(
              Generator.tiles[ (TILE_TYPE)( randy_new / a ) ][ TILE_ORIENTATION.CENTER ],
              Generator.tiles[ (TILE_TYPE)( randy_new / a ) ][ TILE_ORIENTATION.CENTER ],
              new Vector2( i * Global_Settings.tile_pixels, j * Global_Settings.tile_pixels ),
              (TILE_TYPE)( randy_new / a ),
              TILE_ORIENTATION.CENTER
             );
          }
          randy_old = randy_new;
        }
      }

      for ( int i = 0; i < map_ground.Width; i++ )
      {
        for ( int j = 0; j < map_ground.Height; j++ )
        {
          Console.WriteLine( " [" + i + ", " + j + "] " + map_ground.map[ i, j ].type.ToString( ) );
        }
      }
    }

    public void render_ground_map( )
    {
      int max_bioms = Enum.GetNames( typeof( TILE_TYPE ) ).Length - 1;

      int a = 256 / max_bioms;

      for ( int biom = max_bioms; biom >= 0; biom-- )
      {
        for ( int i = 1; i < map_ground.Width; i++ )
        {
          for ( int j = 1; j < map_ground.Height; j++ )
          {
            if ( (int)map_ground.map[ i, j ].type == biom )
            {

              if ( (int)map_ground.map[ i - 1, j - 1 ].type == biom )
              {
                if ( (int)map_ground.map[ i, j - 1 ].type != biom && (int)map_ground.map[ i - 1, j ].type != biom )
                {

                  Console.WriteLine( "Changed Tile [" + i + ", " + ( j - 1 ) + "].type " + map_ground.map[ i, j - 1 ].type + " to " + ( (TILE_TYPE)biom ).ToString( ) +
                                     " because Tile [" + i + ", " + j + "].type " + map_ground.map[ i, j ].type );
                  //map[i - 1, j - 1].type = map[i, j - 1].type;
                  map_ground.map[ i, j - 1 ].change_biom( (TILE_TYPE)biom );
                }
              }

              if ( i < map_ground.Height - 1 && (int)map_ground.map[ i + 1, j - 1 ].type == biom )
              {
                if ( (int)map_ground.map[ i, j - 1 ].type != biom && (int)map_ground.map[ i + 1, j ].type != biom )
                {

                  Console.WriteLine( "Changed Tile [" + i + ", " + ( j - 1 ) + "].type " + map_ground.map[ i, j - 1 ].type + " to " + ( (TILE_TYPE)biom ).ToString( ) +
                                     " because Tile [" + i + ", " + j + "].type " + map_ground.map[ i, j ].type );
                  //map[i - 1, j - 1].type = map[i, j - 1].type;
                  map_ground.map[ i, j - 1 ].change_biom( (TILE_TYPE)biom );
                }
              }
            }
          }
        }
      }
    }
  }
}
