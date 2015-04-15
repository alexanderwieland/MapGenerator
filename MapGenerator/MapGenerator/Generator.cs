using System.IO;
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
            for ( int x = 0; x < GraphicsDevice.BackBuffer.Width ; x += 50 )
            {
                for (int y = 0; y < GraphicsDevice.BackBuffer.Height ; y += 50)
                {
                    tile_list.Add( new Tile( Content.Load<Texture2D>("Grass.png"), new Vector2( x, y ) ) );
                }
            }                       

            base.LoadContent( );
        }

        protected override void Draw( GameTime gameTime )
        {
            GraphicsDevice.Clear( Color.CornflowerBlue );

            //    (this.GraphicsDevice.BackBuffer.Width / 2.0f) - (this.planeTexture.Width / 2.0f),
             //   (this.GraphicsDevice.BackBuffer.Height / 2.0f) - (this.planeTexture.Height / 2.0f));

            spriteBatch.Begin( SpriteSortMode.Deferred, this.GraphicsDevice.BlendStates.NonPremultiplied );

            foreach (Tile t in tile_list)
            {
                spriteBatch.Draw( t.texture, t.position, Color.White);
            }

           

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
