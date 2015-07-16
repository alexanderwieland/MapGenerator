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
  class Texture_Collection
  {
    Texture2D texture_in;
    Texture2D texture_out;

    public Texture_Collection(Texture2D texture_in, Texture2D texture_out)
    {
      this.texture_in = texture_in;
      this.texture_out = texture_out;


    }


  }
}
