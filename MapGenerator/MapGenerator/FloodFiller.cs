using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
  class FloodFiller
  {
    private DungenMap map;

    public FloodFiller( DungenMap map )
    {
      this.map = map;
      this.init_flood_fill( );
    }

    public void defill( )
    {
      while ( this.get_next_3_empty( ) )
      {

      }
    }

    private bool get_next_3_empty( )
    {
      bool i_ve_done_something = false;
      for ( int i = 0; i < map.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < map.tiles.GetLength( 1 ); j++ )
        {
          if ( map.tiles[ i, j ].type == TILE_TYPE.GRASS  )
          {
            int empty_counter = 0;

            if ( this.floodfill_is_down_free( map.tiles[ i, j ] ) )
            {
              empty_counter++;
            }
            if ( this.floodfill_is_left_free( map.tiles[ i, j ] ) )
            {
              empty_counter++;
            }
            if ( this.floodfill_is_right_free( map.tiles[ i, j ] ) )
            {
              empty_counter++;
            }
            if ( this.floodfill_is_up_free( map.tiles[ i, j ] ) )
            {
              empty_counter++;
            }

            if ( empty_counter == 3 )
            {
              i_ve_done_something = true;
              map.tiles[ i, j ].change_biom( TILE_TYPE.NONE );
            }
          }
        }
      }
      return i_ve_done_something;
    }

    public void init_flood_fill( )
    {
      for ( int i = 0; i < map.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < map.tiles.GetLength( 1 ); j++ )
        {
          if ( map.tiles[ i, j ].type == TILE_TYPE.NONE )
          {
            floodfill( map.tiles[ i, j ] );
          }
        }
      }
    }

    public void floodfill( Tile tile )
    {
      Stack<Tile> tiles = new Stack<Tile>( );

      Stack<Tile> backtrack = new Stack<Tile>( );

      // 1
      if ( tile.type != TILE_TYPE.NONE || !tile_is_valid( tile, Orientation.NONE ) )
      {
        return;
      }

      tiles.Push( tile );

      while ( tiles.Count > 0 )
      {
        Tile popped = tiles.Pop( );

        if ( popped.type == TILE_TYPE.NONE )
        {
          int x = (int)popped.X_Position;
          int y = (int)popped.Y_Position;

          map.tiles[ x, y ].change_biom( TILE_TYPE.GRASS );

          if ( tile.X_Position % 2 == 0 && tile.Y_Position % 2 == 0 )
          {
            backtrack.Push( popped );
          }


          int mitte_x = map.tiles.GetLength( 0 ) / 2;
          int mitte_y = map.tiles.GetLength( 1 ) / 2;

          if ( x <= mitte_x && y <= mitte_y )
          {
            // Links oben -> nach rechts unten
          }

          switch ( Generator.rand.Next( 3 ) )
          {
            case 0:
              if ( floodfill_is_left_free( popped ) && tile_is_valid( map.tiles[ x - 1, y ], Orientation.RIGHT ) )
              {
                tiles.Push( map.tiles[ x - 1, y ] );
              }
              else if ( floodfill_is_up_free( popped ) && tile_is_valid( map.tiles[ x, y - 1 ], Orientation.DOWN ) )
              {
                tiles.Push( map.tiles[ x, y - 1 ] );
              }
              else if ( floodfill_is_down_free( popped ) && tile_is_valid( map.tiles[ x, y + 1 ], Orientation.UP ) )
              {
                tiles.Push( map.tiles[ x, y + 1 ] );
              }
              break;

            case 1:
              if ( floodfill_is_right_free( popped ) && tile_is_valid( map.tiles[ x + 1, y ], Orientation.LEFT ) )
              {
                tiles.Push( map.tiles[ x + 1, y ] );
              }
              else if ( floodfill_is_up_free( popped ) && tile_is_valid( map.tiles[ x, y - 1 ], Orientation.DOWN ) )
              {
                tiles.Push( map.tiles[ x, y - 1 ] );
              }
              else if ( floodfill_is_down_free( popped ) && tile_is_valid( map.tiles[ x, y + 1 ], Orientation.UP ) )
              {
                tiles.Push( map.tiles[ x, y + 1 ] );
              }
              else if ( floodfill_is_left_free( popped ) && tile_is_valid( map.tiles[ x - 1, y ], Orientation.RIGHT ) )
              {
                tiles.Push( map.tiles[ x - 1, y ] );
              }
              break;

            case 2:
              if ( floodfill_is_up_free( popped ) && tile_is_valid( map.tiles[ x, y - 1 ], Orientation.DOWN ) )
              {
                tiles.Push( map.tiles[ x, y - 1 ] );
              }
              else if ( floodfill_is_down_free( popped ) && tile_is_valid( map.tiles[ x, y + 1 ], Orientation.UP ) )
              {
                tiles.Push( map.tiles[ x, y + 1 ] );
              }
              else if ( floodfill_is_left_free( popped ) && tile_is_valid( map.tiles[ x - 1, y ], Orientation.RIGHT ) )
              {
                tiles.Push( map.tiles[ x - 1, y ] );
              }
              else if ( floodfill_is_right_free( popped ) && tile_is_valid( map.tiles[ x + 1, y ], Orientation.LEFT ) )
              {
                tiles.Push( map.tiles[ x + 1, y ] );
              }
              break;

            case 3:
              if ( floodfill_is_down_free( popped ) && tile_is_valid( map.tiles[ x, y + 1 ], Orientation.UP ) )
              {
                tiles.Push( map.tiles[ x, y + 1 ] );
              }
              else if ( floodfill_is_left_free( popped ) && tile_is_valid( map.tiles[ x - 1, y ], Orientation.RIGHT ) )
              {
                tiles.Push( map.tiles[ x - 1, y ] );
              }
              else if ( floodfill_is_right_free( popped ) && tile_is_valid( map.tiles[ x + 1, y ], Orientation.LEFT ) )
              {
                tiles.Push( map.tiles[ x + 1, y ] );
              }
              else if ( floodfill_is_up_free( popped ) && tile_is_valid( map.tiles[ x, y - 1 ], Orientation.DOWN ) )
              {
                tiles.Push( map.tiles[ x, y - 1 ] );
              }
              break;
          }

        }
      }

      while ( backtrack.Count > 0 )
      {
        Tile popped = backtrack.Pop( );

        int x = (int)popped.X_Position;
        int y = (int)popped.Y_Position;

        if ( popped.type == TILE_TYPE.NONE )
        {
          map.tiles[ x, y ].change_biom( TILE_TYPE.GRASS );
          if ( tile.X_Position % 2 == 0 && tile.Y_Position % 2 == 0 )
          {
            backtrack.Push( popped );
          }

        }
        switch ( Generator.rand.Next( 3 ) )
        {
          case 0:
            if ( floodfill_is_left_free( popped ) && tile_is_valid( map.tiles[ x - 1, y ], Orientation.RIGHT ) )
            {
              backtrack.Push( map.tiles[ x - 1, y ] );
            }
            else if ( floodfill_is_right_free( popped ) && tile_is_valid( map.tiles[ x + 1, y ], Orientation.LEFT ) )
            {
              backtrack.Push( map.tiles[ x + 1, y ] );
            }
            else if ( floodfill_is_up_free( popped ) && tile_is_valid( map.tiles[ x, y - 1 ], Orientation.DOWN ) )
            {
              backtrack.Push( map.tiles[ x, y - 1 ] );
            }
            else if ( floodfill_is_down_free( popped ) && tile_is_valid( map.tiles[ x, y + 1 ], Orientation.UP ) )
            {
              backtrack.Push( map.tiles[ x, y + 1 ] );
            }
            break;

          case 1:
            if ( floodfill_is_right_free( popped ) && tile_is_valid( map.tiles[ x + 1, y ], Orientation.LEFT ) )
            {
              backtrack.Push( map.tiles[ x + 1, y ] );
            }
            else if ( floodfill_is_up_free( popped ) && tile_is_valid( map.tiles[ x, y - 1 ], Orientation.DOWN ) )
            {
              backtrack.Push( map.tiles[ x, y - 1 ] );
            }
            else if ( floodfill_is_down_free( popped ) && tile_is_valid( map.tiles[ x, y + 1 ], Orientation.UP ) )
            {
              backtrack.Push( map.tiles[ x, y + 1 ] );
            }
            else if ( floodfill_is_left_free( popped ) && tile_is_valid( map.tiles[ x - 1, y ], Orientation.RIGHT ) )
            {
              backtrack.Push( map.tiles[ x - 1, y ] );
            }
            break;

          case 2:
            if ( floodfill_is_up_free( popped ) && tile_is_valid( map.tiles[ x, y - 1 ], Orientation.DOWN ) )
            {
              backtrack.Push( map.tiles[ x, y - 1 ] );
            }
            else if ( floodfill_is_down_free( popped ) && tile_is_valid( map.tiles[ x, y + 1 ], Orientation.UP ) )
            {
              backtrack.Push( map.tiles[ x, y + 1 ] );
            }
            else if ( floodfill_is_left_free( popped ) && tile_is_valid( map.tiles[ x - 1, y ], Orientation.RIGHT ) )
            {
              backtrack.Push( map.tiles[ x - 1, y ] );
            }
            else if ( floodfill_is_right_free( popped ) && tile_is_valid( map.tiles[ x + 1, y ], Orientation.LEFT ) )
            {
              backtrack.Push( map.tiles[ x + 1, y ] );
            }
            break;

          case 3:
            if ( floodfill_is_down_free( popped ) && tile_is_valid( map.tiles[ x, y + 1 ], Orientation.UP ) )
            {
              backtrack.Push( map.tiles[ x, y + 1 ] );
            }
            else if ( floodfill_is_left_free( popped ) && tile_is_valid( map.tiles[ x - 1, y ], Orientation.RIGHT ) )
            {
              backtrack.Push( map.tiles[ x - 1, y ] );
            }
            else if ( floodfill_is_right_free( popped ) && tile_is_valid( map.tiles[ x + 1, y ], Orientation.LEFT ) )
            {
              backtrack.Push( map.tiles[ x + 1, y ] );
            }
            else if ( floodfill_is_up_free( popped ) && tile_is_valid( map.tiles[ x, y - 1 ], Orientation.DOWN ) )
            {
              backtrack.Push( map.tiles[ x, y - 1 ] );
            }
            break;
        }


      }
    }


    public bool tile_is_valid( Tile tile, Orientation or )
    {

      if ( tile.X_Position % 2 == 1 && tile.Y_Position % 2 == 1 )
        return false;

      if ( or != Orientation.LEFT && !floodfill_is_left_free( tile ) )
        return false;

      if ( or != Orientation.RIGHT && !floodfill_is_right_free( tile ) )
        return false;

      if ( or != Orientation.UP && !floodfill_is_up_free( tile ) )
        return false;

      if ( or != Orientation.DOWN && !floodfill_is_down_free( tile ) )
        return false;



      if ( or != Orientation.DOWN && or != Orientation.LEFT && !floodfill_is_down_left_free( tile ) )
        return false;

      if ( or != Orientation.DOWN && or != Orientation.RIGHT && !floodfill_is_down_right_free( tile ) )
        return false;

      if ( or != Orientation.UP && or != Orientation.LEFT && !floodfill_is_up_left_free( tile ) )
        return false;

      if ( or != Orientation.UP && or != Orientation.RIGHT && !floodfill_is_up_right_free( tile ) )
        return false;

      return true;
    }

    public bool floodfill_is_left_free( Tile tile )
    {
      if ( tile.X_Position - 1 > 0 && map.tiles[ tile.X_Position - 1, tile.Y_Position ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool floodfill_is_right_free( Tile tile )
    {
      if ( tile.X_Position + 1 < map.tiles.GetLength( 0 ) && map.tiles[ tile.X_Position + 1, tile.Y_Position ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool floodfill_is_up_free( Tile tile )
    {
      if ( tile.Y_Position - 1 > 0 && map.tiles[ tile.X_Position, tile.Y_Position - 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool floodfill_is_down_free( Tile tile )
    {
      if ( tile.Y_Position + 1 < map.tiles.GetLength( 1 ) && map.tiles[ tile.X_Position, tile.Y_Position + 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool floodfill_is_up_left_free( Tile tile )
    {
      if ( tile.Y_Position - 1 > 0 && tile.X_Position - 1 > 0 && map.tiles[ tile.X_Position - 1, tile.Y_Position - 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool floodfill_is_up_right_free( Tile tile )
    {
      if ( tile.Y_Position - 1 > 0 && tile.X_Position + 1 < map.tiles.GetLength( 0 ) && map.tiles[ tile.X_Position + 1, tile.Y_Position - 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
    public bool floodfill_is_down_right_free( Tile tile )
    {
      if ( tile.Y_Position + 1 < map.tiles.GetLength( 1 ) && tile.X_Position + 1 < map.tiles.GetLength( 0 ) && map.tiles[ tile.X_Position + 1, tile.Y_Position + 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }

    public bool floodfill_is_down_left_free( Tile tile )
    {
      if ( tile.Y_Position + 1 < map.tiles.GetLength( 1 ) && tile.X_Position - 1 > 0 && map.tiles[ tile.X_Position - 1, tile.Y_Position + 1 ].type == TILE_TYPE.NONE )
      {
        return true;
      }
      return false;
    }
  }
}
