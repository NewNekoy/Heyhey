using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Raylib_CsLo;

namespace Heyhey
{
    internal class Tile
    {
        public Rectangle rec;
        public Color color;
        public int index;
        public bool isActionRunning;
        double actionStartTime = 0;
        float actionDuration = 2f;
        Action? onActionComplete = null;
        string actionLabel = "";
        public int use = 0;
        protected Core core;
        public virtual bool CanBeDestroyed => true;
        public List<(string label, string loading, string action, float temp)> definedButtons = new List<(string label, string loading, string action, float temp)>();

        public Tile(int index, int x, int y, int square_size, Core core, Color? _color = null)
        {
            this.index = index;
            rec.X = x; rec.Y = y; rec.width = square_size; rec.height = square_size;
            color = _color ?? Raylib.WHITE;
            isActionRunning = false;
            this.core = core;
        }

        public virtual void DefineButtons(Rectangle contextMenu, View view)
        {
            definedButtons.Clear();
        }

        public void DrawTileSpecificActions(Rectangle contextMenu, View view, ref float currentY)
        {
            foreach (var action in definedButtons)
            {
                float temp;
                if (core.isDev)
                {
                    temp = 0.1f;
                }
                else
                {
                    temp = action.temp;
                }
                Rectangle btn = new Rectangle(contextMenu.X + 20, contextMenu.Y + currentY, 160, 25);

                if (!isActionRunning && RayGui.GuiButton(btn, action.label))
                {
                    StartAction(action.loading, temp, () =>
                    {
                        view.Parse_Action(this, action.action, true);
                    });
                }

                currentY += 30;
            }
        }

        public void Action_Menu(Rectangle contextMenu, View view)
        {
            float currentY = 40;
            DefineButtons(contextMenu, view);
            DrawTileSpecificActions(contextMenu, view, ref currentY);

            Rectangle destroyBtn = new Rectangle(contextMenu.X + 20, contextMenu.Y + currentY, 160, 25);

            if (isActionRunning)
            {
                float progress = (float)((Raylib.GetTime() - actionStartTime) / actionDuration);
                progress = Math.Clamp(progress, 0f, 1f);

                Raylib.DrawRectangleRec(destroyBtn, Raylib.GRAY);

                Rectangle bar = destroyBtn;
                bar.width *= progress;
                Raylib.DrawRectangleRec(bar, Raylib.RED);

                RayGui.GuiLabel(destroyBtn, $"{actionLabel}");

                if (progress >= 1f)
                {
                    isActionRunning = false;
                    onActionComplete?.Invoke();
                    onActionComplete = null;
                }
            }
        }

        public virtual void Update()
        {

        }

        protected void StartAction(string label, float duration, Action onComplete)
        {
            isActionRunning = true;
            actionStartTime = Raylib.GetTime();
            actionDuration = duration;
            actionLabel = label;
            onActionComplete = onComplete;
        }
    }
}
