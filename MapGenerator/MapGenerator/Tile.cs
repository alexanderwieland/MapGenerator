using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;

namespace MapGenerator
{
    class Tile
    {
        
        public Vector2 position;
        public Texture2D texture;

        public  Tile(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }


    }
}
