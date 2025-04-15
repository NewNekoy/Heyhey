using Raylib_CsLo;
using System;

namespace Heyhey
{
    internal class Program
    {
        static void Main()
        {
            Core core = new Core();
            while (!Raylib.WindowShouldClose())
            {
                core.Update();


                Raylib.BeginDrawing();
                Raylib.ClearBackground(new Color(30, 30, 30, 255));

                core.Draw();

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}