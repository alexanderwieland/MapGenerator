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
  class Generator : Game
  {   
    private GraphicsDeviceManager deviceManager;
    private SpriteBatch spriteBatch;

    private List<Tile> tile_list = new List<Tile>();
    int[,] ground_map;
    int[,] sec_map;
    int draw_width = 1600;
    int draw_height = 900;

    public Generator()
    {
      deviceManager = new GraphicsDeviceManager(this);
      deviceManager.PreferredBackBufferWidth = draw_width;
      deviceManager.PreferredBackBufferHeight = draw_height;

      this.Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {            
      this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

      base.Initialize();
    }

    public static bool is_within(int starta, int end, int value)
    {
      if (  value >= starta && value <= end)
      {
        return true;
      }
      return false;
    }

    protected override void LoadContent()
    {
      Texture2D img_dirt = Content.Load<Texture2D>("Grass.png");
      Texture2D img_grass= Content.Load<Texture2D>("Dirt.png");
      Texture2D img_sand = Content.Load<Texture2D>("Sand.png");
      Texture2D img_gravel = Content.Load<Texture2D>("Gravel.png");

      generate_ground_map_v2();
      generate_sec_map_v2();

      for (int x = 0; x < draw_width/50; x++)
      {
        for (int y = 0; y < draw_height/50; y++)
        {
          switch ( ground_map[x, y] )
          {
            case 0:
              tile_list.Add(new Tile(img_grass, new Vector2(x * 50, y * 50), Tile.EDGE_TYPE.GRASS, Tile.EDGE_TYPE.GRASS, Tile.EDGE_TYPE.GRASS, Tile.EDGE_TYPE.GRASS));
              break;
            case 1:
              tile_list.Add(new Tile(img_dirt, new Vector2(x * 50, y * 50), Tile.EDGE_TYPE.DIRT, Tile.EDGE_TYPE.DIRT, Tile.EDGE_TYPE.DIRT, Tile.EDGE_TYPE.DIRT));
              break;
            case 2:
              tile_list.Add(new Tile(img_sand, new Vector2(x * 50, y * 50), Tile.EDGE_TYPE.SAND, Tile.EDGE_TYPE.SAND, Tile.EDGE_TYPE.SAND, Tile.EDGE_TYPE.SAND));
              break;
            case 3:
              tile_list.Add(new Tile(img_gravel, new Vector2(x * 50, y * 50), Tile.EDGE_TYPE.GRAVEL, Tile.EDGE_TYPE.GRAVEL, Tile.EDGE_TYPE.GRAVEL, Tile.EDGE_TYPE.GRAVEL));
              break;
            default:
                break;
          }
                    
        }
      }


      base.LoadContent( );
    }

    private void generate_sec_map_v2()
    {
      Random rand = new Random();
      sec_map = new int[draw_width / 50, draw_height / 50];

      int randy_new = 128;
      int randy_old = 128;
      int max_bioms = 4;

      int a = 256 / max_bioms;

      for (int i = 0; i < draw_width / 50; i++)
      {
        for (int j = 0; j < draw_height / 50; j++)
        {

          int current_tile = ground_map[i, j];

          //   v      v
          //   0 0    0 0
          //   0 0    1 0 
          if ( ground_map[i , j+ 1 ] == current_tile )
          {
            sec_map[i, j + 1] = -1;
          }
          else 
          {
            sec_map[i, j + 1] = current_tile;
          }

          //        
          // > 0 0  > 0 1
          //   0 0    0 0 
          if (ground_map[i, j + 1] == current_tile)
          {
            sec_map[i, j + 1] = 
          }
          else
          {
            sec_map[i, j + 1] = 
          }

        }
      }
    }

    private void generate_ground_map_v2()
    {
      Random rand = new Random();
      ground_map = new int[draw_width / 50, draw_height / 50];

      int randy_new = 128;
      int randy_old = 128;
      int max_bioms = 4;

      int a = 256 / max_bioms;

      for (int i = 0; i < draw_width / 50; i++)
      {
        for (int j = 0; j < draw_height / 50; j++)
        {
          randy_new = rand.Next(randy_old - 20, randy_old + 20);

          int start = 0;

          if (randy_new < 0)
          {
            randy_new = rand.Next( 0, a);
          }
          if (randy_new > 255)
          {
            randy_new = rand.Next(255-a, 255);
          }

          while ( !is_within( start, start + a, randy_new) )
          {
            start += a;
          }

          ground_map[i, j] = randy_new / a;

          randy_old = randy_new;
        }
      }

      for (int j = 0; j < draw_height / 50; j++)
      {
        for (int i = 0; i < draw_width / 50; i++)
          {
            randy_new = rand.Next(randy_old - 20, randy_old + 20);

          int start = 0;

          if (randy_new < 0)
          {
            randy_new = rand.Next(0, a);
          }
          if (randy_new > 255)
          {
            randy_new = rand.Next(255 - a, 255);
          }

          while (!is_within(start, start + a, randy_new))
          {
            start += a;
          }
          if(randy_new %2 == 0)
          ground_map[i, j] = randy_new / a;
          
            randy_old =randy_new;
        }
      }
    }

    private void generate_int_map()
    {
      Random randy = new Random();
      ground_map = new int[draw_width / 50, draw_height / 50];

      for (int i = 0; i < draw_width / 50; i++)
      {
        for (int j = 0; j < draw_height / 50; j++)
        {
          if (i == 0 && j == 0)
          {
            ground_map[i, j] = 0;
            continue;
          }

          if (j == 0)
          {
            ground_map[i, j] = ground_map[i - 1, j];
          }
          else if (i == 0)
          {
            ground_map[i, j] = ground_map[i, j - 1];
          }
          else
          {
            switch (randy.Next(0, 3) + 1)
            {
              case 1:
                ground_map[i, j] = ground_map[i - 1, j];
                break;
              case 2:
                ground_map[i, j] = ground_map[i, j - 1];
                break;
              case 3:
                ground_map[i, j] = ground_map[i - 1, j - 1];
                break;
            }
          }
          if (randy.Next(0, 8 * 2) % 8 == 0)
          {
              ground_map[i, j] = randy.Next(0, 8);
          }
        }
      }


      for (int i = (draw_width / 50) - 1; i >= 0; i--)
      {
        for (int j = 0; j < draw_height / 50; j++)
        {
          if (j == 0 || i == 0 || j == draw_height / 50 - 1 || draw_width / 50 - 1 == i)
          {
            continue;
          }
          else
          {
            switch (randy.Next(0, 3) + 1)
            {
              case 1:
                ground_map[i, j] = ground_map[ i , j + 1];
                break;
              case 2:
                ground_map[i, j] = ground_map[ i + 1, j];
                break;
              case 3:
                ground_map[i, j] = ground_map[i + 1, j + 1];
                break;
              default:
                break;
            }
          }
        }
      }
    }

    protected override void Draw( GameTime gameTime )
    {
      GraphicsDevice.Clear( Color.CornflowerBlue );


      spriteBatch.Begin( SpriteSortMode.Texture, this.GraphicsDevice.BlendStates.NonPremultiplied );

      foreach (Tile t in tile_list)
      {
          spriteBatch.Draw(t.texture, t.position, Color.White);
      }


      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
