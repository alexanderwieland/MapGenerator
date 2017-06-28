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
  public enum TILE_TYPE
  {
    SAND = 0,
    DIRT = 1,
    GRASS = 2,
    GRAVEL = 3,
    NONE = 4,
    MAIN_REGION = 5
  };

  public enum TILE_ORIENTATION
  {
    NW,
    N,
    NE,
    E,
    SE,
    S,
    SW,
    W,
    CENTER
  };

  class Generator : Game
  {
    public GraphicsDeviceManager deviceManager;
    public SpriteBatch spriteBatch;
    public KeyboardManager keyBoardManager;
    public MouseManager mouseManager;

    public static Dictionary<MapGenerator.TILE_TYPE, Dictionary<MapGenerator.TILE_ORIENTATION, Texture2D>> tiles;

    public Camera camera = new Camera( );

    public static Random rand = new Random( );

    public SpriteFont sf;
    //SharpDX.Toolkit.GraphicsDeviceManager GDM;



    public Generator( )
    {
      deviceManager = new GraphicsDeviceManager( this );
      deviceManager.PreferredBackBufferWidth = Global_Settings.draw_width;
      deviceManager.PreferredBackBufferHeight = Global_Settings.draw_height;

      IsMouseVisible = true;


      keyBoardManager = new KeyboardManager( this );
      mouseManager = new MouseManager( this );

      this.Content.RootDirectory = "Content";
    }

    protected override void Initialize( )
    {
      this.spriteBatch = new SpriteBatch( this.GraphicsDevice );
      tiles = new Dictionary<TILE_TYPE, Dictionary<TILE_ORIENTATION, Texture2D>>( );

      base.Initialize( );
    }

    public void AddTexture( MapGenerator.TILE_TYPE type, MapGenerator.TILE_ORIENTATION or, Texture2D texture )
    {
      tiles[ type ].Add( or, texture );
    }

    public void AddTextureType( MapGenerator.TILE_TYPE type )
    {
      tiles.Add( type, new Dictionary<TILE_ORIENTATION, Texture2D>( ) );
    }


    public void loadTextures( )
    {
      AddTextureType( TILE_TYPE.DIRT );
      AddTextureType( TILE_TYPE.GRASS );
      AddTextureType( TILE_TYPE.GRAVEL );
      AddTextureType( TILE_TYPE.SAND );
      AddTextureType( TILE_TYPE.NONE );
      AddTextureType( TILE_TYPE.MAIN_REGION );

      AddTexture( TILE_TYPE.GRASS, TILE_ORIENTATION.CENTER, Content.Load<Texture2D>( "Grass_in.png" ) );
      AddTexture( TILE_TYPE.DIRT, TILE_ORIENTATION.CENTER, Content.Load<Texture2D>( "Dirt_in.png" ) );
      AddTexture( TILE_TYPE.SAND, TILE_ORIENTATION.CENTER, Content.Load<Texture2D>( "Sand_in.png" ) );
      AddTexture( TILE_TYPE.GRAVEL, TILE_ORIENTATION.CENTER, Content.Load<Texture2D>( "Gravel_in.png" ) );
      AddTexture( TILE_TYPE.MAIN_REGION, TILE_ORIENTATION.CENTER, Content.Load<Texture2D>( "MainRegion.png" ) );
      tiles[ TILE_TYPE.NONE ].Add( TILE_ORIENTATION.CENTER, null );
    }

  }
}
