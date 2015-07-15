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

    private List<Texture2D> list_textures = new List<Texture2D>();


    Tile[,] map;

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

  

    public void Draw(SpriteBatch spriteBatch)
    {
      for (int x = 0; x < Width; x++)
      {
        for (int y = 0; y < Height; y++)
        {
          map[x, y].Draw(spriteBatch);
        }
      }
    }

    public void generate_ground_map()
    {
      Random rand = new Random();

      int randy_new = 128;
      int randy_old = 128;
      int max_bioms = 4;

      int a = 256 / max_bioms;

      for (int i = 0; i < Width; i++)
      {
        for (int j = 0; j < Height; j++)
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

          map[i, j] = new Tile(
             Generator.tiles[(TILE_TYPE)(randy_new / a)][TILE_ORIENTATION.CENTER],
             new Vector2(i * Generator.tile_pixels, j * Generator.tile_pixels),
             (TILE_TYPE)(randy_new / a),
             TILE_ORIENTATION.CENTER
            );

          randy_old = randy_new;
        }
      }

      for (int j = 0; j < Height; j++)
      {
        for (int i = 0; i < Width; i++)
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
          if (randy_new % 2 == 0)
          {
            map[i, j] = new Tile(
              Generator.tiles[(TILE_TYPE)(randy_new / a)][TILE_ORIENTATION.CENTER],
              new Vector2(i * Generator.tile_pixels, j * Generator.tile_pixels),
              (TILE_TYPE)(randy_new / a),
              TILE_ORIENTATION.CENTER
             );
          }
          randy_old = randy_new;
        }
      }

      foreach (var item in map)
      {
        Console.WriteLine(item.position.X + " " + item.position.Y);
      }
    }

    private bool is_within(int starta, int end, int value)
    {
      if (value >= starta && value <= end)
      {
        return true;
      }
      return false;
    }

  }
}
