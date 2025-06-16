using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SystemMonitor
{
    public class GraphControl : Control
    {
        private List<float> values = new List<float>();
        private int maxValues = 100;

        private int horizontalGridLines = 5;
        private bool showGrid = true;
        private bool fillUnderLine = true;
        private bool smoothing = true;

        private ToolTip toolTip = new ToolTip();

        [Category("Appearance")]
        public Color LineColor { get; set; } = Color.Green;

        [Category("Behavior")]
        public int MaxValues
        {
            get => maxValues;
            set
            {
                if (value > 1)
                {
                    maxValues = value;
                    if (values.Count > maxValues)
                    {
                        values.RemoveRange(0, values.Count - maxValues);
                        Invalidate();
                    }
                }
            }
        }

        [Category("Appearance")]
        public bool ShowGrid
        {
            get => showGrid;
            set { showGrid = value; Invalidate(); }
        }

        [Category("Appearance")]
        public bool FillUnderLine
        {
            get => fillUnderLine;
            set { fillUnderLine = value; Invalidate(); }
        }

        [Category("Appearance")]
        public bool Smoothing
        {
            get => smoothing;
            set { smoothing = value; Invalidate(); }
        }

        public void AddValue(float value)
        {
            values.Add(value);
            if (values.Count > maxValues)
                values.RemoveAt(0);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.Clear(BackColor);

            if (showGrid)
                DrawGrid(g);

            if (values.Count < 2)
                return;

            float widthStep = (float)Width / (maxValues - 1);
            PointF[] points = new PointF[values.Count];

            for (int i = 0; i < values.Count; i++)
            {
                float x = i * widthStep;
                float y = Height - (values[i] / 100f * Height);
                points[i] = new PointF(x, y);
            }

            if (fillUnderLine)
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(80, LineColor)))
                {
                    PointF[] fillPoints = new PointF[points.Length + 2];
                    points.CopyTo(fillPoints, 0);
                    fillPoints[fillPoints.Length - 2] = new PointF(points[points.Length - 1].X, Height);
                    fillPoints[fillPoints.Length - 1] = new PointF(points[0].X, Height);

                    g.FillPolygon(brush, fillPoints);
                }
            }

            using (Pen pen = new Pen(LineColor, 2f))
            {
                g.SmoothingMode = smoothing ? SmoothingMode.AntiAlias : SmoothingMode.None;
                g.DrawLines(pen, points);
            }
        }

        private void DrawGrid(Graphics g)
        {
            using (Pen gridPen = new Pen(Color.FromArgb(60, Color.Gray)))
            {
                for (int i = 0; i <= horizontalGridLines; i++)
                {
                    float y = i * (Height / (float)horizontalGridLines);
                    g.DrawLine(gridPen, 0, y, Width, y);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (values.Count == 0)
            {
                toolTip.Hide(this);
                return;
            }

            float widthStep = (float)Width / (maxValues - 1);
            // Show tooltip if mouse near the right edge (last data point)
            if (e.X > Width - widthStep * 2)
            {
                float latestValue = values[values.Count - 1];
                toolTip.Show($"{latestValue:F1}%", this, e.Location.X + 15, e.Location.Y - 15, 1500);
            }
            else
            {
                toolTip.Hide(this);
            }
        }
    }
}
