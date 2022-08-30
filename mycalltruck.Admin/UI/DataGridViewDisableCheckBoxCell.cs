using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin.UI
{
    class DataGridViewDisableCheckBoxColumn : DataGridViewCheckBoxColumn
    {
        public DataGridViewDisableCheckBoxColumn()
        {
            this.CellTemplate = new DataGridViewDisableCheckBoxCell();
        }
    }
    class DataGridViewDisableCheckBoxCell : DataGridViewCheckBoxCell
    {
        public bool Enabled { get; set; } = true;
        public bool CheckBoxVisible { get; set; } = true;
        public override object Clone()
        {
            DataGridViewDisableCheckBoxCell cell = (DataGridViewDisableCheckBoxCell)base.Clone();
            cell.Enabled = this.Enabled;
            return cell;
        }
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            if (CheckBoxVisible)
            {
                if (Enabled)
                {
                    base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
                }
                else
                {
                    if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                    {
                        if ((elementState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                        {
                            SolidBrush cellBackground =
                                new SolidBrush(cellStyle.SelectionBackColor);
                            graphics.FillRectangle(cellBackground, cellBounds);
                            cellBackground.Dispose();
                        }
                        else
                        {
                            SolidBrush cellBackground =
                                new SolidBrush(cellStyle.BackColor);
                            graphics.FillRectangle(cellBackground, cellBounds);
                            cellBackground.Dispose();
                        }
                    }

                    if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
                    {
                        PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                            advancedBorderStyle);
                    }

                    Rectangle buttonArea = cellBounds;
                    Rectangle buttonAdjustment =
                        this.BorderWidths(advancedBorderStyle);
                    buttonArea.X += buttonAdjustment.X;
                    buttonArea.Y += buttonAdjustment.Y;
                    buttonArea.Height -= buttonAdjustment.Height;
                    buttonArea.Width -= buttonAdjustment.Width;

                    var gLocation = buttonArea.Location;
                    var gSize = CheckBoxRenderer.GetGlyphSize(graphics, System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedDisabled);
                    gLocation.X += ((buttonArea.Width - gSize.Width) / 2);
                    gLocation.Y += ((buttonArea.Height - gSize.Height) / 2);

                    CheckBoxRenderer.DrawCheckBox(graphics, gLocation,
                        System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedDisabled);

                }
            }
            else
            {
                if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                {
                    if ((elementState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                    {
                        SolidBrush cellBackground =
                            new SolidBrush(cellStyle.SelectionBackColor);
                        graphics.FillRectangle(cellBackground, cellBounds);
                        cellBackground.Dispose();
                    }
                    else
                    {
                        SolidBrush cellBackground =
                            new SolidBrush(cellStyle.BackColor);
                        graphics.FillRectangle(cellBackground, cellBounds);
                        cellBackground.Dispose();
                    }
                }

                if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
                {
                    PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                        advancedBorderStyle);
                }
            }
        }
    }
}
