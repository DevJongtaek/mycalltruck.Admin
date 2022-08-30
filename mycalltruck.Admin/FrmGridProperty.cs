using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmGridProperty : Form
    {
        private DataGridView Grid;
        private DataGridViewColumn[] DefaultColumns;
        private DataGridViewColumn SelecteColumn;
        private Dictionary<string, DataGridViewColumn> DicCols = new Dictionary<string, DataGridViewColumn>();
        public FrmGridProperty(DataGridView iGrid, params DataGridViewColumn[] iDefaultColumns)
        {
            InitializeComponent();
           
            Grid = iGrid;
            DefaultColumns = iDefaultColumns;
            //InitComboBox();
            InitListBox();

            var cond = System.Windows.Markup.XmlLanguage.GetLanguage
                    (System.Globalization.CultureInfo.CurrentUICulture.Name);

            FontFamily[] _Fonts = new InstalledFontCollection().Families.Where(c => c.IsStyleAvailable(FontStyle.Regular)).ToArray();
            //                && c.Name[0] >= '가' && c.Name[0] <= '힣'

            cmbFont.Items.AddRange(_Fonts.Select(c => c.Name).ToArray());
            if (cmbFont.Items.Count > 0) cmbFont.SelectedIndex = 0;
          

            //cmbBackColor.DrawMode = DrawMode.OwnerDrawVariable;
            //cmbBackColor.DropDownStyle = ComboBoxStyle.DropDownList;
            if (cmbBackColor.Items.Count > 0) cmbBackColor.SelectedIndex = 0;
            //cmbBackColor.DrawItem += new DrawItemEventHandler(cmbBackColor_DrawItem);
            //cmbBackColor.SelectedIndexChanged += new EventHandler(cmbBackColor_SelectedIndexChanged);

            //cmbForeColor.DrawMode = DrawMode.OwnerDrawVariable;
            //cmbForeColor.DropDownStyle = ComboBoxStyle.DropDownList;
            if (cmbForeColor.Items.Count > 1) cmbForeColor.SelectedIndex = 1;
            //cmbForeColor.DrawItem += new DrawItemEventHandler(cmbForeColor_DrawItem);
            //cmbForeColor.SelectedIndexChanged += new EventHandler(cmbForeColor_SelectedIndexChanged);
        }
        private void cmbForeColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelecteColumn == null) return;
            var _ColorQuery = new[] {new{Text="하얀색", Value = Color.White},new{Text="검정색", Value = Color.Black}
                ,new{Text="빨강색", Value = Color.Red},new{Text="주황색", Value = Color.Orange},new{Text="노랑색", Value = Color.Yellow}
                ,new{Text="연한 녹색", Value = Color.LightGreen},new{Text="녹색", Value = Color.Green},new{Text="연한 파랑색"
                    , Value = Color.LightBlue},new{Text="파랑색", Value = Color.Blue},new{Text="보라색", Value = Color.Violet}};
            if (cmbForeColor.SelectedItem == null) return;
            if (_ColorQuery.Select(c => c.Text).Contains(cmbForeColor.SelectedItem.ToString()))
            {
                Color _Color = _ColorQuery.Where(c => c.Text == cmbForeColor.SelectedItem.ToString()).First().Value;
                txtPreview.ForeColor = _Color;
                SelecteColumn.DefaultCellStyle.ForeColor = _Color;
            }
        }
        private void cmbForeColor_DrawItem(object sender, DrawItemEventArgs e)
        {
            var _ColorQuery = new[] {new{Text="하얀색", Value = Color.White},new{Text="검정색", Value = Color.Black}
                ,new{Text="빨강색", Value = Color.Red},new{Text="주황색", Value = Color.Orange},new{Text="노랑색", Value = Color.Yellow}
                ,new{Text="연한 녹색", Value = Color.LightGreen},new{Text="녹색", Value = Color.Green},new{Text="연한 파랑색"
                    , Value = Color.LightBlue},new{Text="파랑색", Value = Color.Blue},new{Text="보라색", Value = Color.Violet}};
            if (e.Index < 0) return;
            if (_ColorQuery.Select(c => c.Text).Contains(cmbForeColor.Items[e.Index].ToString()))
            {
                string text = cmbForeColor.Items[e.Index].ToString();
                Color _Color = _ColorQuery.Where(c => c.Text == cmbForeColor.Items[e.Index].ToString()).First().Value;
                Brush brush = null;
                if (_Color.ToArgb() == Color.Black.ToArgb())
                    brush = Brushes.White;
                else brush = Brushes.Black;
                Graphics g = e.Graphics;
                try
                {
                    g.FillRectangle(new SolidBrush(_Color), e.Bounds);
                    g.DrawString(text, new Font("굴림", 9), brush, e.Bounds.Location);
                }
                catch { }
            }
        }

        private void cmbBackColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelecteColumn == null) return;
            var _ColorQuery = new[] {new{Text="하얀색", Value = Color.White},new{Text="검정색", Value = Color.Black}
                ,new{Text="빨강색", Value = Color.Red},new{Text="주황색", Value = Color.Orange},new{Text="노랑색", Value = Color.Yellow}
                ,new{Text="연한 녹색", Value = Color.LightGreen},new{Text="녹색", Value = Color.Green},new{Text="연한 파랑색"
                    , Value = Color.LightBlue},new{Text="파랑색", Value = Color.Blue},new{Text="보라색", Value = Color.Violet}};
            if (cmbBackColor.SelectedItem == null) return;
            if (_ColorQuery.Select(c => c.Text).Contains(cmbBackColor.SelectedItem.ToString()))
            {
                Color _Color = _ColorQuery.Where(c => c.Text == cmbBackColor.SelectedItem.ToString()).First().Value;
                txtPreview.BackColor = _Color;
                SelecteColumn.DefaultCellStyle.BackColor = _Color;
            }
        }
        private void cmbBackColor_DrawItem(object sender, DrawItemEventArgs e)
        {
            var _ColorQuery = new[] {new{Text="하얀색", Value = Color.White},new{Text="검정색", Value = Color.Black}
                ,new{Text="빨강색", Value = Color.Red},new{Text="주황색", Value = Color.Orange},new{Text="노랑색", Value = Color.Yellow}
                ,new{Text="연한 녹색", Value = Color.LightGreen},new{Text="녹색", Value = Color.Green},new{Text="연한 파랑색"
                    , Value = Color.LightBlue},new{Text="파랑색", Value = Color.Blue},new{Text="보라색", Value = Color.Violet}};
            if (e.Index < 0) return;
            if (_ColorQuery.Select(c => c.Text).Contains(cmbBackColor.Items[e.Index].ToString()))
            {
                string text = cmbBackColor.Items[e.Index].ToString();
                Color _Color = _ColorQuery.Where(c => c.Text == cmbBackColor.Items[e.Index].ToString()).First().Value;
                Brush brush = null;
                if (_Color.ToArgb() == Color.Black.ToArgb())
                    brush = Brushes.White;
                else brush = Brushes.Black;
                Graphics g = e.Graphics;
                try
                {
                    g.FillRectangle(new SolidBrush(_Color), e.Bounds);
                    g.DrawString(text, new Font("굴림", 9), brush, e.Bounds.Location);
                }
                catch { }
            }
        }
        private void cmbFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFont.SelectedItem == null) return;
            if (SelecteColumn == null) return;
            try
            {
                System.Drawing.Font font = new Font(cmbFont.SelectedItem.ToString(), 9);
                SelecteColumn.DefaultCellStyle.Font = font;
                txtPreview.Font = font;
            }
            catch { }
        }
        private void cmbFont_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            Graphics g = e.Graphics;
            Font font = new Font(cmbFont.Items[e.Index].ToString(), 9);
            g.DrawString(cmbFont.Items[e.Index].ToString(), font, Brushes.Black, e.Bounds.X, e.Bounds.Y);
        }
        #region cmbFont
        private void initCmbFont()
        {

        }
        #endregion
     
        private void InitListBox()
        {
            lbxCol.Items.Clear();
            DicCols.Clear();
            DataGridViewColumn[] _Cols = new DataGridViewColumn[Grid.ColumnCount];
            Grid.Columns.CopyTo(_Cols, 0);
            foreach (DataGridViewColumn item in _Cols.OrderBy(c => c.DisplayIndex))
            {
                DicCols.Add(item.HeaderText, item);
                lbxCol.Items.Add(item.HeaderText, item.Visible);

            }
        }
        private void numFontSize_ValueChanged(object sender, EventArgs e)
        {
            if (SelecteColumn == null) return;
            txtPreview.Font = new Font(txtPreview.Font.Name, (float)numFontSize.Value);
            if (SelecteColumn != null) SelecteColumn.DefaultCellStyle.Font = txtPreview.Font;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lbxCol_SelectedIndexChanged(object sender, EventArgs e)
        {
            var _ColorQuery = new[] {new{Text="하얀색", Value = Color.White},new{Text="검정색", Value = Color.Black}
                ,new{Text="빨강색", Value = Color.Red},new{Text="주황색", Value = Color.Orange},new{Text="노랑색", Value = Color.Yellow}
                ,new{Text="연한 녹색", Value = Color.LightGreen},new{Text="녹색", Value = Color.Green},new{Text="연한 파랑색"
                    , Value = Color.LightBlue},new{Text="파랑색", Value = Color.Blue},new{Text="보라색", Value = Color.Violet}};
            if (lbxCol.SelectedItem != null) SelecteColumn = DicCols[lbxCol.SelectedItem.ToString()];
            txtPreview.BackColor = Color.White;
            txtPreview.ForeColor = Color.Black;
            txtPreview.Font = new Font("굴림", 9);
            var backColor = _ColorQuery.Where(c => c.Value.ToArgb() == SelecteColumn.DefaultCellStyle.BackColor.ToArgb());
            var foreColor = _ColorQuery.Where(c => c.Value.ToArgb() == SelecteColumn.DefaultCellStyle.ForeColor.ToArgb());
            if (backColor.Count() > 0)
            {
                cmbBackColor.SelectedItem = backColor.First().Text;
                txtPreview.BackColor = backColor.First().Value;
            }
            if (foreColor.Count() > 0)
            {
                cmbForeColor.SelectedItem = foreColor.First().Text;
                txtPreview.ForeColor = foreColor.First().Value;
            }
            string fontName;
            try
            {
                fontName = SelecteColumn.DefaultCellStyle.Font.Name;
            }
            catch { fontName = Grid.DefaultCellStyle.Font.Name; }
            try
            {
                cmbFont.SelectedItem = fontName;
                numFontSize.Value = (decimal)SelecteColumn.DefaultCellStyle.Font.Size;
                txtPreview.Font = new Font(cmbFont.SelectedItem.ToString(), (float)numFontSize.Value);
            }
            catch { }
        }

        private void lbxCol_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            DicCols[lbxCol.Items[e.Index].ToString()].Visible = Convert.ToBoolean(e.NewValue);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (lbxCol.SelectedIndex <= 0) return;
            int index = lbxCol.SelectedIndex;
            //리스트박스에서 이동
            object item = lbxCol.SelectedItem;
            bool check = lbxCol.GetItemChecked(index);
            lbxCol.Items.Remove(lbxCol.SelectedItem);
            lbxCol.Items.Insert(index - 1, item);
            lbxCol.SelectedIndex = index - 1;
            lbxCol.SetItemChecked(index - 1, check);
            //그리드에서이동
            DicCols.Where(c => c.Value.DisplayIndex == index).First().Value.DisplayIndex = index - 1;
        }
        private void btnDown_Click(object sender, EventArgs e)
        {
            if (lbxCol.SelectedIndex < 0) return;
            if (lbxCol.SelectedIndex == lbxCol.Items.Count - 1) return;
            int index = lbxCol.SelectedIndex;
            object item = lbxCol.SelectedItem;
            bool check = lbxCol.GetItemChecked(index);
            lbxCol.Items.Remove(lbxCol.SelectedItem);
            lbxCol.Items.Insert(index + 1, item);
            lbxCol.SelectedIndex = index + 1;
            lbxCol.SetItemChecked(index + 1, check);
            DicCols.Where(c => c.Value.DisplayIndex == index).First().Value.DisplayIndex = index + 1;
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (DataGridViewColumn col in DefaultColumns)
            {
                col.DisplayIndex = i;
                col.Visible = true;
                col.DefaultCellStyle.Font = Grid.DefaultCellStyle.Font;
                col.DefaultCellStyle.BackColor = Grid.DefaultCellStyle.BackColor;
                col.DefaultCellStyle.ForeColor = Grid.DefaultCellStyle.ForeColor;
                i++;
            }
            DataGridViewColumn[] _Cols = new DataGridViewColumn[Grid.ColumnCount];
            Grid.Columns.CopyTo(_Cols, 0);
            foreach (DataGridViewColumn col in _Cols.Except(DefaultColumns))
            {
                col.Visible = false;
            }
            InitListBox();
        }

        private void FrmGridProperty_Load(object sender, EventArgs e)
        {

        }

    }
}
