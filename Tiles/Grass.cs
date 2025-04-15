using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Raylib_CsLo;

namespace Heyhey.Tiles
{
    internal class Grass : Tile
    {
        public override bool CanBeDestroyed => false;
        public Grass(int index, int x, int y, int square_size, Core core)
            : base(index, x, y, square_size, core, new Color(106, 190, 48, 255)) { }

        public override void DefineButtons(Rectangle contextMenu, View view)
        {
            definedButtons.Clear();

            definedButtons.Add(new("Planter une graine", "Execution...", "PlantWood", 5));

            if (core.can_buy)
            {
                definedButtons.Add(new("Expand", "Expanding...", "ExpandMap", 10));
            }
        }
    }
}
