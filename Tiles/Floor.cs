using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heyhey.Tiles
{
    internal class Floor : Tile
    {
        public override bool CanBeDestroyed => false;
        public Floor(int index, int x, int y, int square_size, Core core)
            : base(index, x, y, square_size, core, Raylib.WHITE) { }

        public override void DefineButtons(Rectangle contextMenu, View view)
        {
            definedButtons.Clear();

            definedButtons.Add(new("Scratch", "Scratching...", "Scratch", 1));
        }
    }
}