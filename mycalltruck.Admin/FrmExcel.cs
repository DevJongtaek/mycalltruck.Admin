using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using mycalltruck.Admin.Class.Extensions;


[assembly: PermissionSetAttribute(SecurityAction.RequestOptional, Name = "FullTrust")]

namespace mycalltruck.Admin
{
    public partial class FrmExcel : Form
    {
        //Model
        List<Model> _List = new List<Model>();
        //Step
        Step[] steps = null;
        //Excel
        object[,] objValues = new object[0, 0];
        Excel.Application objApp = null;
        Excel._Workbook objBook = null;
        Excel.Sheets objSheets = null;
        Excel._Worksheet objSheet = null;
        Excel.Range range = null;
        //Val
        private string tableName = String.Empty;
        private DataGridViewColumn[] requiredFileds = null;
        string SHeader = "A1";
        string EHeader = "AZ1";
        int HeaderCount = 0;
        public FrmExcel()
        {
            InitializeComponent();
            steps = new Step[]{
                new Step{ Header="엑셀파일 선택하기", Detail="불러올 엑셀파일을 선택하십시오.", Panel=pnStep1},
                new Step{ Header="컬럼명 설정하기", Detail="엑셀파일의 컬럼명과 본 프로그램의 컬럼명을 짝지워주십시오.", Panel=pnStep2},
                new Step{ Header="엑셀파일 불러오기", Detail="선택한 엑셀파일의 정보들을 본 프로그램에 적용중입니다.", Panel=pnStep3}};
            objApp = new Microsoft.Office.Interop.Excel.Application();
            gridStep1.Rows.Add(20);
            for (int i = 0; i < 20; i++)
            {
                gridStep1[0, i].Value = i + 1;
            }
            this.tableName = "매입관리";
            DataGridViewColumn[] requiereds1 = new DataGridViewColumn[]{

                    //CustomerName,
                    CustomerBizNo,
                    //StartTime,
                    //StopTime,
                    //StartState,
                    //StartCity,
                    //StopState,
                    //StopCity,
                    //ItemSize,
                    ClientPrice,
                    Price,
                    //Etc,
                    DriverCarNo,
                    DriverName,
                    DriverBizNo,
                    DriverMobileNo,
                    DriverCarType,
                    DriverCarSize
            };
            try
            {
                _List.Add(new Model
                {
                    TargetProperty = "CustomerName",
                    ReferenceColumn = CustomerName,
                    SourceText = null,
                });
                _List.Add(new Model
                {
                    TargetProperty = "CustomerBizNo",
                    ReferenceColumn = CustomerBizNo,
                    SourceText = null,
                });
                _List.Add(new Model
                {
                    TargetProperty = "StartTime",
                    ReferenceColumn = StartTime,
                    SourceText = null,
                });
                _List.Add(new Model
                {
                    TargetProperty = "StopTime",
                    ReferenceColumn = StopTime,
                    SourceText = null,
                });
                _List.Add(new Model
                {
                    TargetProperty = "StartState",
                    ReferenceColumn = StartState,
                    SourceText = null,
                });
                _List.Add(new Model
                {
                    TargetProperty = "StartCity",
                    ReferenceColumn = StartCity,
                    SourceText = null,
                });
                _List.Add(new Model
                {
                    TargetProperty = "StopState",
                    ReferenceColumn = StopState,
                    SourceText = null,
                });
                _List.Add(new Model
                {
                    TargetProperty = "StopCity",
                    ReferenceColumn = StopCity,
                    SourceText = null,
                });
                _List.Add(new Model
                {
                    TargetProperty = "ItemSize",
                    ReferenceColumn = ItemSize,
                    SourceText = null,
                });
                _List.Add(new Model
                {
                    TargetProperty = "ClientPrice",
                    ReferenceColumn = ClientPrice,
                    SourceText = null,
                });
                _List.Add(new Model
                {
                    TargetProperty = "Price",
                    ReferenceColumn = Price,
                    SourceText = null,
                });
                _List.Add(new Model
                {
                    TargetProperty = "Etc",
                    ReferenceColumn = Etc,
                    SourceText = null,
                });
                _List.Add(new Model
                {
                    TargetProperty = "DriverCarNo",
                    ReferenceColumn = DriverCarNo,
                    SourceText = null,
                });
                _List.Add(new Model
                {
                    TargetProperty = "DriverName",
                    ReferenceColumn = DriverName,
                    SourceText = null,
                });
                _List.Add(new Model
                {
                    TargetProperty = "DriverBizNo",
                    ReferenceColumn = DriverBizNo,
                    SourceText = null,
                });
                _List.Add(new Model
                {
                    TargetProperty = "DriverMobileNo",
                    ReferenceColumn = DriverMobileNo,
                    SourceText = null,
                });
                _List.Add(new Model
                {
                    TargetProperty = "DriverCarType",
                    ReferenceColumn = DriverCarType,
                    SourceText = null,
                });
                _List.Add(new Model
                {
                    TargetProperty = "DriverCarSize",
                    ReferenceColumn = DriverCarSize,
                    SourceText = null,
                });
                this.requiredFileds = requiereds1;
            }
            catch { }
        }
        private void FrmExcel_Load(object sender, EventArgs e)
        {
            moveStep();
            //Step1
            TreeNode root = new TreeNode("내 컴퓨터",0,0);
            trvFolder.Nodes.Add(root);
            DriveInfo[] drivers = DriveInfo.GetDrives();
            foreach (DriveInfo item in drivers)
            {
                TreeNode node;
                if (item.DriveType == DriveType.Fixed)
                {
                    node = new TreeNode(String.Format("로컬 디스크({0}:)", item.Name), 1, 1);
                    node.Tag = item;
                    root.Nodes.Add(node);
                }
                else if (item.DriveType == DriveType.CDRom)
                {
                    node = new TreeNode(String.Format("CD 드라이브({0}:)", item.Name), 2, 2);
                    node.Tag = item;
                    root.Nodes.Add(node);
                }
            }
            //Step2
            lblItemHead.Text = string.Format("{0} 항목", tableName);
            gridTableItem.Rows.Add(_List.Count);
            int i = 0;
            foreach (DataGridViewColumn item in requiredFileds)
            {
                gridTableItem[0, i].Style.ForeColor = Color.Red;
                if (item.GetType() == typeof(DataGridViewComboBoxColumn))
                    gridTableItem[0, i].Value = item.HeaderText;
                else
                    gridTableItem[0, i].Value = item.HeaderText;
                i++;
            }
            requiredCount = i - 1;
            foreach (DataGridViewColumn item in _List.Where(c=>!requiredFileds.Contains(c.ReferenceColumn)).Select(c=>c.ReferenceColumn))
            {
                if (item.GetType() == typeof(DataGridViewComboBoxColumn))
                    gridTableItem[0, i].Value = item.HeaderText;
                else
                    gridTableItem[0, i].Value = item.HeaderText;
                i++;
            }
        }
        #region Navigate
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (CurrentIndex == 0) return;
            CurrentIndex--;
            moveStep();
        }
        int requiredCount = 0;
        public bool DicDone = false;
        public bool IsClose = false;
        private bool _IsVailidStep()
        {
            bool r = false;
            switch (CurrentIndex)
            {
                case 0:
                    if (lsvFile.SelectedItems.Count > 0)
                    {
                        r = true;
                    }
                    else
                        (this as Form).ThisMessageBox("엑셀 파일이 선택되지 않았습니다.");
                    break;
                case 1:
                    for (int i = 0; i < requiredCount; i++)
                    {
                        if (gridTableItem[1, i].Value == null || gridTableItem[1, i].ToString() == "")
                        {
                            (this as Form).ThisMessageBox("필수항목이 모두 대응되지 않았습니다.");
                            return false;
                        }
                    }
                    r = true;
                    break;
                case 2:
                    break;
                default:
                    break;
            }
            return r;
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (btnNext.Text == "완료")
            {
                IsClose = true;
                Close();
                return;
            }
            DicDone = false;
            if (_IsVailidStep() == false) return;
            CurrentIndex++;
            moveStep();
        }
        public int CurrentIndex = 0;
        private void moveStep()
        {
            if (CurrentIndex == 1)
            {
                SHeader = txt_Start.Text;
                EHeader = txt_End.Text;
                gridExcelItem.Rows.Clear();
                for (int i = 0; i < gridTableItem.RowCount; i++)
                {
                    gridTableItem[1, i].Value = null;
                }
                if (objSheet == null) return;
                range = objSheet.get_Range(SHeader, EHeader);
                //range = objSheet.get_Range("A1", "CZ1");
                objValues = (object[,])range.get_Value(Missing.Value);
                if (objValues.Length > 0)
                {
                    foreach (object item in objValues)
                    {
                        if (item == null) break;
                        int rowIndex = gridExcelItem.Rows.Add();
                        gridExcelItem[0, rowIndex].Value = item;
                    }
                    for (int i = 0; i < gridExcelItem.Rows.Count; i++)
                    {
                        object value = gridExcelItem[0, i].Value;
                        for (int j = 0; j < gridTableItem.Rows.Count; j++)
                        {
                            try
                            {
                                if (gridTableItem[0, j].Value.ToString() == value.ToString())
                                {
                                    gridTableItem[1, j].Value = "{" + value + "}";
                                    break;
                                }
                            }
                            catch { }
                        }
                    }
                }

            }
            else if (CurrentIndex == 2)
            {

                string[] colName = new string[]{"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O"
                    ,"P","Q","R","S","T","U","V","W","X","Y","Z","AA","AB","AC","AD","AE","AF","AG","AH","AI","AJ"
                    ,"AK","AL","AM","AN","AO","AP","AQ","AR","AS","AT","AU","AV","AW","AX","AY","AZ"};
                int Maxrows = 0;
                for (int i = 1; i < objSheet.Rows.Count; i++)
                {
                    object[,] oComp = (object[,])objSheet.get_Range(colName[0] + i.ToString(), colName[gridExcelItem.Rows.Count - 1] + i.ToString()).get_Value(Missing.Value);
                    Maxrows = i;
                    foreach (object item in oComp)
                    {
                        if (item != null)
                        {
                            Maxrows = 0;
                            break;
                        }
                    }
                    if (Maxrows != 0) break;
                }
                Maxrows--;
                for (int i = 0; i < gridTableItem.Rows.Count; i++)
                {
                    var HeaderText = gridTableItem[0, i].Value.ToString();
                    if(_List.Any(c => c.ReferenceColumn.HeaderText == gridTableItem[0, i].Value.ToString()))
                    {
                        var _Model = _List.First(c => c.ReferenceColumn.HeaderText == gridTableItem[0, i].Value.ToString());
                        if (gridTableItem[1, i].Value != null)
                        {
                            _Model.SourceText = gridTableItem[1, i].Value.ToString();
                        }
                        else
                        {
                            _Model.SourceText = null;
                        }
                    }
                    
                }
                pBar.Maximum = Maxrows - 1;
                pBar.Value = 0;
                lblProgress.Text = string.Format("{0}/{1}", pBar.Value, pBar.Maximum);
                DicDone = true;
            }

            Text = String.Format("엑셀 불러오기 마법사 {0}/3", (CurrentIndex + 1).ToString());
            lblStepHead.Text = steps[CurrentIndex].Header;
            lblStepDetail.Text = steps[CurrentIndex].Detail;
            if (CurrentIndex == 0)
                lblAlert.Text = "불러온 파일 중, 상위 20개만 샘플로 보여줍니다. !!!";
            else lblAlert.Text = "";
            pnStep1.Height = 0;
            pnStep2.Height = 0;
            pnStep3.Height = 0;
            steps[CurrentIndex].Panel.Height = 410;
            this.Activate();
        }
        int dotCount = 0;
        public void AddLblDots(Label lbl, string oriString)
        {
            if (dotCount == 5) dotCount = 0;
            else dotCount++                ;
            string str = string.Empty;
            for (int i = 1; i < dotCount; i++)
            {
                str = str + ".";
            }
            lbl.Text = oriString + str;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion
        private void trvFolder_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag == null) return;
            DirectoryInfo dir = null;
            if (e.Node.Tag.GetType() == typeof(DriveInfo))
            {
                dir = (e.Node.Tag as DriveInfo).RootDirectory;
            }
            else if (e.Node.Tag.GetType() == typeof(DirectoryInfo))
                dir = e.Node.Tag as DirectoryInfo;
            if (e.Node.Nodes.Count == 0)
            {
                addFolderNode(dir, e.Node);
            }
            lsvFile.Items.Clear();
            foreach (FileInfo item in dir.GetFiles())
            {
                if (item.Extension == ".xls")
                {
                    ListViewItem file = new ListViewItem(item.Name, 0);
                    file.Tag = item;
                    lsvFile.Items.Add(file);
                }
                else if (item.Extension == ".xlsx")
                {
                    ListViewItem file = new ListViewItem(item.Name, 1);
                    file.Tag = item;
                    lsvFile.Items.Add(file);
                }
            }
        }
        private void addFolderNode(DirectoryInfo dir, TreeNode parent)
        {
            try
            {
                foreach (DirectoryInfo sub in dir.GetDirectories())
                {
                    TreeNode node = new TreeNode(sub.Name, 3, 4);
                    node.Tag = sub;
                    parent.Nodes.Add(node);
                }
            }
            catch (System.UnauthorizedAccessException)
            { return; }
            catch (System.IO.IOException)
            { return; }
        }

        private void lsvFile_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            FileInfo file = e.Item.Tag as FileInfo;
            if (file == null) return;
            try { objBook.Close(false, Missing.Value, Missing.Value); }
            catch { }
            try
            {
                objBook = objApp.Workbooks.Open(file.FullName, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value);
                objSheets = objBook.Worksheets;
                objSheet = (Excel._Worksheet)objSheets.get_Item(1);
               
                SHeader = txt_Start.Text;
                EHeader = txt_End.Text;
                HeaderCount =Convert.ToInt32(txt_HeaderCount.Text);
                //range = objSheet.get_Range(SHeader, EHeader);
                range = objSheet.get_Range("A1", "AZ20");
                objValues = (object[,])range.get_Value(Missing.Value);
                
                for (int i = 1; i <= 20; i++)
                {
                    for (int j = 1; j < gridStep1.ColumnCount; j++)
                    {
                        gridStep1[j, i - 1].Value = objValues[i, j];
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void Nar(object o)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
            }
            catch
            { }
            finally
            {
                o = null;
            }
        }

        private void FrmExcel_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Nar(range);
                Nar(objSheet);
                Nar(objSheets);
                try
                {
                    objBook.Close(false, Missing.Value, Missing.Value);
                }
                catch
                {
                    objBook.Close(false, Missing.Value, Missing.Value);

                    Nar(objBook);
                    objApp.Quit();
                    Nar(objApp);
                }
                Nar(objBook);
                objApp.Quit();
                Nar(objApp);
            }
            catch
            {
            }
        }
        #region Step2
        private void gridExcelItem_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks > 1) return;
            if (gridExcelItem.SelectedCells.Count == 0) return;
            object value = "{" + gridExcelItem.SelectedCells[0].Value + "}";
            gridExcelItem.DoDragDrop(value, DragDropEffects.Copy);
        }
        private void gridTableItem_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(string)))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }
        private void gridTableItem_DragDrop(object sender, DragEventArgs e)
        {
            for (int i = 0; i < gridTableItem.RowCount; i++)
            {
                Rectangle rect = gridTableItem.GetRowDisplayRectangle(i, false);
                Point topLeft = gridTableItem.PointToScreen(rect.Location);
                Point bottomRight = new Point(topLeft.X + rect.Width, topLeft.Y + rect.Height);
                if (e.X >= topLeft.X && e.X <= bottomRight.X &&
                    e.Y >= topLeft.Y && e.Y <= bottomRight.Y)
                {
                    gridTableItem[1, i].Value += e.Data.GetData(typeof(string)).ToString();
                }
            }

        }
        #endregion
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (gridExcelItem.SelectedCells.Count > 0 && gridTableItem.SelectedCells.Count > 0)
            {
                gridTableItem[1, gridTableItem.SelectedCells[0].RowIndex].Value +=
                    "{" + gridExcelItem.SelectedCells[0].Value + "}";
            }
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (gridTableItem.SelectedCells.Count > 0)
            {
                gridTableItem[1, gridTableItem.SelectedCells[0].RowIndex].Value = null;
            }
        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gridExcelItem_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gridExcelItem.SelectedCells.Count > 0 && gridTableItem.SelectedCells.Count > 0)
            {
                gridTableItem[1, gridTableItem.SelectedCells[0].RowIndex].Value =
                    gridExcelItem.SelectedCells[0].Value;
            }
        }
        class Model
        {
            public String TargetProperty { get; set; }
            public DataGridViewColumn ReferenceColumn { get; set; }
            public String SourceText { get; set; }
        }
        class Step
        {
            public string Header { get; set; }
            public string Detail { get; set; }
            public Panel Panel { get; set; }
        }
    }
}
