using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
  class RoomConnector
  {
    private DungenMap map;

    public RoomConnector( DungenMap map )
    {
      this.map = map;
      this.set_connections( );
      this.generate_spanning_tree( );
      
    }

    private void generate_spanning_tree( )
    {
      // Search for main room
      do
      {

        Tile main_room = null;
        while ( ( main_room = map.search_for_tile_without_main_region_from_pos( TILE_TYPE.SAND, main_room ) ) != null )
        {
          main_room.in_main_region = true;         

          if (
               this.is_left_tile_of_type( main_room, TILE_TYPE.GRASS ) && this.is_right_tile_of_type( main_room, TILE_TYPE.DIRT ) ||
               this.is_right_tile_of_type( main_room, TILE_TYPE.GRASS ) && this.is_left_tile_of_type( main_room, TILE_TYPE.DIRT )
             )
          {
            add_all_ajectiled_tiles_of_type_to_main_region( map.tiles[ main_room.X_Position - 1, main_room.Y_Position ] );
            add_all_ajectiled_tiles_of_type_to_main_region( map.tiles[ main_room.X_Position + 1, main_room.Y_Position ] );
            this.remove_all_unnessesary_connections( );
          }
          else if (
                     this.is_up_tile_of_type( main_room, TILE_TYPE.GRASS ) && this.is_down_tile_of_type( main_room, TILE_TYPE.DIRT ) ||
                     this.is_down_tile_of_type( main_room, TILE_TYPE.GRASS ) && this.is_up_tile_of_type( main_room, TILE_TYPE.DIRT )
                   )
          {
            
            add_all_ajectiled_tiles_of_type_to_main_region( map.tiles[ main_room.X_Position, main_room.Y_Position - 1 ] );
            add_all_ajectiled_tiles_of_type_to_main_region( map.tiles[ main_room.X_Position, main_room.Y_Position + 1 ] );
            this.remove_all_unnessesary_connections( );
          }

        }

      } while ( map.search_for_tile_without_main_region( TILE_TYPE.SAND ) != null );
      this.remove_all_unnessesary_connections2( );

    }

    private void remove_all_unnessesary_connections2( )
    {
      for ( int i = 0; i < map.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < map.tiles.GetLength( 1 ); j++ )
        {

          if (
               map.tiles[ i, j ].type == TILE_TYPE.SAND &&
               this.is_left_tile_in_main_region( map.tiles[ i, j ] ) &&
               this.is_right_tile_in_main_region( map.tiles[ i, j ] ) &&
               this.is_left_tile_of_type( map.tiles[ i, j ], TILE_TYPE.DIRT ) &&
               this.is_right_tile_of_type( map.tiles[ i, j ], TILE_TYPE.DIRT )
             )
          {            
              if ( Generator.rand.Next( 100 ) < 5 )
              {
                map.tiles[ i, j ].in_main_region = true;
                continue;
              }
              map.tiles[ i, j ].change_biom( TILE_TYPE.NONE );
          }
           if (
                    map.tiles[ i, j ].type == TILE_TYPE.SAND &&
                    this.is_up_tile_in_main_region( map.tiles[ i, j ] ) &&
                    this.is_down_tile_in_main_region( map.tiles[ i, j ] ) &&
                    this.is_up_tile_of_type( map.tiles[ i, j ], TILE_TYPE.DIRT ) &&
                    this.is_down_tile_of_type( map.tiles[ i, j ], TILE_TYPE.DIRT )
                  )
            {
              
                if ( Generator.rand.Next( 100 ) < 5 )
                {
                  map.tiles[ i, j ].in_main_region = true;
                  continue;
                }
                map.tiles[ i, j ].change_biom( TILE_TYPE.NONE );
          }

        }
      }
    }

    private void remove_all_unnessesary_connections( )
    {
      for ( int i = 0; i < map.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 0; j < map.tiles.GetLength( 1 ); j++ )
        {

          if ( map.tiles[ i, j ].type == TILE_TYPE.SAND && map.tiles[ i, j ].in_main_region == false )
          {
            if (
              ( this.is_left_tile_in_main_region( map.tiles[ i, j ] ) && this.is_right_tile_in_main_region( map.tiles[ i, j ] ) ) ||
              ( this.is_up_tile_in_main_region( map.tiles[ i, j ] ) && this.is_down_tile_in_main_region( map.tiles[ i, j ] ) )
            )
            {
              if ( (this.is_up_tile_of_type( map.tiles[ i, j ], TILE_TYPE.DIRT  ) && this.is_down_tile_of_type( map.tiles[ i, j ], TILE_TYPE.DIRT ) ) ||
                    ( this.is_left_tile_of_type( map.tiles[ i, j ], TILE_TYPE.DIRT ) && this.is_right_tile_of_type( map.tiles[ i, j ], TILE_TYPE.DIRT ) )
                )
              {
                continue;
              }
              if ( Generator.rand.Next( 100 ) < 3 )
              {
                map.tiles[ i, j ].in_main_region = true;
                continue;
              }
              map.tiles[ i, j ].change_biom( TILE_TYPE.NONE );
            }
          }
       
        }
      }
    }

    public void add_all_ajectiled_tiles_of_type_to_main_region( Tile tile )
    {
      tile.in_main_region = true;
      if ( this.is_left_tile_of_type( tile, tile.type ) && !map.tiles[ tile.X_Position - 1, tile.Y_Position ].in_main_region )
      {
        add_all_ajectiled_tiles_of_type_to_main_region( map.tiles[ tile.X_Position - 1, tile.Y_Position ] );
      }
      if ( this.is_right_tile_of_type( tile, tile.type ) && !map.tiles[ tile.X_Position + 1, tile.Y_Position ].in_main_region)
      {
        add_all_ajectiled_tiles_of_type_to_main_region( map.tiles[ tile.X_Position + 1, tile.Y_Position ] );
      }
      if ( this.is_up_tile_of_type( tile, tile.type ) && !map.tiles[ tile.X_Position, tile.Y_Position - 1 ].in_main_region )
      {
        add_all_ajectiled_tiles_of_type_to_main_region( map.tiles[ tile.X_Position, tile.Y_Position - 1 ] );
      }
      if ( this.is_down_tile_of_type( tile, tile.type ) && !map.tiles[ tile.X_Position, tile.Y_Position + 1 ].in_main_region )
      {
        add_all_ajectiled_tiles_of_type_to_main_region( map.tiles[ tile.X_Position, tile.Y_Position + 1 ] );
      }
    }

    public bool is_left_tile_in_main_region( Tile tile)
    {
      if ( tile.X_Position - 1 > 0 && map.tiles[ tile.X_Position - 1, tile.Y_Position ].in_main_region )
      {
        return true;
      }
      return false;
    }

    public bool is_right_tile_in_main_region( Tile tile )
    {
      if ( tile.X_Position + 1 < map.tiles.GetLength( 0 ) && map.tiles[ tile.X_Position + 1, tile.Y_Position ].in_main_region )
      {
        return true;
      }
      return false;
    }

    public bool is_up_tile_in_main_region( Tile tile )
    {
      if ( tile.Y_Position - 1 > 0 && map.tiles[ tile.X_Position, tile.Y_Position - 1 ].in_main_region )
      {
        return true;
      }
      return false;
    }

    public bool is_down_tile_in_main_region( Tile tile )
    {
      if ( tile.Y_Position + 1 < map.tiles.GetLength( 1 ) && map.tiles[ tile.X_Position, tile.Y_Position + 1 ].in_main_region )
      {
        return true;
      }
      return false;
    }

    public bool is_left_tile_of_type( Tile tile, TILE_TYPE type )
    {
      if ( tile.X_Position - 1 > 0 && map.tiles[ tile.X_Position - 1, tile.Y_Position ].type == type )
      {
        return true;
      }
      return false;
    }

    public bool is_right_tile_of_type( Tile tile, TILE_TYPE type )
    {
      if ( tile.X_Position + 1 < map.tiles.GetLength( 0 ) && map.tiles[ tile.X_Position + 1, tile.Y_Position ].type == type )
      {
        return true;
      }
      return false;
    }

    public bool is_up_tile_of_type( Tile tile, TILE_TYPE type )
    {
      if ( tile.Y_Position - 1 > 0 && map.tiles[ tile.X_Position, tile.Y_Position - 1 ].type == type )
      {
        return true;
      }
      return false;
    }

    public bool is_down_tile_of_type( Tile tile, TILE_TYPE type )
    {
      if ( tile.Y_Position + 1 < map.tiles.GetLength( 1 ) && map.tiles[ tile.X_Position, tile.Y_Position + 1 ].type == type )
      {
        return true;
      }
      return false;
    }

    public void set_connections( )
    {
      for ( int i = 1; i < map.tiles.GetLength( 0 ); i++ )
      {
        for ( int j = 1; j < map.tiles.GetLength( 1 ); j++ )
        {
          //if ( map.map[ i, j ].type == TILE_TYPE.NONE && map.map[ i, j ].X_Position % 2 == 1 && map.map[ i, j ].Y_Position % 2 == 1  )
          if ( map.tiles[ i, j ].type == TILE_TYPE.NONE )
          {
            // UP DOWN
            if ( map.tiles[ i, j ].Y_Position + 1 < map.tiles.GetLength( 1 ) && ( map.tiles[ i, j + 1 ].type == TILE_TYPE.GRASS || map.tiles[ i, j + 1 ].type == TILE_TYPE.DIRT ) &&
                 map.tiles[ i, j ].Y_Position - 1 < map.tiles.GetLength( 1 ) && map.tiles[ i, j - 1 ].type == TILE_TYPE.DIRT
              )
            {
              map.tiles[ i, j ].change_biom( TILE_TYPE.SAND );
            }
            if ( map.tiles[ i, j ].Y_Position - 1 < map.tiles.GetLength( 1 ) && ( map.tiles[ i, j - 1 ].type == TILE_TYPE.GRASS || map.tiles[ i, j - 1 ].type == TILE_TYPE.DIRT ) &&
                 map.tiles[ i, j ].Y_Position + 1 < map.tiles.GetLength( 1 ) && map.tiles[ i, j + 1 ].type == TILE_TYPE.DIRT
              )
            {
              map.tiles[ i, j ].change_biom( TILE_TYPE.SAND );
            }
            if ( map.tiles[ i, j ].X_Position + 1 < map.tiles.GetLength( 0 ) && ( map.tiles[ i + 1, j ].type == TILE_TYPE.GRASS || map.tiles[ i + 1, j ].type == TILE_TYPE.DIRT ) &&
                 map.tiles[ i, j ].X_Position - 1 < map.tiles.GetLength( 0 ) && map.tiles[ i - 1, j ].type == TILE_TYPE.DIRT
                 )
            {
              map.tiles[ i, j ].change_biom( TILE_TYPE.SAND );
            }
            if ( map.tiles[ i, j ].X_Position - 1 < map.tiles.GetLength( 0 ) && ( map.tiles[ i - 1, j ].type == TILE_TYPE.GRASS || map.tiles[ i - 1, j ].type == TILE_TYPE.DIRT ) &&
                 map.tiles[ i, j ].X_Position + 1 < map.tiles.GetLength( 0 ) && map.tiles[ i + 1, j ].type == TILE_TYPE.DIRT
                 )
            {
              map.tiles[ i, j ].change_biom( TILE_TYPE.SAND );
            }
          }
        }
      }
    }
  }
}
