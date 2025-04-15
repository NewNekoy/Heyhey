using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heyhey.Tiles
{
    internal class Wood : Tile
    {
        private int growTicks = 0;
        private int maxGrowTicks = 10;
        public Wood(int index, int x, int y, int square_size, Core core)
            : base(index, x, y, square_size, core, new Color(106, 190, 48, 255)) { }

        public override void DefineButtons(Rectangle contextMenu, View view)
        {
            definedButtons.Clear();

            if (growTicks >= maxGrowTicks)
            {
                definedButtons.Add(new("Chop", "Chopping...", "ChoppingTree", 2));
            }
            definedButtons.Add(new("Destroy", "Destroying...", "Destroy", 5));
        }

        public override void Update()
        {
            if (growTicks < maxGrowTicks)
            {
                growTicks++;

                float t = (float)growTicks / maxGrowTicks;

                int r = MathUtils.Lerp(130, 55, t);
                int g = MathUtils.Lerp(210, 125, t);
                int b = MathUtils.Lerp(70, 34, t);

                color = new Color(r, g, b, 255);
            }
        }
    }
}
