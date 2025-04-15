using Heyhey.Tiles;
using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Heyhey
{
    internal class Core
    {
        public double tickInterval = 1.0;
        public double lastTickTime;
        public int tile_length = 1;
        public int tile_height = 1;
        public int square_size = 10;

        List<List<Tile>> map = new List<List<Tile>>();
        View view;
        public Inventory inventory;
        public Actions actions;
        public bool isDev = true;

        public int next_tile = 5;
        public bool can_buy = false;
        public Core()
        {
            lastTickTime = Raylib.GetTime();

            Build_Map();

            Tile centerTile = map[tile_height / 2][tile_length / 2];
            view = new View();
            view.core = this;
            actions = new Actions(this);
            inventory = new Inventory(this);

            view.Setup_Camera_FromMap(map);
            view.AutoZoomToFitMap(tile_length, tile_height, square_size);
            view.InitActions();
            inventory.Add_Material("Vitality", 150);
        }

        public void Build_Map()
        {
            List<List<Tile>> newMap = new List<List<Tile>>();

            int index = 1;
            for (int y = 0; y < tile_height; y++)
            {
                List<Tile> row = new List<Tile>();

                for (int x = 0; x < tile_length; x++)
                {
                    Tile? tileToReuse = null;

                    if (y < map.Count && x < map[y].Count)
                    {
                        tileToReuse = map[y][x];
                        tileToReuse.rec.X = x * square_size;
                        tileToReuse.rec.Y = y * square_size;
                    }

                    if (tileToReuse != null)
                    {
                        row.Add(tileToReuse);
                    }
                    else
                    {
                        Tile newTile = new Floor(index, x * square_size, y * square_size, square_size, this);
                        row.Add(newTile);
                    }

                    index++;
                }

                newMap.Add(row);
            }

            map = newMap;
        }

        public void Update()
        {
            double currentTime = Raylib.GetTime();

            if (currentTime - lastTickTime >= tickInterval)
            {
                lastTickTime = currentTime;

                foreach (var row in map)
                    foreach (var tile in row)
                        tile.Update();
            }

            if (!view.showContextMenu && Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                Vector2 mouse = Raylib.GetMousePosition();

                view.Tile_Window_Update(map, mouse);
            }

            if (inventory.HasEnough("Vitality", next_tile))
            {
                can_buy = true;
            }

            view.UpdateCameraSmooth();
            view.UpdateCameraZoomSmooth();
        }

        public void Draw()
        {
            Raylib.DrawFPS(10, 10);
            int y = 40;
            foreach ((string label, int amount) in inventory.materials)
            {
                Raylib.DrawText($"{label}: {amount}", 10, y, 20, Raylib.WHITE);
                y += 20;
            }

            Raylib.BeginMode2D(view.camera);
            Draw_Map();
            Raylib.EndMode2D();

            Draw_UI();
        }

        public void Draw_UI()
        {
            view.Draw_UI();
        }

        public void Draw_Map()
        {
            float t = 0.4f;

            foreach (var row in map)
            {
                foreach (var tile in row)
                {
                    Raylib.DrawRectangleRec(tile.rec, tile.color);

                    float x = tile.rec.X;
                    float y = tile.rec.Y;
                    float w = tile.rec.width;
                    float h = tile.rec.height;

                    DrawLine(x + w, y, x + w, y + h, t);
                    DrawLine(x, y + h, x + w, y + h, t);

                    if (tile.rec.X == 0)
                        DrawLine(x + t / 2, y, x + t / 2, y + h, t);

                    if (tile.rec.Y == 0)
                        DrawLine(x, y + t / 2, x + w, y + t / 2, t);
                }
            }

            void DrawLine(float x1, float y1, float x2, float y2, float thickness)
            {
                Raylib.DrawLineEx(new Vector2(x1, y1), new Vector2(x2, y2), thickness, Raylib.BLACK);
            }
        }

        public void Replace_Tile(int index, Tile newTile)
        {
            for (int y = 0; y < map.Count; y++)
            {
                for (int x = 0; x < map[y].Count; x++)
                {
                    if (map[y][x].index == index)
                    {
                        map[y][x] = newTile;
                        return;
                    }
                }
            }
        }

        public Tile? Get_Tile(int index)
        {
            for (int y = 0; y < map.Count; y++)
            {
                for (int x = 0; x < map[y].Count; x++)
                {
                    if (map[y][x].index == index)
                    {
                        return map[y][x];
                    }
                }
            }
            return null;
        }

        public int Get_Number_Tiles()
        {
            int nb = 0;
            foreach (List<Tile> list in map)
            {
                foreach (Tile tile in list)
                {
                    nb += 1;
                }
            }
            return nb;
        }

        public void ExpandMap()
        {
            if (tile_length == tile_height)
                tile_length += 1;
            else
                tile_height += 1;

            Build_Map();

            view.Setup_Camera_FromMap(map);
            view.AutoZoomToFitMap(tile_length, tile_height, square_size);
        }
    }
}
