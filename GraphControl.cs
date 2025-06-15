using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SystemMonitor
{
    public class GraphControl : Control
    {
        private readonly Queue<float> values = new Queue<float>();
        private int maxValues = 50;
        private Color graphColor = Color.Lime;

        [Category("Appearance")]
        public Color GraphColor
        {
            get => graphColor;
            set { graphColor = value; Invalidate(); }
        }

        [Category("Data")]
        public int MaxValues
        {
            get => maxValues;
            set { maxValues = value; Invalidate(); }
        }

        public void AddValue(float value)
        {
            if (values.Count >= maxValues)
                values.Dequeue();
            values.Enqueue(value);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(BackColor);

            if (values.Count < 2)
                return;

            float[] dataArray = values.ToArray();
            float max = 100f;
            float scaleX = (float)Width / (maxValues - 1);
            float scaleY = (float)Height / max;

            using (Pen pen = new Pen(graphColor, 2))
            {
                for (int i = 1; i < dataArray.Length; i++)
                {
                    float x1 = (i - 1) * scaleX;
                    float y1 = Height - (dataArray[i - 1] * scaleY);
                    float x2 = i * scaleX;
                    float y2 = Height - (dataArray[i] * scaleY);
                    e.Graphics.DrawLine(pen, x1, y1, x2, y2);
                }
            }
        }
    }
}
