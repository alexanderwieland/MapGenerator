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

        public Generator()
        {
            deviceManager = new GraphicsDeviceManager(this);
            deviceManager.PreferredBackBufferWidth = 800;
            deviceManager.PreferredBackBufferHeight = 600;

            this.Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {            
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Texture2D img_dirt = Content.Load<Texture2D>("Grass.png");
            Texture2D img_grass= Content.Load<Texture2D>("Dirt.png");
            Texture2D img_sand = Content.Load<Texture2D>("Sand.png");
            Texture2D img_gravel = Content.Load<Texture2D>("Gravel.png");


            Random randy = new Random();

            int[,] map = new int[ 800 / 50, 600 / 50 ];

            for (int i = 0; i < 800 / 50; i++)
            {
                for (int j = 0; j < 600 / 50; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        map[i, j] = 0;
                        continue;
                    }

                    if (j == 0)
                    {
                        map[i, j] = map[i - 1, j];
                    }
                    else if (i == 0)
                    {
                        map[i, j] = map[i, j - 1];
                    }
                    else
                    {
                        switch (randy.Next(0, 3) + 1)
                        {
                            case 1:
                                map[i, j] = map[i - 1, j];
                                break;
                            case 2:
                                map[i, j] = map[i, j - 1];
                                break;
                            case 3:
                                map[i, j] = map[i - 1, j - 1];
                                break;
                        }
                    }
                    if (randy.Next(0, 8 * 2 ) % 8 == 0)
                    {
                        map[i, j] = randy.Next(0, 8);
                    }
                }
            }

            for (int x = 0; x < 800/50; x++)
            {
                for (int y = 0; y < 600/50; y++)
                {
                    switch (map[x, y] % 4)
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

            //int[,] map = new int[800 / 50, 600 / 50];

            //Random randy = new Random();

            //for (int j = 0; j < biome_anz; j++)
            //{
            //    biom_tiles = new List<Tile>();
            //    int tiles_written = 0;
            //    while (tiles_written != max_tiles_per_biom)
            //    {
            //        int x = randy.Next(0, 800 / 50) ;
            //        int y = randy.Next(0, 600 / 50) ;

            //        if( map[x, y] == 1 )
            //        {
            //            continue;
            //        }

            //        if ( randy.Next(0,10) % 3 == 0)
            //        {
            //            biom_tiles.Add(new Tile(img_grass, new Vector2(x* 50, y* 50), Tile.EDGE_TYPE.GRASS, Tile.EDGE_TYPE.GRASS, Tile.EDGE_TYPE.GRASS, Tile.EDGE_TYPE.GRASS));
            //            map[x, y] = 1;
            //            tiles_written++;
            //        }
            //        else
            //        {
            //            biom_tiles.Add(new Tile(img_dirt, new Vector2(x * 50, y * 50), Tile.EDGE_TYPE.DIRT, Tile.EDGE_TYPE.DIRT, Tile.EDGE_TYPE.DIRT, Tile.EDGE_TYPE.DIRT));
            //            map[x, y] = 1;
            //            tiles_written++;
            //        }
            //    }
            //    biome.Add(biom_tiles);
            //}


            //for ( int x = 0; x < GraphicsDevice.BackBuffer.Width ; x += 50 )
            //{
            //    for (int y = 0; y < GraphicsDevice.BackBuffer.Height ; y += 50)
            //    {
            //        if (x%100==0)
            //        {
            //            tile_list.Add(new Tile( img_grass, new Vector2(x, y), Tile.EDGE_TYPE.GRASS, Tile.EDGE_TYPE.GRASS, Tile.EDGE_TYPE.GRASS, Tile.EDGE_TYPE.GRASS) );
            //        }
            //        else
            //        {
            //            tile_list.Add( new Tile( img_dirt, new Vector2(x, y), Tile.EDGE_TYPE.DIRT, Tile.EDGE_TYPE.DIRT, Tile.EDGE_TYPE.DIRT, Tile.EDGE_TYPE.DIRT) );
            //        }
            //    }
            //}                       

            base.LoadContent( );
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
