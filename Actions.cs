using Heyhey.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Heyhey
{
    internal class Actions
    {
        private Core core;

        public Actions(Core core)
        {
            this.core = core;
        }

        public void GrassTransformation(Tile oldTile)
        {
            Tile newTile = new Grass(oldTile.index, (int)oldTile.rec.X, (int)oldTile.rec.Y, core.square_size, core);
            core.Replace_Tile(oldTile.index, newTile);
        }

        public void Scratch(Tile tile)
        {
            tile.use += 1;
            if (tile.use == 5)
            {
                GrassTransformation(tile);
            }
        }

        public void Destroy(Tile oldTile)
        {
            Tile newTile = new Floor(oldTile.index, (int)oldTile.rec.X, (int)oldTile.rec.Y, core.square_size, core);
            core.Replace_Tile(oldTile.index, newTile);
        }

        public void PlantWood(Tile oldTile)
        {
            Tile newTile = new Wood(oldTile.index, (int)oldTile.rec.X, (int)oldTile.rec.Y, core.square_size, core);
            core.Replace_Tile(oldTile.index, newTile);
        }

        public void ChoppingTree(Tile oldTile)
        {
            BackToFloor(oldTile);
            core.inventory.Add_Material("Wood", 1);
            core.inventory.Add_Material("Vitality", 1);
        }

        public void ExpandMap(Tile oldTile)
        {
            BackToFloor(oldTile);
            core.can_buy = false;
            core.inventory.Remove_Material("Vitality",  core.next_tile);
            core.next_tile = (int)(core.next_tile * 1.25);
            core.ExpandMap();
        }

        public void BackToFloor(Tile oldTile)
        {
            Tile newTile = new Floor(oldTile.index, (int)oldTile.rec.X, (int)oldTile.rec.Y, core.square_size, core);
            core.Replace_Tile(oldTile.index, newTile);
        }
    }
}
