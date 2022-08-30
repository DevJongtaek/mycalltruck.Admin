using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using mycalltruck.Admin.Class.Extensions;

namespace mycalltruck.Admin.Class.Extensions
{
    public static class Excel_Epplus
    {
        //2013.10.25 오종택 추가
        public static void Ep_Export(DataGridView grid, string noneTitle, string fileString, ProgressBar bar, bool barHide)
        {

            bool isOpen = false;
            DialogResult tResult = DialogResult.Yes;

            //열기
            if (tResult == System.Windows.Forms.DialogResult.Yes)
            {
                isOpen = true;
                fileString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileString + ".xlsx");
            }
            else
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.DefaultExt = "xlsx";
                saveFileDialog1.FileName = fileString;
                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fileString = saveFileDialog1.FileName;
                }
                else
                {
                    MessageBox.Show("사용자에 의해 취소되었습니다.", noneTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            FileInfo fileinfo = null;
            if (File.Exists(fileString) == true)
            {
                try
                {
                    File.Delete(fileString);
                    fileinfo = new FileInfo(fileString);
                }

                catch (Exception E)
                {
                    MessageBox.Show("엑셀 내보내기에 실패하였습니다.\n" + E.Message, "엑셀내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);


                }

            }
            else
            {
                fileinfo = new FileInfo(fileString);
            }

            if (string.IsNullOrEmpty(noneTitle) == true)
                noneTitle = "sheet1";
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

                using (OfficeOpenXml.ExcelPackage excelApp = new OfficeOpenXml.ExcelPackage(fileinfo))
                {
                    OfficeOpenXml.ExcelWorksheet Title = excelApp.Workbook.Worksheets.Add(noneTitle);

                    int indexCol = 1;

                    //var numStyle = excelApp.Workbook.Styles.CreateNamedStyle("TableNumber");
                    //numStyle.Style.Numberformat.Format = "#,##0.0";

                    grid.Invoke(new Action(() =>
                    {



                        foreach (DataGridViewColumn col in grid.Columns)
                        {


                            if (col.Visible == true)
                            {

                                Title.Cells[1, indexCol].Value = col.HeaderText;
                                Title.Cells[1, indexCol].Style.Font.Bold = true;
                                Title.Cells[1, indexCol].Style.Font.Name = "맑은고딕";
                                Title.Cells[1, indexCol].Style.Font.Size = 11;
                                indexCol++;

                            }

                        }
                        int IndexRow = 2;

                        foreach (DataGridViewRow row in grid.Rows)
                        {
                            if (bar.InvokeRequired)
                            {
                                bar.Invoke(new Action(() => bar.Value++));
                            }
                            else
                            {
                                bar.Value++;
                            }

                            indexCol = 1;

                            foreach (DataGridViewColumn col in grid.Columns)
                            {
                                //if (col.CellType == typeof(DataGridViewTextBoxCell))
                                if (col.Visible == true)
                                {
                                    // Title.Cells[IndexRow, indexCol].Style.Numberformat.Format = "@";
                                    //Title.Cells[IndexRow, indexCol].Value = row.Cells[col.Index].FormattedValue;
                                    //Title.Cells[IndexRow, indexCol].Style.Font.Name = "맑은고딕";
                                    //Title.Cells[IndexRow, indexCol].Style.Font.Size = 9;


                                    //indexCol++;
                                    string colvalue = row.Cells[col.Index].FormattedValue.ToString();
                                    decimal decimalresult = 0;

                                    //bool result = Decimal.TryParse(colvalue, out decimalresult);
                                    //if (result == true)
                                    //{
                                    //    Title.Cells[IndexRow, indexCol].Value = Convert.ToDecimal(row.Cells[col.Index].FormattedValue);
                                    //}
                                    //else
                                    //{

                                    //    Title.Cells[IndexRow, indexCol].Value = row.Cells[col.Index].FormattedValue;
                                    //}

                                    Title.Cells[IndexRow, indexCol].Value = row.Cells[col.Index].FormattedValue;

                                    Title.Cells[IndexRow, indexCol].Style.Font.Name = "맑은고딕";
                                    Title.Cells[IndexRow, indexCol].Style.Font.Size = 11;


                                    indexCol++;

                                }
                            }
                            IndexRow++;
                        }
                        //indexCol = 2;
                        //for (int i = 0; i < grid.RowCount; i++)
                        //{


                        //    for (int j = 0; j < grid.ColumnCount; j++)
                        //    {

                        //        object value = grid[j, i].Value;
                        //        object formmatedValue = grid[j, i].FormattedValue;
                        //        if (formmatedValue != null)
                        //        {
                        //            if (value is DateTime || value is string)
                        //                Title.Cells[indexCol, j + 1].Value = "'" + formmatedValue.ToString();
                        //            else
                        //                Title.Cells[indexCol, j + 1].Value = formmatedValue.ToString();
                        //            Title.Cells[indexCol, j + 1].Style.Font.Name = "맑은고딕";
                        //            Title.Cells[indexCol, j + 1].Style.Font.Size = 9;

                        //        }
                        //    }
                        //    indexCol++;
                        //}



                    }));

                    if (isOpen)
                    {

                        excelApp.Save();


                        System.Diagnostics.Process.Start(fileString);

                    }
                    else
                    {
                        excelApp.Workbook.Properties.Title = noneTitle;
                        excelApp.Workbook.Properties.Author = "chasero";
                        excelApp.Workbook.Properties.Comments = "";
                        excelApp.Workbook.Properties.Company = "에듀빌시스템";
                        excelApp.Save();
                        MessageBox.Show("데이터를 엑셀로 내보내어 저장하였습니다.", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    Title = null;
                }
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
            catch
            {
                MessageBox.Show("엑셀 내보내기에 실패하였습니다.", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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


        public static void Ep_FileExport(DataGridView grid, string title, string fileString, ProgressBar bar, bool barHide, byte[] iexExcel, int insertRowIndex)
        {

            bool isOpen = false;
            DialogResult tResult = new TaskDialogForm().Show("엑셀 내보내기", fileString);
            string existExcelFilePath = string.Empty;
            //열기
            if (tResult == System.Windows.Forms.DialogResult.Yes)
            {
                isOpen = true;
                existExcelFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");

                //existExcelFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Templates), fileString + ".xls");
            }
            else
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.DefaultExt = "xlsx";
                saveFileDialog1.Filter = "*.xlsx|엑셀";
                saveFileDialog1.FileName = fileString;
                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    existExcelFilePath = saveFileDialog1.FileName;


                }
                else
                {
                    MessageBox.Show("사용자에 의해 취소되었습니다.", title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            FileInfo fileinfo = null;


            fileinfo = new FileInfo(existExcelFilePath);


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




                File.WriteAllBytes(existExcelFilePath, iexExcel);
                //using (OfficeOpenXml.ExcelPackage excelApp = new ExcelPackage(new System.IO.FileInfo(existExcelFilePath)))
                using (OfficeOpenXml.ExcelPackage excelApp = new OfficeOpenXml.ExcelPackage(fileinfo, true))
                {



                    OfficeOpenXml.ExcelWorksheet ws = excelApp.Workbook.Worksheets[1];
                    //ExcelWorksheet ws = excelApp.Workbook.Worksheets.Add("Inventory");



                    int IndexRow = insertRowIndex;

                    int indexCol = 1;

                    foreach (DataGridViewRow row in grid.Rows)
                    {
                        if (bar.InvokeRequired)
                        {
                            bar.Invoke(new Action(() => bar.Value++));
                        }
                        else
                        {
                            bar.Value++;
                        }

                        indexCol = 1;

                        foreach (DataGridViewColumn col in grid.Columns)
                        {
                            ////&& col.CellType == typeof(DataGridViewTextBoxCell)
                            if (col.Visible == true)
                            {

                                string colvalue = row.Cells[col.Index].FormattedValue.ToString();
                                decimal decimalresult = 0;

                                bool result = Decimal.TryParse(colvalue, out decimalresult);
                                if (result == true)
                                {
                                    ws.Cells[IndexRow, indexCol].Value = Convert.ToDecimal(row.Cells[col.Index].FormattedValue);
                                }
                                else
                                {

                                    ws.Cells[IndexRow, indexCol].Value = row.Cells[col.Index].FormattedValue;
                                }

                                ws.Cells[IndexRow, indexCol].Style.Font.Name = "맑은고딕";
                                ws.Cells[IndexRow, indexCol].Style.Font.Size = 9;


                                indexCol++;
                            }
                        }
                        IndexRow++;
                    }

                    if (isOpen)
                    {
                        excelApp.SaveAs(fileinfo);


                        System.Diagnostics.Process.Start(existExcelFilePath);

                    }
                    else
                    {
                        excelApp.Workbook.Properties.Title = title;
                        excelApp.Workbook.Properties.Author = "chasero";
                        excelApp.Workbook.Properties.Comments = "";
                        excelApp.Workbook.Properties.Company = "에듀빌";
                        //  excelApp.Save();

                        excelApp.SaveAs(fileinfo);
                        MessageBox.Show("데이터를 엑셀로 내보내어 저장하였습니다.", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    title = null;

                }
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
            catch (Exception E)
            {
                MessageBox.Show("엑셀 내보내기에 실패하였습니다.\n" + E.Message, "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
        public static void Ep_FileExport2(DataGridView grid, string title, string fileString, ProgressBar bar, bool barHide, byte[] iexExcel, int insertRowIndex, string FolderPath)
        {

            bool isOpen = false;
          // DialogResult tResult = new TaskDialogForm().Show("엑셀 내보내기", fileString);
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

           
            FileInfo fileinfo = null;


            fileinfo = new FileInfo(existExcelFilePath);
          

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




                File.WriteAllBytes(existExcelFilePath, iexExcel);
                //using (OfficeOpenXml.ExcelPackage excelApp = new ExcelPackage(new System.IO.FileInfo(existExcelFilePath)))
                using (OfficeOpenXml.ExcelPackage excelApp = new OfficeOpenXml.ExcelPackage(fileinfo, true))
                {



                    OfficeOpenXml.ExcelWorksheet ws = excelApp.Workbook.Worksheets[1];
                    //ExcelWorksheet ws = excelApp.Workbook.Worksheets.Add("Inventory");



                    int IndexRow = insertRowIndex;

                    int indexCol = 1;

                    foreach (DataGridViewRow row in grid.Rows)
                    {
                        if (bar.InvokeRequired)
                        {
                            bar.Invoke(new Action(() => bar.Value++));
                        }
                        else
                        {
                            bar.Value++;
                        }

                        indexCol = 1;

                        foreach (DataGridViewColumn col in grid.Columns)
                        {
                            ////&& col.CellType == typeof(DataGridViewTextBoxCell)
                            if (col.Visible == true)
                            {

                                string colvalue = row.Cells[col.Index].FormattedValue.ToString();
                                decimal decimalresult = 0;

                                bool result = Decimal.TryParse(colvalue, out decimalresult);
                                if (result == true)
                                {
                                    ws.Cells[IndexRow, indexCol].Value = Convert.ToDecimal(row.Cells[col.Index].FormattedValue);
                                }
                                else
                                {

                                    ws.Cells[IndexRow, indexCol].Value = row.Cells[col.Index].FormattedValue;
                                }

                                ws.Cells[IndexRow, indexCol].Style.Font.Name = "맑은고딕";
                                ws.Cells[IndexRow, indexCol].Style.Font.Size = 9;


                                indexCol++;
                            }
                        }
                        IndexRow++;
                    }

                    if (isOpen)
                    {
                        excelApp.SaveAs(fileinfo);


                        System.Diagnostics.Process.Start(existExcelFilePath);

                    }
                    else
                    {
                        excelApp.Workbook.Properties.Title = title;
                        excelApp.Workbook.Properties.Author = "chasero";
                        excelApp.Workbook.Properties.Comments = "";
                        excelApp.Workbook.Properties.Company = "에듀빌";
                        //  excelApp.Save();

                        excelApp.SaveAs(fileinfo);
                        MessageBox.Show("데이터를 엑셀로 내보내어 저장하였습니다.", "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                        System.Diagnostics.Process.Start(existExcelFilePath);
                    }
                    title = null;

                }
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
            catch (Exception E)
            {
                MessageBox.Show("엑셀 내보내기에 실패하였습니다.\n" + E.Message, "엑셀 내보내기", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
    }

}
