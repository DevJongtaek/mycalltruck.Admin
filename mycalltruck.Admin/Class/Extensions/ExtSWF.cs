using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using mycalltruck.Admin.Class.Common;
using mycalltruck.Admin.Class.Extensions;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using mycalltruck.Admin.DataSets;
using mycalltruck.Admin.DataSets.BaseDataSetTableAdapters;


namespace mycalltruck.Admin.Class.Extensions
{ 
  
     public static class ExtSWF
    {
      
      

        public static void ThisMessageBox(this Form value, string message)
         {
             MessageBox.Show(message, value.Text);
         }
         private static void Nar(object o)
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
         public static string GetFilterString(this ComboBox iCombo, DataGridView iGrid, string iSearch)
         {
             string rFilter = string.Empty;
             foreach (DataGridViewColumn item in iGrid.Columns)
             {
                 if (item.HeaderText == iCombo.Text)
                 {
                     if (item.GetType().Name == "DataGridViewComboBoxColumn")
                     {
                         rFilter = string.Format("{0} IN (", item.DataPropertyName);
                         DataGridViewComboBoxColumn cmbCol = item as DataGridViewComboBoxColumn;
                         string dis = cmbCol.DisplayMember;
                         string val = cmbCol.ValueMember;
                         foreach (var subItem in cmbCol.Items)
                         {
                             //System.Data.DataRowView drv = subItem as System.Data.DataRowView;
                             if (subItem.GetType().GetProperty(cmbCol.DisplayMember).GetValue(subItem, null).ToString()
                                 .Contains(iSearch))
                             {
                                 rFilter += string.Format("'{0}', ",
                                     subItem.GetType().GetProperty(cmbCol.ValueMember).GetValue(subItem, null).ToString());
                             }
                         }
                         if (rFilter.Substring(rFilter.Length - 2) == ", ") rFilter = rFilter.Substring(0, rFilter.Length - 2) + ")";
                         else rFilter = string.Format("{0} = 'NotMatched'", item.DataPropertyName);
                         break;
                     }
                     else rFilter = item.DataPropertyName + " LIKE '%" + iSearch + "%'";
                     break;
                 }
             }
             return rFilter;
         }

         public static void ExportCSVFile(DataGridView grid, string title, string fileString, bool DisplayHeader, string FolderPath)
         {
             //저장 or 열기
             bool isOpen = false;
             string existExcelFilePath = string.Empty;
             //DialogResult tResult = new TaskDialogForm().Show("엑셀 내보내기", fileString);
             //if (tResult == System.Windows.Forms.DialogResult.Yes)
             //{
             //    isOpen = true;
             //    fileString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Templates), fileString);
             //}
             //else
             //{
             //    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
             //    saveFileDialog1.DefaultExt = "csv";
             //    saveFileDialog1.FileName = fileString;
             //    if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
             //    {
             //        fileString = saveFileDialog1.FileName;
             //    }
             //    else
             //    {
             //        grid.FindForm().Invoke(new Action(() =>
             //        {
             //            MessageBox.Show("사용자에 의해 취소되었습니다.", title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
             //        }));
             //        return;
             //    }
             //}

             string tempFileString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Templates), fileString);


             string sDirPath;

             sDirPath = FolderPath;

             DirectoryInfo di = new DirectoryInfo(sDirPath);

             if (di.Exists == false)
             {

                 di.Create();

                 existExcelFilePath = FolderPath + "\\" + fileString + ".csv";

             }
             else
             {
                 existExcelFilePath = FolderPath + "\\" + fileString + ".csv";
             }


             StringBuilder CSVBuilder = new StringBuilder();
             //타이틀
           //  CSVBuilder.AppendLine(title);
             //CSVBuilder.Append(",");
            // CSVBuilder.AppendLine();
             //헤더
             DataGridViewColumn[] cols = grid.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToArray();
             if (DisplayHeader)
             {
                 foreach (var column in cols)
                 {
                     CSVBuilder.Append(column.HeaderText);
                     CSVBuilder.Append(",");
                 }
                // CSVBuilder.AppendLine();
                 CSVBuilder.AppendLine();
             }
             //데이터
             grid.Invoke(new Action(() =>
             {
                 for (int i = 0; i < grid.RowCount; i++)
                 {
                     for (int j = 0; j < cols.Length; j++)
                     {
                         object value = grid[cols[j].Index, i].Value;
                         object formmatedValue = grid[cols[j].Index, i].FormattedValue;
                         if (formmatedValue != null)
                         {
                             CSVBuilder.Append(formmatedValue.ToString().Replace(",", ""));
                         }
                         CSVBuilder.Append(",");
                     }
                     CSVBuilder.AppendLine();
                 }
             }));
             //5. 저장 혹은 열기
             try
             {
                 File.WriteAllText(existExcelFilePath, CSVBuilder.ToString(), Encoding.Default);
             }
             catch (IOException)
             {
                 try
                 {
                     String dir = Path.GetDirectoryName(fileString);
                     String fileName = Path.GetFileNameWithoutExtension(fileString) + "(1)";
                     fileString = Path.Combine(dir, fileName + ".csv");
                     File.WriteAllText(fileString, CSVBuilder.ToString(), Encoding.Default);
                 }
                 catch
                 {
                     grid.FindForm().Invoke(new Action(() =>
                     {
                         MessageBox.Show("알 수 없는 오류로 작업이 실패하였습니다..", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                     }));
                     return;
                 }
             }
             if (isOpen)
             {
                 ProcessStartInfo ExcelApp = new ProcessStartInfo("EXPLORER.EXE");
                 ExcelApp.Arguments = fileString;
                 Process.Start(ExcelApp);

             }
             else
             {
                 grid.FindForm().Invoke(new Action(() =>
                 {
                     MessageBox.Show("데이터를 엑셀로 내보내어 저장하였습니다.", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                 }));

                 System.Diagnostics.Process.Start(existExcelFilePath);
             }

         }


        public static void ExportExistExcel(this DataGridView grid, string title, string fileString, ProgressBar bar, bool barHide, byte[] iexExcel, int insertRowIndex, string FolderPath)
        {
            bool isOpen = false;

            string existExcelFilePath = string.Empty;

            string sDirPath;

            sDirPath = FolderPath;

            DirectoryInfo di = new DirectoryInfo(sDirPath);

            if (di.Exists == false)
            {

                di.Create();

                existExcelFilePath = FolderPath + "\\" + fileString + ".xls";

            }
            else
            {
                existExcelFilePath = FolderPath + "\\" + fileString + ".xls";
            }


            //기본이 되는 파일을 만든다.
            string tempFileString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Templates), fileString);
            File.WriteAllBytes(tempFileString, iexExcel);
            //변수 선언
            grid.Columns[0].Visible = false;
            DataGridViewColumn[] cols = grid.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToArray();
            //여기서 열이름을 가지는 배열을 만든다.
            string[] abc;
            {
                List<string> listABC = new List<string>();
                int iCount = 0;
                string[] _iABC = new string[] { "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                string[] _jABC = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                for (int i = 0; i < _iABC.Length; i++)
                {
                    for (int j = 0; j < _jABC.Length; j++)
                    {
                        if (iCount >= cols.Length) break;
                        listABC.Add(_iABC[i] + _jABC[j]);
                        iCount++;
                    }
                    if (iCount >= cols.Length) break;
                }
                abc = listABC.ToArray();
            }
            int rowIndex = insertRowIndex;
            Excel.Application excelApp = null;
            Excel.Workbook excelBook = null;
            Excel.Worksheet excelWorksheet = null;

            try
            {
                //bar 설정
                if (bar.InvokeRequired)
                {
                    bar.Invoke(new Action(() =>
                    {
                        bar.Value = 0;
                        bar.Maximum = grid.RowCount;
                    }));
                }
                else
                {
                    bar.Value = 0;
                    bar.Maximum = grid.RowCount;
                }
                //엑셀 열고




                excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelBook = excelApp.Workbooks.Add(tempFileString);
                excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets[1];
                excelApp.Visible = false;

                //호환성체크창 안뜨게
                excelBook.CheckCompatibility = false;

                Excel.Range range = null;
                //3.데이터
                for (int i = 0; i < grid.RowCount; i++)
                {

                    if (bar.InvokeRequired)
                    {
                        bar.Invoke(new Action(() => bar.Value++));
                    }
                    else
                    {
                        bar.Value++;
                    }

                    for (int j = 0; j < cols.Length; j++)
                    {
                        object value = grid[cols[j].Index, i].Value;
                        excelWorksheet.Cells[rowIndex, j + 1] = value;




                    }
                    rowIndex++;


                }

                if (fileString.Contains("운송"))
                {
                    string LastCol = "O" + (grid.RowCount + 2);
                    range = excelWorksheet.get_Range("a3", LastCol);

                    //Excel.AllowEditRange renges = excelWorksheet.Protection.AllowEditRanges;
                    //if (!excelWorksheet.ProtectContents)
                    //{
                    //    excelWorksheet.Unprotect(Type.Missing);

                    //    for (int i = range.Count; i >= 1; i--)
                    //    {
                    //        range[i].Delete();
                    //    }

                    //}
                    //   range.AllowEdit
                    range.Locked = false;

                    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Excel.XlBorderWeight.xlThin;


                }
                else if (fileString.Contains("위탁"))
                {
                    string LastCol = "O" + (grid.RowCount + 2);
                    range = excelWorksheet.get_Range("a3", LastCol);

                    //if (!excelWorksheet.ProtectContents)
                    //{
                    //    excelWorksheet.Unprotect(Type.Missing);

                    //    //for (int i = range.Count; i >= 1; i--)
                    //    //{
                    //    //    range[i].Delete();
                    //    //}

                    //}

                    //  excelWorksheet.EnableSelection = Microsoft.Office.Interop.Excel.XlEnableSelection.xlNoSelection;
                    range.Locked = false;

                    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Excel.XlBorderWeight.xlThin;



                }
                else if (fileString.Contains("차주몰") || fileString.Contains("운송사몰"))
                {
                    string LastCol = "R" + (grid.RowCount + 1);
                    range = excelWorksheet.get_Range("a1", LastCol);

                    //if (!excelWorksheet.ProtectContents)
                    //{
                    //    excelWorksheet.Unprotect(Type.Missing);

                    //    //for (int i = range.Count; i >= 1; i--)
                    //    //{
                    //    //    range[i].Delete();
                    //    //}

                    //}

                    //  excelWorksheet.EnableSelection = Microsoft.Office.Interop.Excel.XlEnableSelection.xlNoSelection;
                    range.Locked = false;

                    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                }
                // range.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                //5. 저장 혹은 열기
                if (isOpen)
                {
                    //  excelApp.Visible = true;
                }
                else
                {
                    //2013.10.23 오종택 수정
                    //excelBook.SaveAs(fileString, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                    //        Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    //fileStrin -> existExcelFilePath 변경
                    excelBook.SaveAs(existExcelFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                            Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);


                    MessageBox.Show("데이터를 엑셀로 내보내어 저장하였습니다.", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    Nar(excelWorksheet);
                    excelBook.Close(false, Missing.Value, Missing.Value);
                    Nar(excelBook);
                    excelApp.Quit();
                    Nar(excelApp);
                    GC.GetTotalMemory(false);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.GetTotalMemory(true);

                    Process.Start(existExcelFilePath);
                }
                //바 넣고
                if (barHide)
                {
                    if (bar.InvokeRequired)
                        bar.Invoke(new Action(() => bar.Visible = false));
                    else
                        bar.Visible = false;


                }
            }
            catch (Exception E)
            {
                //MessageBox.Show("엑셀 내보내기에 실패하였습니다.\n" + E.Message, "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show("엑셀 내보내기에 실패하였습니다.", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                try
                {
                    Nar(excelWorksheet);
                    excelBook.Close(false, Missing.Value, Missing.Value);
                    Nar(excelBook);
                    excelApp.Quit();
                    Nar(excelApp);
                    GC.GetTotalMemory(false);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.GetTotalMemory(true);
                }
                catch (Exception)
                {
                }


                //바 넣고
                if (barHide)
                {
                    if (bar.InvokeRequired)
                        bar.Invoke(new Action(() => bar.Visible = false));
                    else
                        bar.Visible = false;
                }
            }
        }

         public static void ExportExistExcel3(this DataGridView grid, string title, string fileString, ProgressBar bar, bool barHide, byte[] iexExcel, int insertRowIndex, string FolderPath)
         {
             bool isOpen = false;
             //  DialogResult tResult = new TaskDialogForm().Show("엑셀 내보내기", fileString);
             string existExcelFilePath = string.Empty;
             //if (tResult == System.Windows.Forms.DialogResult.Yes)
             //{
             //    isOpen = true;
             //    existExcelFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Templates), DateTime.Now.ToString("yyyyMMddHHmmss"));
             //   // existExcelFilePath = Path.Combine(FolderPath, DateTime.Now.ToString("yyyyMMddHHmmss"));
             //}
             //else if (tResult == System.Windows.Forms.DialogResult.Cancel)
             //{
             //        MessageBox.Show("사용자에 의해 취소되었습니다.", title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
             //      return;
             //}
             //else
             //{

             //    string sDirPath;

             //    sDirPath = FolderPath;

             //    DirectoryInfo di = new DirectoryInfo(sDirPath);

             //    if (di.Exists == false)
             //    {

             //        di.Create();

             //        existExcelFilePath = FolderPath + "\\" + fileString + ".xls";

             //    }
             //    else
             //    {
             //        existExcelFilePath = FolderPath + "\\" + fileString + ".xls";
             //    }
             //}

             string sDirPath;

             sDirPath = FolderPath;

             DirectoryInfo di = new DirectoryInfo(sDirPath);

             if (di.Exists == false)
             {

                 di.Create();

                 existExcelFilePath = FolderPath + "\\" + fileString + ".xls";

             }
             else
             {
                 existExcelFilePath = FolderPath + "\\" + fileString + ".xls";
             }


             //기본이 되는 파일을 만든다.
             string tempFileString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Templates), fileString);
             File.WriteAllBytes(tempFileString, iexExcel);
             //변수 선언
             DataGridViewColumn[] cols = grid.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToArray();
             //여기서 열이름을 가지는 배열을 만든다.
             string[] abc;
             {
                 List<string> listABC = new List<string>();
                 int iCount = 0;
                 string[] _iABC = new string[] { "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                 string[] _jABC = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                 for (int i = 0; i < _iABC.Length; i++)
                 {
                     for (int j = 0; j < _jABC.Length; j++)
                     {
                         if (iCount >= cols.Length) break;
                         listABC.Add(_iABC[i] + _jABC[j]);
                         iCount++;
                     }
                     if (iCount >= cols.Length) break;
                 }
                 abc = listABC.ToArray();
             }
             int rowIndex = insertRowIndex;
             Excel.Application excelApp = null;
             Excel.Workbook excelBook = null;
             Excel.Worksheet excelWorksheet = null;
             try
             {
                 //bar 설정
                 if (bar.InvokeRequired)
                 {
                     bar.Invoke(new Action(() =>
                     {
                         bar.Value = 0;
                         bar.Maximum = grid.RowCount;
                     }));
                 }
                 else
                 {
                     bar.Value = 0;
                     bar.Maximum = grid.RowCount;
                 }
                 //엑셀 열고
                 excelApp = new Microsoft.Office.Interop.Excel.Application();
                 excelBook = excelApp.Workbooks.Add(tempFileString);
                 excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets[1];
                 excelApp.Visible = false;
                 Excel.Range range = null;
                 //3.데이터
                 for (int i = 0; i < grid.RowCount; i++)
                 {

                     if (bar.InvokeRequired)
                     {
                         bar.Invoke(new Action(() => bar.Value++));
                     }
                     else
                     {
                         bar.Value++;
                     }

                     for (int j = 0; j < cols.Length; j++)
                     {
                         object value = grid[cols[j].Index, i].Value;
                         excelWorksheet.Cells[rowIndex, j + 1] = value;




                     }
                     rowIndex++;


                 }

                 if (fileString.Contains("운송"))
                 {
                     string LastCol = "z" + (grid.RowCount + 2);
                     range = excelWorksheet.get_Range("a3", LastCol);
                     range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                     range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                 }
                 else if (fileString.Contains("위탁"))
                 {
                     string LastCol = "AE" + (grid.RowCount + 2);
                     range = excelWorksheet.get_Range("a3", LastCol);
                     range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                     range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                 }
                 // range.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                 //5. 저장 혹은 열기
                 if (isOpen)
                 {
                     //  excelApp.Visible = true;
                 }
                 else
                 {
                     //2013.10.23 오종택 수정
                     //excelBook.SaveAs(fileString, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                     //        Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                     //fileStrin -> existExcelFilePath 변경
                     excelBook.SaveAs(existExcelFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                             Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);


                     MessageBox.Show("데이터를 엑셀로 내보내어 저장하였습니다.", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                     Nar(excelWorksheet);
                     excelBook.Close(false, Missing.Value, Missing.Value);
                     Nar(excelBook);
                     excelApp.Quit();
                     Nar(excelApp);
                     GC.GetTotalMemory(false);
                     GC.Collect();
                     GC.WaitForPendingFinalizers();
                     GC.Collect();
                     GC.GetTotalMemory(true);


                 }
                 //바 넣고
                 if (barHide)
                 {
                     if (bar.InvokeRequired)
                         bar.Invoke(new Action(() => bar.Visible = false));
                     else
                         bar.Visible = false;


                 }
             }
             catch (Exception E)
             {
                 //MessageBox.Show("엑셀 내보내기에 실패하였습니다.\n" + E.Message, "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                 MessageBox.Show("엑셀 내보내기에 실패하였습니다.", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                 Nar(excelWorksheet);
                 excelBook.Close(false, Missing.Value, Missing.Value);
                 Nar(excelBook);
                 excelApp.Quit();
                 Nar(excelApp);
                 GC.GetTotalMemory(false);
                 GC.Collect();
                 GC.WaitForPendingFinalizers();
                 GC.Collect();
                 GC.GetTotalMemory(true);

                 //바 넣고
                 if (barHide)
                 {
                     if (bar.InvokeRequired)
                         bar.Invoke(new Action(() => bar.Visible = false));
                     else
                         bar.Visible = false;
                 }
             }
         }
         public static void ExportExistExcel2(this DataGridView grid, string title, string fileString, ProgressBar bar, bool barHide, byte[] iexExcel, int insertRowIndex, string FolderPath)
         {
             bool isOpen = false;
             //  DialogResult tResult = new TaskDialogForm().Show("엑셀 내보내기", fileString);
             string existExcelFilePath = string.Empty;
           

             string sDirPath;

             sDirPath = FolderPath;

             DirectoryInfo di = new DirectoryInfo(sDirPath);

             if (di.Exists == false)
             {

                 di.Create();

                 existExcelFilePath = FolderPath + "\\" + fileString + ".xls";

             }
             else
             {
                 existExcelFilePath = FolderPath + "\\" + fileString + ".xls";
             }


             //기본이 되는 파일을 만든다.
             string tempFileString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Templates), fileString);
             File.WriteAllBytes(tempFileString, iexExcel);
             //변수 선언
             DataGridViewColumn[] cols = grid.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToArray();
             //여기서 열이름을 가지는 배열을 만든다.
             string[] abc;
             {
                 List<string> listABC = new List<string>();
                 int iCount = 0;
                 string[] _iABC = new string[] { "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                 string[] _jABC = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                 for (int i = 0; i < _iABC.Length; i++)
                 {
                     for (int j = 0; j < _jABC.Length; j++)
                     {
                         if (iCount >= cols.Length) break;
                         listABC.Add(_iABC[i] + _jABC[j]);
                         iCount++;
                     }
                     if (iCount >= cols.Length) break;
                 }
                 abc = listABC.ToArray();
             }
             int rowIndex = insertRowIndex;
             Excel.Application excelApp = null;
             Excel.Workbook excelBook = null;
             Excel.Worksheet excelWorksheet = null;
             try
             {
                 //bar 설정
                 if (bar.InvokeRequired)
                 {
                     bar.Invoke(new Action(() =>
                     {
                         bar.Value = 0;
                         bar.Maximum = grid.RowCount;
                     }));
                 }
                 else
                 {
                     bar.Value = 0;
                     bar.Maximum = grid.RowCount;
                 }
                 //엑셀 열고
                 excelApp = new Microsoft.Office.Interop.Excel.Application();
                 excelBook = excelApp.Workbooks.Add(tempFileString);
                 excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets[1];
                 excelApp.Visible = false;

                 //호환성체크창 안뜨게
                 excelBook.CheckCompatibility = false;


                 Excel.Range range = null;
                 //3.데이터
                 for (int i = 0; i < grid.RowCount; i++)
                 {

                     if (bar.InvokeRequired)
                     {
                         bar.Invoke(new Action(() => bar.Value++));
                     }
                     else
                     {
                         bar.Value++;
                     }

                     for (int j = 0; j < cols.Length; j++)
                     {
                         if (grid[cols[j].Index, i].Value == null)
                         {
                             object value = grid[cols[j].Index, i].Value;


                             excelWorksheet.Cells[rowIndex, j + 1] = value;
                         }
                         else
                         {
                             object value = grid[cols[j].Index, i].Value.ToString();


                             excelWorksheet.Cells[rowIndex, j + 1] = value;
                         }




                     }
                     rowIndex++;


                 }

                 if (fileString.Contains("배차"))
                 {
                     string LastCol = "z" + (grid.RowCount + 2);
                     range = excelWorksheet.get_Range("a3", LastCol);
                     range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                     range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                 }
                 else if (fileString.Contains("위탁"))
                 {
                     string LastCol = "AE" + (grid.RowCount + 2);
                     range = excelWorksheet.get_Range("a3", LastCol);
                     range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                     range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                 }
                 else if (fileString.Contains("차주몰") || fileString.Contains("운송사몰"))
                 {
                     string LastCol = "R" + (grid.RowCount + 1);
                     range = excelWorksheet.get_Range("a1", LastCol);

                     
                     range.Locked = false;

                     range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                     range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                 }
                 // range.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                 //5. 저장 혹은 열기
                 if (isOpen)
                 {
                     //  excelApp.Visible = true;
                 }
                 else
                 {
                   
                     excelBook.SaveAs(existExcelFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                             Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);


                     MessageBox.Show("데이터를 엑셀로 내보내어 저장하였습니다.", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                     Nar(excelWorksheet);
                     excelBook.Close(false, Missing.Value, Missing.Value);
                     Nar(excelBook);
                     excelApp.Quit();
                     Nar(excelApp);
                     GC.GetTotalMemory(false);
                     GC.Collect();
                     GC.WaitForPendingFinalizers();
                     GC.Collect();
                     GC.GetTotalMemory(true);

                     System.Diagnostics.Process.Start(existExcelFilePath);

                 }
                 //바 넣고
                 if (barHide)
                 {
                     if (bar.InvokeRequired)
                         bar.Invoke(new Action(() => bar.Visible = false));
                     else
                         bar.Visible = false;


                 }
             }
             catch (Exception E)
             {
                 //MessageBox.Show("엑셀 내보내기에 실패하였습니다.\n" + E.Message, "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                 MessageBox.Show("엑셀 내보내기에 실패하였습니다.", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                 Nar(excelWorksheet);
                 excelBook.Close(false, Missing.Value, Missing.Value);
                 Nar(excelBook);
                 excelApp.Quit();
                 Nar(excelApp);
                 GC.GetTotalMemory(false);
                 GC.Collect();
                 GC.WaitForPendingFinalizers();
                 GC.Collect();
                 GC.GetTotalMemory(true);

                 //바 넣고
                 if (barHide)
                 {
                     if (bar.InvokeRequired)
                         bar.Invoke(new Action(() => bar.Visible = false));
                     else
                         bar.Visible = false;


                 }
             }
         }

        public static void ExportExistExcelBank(this DataGridView grid, string title, string fileString, ProgressBar bar, bool barHide, byte[] iexExcel, int insertRowIndex, string FolderPath)
        {
            bool isOpen = false;
            //  DialogResult tResult = new TaskDialogForm().Show("엑셀 내보내기", fileString);
            string existExcelFilePath = string.Empty;


            string sDirPath;

            sDirPath = FolderPath;

            DirectoryInfo di = new DirectoryInfo(sDirPath);

            if (di.Exists == false)
            {

                di.Create();

                existExcelFilePath = FolderPath + "\\" + fileString + ".xls";

            }
            else
            {
                existExcelFilePath = FolderPath + "\\" + fileString + ".xls";
            }


            //기본이 되는 파일을 만든다.
            string tempFileString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Templates), fileString);
            File.WriteAllBytes(tempFileString, iexExcel);
            //변수 선언
            DataGridViewColumn[] cols = grid.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToArray();
            //여기서 열이름을 가지는 배열을 만든다.
            string[] abc;
            {
                List<string> listABC = new List<string>();
                int iCount = 0;
                string[] _iABC = new string[] { "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                string[] _jABC = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                for (int i = 0; i < _iABC.Length; i++)
                {
                    for (int j = 0; j < _jABC.Length; j++)
                    {
                        if (iCount >= cols.Length) break;
                        listABC.Add(_iABC[i] + _jABC[j]);
                        iCount++;
                    }
                    if (iCount >= cols.Length) break;
                }
                abc = listABC.ToArray();
            }
            int rowIndex = insertRowIndex;
            Excel.Application excelApp = null;
            Excel.Workbook excelBook = null;
            Excel.Worksheet excelWorksheet = null;
            try
            {
                //bar 설정
                if (bar.InvokeRequired)
                {
                    bar.Invoke(new Action(() =>
                    {
                        bar.Value = 0;
                        bar.Maximum = grid.RowCount;
                    }));
                }
                else
                {
                    bar.Value = 0;
                    bar.Maximum = grid.RowCount;
                }
                //엑셀 열고
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelBook = excelApp.Workbooks.Add(tempFileString);
                excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets[1];
                excelApp.Visible = false;

                //호환성체크창 안뜨게
                excelBook.CheckCompatibility = false;


                Excel.Range range = null;
                //3.데이터
                for (int i = 0; i < grid.RowCount; i++)
                {

                    if (bar.InvokeRequired)
                    {
                        bar.Invoke(new Action(() => bar.Value++));
                    }
                    else
                    {
                        bar.Value++;
                    }

                    for (int j = 0; j < cols.Length; j++)
                    {
                        if (grid[cols[j].Index, i].Value == null)
                        {
                            object value = grid[cols[j].Index, i].Value;


                            excelWorksheet.Cells[rowIndex, j + 1] = value;
                        }
                        else
                        {
                            object value = grid[cols[j].Index, i].Value.ToString().Replace(",", "");


                            excelWorksheet.Cells[rowIndex, j + 1] = value;
                        }




                    }
                    rowIndex++;


                }

                if (fileString.Contains("배차"))
                {
                    string LastCol = "z" + (grid.RowCount + 2);
                    range = excelWorksheet.get_Range("a3", LastCol);
                    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                }
                else if (fileString.Contains("위탁"))
                {
                    string LastCol = "AE" + (grid.RowCount + 2);
                    range = excelWorksheet.get_Range("a3", LastCol);
                    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                }
                else if (fileString.Contains("차주몰") || fileString.Contains("운송사몰"))
                {
                    string LastCol = "R" + (grid.RowCount + 1);
                    range = excelWorksheet.get_Range("a1", LastCol);


                    range.Locked = false;

                    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                }
                // range.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                //5. 저장 혹은 열기
                if (isOpen)
                {
                    //  excelApp.Visible = true;
                }
                else
                {
                    excelApp.DisplayAlerts = false;
                    excelBook.SaveAs(existExcelFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                            Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    //excelBook.Save();

                   // MessageBox.Show("데이터를 엑셀로 내보내어 저장하였습니다.", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    Nar(excelWorksheet);
                    excelBook.Close(false, Missing.Value, Missing.Value);
                    Nar(excelBook);
                    excelApp.Quit();
                    Nar(excelApp);
                    GC.GetTotalMemory(false);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.GetTotalMemory(true);

                   // System.Diagnostics.Process.Start(existExcelFilePath);

                }
                //바 넣고
                if (barHide)
                {
                    if (bar.InvokeRequired)
                        bar.Invoke(new Action(() => bar.Visible = false));
                    else
                        bar.Visible = false;


                }
            }
            catch (Exception E)
            {
                //MessageBox.Show("엑셀 내보내기에 실패하였습니다.\n" + E.Message, "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show("엑셀 내보내기에 실패하였습니다.", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Nar(excelWorksheet);
                excelBook.Close(false, Missing.Value, Missing.Value);
                Nar(excelBook);
                excelApp.Quit();
                Nar(excelApp);
                GC.GetTotalMemory(false);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.GetTotalMemory(true);

                //바 넣고
                if (barHide)
                {
                    if (bar.InvokeRequired)
                        bar.Invoke(new Action(() => bar.Visible = false));
                    else
                        bar.Visible = false;


                }
            }
        }


        public static void ExportExistExcelBank2(this DataGridView grid, DataGridView grid2, string title, string fileString, ProgressBar bar, bool barHide, byte[] iexExcel, int insertRowIndex, int insertRowIndex2, string FolderPath)
        {
            bool isOpen = false;
            //  DialogResult tResult = new TaskDialogForm().Show("엑셀 내보내기", fileString);
            string existExcelFilePath = string.Empty;


            string sDirPath;

            sDirPath = FolderPath;

            DirectoryInfo di = new DirectoryInfo(sDirPath);

            if (di.Exists == false)
            {

                di.Create();

                existExcelFilePath = FolderPath + "\\" + fileString + ".xls";

            }
            else
            {
                existExcelFilePath = FolderPath + "\\" + fileString + ".xls";
            }


            //기본이 되는 파일을 만든다.
            string tempFileString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Templates), fileString);
            File.WriteAllBytes(tempFileString, iexExcel);
            //변수 선언
            DataGridViewColumn[] cols = grid.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToArray();
            DataGridViewColumn[] cols2 = grid2.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToArray();
            //여기서 열이름을 가지는 배열을 만든다.
            string[] abc;
            {
                List<string> listABC = new List<string>();
                int iCount = 0;
                string[] _iABC = new string[] { "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                string[] _jABC = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                for (int i = 0; i < _iABC.Length; i++)
                {
                    for (int j = 0; j < _jABC.Length; j++)
                    {
                        if (iCount >= cols.Length) break;
                        listABC.Add(_iABC[i] + _jABC[j]);
                        iCount++;
                    }
                    if (iCount >= cols.Length) break;
                }
                abc = listABC.ToArray();
            }

            string[] abc2;
            {
                List<string> listABC2 = new List<string>();
                int iCount2 = 0;
                string[] _iABC2 = new string[] { "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                string[] _jABC2 = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                for (int i = 0; i < _iABC2.Length; i++)
                {
                    for (int j = 0; j < _jABC2.Length; j++)
                    {
                        if (iCount2 >= cols2.Length) break;
                        listABC2.Add(_iABC2[i] + _jABC2[j]);
                        iCount2++;
                    }
                    if (iCount2 >= cols2.Length) break;
                }
                abc2 = listABC2.ToArray();
            }


            int rowIndex = insertRowIndex;
            int rowIndex2 = insertRowIndex2;
            int rowIndex3 = insertRowIndex2 - 2;
            Excel.Application excelApp = null;
            Excel.Workbook excelBook = null;
            Excel.Worksheet excelWorksheet = null;
            Excel.Worksheet excelWorksheet2 = null;
            try
            {
                //bar 설정
                if (bar.InvokeRequired)
                {
                    bar.Invoke(new Action(() =>
                    {
                        bar.Value = 0;
                        bar.Maximum = grid2.RowCount;
                    }));
                }
                else
                {
                    bar.Value = 0;
                    bar.Maximum = grid2.RowCount;
                }
                //엑셀 열고
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelBook = excelApp.Workbooks.Add(tempFileString);
                excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets[1];
                excelWorksheet2 = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets[2];
                excelApp.Visible = false;

                //호환성체크창 안뜨게
                excelBook.CheckCompatibility = false;


                Excel.Range range = null;
                //3.데이터
                for (int i = 0; i < grid.RowCount; i++)
                {

                    //if (bar.InvokeRequired)
                    //{
                    //    bar.Invoke(new Action(() => bar.Value++));
                    //}
                    //else
                    //{
                    //    bar.Value++;
                    //}

                    for (int j = 0; j < cols.Length; j++)
                    {
                        if (grid[cols[j].Index, i].Value == null)
                        {
                            object value = grid[cols[j].Index, i].Value;


                            excelWorksheet.Cells[rowIndex, j + 1] = value;
                        }
                        else
                        {
                            object value = grid[cols[j].Index, i].Value.ToString().Replace(",", "");


                            excelWorksheet.Cells[rowIndex, j + 1] = value;
                        }




                    }
                    rowIndex++;


                }

                excelWorksheet2.Cells[rowIndex3, 1] = DateTime.Now.ToString("yyyy-MM-dd");
                for (int i = 0; i < grid2.RowCount ; i++)
                {

                    if (bar.InvokeRequired)
                    {
                        bar.Invoke(new Action(() => bar.Value++));
                    }
                    else
                    {
                        bar.Value++;
                    }
                  

                    for (int j = 0; j < cols2.Length; j++)
                    {
                        if (grid2[cols2[j].Index, i].Value == null)
                        {
                            object value = grid2[cols2[j].Index, i].Value;


                            excelWorksheet2.Cells[rowIndex2, j + 1] = value;
                        }
                        
                        else
                        {
                            object value = grid2[cols2[j].Index, i].Value.ToString().Replace(",", "");

                            

                            excelWorksheet2.Cells[rowIndex2, j + 1] = value;
                        }




                    }
                    rowIndex2++;


                }

               


                string LastCol = "L" + (grid2.RowCount + 7);
                range = excelWorksheet2.get_Range("a7", LastCol);
                range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                range.Borders.Weight = Excel.XlBorderWeight.xlThin;


          


                //if (fileString.Contains("배차"))
                //{
                //    string LastCol = "z" + (grid.RowCount + 2);
                //    range = excelWorksheet.get_Range("a3", LastCol);
                //    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                //    range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                //}
                //else if (fileString.Contains("위탁"))
                //{
                //    string LastCol = "AE" + (grid.RowCount + 2);
                //    range = excelWorksheet.get_Range("a3", LastCol);
                //    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                //    range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                //}
                //else if (fileString.Contains("차주몰") || fileString.Contains("운송사몰"))
                //{
                //    string LastCol = "R" + (grid.RowCount + 1);
                //    range = excelWorksheet.get_Range("a1", LastCol);


                //    range.Locked = false;

                //    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                //    range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                //}
                // range.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                //5. 저장 혹은 열기
                if (isOpen)
                {
                    //  excelApp.Visible = true;
                }
                else
                {
                    excelApp.DisplayAlerts = false;
                    excelBook.SaveAs(existExcelFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                            Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    //excelBook.Save();

                    // MessageBox.Show("데이터를 엑셀로 내보내어 저장하였습니다.", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    Nar(excelWorksheet);
                    excelBook.Close(false, Missing.Value, Missing.Value);
                    Nar(excelBook);
                    excelApp.Quit();
                    Nar(excelApp);
                    GC.GetTotalMemory(false);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.GetTotalMemory(true);

                    System.Diagnostics.Process.Start(existExcelFilePath);

                }
                //바 넣고
                if (barHide)
                {
                    if (bar.InvokeRequired)
                        bar.Invoke(new Action(() => bar.Visible = false));
                    else
                        bar.Visible = false;


                }
            }
            catch (Exception E)
            {
                //MessageBox.Show("엑셀 내보내기에 실패하였습니다.\n" + E.Message, "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show("엑셀 내보내기에 실패하였습니다.", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Nar(excelWorksheet);
                excelBook.Close(false, Missing.Value, Missing.Value);
                Nar(excelBook);
                excelApp.Quit();
                Nar(excelApp);
                GC.GetTotalMemory(false);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.GetTotalMemory(true);

                //바 넣고
                if (barHide)
                {
                    if (bar.InvokeRequired)
                        bar.Invoke(new Action(() => bar.Visible = false));
                    else
                        bar.Visible = false;


                }
            }
        }

        public static void ExportExistExcel2_xlsx(this DataGridView grid, string title, string fileString, ProgressBar bar, bool barHide, byte[] iexExcel, int insertRowIndex, string FolderPath)
        {
            bool isOpen = false;
            //  DialogResult tResult = new TaskDialogForm().Show("엑셀 내보내기", fileString);
            string existExcelFilePath = string.Empty;


            string sDirPath;

            sDirPath = FolderPath;

            DirectoryInfo di = new DirectoryInfo(sDirPath);

            if (di.Exists == false)
            {

                di.Create();

                existExcelFilePath = FolderPath + "\\" + fileString + ".xlsx";

            }
            else
            {
                existExcelFilePath = FolderPath + "\\" + fileString + ".xlsx";
            }


            //기본이 되는 파일을 만든다.
            string tempFileString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Templates), fileString);
            File.WriteAllBytes(tempFileString, iexExcel);
            //변수 선언
            DataGridViewColumn[] cols = grid.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToArray();
            //여기서 열이름을 가지는 배열을 만든다.
            string[] abc;
            {
                List<string> listABC = new List<string>();
                int iCount = 0;
                string[] _iABC = new string[] { "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                string[] _jABC = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                for (int i = 0; i < _iABC.Length; i++)
                {
                    for (int j = 0; j < _jABC.Length; j++)
                    {
                        if (iCount >= cols.Length) break;
                        listABC.Add(_iABC[i] + _jABC[j]);
                        iCount++;
                    }
                    if (iCount >= cols.Length) break;
                }
                abc = listABC.ToArray();
            }
            int rowIndex = insertRowIndex;
            Excel.Application excelApp = null;
            Excel.Workbook excelBook = null;
            Excel.Worksheet excelWorksheet = null;
            try
            {
                //bar 설정
                if (bar.InvokeRequired)
                {
                    bar.Invoke(new Action(() =>
                    {
                        bar.Value = 0;
                        bar.Maximum = grid.RowCount;
                    }));
                }
                else
                {
                    bar.Value = 0;
                    bar.Maximum = grid.RowCount;
                }
                //엑셀 열고
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelBook = excelApp.Workbooks.Add(tempFileString);
                excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets[1];
                excelApp.Visible = false;

                //호환성체크창 안뜨게
                excelBook.CheckCompatibility = false;


                Excel.Range range = null;
                //3.데이터
                for (int i = 0; i < grid.RowCount; i++)
                {

                    if (bar.InvokeRequired)
                    {
                        bar.Invoke(new Action(() => bar.Value++));
                    }
                    else
                    {
                        bar.Value++;
                    }

                    for (int j = 0; j < cols.Length; j++)
                    {
                        if (grid[cols[j].Index, i].Value == null)
                        {
                            object value = grid[cols[j].Index, i].Value;


                            excelWorksheet.Cells[rowIndex, j + 1] = value;
                        }
                        else
                        {
                            object value = grid[cols[j].Index, i].Value.ToString();


                            excelWorksheet.Cells[rowIndex, j + 1] = value;
                        }




                    }
                    rowIndex++;


                }

                if (fileString.Contains("배차"))
                {
                    string LastCol = "z" + (grid.RowCount + 2);
                    range = excelWorksheet.get_Range("a3", LastCol);
                    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                }
                else if (fileString.Contains("위탁"))
                {
                    string LastCol = "AE" + (grid.RowCount + 2);
                    range = excelWorksheet.get_Range("a3", LastCol);
                    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                }
                else if (fileString.Contains("차주몰") || fileString.Contains("운송사몰"))
                {
                    string LastCol = "R" + (grid.RowCount + 1);
                    range = excelWorksheet.get_Range("a1", LastCol);


                    range.Locked = false;

                    range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                }
                // range.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                //5. 저장 혹은 열기
                if (isOpen)
                {
                    //  excelApp.Visible = true;
                }
                else
                {

                    excelBook.SaveAs(existExcelFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                            Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);


                    MessageBox.Show("데이터를 엑셀로 내보내어 저장하였습니다.", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    Nar(excelWorksheet);
                    excelBook.Close(false, Missing.Value, Missing.Value);
                    Nar(excelBook);
                    excelApp.Quit();
                    Nar(excelApp);
                    GC.GetTotalMemory(false);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.GetTotalMemory(true);

                    System.Diagnostics.Process.Start(existExcelFilePath);

                }
                //바 넣고
                if (barHide)
                {
                    if (bar.InvokeRequired)
                        bar.Invoke(new Action(() => bar.Visible = false));
                    else
                        bar.Visible = false;


                }
            }
            catch (Exception E)
            {
                //MessageBox.Show("엑셀 내보내기에 실패하였습니다.\n" + E.Message, "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show("엑셀 내보내기에 실패하였습니다.", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Nar(excelWorksheet);
                excelBook.Close(false, Missing.Value, Missing.Value);
                Nar(excelBook);
                excelApp.Quit();
                Nar(excelApp);
                GC.GetTotalMemory(false);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.GetTotalMemory(true);

                //바 넣고
                if (barHide)
                {
                    if (bar.InvokeRequired)
                        bar.Invoke(new Action(() => bar.Visible = false));
                    else
                        bar.Visible = false;


                }
            }
        }


        public static void ExportExistExcel4(this DataGridView grid, string title, string fileString, ProgressBar bar, bool barHide, byte[] iexExcel, int insertRowIndex, string FolderPath)
         {
           

             bool isOpen = false;
             //  DialogResult tResult = new TaskDialogForm().Show("엑셀 내보내기", fileString);
             string existExcelFilePath = string.Empty;
           

             string sDirPath;

             sDirPath = FolderPath;

             DirectoryInfo di = new DirectoryInfo(sDirPath);

             if (di.Exists == false)
             {

                 di.Create();

                 existExcelFilePath = FolderPath + "\\" + fileString + ".xls";

             }
             else
             {
                 existExcelFilePath = FolderPath + "\\" + fileString + ".xls";
             }


             //기본이 되는 파일을 만든다.
             string tempFileString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Templates), fileString);
             File.WriteAllBytes(tempFileString, iexExcel);
             //변수 선언
             DataGridViewColumn[] cols = grid.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToArray();
             //여기서 열이름을 가지는 배열을 만든다.
             string[] abc;
             {
                 List<string> listABC = new List<string>();
                 int iCount = 0;
                 string[] _iABC = new string[] { "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                 string[] _jABC = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                 for (int i = 0; i < _iABC.Length; i++)
                 {
                     for (int j = 0; j < _jABC.Length; j++)
                     {
                         if (iCount >= cols.Length) break;
                         listABC.Add(_iABC[i] + _jABC[j]);
                         iCount++;
                     }
                     if (iCount >= cols.Length) break;
                 }
                 abc = listABC.ToArray();
             }
             int rowIndex = insertRowIndex;
             Excel.Application excelApp = null;
             Excel.Workbook excelBook = null;
             Excel.Worksheet excelWorksheet = null;
             try
             {
                 //bar 설정
                 if (bar.InvokeRequired)
                 {
                     bar.Invoke(new Action(() =>
                     {
                         bar.Value = 0;
                         bar.Maximum = grid.RowCount;
                     }));
                 }
                 else
                 {
                     bar.Value = 0;
                     bar.Maximum = grid.RowCount;
                 }
                 //엑셀 열고
                 excelApp = new Microsoft.Office.Interop.Excel.Application();
                 excelBook = excelApp.Workbooks.Add(tempFileString);
                 excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets[1];
                 excelApp.Visible = false;

                 //호환성체크창 안뜨게
                 excelBook.CheckCompatibility = false;


                 Excel.Range range = null;
                 //3.데이터

                 DESCrypt m_crypt = null;


                 m_crypt = new DESCrypt("12345678");


                 for (int i = 0; i < grid.RowCount; i++)
                 {

                     if (bar.InvokeRequired)
                     {
                         bar.Invoke(new Action(() => bar.Value++));
                     }
                     else
                     {
                         bar.Value++;
                     }

                     for (int j = 0; j < cols.Length; j++)
                     {
                         if (grid[cols[j].Index, i].Value == null)
                         {
                             object value = grid[cols[j].Index, i].Value;


                             excelWorksheet.Cells[rowIndex, j + 1] = value;
                         }

                         if (fileString.Contains("카드페이") )
                         {
                             if (j == 9)
                             {
                                 object value = grid[cols[j].Index, i].Value;

                                 string SPayAccountNo = string.Empty;
                                 string tempString = string.Empty;
                                 tempString = grid[cols[j].Index, i].Value.ToString();

                                 try
                                 {
                                     byte[] data = Convert.FromBase64String(tempString);


                                     SPayAccountNo = m_crypt.Decrypt(tempString);
                                 }
                                 catch
                                 {
                                     SPayAccountNo = tempString;
                                 }

                                 excelWorksheet.Cells[rowIndex, j + 1] =  SPayAccountNo;
                             }
                             else
                             {
                                 object value = grid[cols[j].Index, i].Value.ToString();


                                 excelWorksheet.Cells[rowIndex, j + 1] = value;
                             }
                             //if (grid[cols[j].Index, i].Value == null)
                             //{
                             //    object value = grid[cols[j].Index, i].Value;


                             //    excelWorksheet.Cells[rowIndex, j + 1] = value;
                             //}
                         }
                         else if (fileString.Contains("오프라인PG"))
                         {
                             if (j == 0)
                             {
                                 

                                 excelWorksheet.Cells[rowIndex, j + 1] = rowIndex-1;
                             }
                             else  if (j == 25)
                             {
                                 object value = grid[cols[j].Index, i].Value;

                                 string SPayAccountNo = string.Empty;
                                 string tempString = string.Empty;
                                 tempString = grid[cols[j].Index, i].Value.ToString();

                                 try
                                 {
                                     byte[] data = Convert.FromBase64String(tempString);


                                     SPayAccountNo = m_crypt.Decrypt(tempString);
                                 }
                                 catch
                                 {
                                     SPayAccountNo = tempString;
                                 }

                                 excelWorksheet.Cells[rowIndex, j + 1] = "'" + SPayAccountNo.Replace(" " ,"").Replace(".","").ToString();
                             }
                             else
                             {
                                 object value = grid[cols[j].Index, i].Value.ToString().Replace(" ", "");


                                 excelWorksheet.Cells[rowIndex, j + 1] =  value;
                             }
                         }

                         else
                         {
                             object value = grid[cols[j].Index, i].Value.ToString().Replace(" ", "");


                             excelWorksheet.Cells[rowIndex, j + 1] = value;
                         }




                     }
                     rowIndex++;


                 }

                 if (fileString.Contains("배차"))
                 {
                     string LastCol = "z" + (grid.RowCount + 2);
                     range = excelWorksheet.get_Range("a3", LastCol);
                     range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                     range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                 }
                 else if (fileString.Contains("위탁"))
                 {
                     string LastCol = "AE" + (grid.RowCount + 2);
                     range = excelWorksheet.get_Range("a3", LastCol);
                     range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                     range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                 }
                 else if (fileString.Contains("차주몰") || fileString.Contains("운송사몰"))
                 {
                     string LastCol = "R" + (grid.RowCount + 1);
                     range = excelWorksheet.get_Range("a1", LastCol);

                     
                     range.Locked = false;

                     range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                     range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                 }
                 // range.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                 //5. 저장 혹은 열기
                 if (isOpen)
                 {
                     //  excelApp.Visible = true;
                 }
                 else
                 {
                   
                     excelBook.SaveAs(existExcelFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                             Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);


                     MessageBox.Show("데이터를 엑셀로 내보내어 저장하였습니다.", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                     Nar(excelWorksheet);
                     excelBook.Close(false, Missing.Value, Missing.Value);
                     Nar(excelBook);
                     excelApp.Quit();
                     Nar(excelApp);
                     GC.GetTotalMemory(false);
                     GC.Collect();
                     GC.WaitForPendingFinalizers();
                     GC.Collect();
                     GC.GetTotalMemory(true);

                     System.Diagnostics.Process.Start(existExcelFilePath);

                 }
                 //바 넣고
                 if (barHide)
                 {
                     if (bar.InvokeRequired)
                         bar.Invoke(new Action(() => bar.Visible = false));
                     else
                         bar.Visible = false;


                 }
             }
             catch (Exception E)
             {
                 //MessageBox.Show("엑셀 내보내기에 실패하였습니다.\n" + E.Message, "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                 MessageBox.Show("엑셀 내보내기에 실패하였습니다.", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                 Nar(excelWorksheet);
                 excelBook.Close(false, Missing.Value, Missing.Value);
                 Nar(excelBook);
                 excelApp.Quit();
                 Nar(excelApp);
                 GC.GetTotalMemory(false);
                 GC.Collect();
                 GC.WaitForPendingFinalizers();
                 GC.Collect();
                 GC.GetTotalMemory(true);

                 //바 넣고
                 if (barHide)
                 {
                     if (bar.InvokeRequired)
                         bar.Invoke(new Action(() => bar.Visible = false));
                     else
                         bar.Visible = false;


                 }
             }
         }
         public static Dictionary<DataGridViewColumn, object[,]> ImportExcelFile(this Form value, OpenFileDialog openFileDialog1, DataGridView grid1, DataGridViewColumn codeCol)
         {
             //-- 기본파일저장정보설정.
             //- 파일명
             openFileDialog1.FileName = "TempFile";
             //- 확장자
             openFileDialog1.DefaultExt = "xls";
             //- 필터
             openFileDialog1.Filter = "Excel files (*.xls)|*.xls";
             //Excel Declare
             Excel.Application objApp = null;
             Excel._Workbook objBook = null;
             Excel.Sheets objSheets = null;
             Excel._Worksheet objSheet = null;
             Excel.Range range = null;
             Dictionary<DataGridViewColumn, object[,]> dic = null;
             if (openFileDialog1.ShowDialog() != DialogResult.OK) return null;
             string fileName = openFileDialog1.FileName;
             try
             {
                 objApp = new Microsoft.Office.Interop.Excel.Application();
                 objBook = objApp.Workbooks.Open(fileName, Missing.Value, Missing.Value,
                     Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value
                     , Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                 try
                 {
                     //Get a reference to the first sheet of the workbook.
                     objSheets = objBook.Worksheets;
                     objSheet = (Excel._Worksheet)objSheets.get_Item(1);
                 }

                 catch (Exception)
                 {
                     String errorMessage;
                     errorMessage = "Can't find the Excel workbook.  Try clicking Button1 " +
                        "to create an Excel workbook with data before running Button2.";

                     MessageBox.Show(errorMessage, "Missing Workbook?");

                     //You can't automate Excel if you can't find the data you created, so 
                     //leave the subroutine.
                     return null;
                 }
                 dic = new Dictionary<DataGridViewColumn, object[,]>();
                 string[] colName = new string[]{"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O"
                    ,"P","Q","R","S","T","U","V","W","X","Y","Z","AA","AB","AC","AD","AE","AF","AG","AH","AI","AJ"
                    ,"AK","AL","AM","AN","AO","AP","AQ","AR","AS","AT","AU","AV","AW","AX","AY","AZ"};
                 int Maxrows = 0;
                 for (int i = 1; i < objSheet.Rows.Count; i++)
                 {
                     object[,] oComp = (object[,])objSheet.get_Range("A" + i.ToString(), "AZ" + i.ToString()).get_Value(Missing.Value);
                     //if (oComp..Where(o => o != null).Count() == 0)
                     //{
                     //    Maxrows = i;
                     //    break;
                     //}
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
                 if (Maxrows < 1)
                 {
                     throw new Exception("빈 엑셀 문서이거나, 첫번째 쉬트에 데이터가 없거나, 첫번째 줄이 빈열입니다.\n 가져올 엑셀 문서는 첫번째 열, 첫번째 행부터 데이터가 채워져있어야합니다.");
                 }
                 DataGridViewColumn[] cols = new DataGridViewColumn[grid1.ColumnCount];
                 grid1.Columns.CopyTo(cols, 0);
                 for (int i = 0; i < cols.Length; i++)
                 {
                     object o = objSheet.get_Range(colName[i] + "1", colName[i] + "1").get_Value(Missing.Value);
                     if (o == null) break;
                     string headText = o.ToString();
                     var query = cols.Where(c => c.HeaderText == headText);
                     if (query.Count() > 0)
                     {
                         query.First().Visible = true;
                         dic.Add(query.First(), (object[,])objSheet.get_Range(colName[i] + "1", colName[i] + Maxrows.ToString()).get_Value(Missing.Value));
                     }
                 }
                 if (dic.Keys.Where(c => c == codeCol).Count() == 0)
                 {
                     //if (DialogResult.Yes == MessageBox.Show("엑셀문서에 코드값을 가진 열이 없습니다. 기존 정보와 값을 연결할 수 없습니다.\n" +
                     //    "계속하여 진행하시면, 엑셀문서의 모든 값들은 추가만 될것입니다. 계속하시겠습니까?", "코드값 없음", MessageBoxButtons.YesNo))
                     //{
                     //    for (int i = 1; i <= Maxrows; i++)
                     //    {
                     //        foreach (DataGridViewColumn col in dic.Keys)
                     //        {

                     //        }
                     //    }
                     //}
                     //else throw new Exception("엑셀문서내에 코드값을 가진 열이 없습니다.");
                     throw new Exception("엑셀문서내에 코드값을 가진 열이 없습니다.");
                 }
             }
             catch (Exception theException)
             {
                 String errorMessage;
                 errorMessage = "Error: ";
                 errorMessage = String.Concat(errorMessage, theException.Message);
                 errorMessage = String.Concat(errorMessage, " Line: ");
                 errorMessage = String.Concat(errorMessage, theException.Source);
                 MessageBox.Show(errorMessage, "Error");
                 dic = null;
             }
             if (objApp != null)
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
                     catch { }
                     Nar(objBook);
                     objApp.Quit();
                     Nar(objApp);
                 }
                 catch
                 {
                 }
             }
             return dic;
         }
        public static bool ReturnClose(this Form value)
        {
            bool r = false;
            value.FormClosed += new FormClosedEventHandler((object sender, FormClosedEventArgs e)
            =>
            {
                r = true;
            });
            value.Close();
            return r;
        }
        public static MenuAuth GetAuth(this Form value)
        {
            

            MenuAuth r = MenuAuth.None;

            string menucode = value.Name;
            string[] Array_MenuCode;

            Array_MenuCode = menucode.Split('_');
            if (Array_MenuCode.Length > 0)
            {
                menucode = Array_MenuCode[0];
            }
            
            if (menucode == "FrmMN0212")
            {
                menucode = "mnu0401";
 
            }
            if (menucode == "FrmMN0301")
            {
                menucode = "mnu0801";

            }

            
            if(menucode == "FrmMN0803")
            {
                menucode = "mnu0803";
            }
            if (menucode == "FrmMN0204")
            {
                menucode = "mnu0204_1";
            }
            if(menucode == "FrmMN0000")
            {
                menucode = "mnuAS";
            }
            if(menucode == "FrmMN0301Default")
            {
                menucode = "mnu0301default";
            }

            if (menucode == "FrmMN0301G")
            {
                menucode = "mnu0301g";
            }


            if (menucode.ToUpper().Contains("FRMMN"))
            {
                menucode = menucode.ToUpper().Replace("FRMMN", "MNU");
            }
            else
            {
                menucode = menucode.ToUpper().Replace("FRM", "MNU");
            }

            
            string usdeID = LocalUser.Instance.LogInInformation.LoginId;
            try
            {
                UserAuthorityTableAdapter userAuthorityTableAdapter = new DataSets.BaseDataSetTableAdapters.UserAuthorityTableAdapter();
                BaseDataSet _BInnerDataSet = new BaseDataSet();
                userAuthorityTableAdapter.Fill(_BInnerDataSet.UserAuthority);


                BaseDataSet.UserAuthorityRow auth = _BInnerDataSet.UserAuthority.FindByUserIdMenuCode(usdeID, menucode.ToLower());
                if (auth.WriteAuth) r = MenuAuth.Write;
                else if (auth.ReadAuth) r = MenuAuth.Read;
            }
            catch { }




            return r;
        }
       
        public static MenuAuth GetAuth(this string value)
        {
           
            UserAuthorityTableAdapter userAuthorityTableAdapter = new DataSets.BaseDataSetTableAdapters.UserAuthorityTableAdapter();
            BaseDataSet _BInnerDataSet = new BaseDataSet();
            userAuthorityTableAdapter.Fill(_BInnerDataSet.UserAuthority);
            
       

                MenuAuth r = MenuAuth.None;
           
                string menucode = value;
                string usdeID = LocalUser.Instance.LogInInformation.LoginId;
                try
                {
               
                    BaseDataSet.UserAuthorityRow auth = _BInnerDataSet.UserAuthority.FindByUserIdMenuCode(usdeID, menucode);

                //SingleDataSet.Instance.UserAuthority.FindByUserIdMenuCode(usdeID, menucode);

                if (auth.WriteAuth) r = MenuAuth.Write;
                    else if (auth.ReadAuth) r = MenuAuth.Read;
                }
                catch { }

           
            return r;
        }
    }
    class Model : INotifyPropertyChanged
    {
        private String _S_Idx = "";
        private String _SBiz_NO = "";
        private String _SName = "";
        private String _SUptae = "";
        private String _SUpjong = "";
        private String _SCeo = "";
        private String _SCeoBirth = "";
        private String _SMobileNo = "";
        private String _SPhoneNo = "";
        private String _SFaxNo = "";
        private String _SEmail = "";
        private String _SState = "";
        private String _SCity = "";
        private String _SStreet = "";
        private String _SBizGubun = "";
        private String _SRouteType = "";
        private String _SInsurance = "";
        private String _SCarNo = "";
        private String _SCarType = "";
        private String _SCarSize = "";
        private String _SCarGubun = "";
        private String _SCarYear = "";
        private String _SPayBankName = "";
        private String _SPayAccountNo = "";
        private String _SInputName = "";
        private String _SCarstate = "";
        private String _SCarcity = "";
        private String _SCarStreet = "";
        private String _SfpisCartype = "";
        private String _Error = "";

        public string S_Idx
        {
            get
            {
                return _S_Idx;
            }

            set
            {
                SetField(ref _S_Idx, value);
            }
        }

        public string SBiz_NO
        {
            get
            {
                return _SBiz_NO;
            }

            set
            {
                SetField(ref _SBiz_NO, value);
            }
        }

        public string SName
        {
            get
            {
                return _SName;
            }

            set
            {
                SetField(ref _SName, value);
            }
        }

        public string SUptae
        {
            get
            {
                return _SUptae;
            }

            set
            {
                SetField(ref _SUptae, value);
            }
        }

        public string SUpjong
        {
            get
            {
                return _SUpjong;
            }

            set
            {
                SetField(ref _SUpjong, value);
            }
        }

        public string SCeo
        {
            get
            {
                return _SCeo;
            }

            set
            {
                SetField(ref _SCeo, value);
            }
        }

        public string SCeoBirth
        {
            get
            {
                return _SCeoBirth;
            }

            set
            {
                SetField(ref _SCeoBirth, value);
            }
        }

        public string SMobileNo
        {
            get
            {
                return _SMobileNo;
            }

            set
            {
                SetField(ref _SMobileNo, value);
            }
        }

        public string SPhoneNo
        {
            get
            {
                return _SPhoneNo;
            }

            set
            {
                SetField(ref _SPhoneNo, value);
            }
        }

        public string SFaxNo
        {
            get
            {
                return _SFaxNo;
            }

            set
            {
                SetField(ref _SFaxNo, value);
            }
        }

        public string SEmail
        {
            get
            {
                return _SEmail;
            }

            set
            {
                SetField(ref _SEmail, value);
            }
        }

        public string SState
        {
            get
            {
                return _SState;
            }

            set
            {
                SetField(ref _SState, value);
            }
        }

        public string SCity
        {
            get
            {
                return _SCity;
            }

            set
            {
                SetField(ref _SCity, value);
            }
        }

        public string SStreet
        {
            get
            {
                return _SStreet;
            }

            set
            {
                SetField(ref _SStreet, value);
            }
        }

        public string SBizGubun
        {
            get
            {
                return _SBizGubun;
            }

            set
            {
                SetField(ref _SBizGubun, value);
            }
        }

        public string SRouteType
        {
            get
            {
                return _SRouteType;
            }

            set
            {
                SetField(ref _SRouteType, value);
            }
        }

        public string SInsurance
        {
            get
            {
                return _SInsurance;
            }

            set
            {
                SetField(ref _SInsurance, value);
            }
        }

        public string SCarNo
        {
            get
            {
                return _SCarNo;
            }

            set
            {
                SetField(ref _SCarNo, value);
            }
        }

        public string SCarType
        {
            get
            {
                return _SCarType;
            }

            set
            {
                SetField(ref _SCarType, value);
            }
        }

        public string SCarSize
        {
            get
            {
                return _SCarSize;
            }

            set
            {
                SetField(ref _SCarSize, value);
            }
        }

        public string SCarGubun
        {
            get
            {
                return _SCarGubun;
            }

            set
            {
                SetField(ref _SCarGubun, value);
            }
        }

        public string SCarYear
        {
            get
            {
                return _SCarYear;
            }

            set
            {
                SetField(ref _SCarYear, value);
            }
        }

        public string SPayBankName
        {
            get
            {
                return _SPayBankName;
            }

            set
            {
                SetField(ref _SPayBankName, value);
            }
        }

        public string SPayAccountNo
        {
            get
            {
                return _SPayAccountNo;
            }

            set
            {
                SetField(ref _SPayAccountNo, value);
            }
        }

        public string SInputName
        {
            get
            {
                return _SInputName;
            }

            set
            {
                SetField(ref _SInputName, value);
            }
        }

        public string SCarstate
        {
            get
            {
                return _SCarstate;
            }

            set
            {
                SetField(ref _SCarstate, value);
            }
        }

        public string SCarcity
        {
            get
            {
                return _SCarcity;
            }

            set
            {
                SetField(ref _SCarcity, value);
            }
        }

        public string SCarStreet
        {
            get
            {
                return _SCarStreet;
            }

            set
            {
                SetField(ref _SCarStreet, value);
            }
        }

        public string SfpisCartype
        {
            get
            {
                return _SfpisCartype;
            }

            set
            {
                SetField(ref _SfpisCartype, value);
            }
        }

        public string Error
        {
            get
            {
                return _Error;
            }

            set
            {
                SetField(ref _Error, value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
