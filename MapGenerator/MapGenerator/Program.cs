using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
      //using ( LandGenerator map_gen = new LandGenerator( ) )
      //{
      //    map_gen.Run( );
      //}

      using ( DungeonGenerator map_gen = new DungeonGenerator( ) )
      {
        map_gen.Run( );
      }
    }
  }
}
