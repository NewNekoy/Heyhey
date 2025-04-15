using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heyhey.Tiles
{
    internal class Mine : Tile
    {
        public Mine(int index, int x, int y, int square_size, Core core)
            : base(index, x, y, square_size, core, Raylib.DARKGRAY) { }

        public override void DefineButtons(Rectangle contextMenu, View view)
        {
            definedButtons.Clear();

            definedButtons.Add(new("Mine", "Mining...", "Mine", 2));
        }
    }
}
