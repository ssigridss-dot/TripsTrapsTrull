using Microsoft.Maui.Graphics;

namespace TripsTrapsTrull
{
    public class WinningLineDrawable : IDrawable
    {
        public float StartX { get; set; }
        public float StartY { get; set; }

        public float EndX { get; set; }
        public float EndY { get; set; }

        public bool IsVisible { get; set; } = false;


        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (!IsVisible)
            {
                return;
            }

            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 7;

            canvas.StrokeLineCap = LineCap.Round;

            canvas.DrawLine(
                StartX,
                StartY,
                EndX,
                EndY);
        }
    }
}