using System.IO;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using System.Collections.Generic;

namespace MapGenerator
{
  public enum TILE_TYPE
  {
    SAND = 0,
    DIRT = 1,
    GRASS = 2,
    GRAVEL = 3
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
    private GraphicsDeviceManager deviceManager;
    private SpriteBatch spriteBatch;

    public static Dictionary<MapGenerator.TILE_TYPE, Dictionary<MapGenerator.TILE_ORIENTATION, Texture2D>> tiles;

    public static int tile_pixels = 50;
    
    private List<Tile> tile_list = new List<Tile>();

    TileMap map_ground;

    int draw_width = 1600;
    int draw_height = 900;

    public Generator()
    {
      deviceManager = new GraphicsDeviceManager(this);
      deviceManager.PreferredBackBufferWidth = draw_width;
      deviceManager.PreferredBackBufferHeight = draw_height;

      IsMouseVisible = true;

      this.Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
      this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
      tiles = new Dictionary<TILE_TYPE, Dictionary<TILE_ORIENTATION, Texture2D>>();


      base.Initialize();
    }

    protected override void LoadContent()
    {
      LoadTextures();

      

      map_ground = new TileMap(draw_width / tile_pixels, draw_height / tile_pixels);
      map_ground.generate_ground_map();

      base.LoadContent();
    }

    private void LoadTextures()
    {
      AddTextureType(TILE_TYPE.DIRT);
      AddTextureType(TILE_TYPE.GRASS);
      AddTextureType(TILE_TYPE.GRAVEL);
      AddTextureType(TILE_TYPE.SAND);

      AddTexture(TILE_TYPE.GRASS, TILE_ORIENTATION.CENTER, Content.Load<Texture2D>("Grass_in.png"));
      AddTexture(TILE_TYPE.DIRT, TILE_ORIENTATION.CENTER, Content.Load<Texture2D>("Dirt_in.png"));
      AddTexture(TILE_TYPE.SAND, TILE_ORIENTATION.CENTER, Content.Load<Texture2D>("Sand_in.png"));
      AddTexture(TILE_TYPE.GRAVEL, TILE_ORIENTATION.CENTER, Content.Load<Texture2D>("Gravel_in.png"));
      
    }

    public void AddTexture(MapGenerator.TILE_TYPE type, MapGenerator.TILE_ORIENTATION or, Texture2D texture)
    {
      tiles[type].Add(or, texture);
    }

    public void AddTextureType(MapGenerator.TILE_TYPE type)
    {
      tiles.Add(type, new Dictionary<TILE_ORIENTATION, Texture2D>());
    }

    

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);


      spriteBatch.Begin(SpriteSortMode.Texture, this.GraphicsDevice.BlendStates.NonPremultiplied);

      map_ground.Draw(spriteBatch);


      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
