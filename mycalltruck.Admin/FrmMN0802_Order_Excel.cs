using DynamicExpresso;
using mycalltruck.Admin.Class.Common;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace mycalltruck.Admin
{
    public partial class FrmMN0802_Order_Excel : Form
    {
        public FrmMN0802_Order_Excel()
        {
            InitializeComponent();
            SourceFileOpen.Click += SourceFileOpen_Click;
            for (int i = 0; i < 20; i++)
            {
                SourceFileDataGridView.Rows.Add();
                TargetFileDataGridView.Rows.Add();
                SourceFileDataGridView[1, i].Value = (i + 1).ToString();
                TargetFileDataGridView[0, i].Value = (i + 1).ToString();
                TargetFileDataGridView[1, i].Value = (i).ToString();
            }
            InitializeTarget();
            _Select();
        }

        private void InitializeTarget()
        {
            TargetFileDataGridView[T_ColumnA.Index, 0].Value = "IDX";
            TargetFileDataGridView[T_ColumnB.Index, 0].Value = "상호(화주)";
            TargetFileDataGridView[T_ColumnC.Index, 0].Value = "사업자번호(화주)";
            TargetFileDataGridView[T_ColumnD.Index, 0].Value = "출발일";
            TargetFileDataGridView[T_ColumnE.Index, 0].Value = "도착일";
            TargetFileDataGridView[T_ColumnF.Index, 0].Value = "출발지(시도)";
            TargetFileDataGridView[T_ColumnG.Index, 0].Value = "출발지(시군구)";
            TargetFileDataGridView[T_ColumnH.Index, 0].Value = "도착지(시도)";
            TargetFileDataGridView[T_ColumnI.Index, 0].Value = "도착지(시군구)";
            TargetFileDataGridView[T_ColumnJ.Index, 0].Value = "화물량(톤)";
            TargetFileDataGridView[T_ColumnK.Index, 0].Value = "화주운송료";
            TargetFileDataGridView[T_ColumnL.Index, 0].Value = "배차운송료";
            TargetFileDataGridView[T_ColumnM.Index, 0].Value = "특이사항";
            TargetFileDataGridView[T_ColumnN.Index, 0].Value = "차량번호";
            TargetFileDataGridView[T_ColumnO.Index, 0].Value = "기사명";
            TargetFileDataGridView[T_ColumnP.Index, 0].Value = "사업자번호(차량)";
            TargetFileDataGridView[T_ColumnQ.Index, 0].Value = "핸드폰번호";
            TargetFileDataGridView[T_ColumnR.Index, 0].Value = "차종";
            TargetFileDataGridView[T_ColumnS.Index, 0].Value = "톤수";



            Expression.Add(new Model
            {
                TargetProperty = "IDX",
                TargetFileColumnIndex = T_ColumnA.Index,
                IsReadOnly = true,
            });
            Expression.Add(new Model
            {
                TargetProperty = "상호(화주)",
                TargetFileColumnIndex = T_ColumnB.Index,
            });
            Expression.Add(new Model
            {
                TargetProperty = "사업자번호(화주)",
                TargetFileColumnIndex = T_ColumnC.Index,
                IsOnlyNumber = true,
                IsRequire = true,
            });
            Expression.Add(new Model
            {
                TargetProperty = "출발일",
                TargetFileColumnIndex = T_ColumnD.Index,
                IsOnlyNumber = true,
                IsDate = true,
            });
            Expression.Add(new Model
            {
                TargetProperty = "도착일",
                TargetFileColumnIndex = T_ColumnE.Index,
                IsOnlyNumber = true,
                IsDate = true,
            });
            Expression.Add(new Model
            {
                TargetProperty = "출발지(시도)",
                TargetFileColumnIndex = T_ColumnF.Index,
                AddressIndex = 1,
            });
            Expression.Add(new Model
            {
                TargetProperty = "출발지(시군구)",
                TargetFileColumnIndex = T_ColumnG.Index,
                AddressIndex = 2,
            });
            Expression.Add(new Model
            {
                TargetProperty = "도착지(시도)",
                TargetFileColumnIndex = T_ColumnH.Index,
                AddressIndex = 1,
            });
            Expression.Add(new Model
            {
                TargetProperty = "도착지(시군구)",
                TargetFileColumnIndex = T_ColumnI.Index,
                AddressIndex = 2,
            });
            Expression.Add(new Model
            {
                TargetProperty = "화물량(톤)",
                TargetFileColumnIndex = T_ColumnJ.Index,
            });
            Expression.Add(new Model
            {
                TargetProperty = "화주운송료",
                TargetFileColumnIndex = T_ColumnK.Index,
                IsNumber = true,
            });
            Expression.Add(new Model
            {
                TargetProperty = "배차운송료",
                TargetFileColumnIndex = T_ColumnL.Index,
                IsNumber = true,
            });
            Expression.Add(new Model
            {
                TargetProperty = "특이사항",
                TargetFileColumnIndex = T_ColumnM.Index,
            });
            Expression.Add(new Model
            {
                TargetProperty = "차량번호",
                TargetFileColumnIndex = T_ColumnN.Index,
                IsRequire = true,
            });
            Expression.Add(new Model
            {
                TargetProperty = "기사명",
                TargetFileColumnIndex = T_ColumnO.Index,
            });
            Expression.Add(new Model
            {
                TargetProperty = "사업자번호(차량)",
                TargetFileColumnIndex = T_ColumnP.Index,
                IsOnlyNumber = true,
            });
            Expression.Add(new Model
            {
                TargetProperty = "핸드폰번호",
                TargetFileColumnIndex = T_ColumnQ.Index,
                IsOnlyNumber = true,
                IsRequire = true,
            });
            Expression.Add(new Model
            {
                TargetProperty = "차종",
                TargetFileColumnIndex = T_ColumnR.Index,
                Reference = "CarType",
            });
            Expression.Add(new Model
            {
                TargetProperty = "톤수",
                TargetFileColumnIndex = T_ColumnS.Index,
                Reference = "CarSize",
            });
            Expression.Add(new Model
            {
                TargetProperty = "CHECK",
                TargetFileColumnIndex = T_ColumnT.Index,
                IsCheck = true,
            });

            foreach (var model in Expression)
            {
                int rowIndex = TargetMatchDataGridView.Rows.Add();
                TargetMatchDataGridView[ColumnTargetProperty.Index, rowIndex].Value = model.TargetProperty;
                if (model.IsReadOnly)
                {
                    TargetMatchDataGridView[ColumnTargetProperty.Index, rowIndex].ReadOnly = true;
                }
                if (model.IsRequire)
                {
                    TargetMatchDataGridView[ColumnTargetProperty.Index, rowIndex].Style.ForeColor = Color.Red;
                }
            }

            //int idxRowIndex = TargetMatchDataGridView.Rows.Add();
            //TargetMatchDataGridView[ColumnTargetProperty.Index, idxRowIndex].Value = "IDX";
            //TargetMatchDataGridView[ColumnTargetProperty.Index, idxRowIndex].ReadOnly = true;
            //TargetMatchDataGridView[ColumnTargetProperty.Index, TargetMatchDataGridView.Rows.Add()].Value = "상호(화주)";
            //TargetMatchDataGridView[ColumnTargetProperty.Index, TargetMatchDataGridView.Rows.Add()].Value = "사업자번호(화주)";
            //TargetMatchDataGridView[ColumnTargetProperty.Index, TargetMatchDataGridView.Rows.Add()].Value = "출발일";
            //TargetMatchDataGridView[ColumnTargetProperty.Index, TargetMatchDataGridView.Rows.Add()].Value = "도착일";
            //TargetMatchDataGridView[ColumnTargetProperty.Index, TargetMatchDataGridView.Rows.Add()].Value = "출발지(시도)";
            //TargetMatchDataGridView[ColumnTargetProperty.Index, TargetMatchDataGridView.Rows.Add()].Value = "출발지(시군구)";
            //TargetMatchDataGridView[ColumnTargetProperty.Index, TargetMatchDataGridView.Rows.Add()].Value = "도착지(시도)";
            //TargetMatchDataGridView[ColumnTargetProperty.Index, TargetMatchDataGridView.Rows.Add()].Value = "도착지(시군구)";
            //TargetMatchDataGridView[ColumnTargetProperty.Index, TargetMatchDataGridView.Rows.Add()].Value = "화물량(톤)";
            //TargetMatchDataGridView[ColumnTargetProperty.Index, TargetMatchDataGridView.Rows.Add()].Value = "화주운송료";
            //TargetMatchDataGridView[ColumnTargetProperty.Index, TargetMatchDataGridView.Rows.Add()].Value = "배차운송료";
            //TargetMatchDataGridView[ColumnTargetProperty.Index, TargetMatchDataGridView.Rows.Add()].Value = "특이사항";
            //TargetMatchDataGridView[ColumnTargetProperty.Index, TargetMatchDataGridView.Rows.Add()].Value = "차량번호";
            //TargetMatchDataGridView[ColumnTargetProperty.Index, TargetMatchDataGridView.Rows.Add()].Value = "기사명";
            //TargetMatchDataGridView[ColumnTargetProperty.Index, TargetMatchDataGridView.Rows.Add()].Value = "사업자번호(차량)";
            //TargetMatchDataGridView[ColumnTargetProperty.Index, TargetMatchDataGridView.Rows.Add()].Value = "핸드폰번호";
            //TargetMatchDataGridView[ColumnTargetProperty.Index, TargetMatchDataGridView.Rows.Add()].Value = "차종";
            //TargetMatchDataGridView[ColumnTargetProperty.Index, TargetMatchDataGridView.Rows.Add()].Value = "톤수";
        }

        private void _Select()
        {
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                using (SqlCommand _Command = _Connection.CreateCommand())
                {
                    _Command.CommandText = "SELECT [ExcelInfoId], [Name], [Match], [HeaderRowIndex] FROM [ExcelInfoes]";
                    using (SqlDataReader _Reader = _Command.ExecuteReader())
                    {
                        while (_Reader.Read())
                        {
                            cmbExcelInfoes.Items.Add(new ExcelInfo
                            {
                                ExcelInfoId = _Reader.GetInt32(0),
                                Name = _Reader.GetString(1),
                                Match = _Reader.GetString(2),
                                HeaderRowIndex = _Reader.GetInt32(3),
                            });
                        }
                        _Reader.Close();
                    }
                }
                _Connection.Close();
            }
        }

        private void CreateExpression()
        {
            foreach (var model in Expression)
            {
                model.TargetExpression = "";
            }
            for (int i = 0; i < TargetMatchDataGridView.RowCount; i++)
            {
                var _TargetProperty = TargetMatchDataGridView[ColumnTargetProperty.Index, i].Value;
                var _TargetExpression = TargetMatchDataGridView[ColumnTargetExpression.Index, i].Value;
                if (_TargetProperty != null && _TargetExpression != null &&
                    !String.IsNullOrWhiteSpace(_TargetProperty.ToString()) && !String.IsNullOrWhiteSpace(_TargetExpression.ToString()))
                {
                    //var _TargetFileColumnIndex = 0;
                    //switch (_TargetProperty)
                    //{
                    //    case "IDX": _TargetFileColumnIndex = T_ColumnA.Index; break;
                    //    case "상호(화주)": _TargetFileColumnIndex = T_ColumnB.Index; break;
                    //    case "사업자번호(화주)": _TargetFileColumnIndex = T_ColumnC.Index; break;
                    //    case "출발일": _TargetFileColumnIndex = T_ColumnD.Index; break;
                    //    case "도착일": _TargetFileColumnIndex = T_ColumnE.Index; break;
                    //    case "출발지(시도)": _TargetFileColumnIndex = T_ColumnF.Index; break;
                    //    case "출발지(시군구)": _TargetFileColumnIndex = T_ColumnG.Index; break;
                    //    case "도착지(시도)": _TargetFileColumnIndex = T_ColumnH.Index; break;
                    //    case "도착지(시군구)": _TargetFileColumnIndex = T_ColumnI.Index; break;
                    //    case "화물량(톤)": _TargetFileColumnIndex = T_ColumnJ.Index; break;
                    //    case "화주운송료": _TargetFileColumnIndex = T_ColumnK.Index; break;
                    //    case "배차운송료": _TargetFileColumnIndex = T_ColumnL.Index; break;
                    //    case "특이사항": _TargetFileColumnIndex = T_ColumnM.Index; break;
                    //    case "차량번호": _TargetFileColumnIndex = T_ColumnN.Index; break;
                    //    case "기사명": _TargetFileColumnIndex = T_ColumnO.Index; break;
                    //    case "사업자번호(차량)": _TargetFileColumnIndex = T_ColumnP.Index; break;
                    //    case "핸드폰번호": _TargetFileColumnIndex = T_ColumnQ.Index; break;
                    //    case "차종": _TargetFileColumnIndex = T_ColumnR.Index; break;
                    //    case "톤수": _TargetFileColumnIndex = T_ColumnS.Index; break;
                    //    default:
                    //        break;
                    //}
                    Expression.FirstOrDefault(c => c.TargetProperty == _TargetProperty.ToString()).TargetExpression = _TargetExpression.ToString();
                }
            }
        }

        private void ParseSourceExpression()
        {
            for (int i = 1; i < TargetFileDataGridView.RowCount; i++)
            {
                for (int j = 2; j < TargetFileDataGridView.ColumnCount; j++)
                {
                    TargetFileDataGridView[j, i].Value = "";
                }
            }
            foreach (var _Model in Expression.Where(c => !c.IsReadOnly))
            {
                int TargetRowIndex = 1;
                for (int i = SourceHeaderRowIndex + 1; i < 20; i++)
                {
                    var _Expression = _Model.TargetExpression;
                    _Expression = _Expression.Trim();
                    while (_Expression.IndexOf("{{") > -1)
                    {
                        var _Index = _Expression.IndexOf("{{");
                        var _EndIndex = _Expression.IndexOf("}}", _Index);
                        var _HeaderWord = _Expression.Substring(_Index, _EndIndex - _Index + 2);
                        var _SourceColumnIndex = ParseHeader(_HeaderWord);
                        var _Value = "";
                        if (_SourceColumnIndex > 1 && SourceFileDataGridView[_SourceColumnIndex, i].Value != null)
                        {
                            _Value = SourceFileDataGridView[_SourceColumnIndex, i].Value.ToString();
                        }
                        _Expression = _Replace(_Expression, _HeaderWord, _Value);
                    }
                    while (_Expression.IndexOf("[[") > -1)
                    {
                        var _Index = _Expression.IndexOf("[[");
                        var _EndIndex = _Expression.IndexOf("]]", _Index);
                        var _HeaderWord = _Expression.Substring(_Index, _EndIndex - _Index + 2);
                        var _SourceColumnIndex = ParseHeader(_HeaderWord);
                        var _Value = "";
                        if (_SourceColumnIndex > 1 && SourceFileDataGridView[_SourceColumnIndex, i].Value != null)
                        {
                            _Value = SourceFileDataGridView[_SourceColumnIndex, i].Value.ToString();
                        }
                        _Expression = _Replace(_Expression, _HeaderWord, _Value);
                    }
                    if (_Model.IsCheck)
                    {
                        if (String.IsNullOrEmpty(_Expression))
                            _Expression = (true).ToString();
                        else
                        {
                            bool _R = false;
                            if (_Expression.Contains("=") && _Expression.Split('=').Length > 1)
                            {
                                _R = _Expression.Split('=')[0].Trim() == _Expression.Split('=')[1].Trim();
                            }
                            _Expression = _R.ToString();
                        }
                    }
                    if (_Model.IsDate)
                    {
                        _Expression = _Expression.Replace("오전", DateTime.Now.ToString("yyyyMMdd"));
                        _Expression = _Expression.Replace("오후", DateTime.Now.ToString("yyyyMMdd"));
                        _Expression = _Expression.Replace("오늘", DateTime.Now.ToString("yyyyMMdd"));
                    }
                    if (_Model.IsOnlyNumber)
                    {
                        _Expression = String.Join("", _Expression.Where(c => c >= '0' && c <= '9'));
                    }
                    if (_Model.IsNumber)
                    {
                        _Expression = _Expression.Replace(",", "").Trim();
                        try
                        {
                            var _Interpreter = new Interpreter();
                            _Expression = _Interpreter.Eval(_Expression).ToString();
                        }
                        catch (Exception)
                        {
                            _Expression = "";
                        }
                    }
                    TargetFileDataGridView[_Model.TargetFileColumnIndex, TargetRowIndex].Value = _Expression;
                    TargetRowIndex++;
                }
            }
        }

        private int ParseHeader(String HeaderWord)
        {
            int _ColumnIndex = 0;
            var _HeaderText = HeaderWord.Substring(2, HeaderWord.Length - 4);
            if (HeaderWord.StartsWith("{{"))
            {
                for (int i = 2; i < SourceFileDataGridView.ColumnCount; i++)
                {
                    var _Value = SourceFileDataGridView[i, SourceHeaderRowIndex].Value;
                    if (_Value != null && _Value.ToString() == _HeaderText)
                    {
                        _ColumnIndex = i;
                        break;
                    }
                }
            }
            else if (HeaderWord.StartsWith("[["))
            {
                for (int i = 2; i < SourceFileDataGridView.ColumnCount; i++)
                {
                    var _Value = SourceFileDataGridView.Columns[i].HeaderText;
                    if (_Value != null && _Value.ToString() == _HeaderText)
                    {
                        _ColumnIndex = i;
                        break;
                    }
                }
            }
            return _ColumnIndex;
        }

        private void SourceFileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.DefaultExt = "xlsx";
            d.Filter = "엑셀 (*.xlsx)|*.xlsx";
            d.FilterIndex = 0;
            DirectoryInfo di = new DirectoryInfo(LocalUser.Instance.PersonalOption.MYCAR);
            if (di.Exists == false)
            {
                di.Create();
            }
            d.InitialDirectory = LocalUser.Instance.PersonalOption.MYCAR;
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ExcelPackage _Excel = null;
                try
                {
                    _Excel = new ExcelPackage(new System.IO.FileInfo(d.FileName));
                }
                catch (Exception)
                {
                    MessageBox.Show("엑셀 파일을 읽을 수 없습니다. 만약 동일한 엑셀이 열려있다면, 먼저 엑셀을 닫고 진행해 주시길바랍니다.", this.Text, MessageBoxButtons.OK);
                    return;
                }
                if (_Excel.Workbook.Worksheets.Count < 1)
                {
                    MessageBox.Show("엑셀 파일의 쉬트가 1개 보다 작습니다. 확인 부탁드립니다.", this.Text, MessageBoxButtons.OK);
                    return;
                }
                SourceMatchDataGridView.Rows.Clear();
                foreach (var model in Expression)
                {
                    model.TargetExpression = "";
                }
                SouceFilePath.Text = d.FileName;
                var _Sheet = _Excel.Workbook.Worksheets[1];

                for (int i = 0; i < SourceFileDataGridView.RowCount; i++)
                {
                    SourceFileDataGridView[ColumnRowCheck.Index, i].Value = false;
                    for (int j = 0; j < SourceFileDataGridView.ColumnCount - 2; j++)
                    {
                        SourceFileDataGridView[j + 2, i].Value = "";
                        if (_Sheet.Cells[i + 1, j + 1].Value != null)
                        {
                            if (_Sheet.Cells[i + 1, j + 1].Value.GetType() == typeof(DateTime))
                            {
                                SourceFileDataGridView[j + 2, i].Value = ((DateTime)_Sheet.Cells[i + 1, j + 1].Value).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else
                            {
                                SourceFileDataGridView[j + 2, i].Value = _Sheet.Cells[i + 1, j + 1].Text;
                            }
                        }
                    }
                }
                SourceFileDataGridView[ColumnRowCheck.Index, 0].Value = true;
            }
        }

        private void SourceFileDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == ColumnRowCheck.Index && (bool)SourceFileDataGridView[e.ColumnIndex, e.RowIndex].Value == true)
            {
                SourceMatchDataGridView.Rows.Clear();
                for (int i = 2; i < SourceFileDataGridView.ColumnCount; i++)
                {
                    var Value = SourceFileDataGridView[i, e.RowIndex].Value;
                    int _RowIndex = SourceMatchDataGridView.Rows.Add();
                    var _SourceIndex = SourceFileDataGridView.Columns[i].HeaderText;
                    SourceMatchDataGridView[0, _RowIndex].Value = _SourceIndex;
                    SourceMatchDataGridView[1, _RowIndex].Value = null;
                    if (Value != null && !String.IsNullOrWhiteSpace(Value.ToString()))
                    {
                        SourceMatchDataGridView[1, _RowIndex].Value = Value.ToString();
                    }
                }
                for (int i = 0; i < SourceFileDataGridView.RowCount; i++)
                {
                    if (i == e.RowIndex)
                        continue;
                    SourceFileDataGridView[ColumnRowCheck.Index, i].Value = false;
                }
                SourceHeaderRowIndex = e.RowIndex;
            }
        }

        private void SourceFileDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (SourceFileDataGridView.IsCurrentCellDirty)
            {
                SourceFileDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void SourceMatchDataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Clicks == 1 && e.RowIndex > -1 && SourceMatchDataGridView[e.ColumnIndex, e.RowIndex].Value != null)
            {
                if (e.ColumnIndex == ColumnSourceIndex.Index)
                {
                    var value = "[[" + SourceMatchDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString() + "]]";
                    SourceMatchDataGridView.DoDragDrop(value, DragDropEffects.Copy);
                }
                else if (e.ColumnIndex == ColumnSourceHeader.Index)
                {
                    var value = "{{" + SourceMatchDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString() + "}}";
                    SourceMatchDataGridView.DoDragDrop(value, DragDropEffects.Copy);
                }
            }
        }

        private void TargetMatchDataGridView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(string)))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void TargetMatchDataGridView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(string)) == null)
                return;
            for (int i = 0; i < TargetMatchDataGridView.RowCount; i++)
            {
                Rectangle rect = TargetMatchDataGridView.GetRowDisplayRectangle(i, false);
                Point topLeft = TargetMatchDataGridView.PointToScreen(rect.Location);
                Point bottomRight = new Point(topLeft.X + rect.Width, topLeft.Y + rect.Height);
                if (e.X >= topLeft.X && e.X <= bottomRight.X &&
                    e.Y >= topLeft.Y && e.Y <= bottomRight.Y)
                {
                    AppendMatch(i, e.Data.GetData(typeof(string)).ToString());
                }
            }
        }

        private void AppendMatch(int RowIndex, String Value)
        {
            try
            {
                var _TargetProperty = TargetMatchDataGridView[0, RowIndex].Value.ToString();
                if (Expression.FirstOrDefault(c => c.TargetProperty == _TargetProperty).IsReadOnly)
                {
                    return;
                }
                TargetMatchDataGridView[1, RowIndex].Value += Value;
            }
            catch (Exception)
            {
            }
        }

        private string _Replace(String _Source, String _Expression, String _Value)
        {
            var _Splited = _Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            _Source = _Source.Replace(_Expression + "[4~]", _Splited.Length > 3 ? String.Join(" ", _Splited.Skip(3)) : "");
            _Source = _Source.Replace(_Expression + "[3~]", _Splited.Length > 2 ? String.Join(" ", _Splited.Skip(2)) : "");
            _Source = _Source.Replace(_Expression + "[2~]", _Splited.Length > 1 ? String.Join(" ", _Splited.Skip(1)) : "");
            _Source = _Source.Replace(_Expression + "[1~]", _Splited.Length > 0 ? String.Join(" ", _Splited.Skip(0)) : "");
            _Source = _Source.Replace(_Expression + "[5]", _Splited.Length > 4 ? _Splited[4] : "");
            _Source = _Source.Replace(_Expression + "[4]", _Splited.Length > 3 ? _Splited[3] : "");
            _Source = _Source.Replace(_Expression + "[3]", _Splited.Length > 2 ? _Splited[2] : "");
            _Source = _Source.Replace(_Expression + "[2]", _Splited.Length > 1 ? _Splited[1] : "");
            _Source = _Source.Replace(_Expression + "[1]", _Splited.Length > 0 ? _Splited[0] : "");
            _Source = _Source.Replace(_Expression, _Value);
            return _Source;
        }

        private void TargetMatchDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == ColumnTargetExpression.Index)
            {
                CreateExpression();
                ParseSourceExpression();
                ExcelInfoSave.Enabled = Expression.Where(c => c.IsRequire).All(c => !String.IsNullOrWhiteSpace(c.TargetExpression));
            }
        }

        private void ExcelInfoSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection _Connection = new SqlConnection(Properties.Settings.Default.TruckConnectionString))
            {
                _Connection.Open();
                SqlCommand _Command = _Connection.CreateCommand();
                _Command.CommandText =
                    "INSERT INTO ExcelInfoes (Name, HeaderRowIndex, Match) VALUES (@Name, @HeaderRowIndex, @Match)";
                _Command.Parameters.AddWithValue("@Name", ExcelInfoName.Text);
                _Command.Parameters.AddWithValue("@HeaderRowIndex", SourceHeaderRowIndex);
                _Command.Parameters.AddWithValue("@Match", JsonConvert.SerializeObject(Expression));
                _Command.ExecuteNonQuery();
            }

            SourceMatchDataGridView.Rows.Clear();
            foreach (var model in Expression)
            {
                model.TargetExpression = "";
            }
            SouceFilePath.Clear();
            for (int i = 0; i < SourceFileDataGridView.RowCount; i++)
            {
                SourceFileDataGridView[ColumnRowCheck.Index, i].Value = false;
                for (int j = 0; j < SourceFileDataGridView.ColumnCount - 2; j++)
                {
                    SourceFileDataGridView[j + 2, i].Value = "";
                }
            }
            for (int i = 1; i < TargetFileDataGridView.RowCount; i++)
            {
                for (int j = 2; j < TargetFileDataGridView.ColumnCount; j++)
                {
                    TargetFileDataGridView[j, i].Value = "";
                }
            }
            for (int i = 0; i < TargetMatchDataGridView.RowCount; i++)
            {
                TargetMatchDataGridView[ColumnTargetExpression.Index, i].Value = "";
            }
            ExcelInfoName.Clear();
            MessageBox.Show("배차정보 엑셀연동이 저장되었습니다.");
        }

        List<Model> Expression = new List<Model>();
        int SourceHeaderRowIndex = 0;
        class Model
        {
            public String TargetProperty { get; set; }
            public String TargetExpression { get; set; }
            public int TargetFileColumnIndex { get; set; }
            public bool IsReadOnly { get; set; }
            public bool IsCheck { get; set; }
            public bool IsNumber { get; set; }
            public bool IsOnlyNumber { get; set; }
            public bool IsRequire { get; set; }
            public String Reference { get; set; }
            public int AddressIndex { get; set; }
            public bool IsDate { get; set; }
        }

        private void BtnReadExcelInfo_Click(object sender, EventArgs e)
        {
            if (SourceMatchDataGridView.RowCount == 0)
            {
                MessageBox.Show("먼저 외부 엑셀을 불러와주세요.");
                return;
            }

            if (cmbExcelInfoes.SelectedItem == null)
            {
                MessageBox.Show("불러올 엑셀 정보를 선택해주세요.");
                return;
            }

            ExcelInfo mExcelInfo = cmbExcelInfoes.SelectedItem as ExcelInfo;
            ExcelInfoName.Text = mExcelInfo.Name;
            if((bool)SourceFileDataGridView[ColumnRowCheck.Index,mExcelInfo.HeaderRowIndex].Value != true)
            {
                for (int i = 0; i < SourceFileDataGridView.RowCount; i++)
                {
                    SourceFileDataGridView[ColumnRowCheck.Index, i].Value = false;
                }
                SourceFileDataGridView[ColumnRowCheck.Index, mExcelInfo.HeaderRowIndex].Value = true;
                SourceFileDataGridView_CellValueChanged(null, new DataGridViewCellEventArgs(ColumnRowCheck.Index, mExcelInfo.HeaderRowIndex));
            }
            var ExpressionList = new List<ExpressionModel>(JsonConvert.DeserializeObject<ExpressionModel[]>(mExcelInfo.Match));
            for (int i = 0; i < Expression.Count; i++)
            {
                var model = Expression[i];
                TargetMatchDataGridView[ColumnTargetExpression.Index, i].Value = "";
                if (ExpressionList.Any(c => c.TargetProperty == model.TargetProperty))
                {
                    TargetMatchDataGridView[ColumnTargetExpression.Index, i].Value = ExpressionList.First(c => c.TargetProperty == model.TargetProperty).TargetExpression;
                }
            }
            CreateExpression();
            ParseSourceExpression();
            ExcelInfoSave.Enabled = Expression.Where(c => c.IsRequire).All(c => !String.IsNullOrWhiteSpace(c.TargetExpression));
        }

        class ExcelInfo
        {
            public int ExcelInfoId { get; set; }
            public string Name { get; set; }
            public string Match { get; set; }
            public int HeaderRowIndex { get; set; }
        }

        class ExpressionModel
        {
            public String TargetProperty { get; set; }
            public String TargetExpression { get; set; }
            public String TargetFileColumnIndex { get; set; }
            public bool IsReadOnly { get; set; }
            public bool IsNumber { get; set; }
            public bool IsDate { get; set; }
            public bool IsOnlyNumber { get; set; }
            public bool IsRequire { get; set; }
            public bool IsCheck { get; set; }
            public String Reference { get; set; }
            public int AddressIndex { get; set; }
        }
    }
}
