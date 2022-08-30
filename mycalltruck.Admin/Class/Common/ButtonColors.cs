using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin.Class.Common
{
    public class ButtonColors
    {
        public void Button_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (sender as Control != null)
                {
                    Button pnShortCut = sender as Control as Button;
                    if (pnShortCut.Enabled)
                    {
                        pnShortCut.BackColor = Color.Green;
                    }
                }
            }
            catch { }
        }
        public void Button_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                if (sender as Control != null)
                {
                    Button pnShortCut = sender as Control as Button;
                    // pnShortCut.Invalidate();
                    pnShortCut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
                }
            }
            catch { }

        }

        public void Button_MouseLeave1(object sender, EventArgs e)
        {
            try
            {
                if (sender as Control != null)
                {
                    Button pnShortCut = sender as Control as Button;
                    // pnShortCut.Invalidate();
                    pnShortCut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(189)))), ((int)(((byte)(188)))));
                }
            }
            catch { }

        }
    }
}
