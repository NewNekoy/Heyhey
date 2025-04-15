using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Heyhey
{
    internal class View
    {
        public Core? core;

        public int screenWidth = 1000;
        public int screenHeight = 680;
        public Camera2D camera;

        public bool showContextMenu = false;
        Rectangle contextMenu = new Rectangle(100, 100, 200, 120);
        public Tile? selectedTile = null;

        public Vector2 cameraTargetGoal;
        public float cameraLerpSpeed = 5.0f;
        public float cameraZoomGoal = 25.0f;
        public float zoomLerpSpeed = 5.0f;

        Dictionary<string, Action<Tile>> actionMap = new();

        public View()
        {
            Raylib.InitWindow(screenWidth, screenHeight, "Hey_Hey");
            Raylib.SetTargetFPS(240);

            camera = new Camera2D();
        }

        public void Setup_Camera_FromMap(List<List<Tile>> map)
        {
            int tileWidth = (int)map[0][0].rec.width;
            int tileHeight = (int)map[0][0].rec.height;

            int cols = map[0].Count;
            int rows = map.Count;

            float totalWidth = cols * tileWidth;
            float totalHeight = rows * tileHeight;

            float centerX = totalWidth / 2f;
            float centerY = totalHeight / 2f;

            cameraTargetGoal = new Vector2(centerX, centerY);

            camera.offset = new Vector2(screenWidth / 2f, screenHeight / 2f);
            camera.zoom = 25.0f;
        }

        public void UpdateCameraSmooth()
        {
            camera.target = Vector2.Lerp(camera.target, cameraTargetGoal, Raylib.GetFrameTime() * cameraLerpSpeed);
        }

        public void AutoZoomToFitMap(int tileCols, int tileRows, int tileSize)
        {
            float mapWidth = tileCols * tileSize;
            float mapHeight = tileRows * tileSize;

            float zoomX = screenWidth / mapWidth;
            float zoomY = screenHeight / mapHeight;

            float margin = 0.60f;
            float targetZoom = MathF.Min(zoomX, zoomY) * margin;

            cameraZoomGoal = Math.Clamp(targetZoom, 2f, 100f);
        }

        public void UpdateCameraZoomSmooth()
        {
            float t = Raylib.GetFrameTime() * zoomLerpSpeed;
            camera.zoom += (cameraZoomGoal - camera.zoom) * t;
        }

        public void Tile_Window_Update(List<List<Tile>> map, Vector2 mouse)
        {
            if (showContextMenu && !Raylib.CheckCollisionPointRec(mouse, contextMenu))
            {
                showContextMenu = false;
                selectedTile = null;
            }
            else
            {
                Vector2 mouseWorld = Raylib.GetScreenToWorld2D(mouse, camera);
                foreach (var row in map)
                {
                    foreach (var tile in row)
                    {
                        if (Raylib.CheckCollisionPointRec(mouseWorld, tile.rec) && !showContextMenu)
                        {
                            selectedTile = tile;
                            showContextMenu = true;

                            contextMenu.X = Math.Clamp(mouse.X - contextMenu.width / 2, 0, screenWidth - contextMenu.width);
                            contextMenu.Y = mouse.Y - 10;
                        }
                    }
                }
            }
        }
        public void Draw_UI()
        {
            if (!showContextMenu || selectedTile == null) return;
            float buttonStartY = 40;
            int numberOfButtons = selectedTile.definedButtons.Count;
            float spacePerButton = 30;

            float destroyHeight = selectedTile.CanBeDestroyed ? spacePerButton : 0;
            contextMenu.height = buttonStartY + (numberOfButtons * spacePerButton) + destroyHeight + 20;
            if (!selectedTile.isActionRunning && RayGui.GuiWindowBox(contextMenu, "Actions"))
            {
                showContextMenu = false;
                selectedTile = null;
            }

            if (selectedTile != null)
            {
                selectedTile!.Action_Menu(contextMenu, this);
            }
        }

        public void Parse_Action(Tile tile, string actionName, bool close = false)
        {
            if (actionMap.ContainsKey(actionName))
            {
                actionMap[actionName].Invoke(tile);
            }

            if (close)
            {
                showContextMenu = false;
                selectedTile = null;
            }
        }

        public void InitActions()
        {
            if (core == null || core.actions == null) return;

            actionMap["Scratch"] = core.actions.Scratch;
            actionMap["PlantWood"] = core.actions.PlantWood;
            actionMap["ChoppingTree"] = core.actions.ChoppingTree;
            actionMap["ExpandMap"] = core.actions.ExpandMap;
            actionMap["Destroy"] = core.actions.Destroy;
        }
    }
}
