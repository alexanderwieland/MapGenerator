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

        private EDGE_TYPE north;

        public EDGE_TYPE North
        {
            get { return north; }
            set { north = value; }
        }

        private EDGE_TYPE east;

        public EDGE_TYPE East
        {
            get { return east; }
            set { east = value; }
        }

        private EDGE_TYPE south;

        public EDGE_TYPE South
        {
            get { return south; }
            set { south = value; }
        }

        private EDGE_TYPE west;

        public EDGE_TYPE West
        {
            get { return west; }
            set { west = value; }
        }


        public  Tile(Texture2D texture, Vector2 position, EDGE_TYPE north, EDGE_TYPE east, EDGE_TYPE south, EDGE_TYPE west)
        {
            this.north = north;
            this.east = east;
            this.south = south;
            this.west = west;

            this.texture = texture;
            this.position = position;
        }

        public enum EDGE_TYPE
        {
            GRASS,
            DIRT,
            GRAVEL,
            SAND

        };


    }
}
