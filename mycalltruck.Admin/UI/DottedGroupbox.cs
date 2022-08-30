using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin.UI
{
    class DottedGroupbox : GroupBox
    {
        public Color BorderColor { get; set; } = Color.FromArgb(0xff, 0x21, 0x21, 0x21);

        protected override void OnPaint(PaintEventArgs e)
        {
            if(e.ClipRectangle.Width != this.Width)
            {
                return;
            }
            Size tSize = TextRenderer.MeasureText(this.Text, this.Font);
            Rectangle borderRect = e.ClipRectangle;
            borderRect.Y += tSize.Height / 2;
            borderRect.Height -= tSize.Height / 2;
            ControlPaint.DrawBorder(e.Graphics, borderRect, this.BorderColor, ButtonBorderStyle.Dotted);
            Rectangle textRect = e.ClipRectangle;
            textRect.X += 6;
            textRect.Width = tSize.Width;
            textRect.Height = tSize.Height;
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), textRect);
            e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), textRect);
        }
    }
}
