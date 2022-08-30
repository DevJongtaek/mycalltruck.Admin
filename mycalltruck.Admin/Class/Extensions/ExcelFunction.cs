using mycalltruck.Admin.Class.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace mycalltruck.Admin.Class.Extensions
{
    public class ExcelImport
    {
        //public Action<int> OnRowCountNotify;
        //public Action<int> OnProgressValueChagned;
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
        }
        public object[,] Import(String FileString, int HeaderRowCount, int HeaderColumnCount, int DataColumnCount)
        {
            Excel.Application excelApp; // 1
            Excel.Workbooks excelBooks;
            Excel.Workbook excelBook;
            Excel.Sheets excelSheets;
            Excel.Worksheet worksheet;
            Excel.Range range;
            List<object[]> T = new List<object[]>();
            excelApp = new Excel.Application();
            excelBooks = excelApp.Workbooks;
            excelBook = excelBooks.Open(FileString);
            excelSheets = excelBook.Sheets;
            worksheet = excelSheets.get_Item(1);
            range = worksheet.UsedRange;

            int rowCount = range.Rows.Count;
            int columnCount = range.Columns.Count;
            int i = 0;


            //if (OnRowCountNotify != null)
            //    OnRowCountNotify(range.Rows.Count - HeaderRowCount);

            int rowIndex = HeaderRowCount + 1;
            var objValues = (object[,])range.get_Value(Missing.Value);
            int rowcountval = range.Rows.Count;
            for (int k = rowIndex; k < rowcountval + 1; k++)
            {
                object[] row = new object[columnCount - HeaderColumnCount];
                i = 0;
                for (int l = HeaderColumnCount + 1; l < columnCount + 1; l++)
                {
                    row[i] = objValues[k, l];
                    i++;
                }
                T.Add(row);
            }

            i = 0;
            object[,] r = new object[T.Count, DataColumnCount];
            foreach (var item in T)
            {
                for (int j = 0; j < DataColumnCount; j++)
                {
                    if (j < item.Length)
                        r[i, j] = item[j];
                }
                i++;
            }
            excelBook.Close();
            excelApp.Quit();
            releaseObject(range);
            releaseObject(worksheet);
            releaseObject(excelSheets);
            releaseObject(excelBook);
            releaseObject(excelBooks);
            releaseObject(excelApp);
            return r;
        }

        public object[,] Import(String FileString, int DataColumnCount)
        {
            return Import(FileString, 1, 0, DataColumnCount);
        }
    }

    class ExcelFunction
    {
        #region EXCEL FUNCTION
        public class ExcelJob
        {
            //public Action<int> OnRowCountNotify;
            //public Action<int> OnProgressValueChagned;
            private string headercolumn(int inputval)
            {
                Stack<char> arr = new Stack<char> { };
                int unit1 = 0;

                do
                {
                    unit1 = inputval % 26;
                    inputval = inputval / 26;
                    arr.Push((char)(unit1 + 65));
                    if (0 < inputval && inputval < 26)
                    {
                        arr.Push((char)(inputval + 64));
                    }
                }
                while (inputval > 26);
                string value = null;
                int non = arr.Count;
                for (int i = 0; i < non; i++)
                {
                    value = value + arr.Pop();
                }
                return value;

            }
            public object[,] Import(String FileString, int HeaderRowCount, int HeaderColumnCount, int DataColumnCount)
            {
                Excel.Application excelApp; // 1
                Excel.Workbooks excelBooks;
                Excel.Workbook excelBook;
                Excel.Sheets excelSheets;
                Excel.Worksheet worksheet;
                Excel.Range range;
                List<object[]> T = new List<object[]>();
                excelApp = new Excel.Application();
                excelBooks = excelApp.Workbooks;
                excelBook = excelBooks.Open(FileString);
                excelSheets = excelBook.Sheets;
                worksheet = excelSheets.get_Item(1);
                range = worksheet.UsedRange;

                int rowCount = range.Rows.Count;
                int columnCount = range.Columns.Count;
                int i = 0;


                //if (OnRowCountNotify != null)
                //    OnRowCountNotify(range.Rows.Count - HeaderRowCount);

                int rowIndex = HeaderRowCount + 1;
                var objValues = (object[,])range.get_Value(Missing.Value);

                for (int k = rowIndex; k < range.Rows.Count + 1; k++)
                {
                    object[] row = new object[columnCount - HeaderColumnCount];
                    i = 0;
                    for (int l = HeaderColumnCount + 1; l < columnCount + 1; l++)
                    {
                        row[i] = objValues[k, l];
                        i++;
                    }
                    T.Add(row);
                }

                i = 0;
                object[,] r = new object[T.Count, DataColumnCount];
                foreach (var item in T)
                {
                    for (int j = 0; j < DataColumnCount; j++)
                    {
                        if (j < item.Length)
                            r[i, j] = item[j];
                    }
                    i++;
                }
                excelBook.Close();
                excelApp.Quit();
                releaseObject(objValues);
                releaseObject(range);
                releaseObject(worksheet);
                releaseObject(excelSheets);
                releaseObject(excelBook);
                releaseObject(excelBooks);
                releaseObject(excelApp);
                return r;
            }

            public object[,] Import(String FileString, int DataColumnCount)
            {
                return Import(FileString, 1, 0, DataColumnCount);
            }
            public void GridViewExcel(DataGridView grid)
            {
                Excel.Application oXL; // 1
                Excel.Workbooks oWBs;
                Excel.Workbook oWB;
                Excel.Worksheet oSheet;
                //edubill 테스트


                try
                {
                    //start Excel and get Application object.
                    oXL = new Excel.Application();
                    //oXL.Visible = true;
                    //테스트
                    oWBs = oXL.Workbooks;
                    //Get a new workbook

                    oWB = (Excel.Workbook)(oWBs.Add(Missing.Value));
                    oSheet = (Excel.Worksheet)oWB.ActiveSheet;

                    //Add table headers going cell by cell.
                    int k = 0;
                    int u = grid.Columns.Count;
                    int l = grid.Columns.Count / 26;
                    int m = 0;
                    string[] colHeader = new string[grid.ColumnCount];
                    for (int i = 0; i < u; i++)
                    {
                        oSheet.Cells[1, i + 1] = grid.Columns[i].HeaderText;

                        k = i + 65;
                        colHeader[i] = headercolumn(i);
                        //if (i % 26 == 0)
                        //    m++;
                        //if (i < 26)
                        //    colHeader[i] = Convert.ToString((char)k);
                        //else
                        //    colHeader[i] = Convert.ToString('A')+Convert.ToString((char)(k - 26));
                    }
                    //Format A1:D1 as bold, vertical alignment = center.
                    oSheet.get_Range("A1", colHeader[colHeader.Length - 1] + "1").Font.Bold = true;
                    oSheet.get_Range("A1", colHeader[colHeader.Length - 1] + "1").VerticalAlignment =
                        Excel.XlVAlign.xlVAlignCenter;

                    //Create an array to multiple values at once.
                    object[,] saNames = new object[grid.RowCount, grid.ColumnCount];


                    Type tp;
                    for (int i = 0; i < grid.RowCount; i++)
                    {
                        for (int j = 0; j < grid.ColumnCount; j++)
                        {
                            tp = grid.Rows[i].Cells[j].ValueType;

                            if (tp.Name == "String") // 3333-01-01형태의 날짜 필터하기 위함(숫자로 변환 방지)
                            {
                                if (grid.Rows[i].Cells[j].Value != null)
                                    saNames[i, j] = "'" + grid.Rows[i].Cells[j].Value.ToString();
                            }
                            else
                            {
                                if (tp.IsValueType)
                                {
                                    if (grid.Rows[i].Cells[j].Value != null)
                                        saNames[i, j] = grid.Rows[i].Cells[j].Value;
                                    else
                                        saNames[i, j] = "";
                                }
                                else if (tp == typeof(object))
                                {
                                    if (grid.Rows[i].Cells[j].Value != null)
                                        saNames[i, j] = grid.Rows[i].Cells[j].Value;
                                    else
                                        saNames[i, j] = "";
                                }
                                else
                                    saNames[i, j] = "";
                            }
                        }
                    }

                    //Fill A2:B6 with an array of values(First and Last Names).
                    //oSheet.get_Range("A2","B6").Value2 = saNames;
                    oSheet.get_Range(colHeader[0] + "2", colHeader[colHeader.Length - 1] +
                        (grid.RowCount + 1)).Value2 = saNames;

                    oXL.Visible = true;

                    oXL.UserControl = true;
                    releaseObject(oSheet);
                    releaseObject(oWB);
                    releaseObject(oWBs);
                    releaseObject(oXL);
                    //oXL.Quit();
                }
                catch (Exception ex)
                {
                    string errmsg;
                    errmsg = "Error: ";
                    errmsg = string.Concat(errmsg, ex.Message);
                    errmsg = string.Concat(errmsg, "Line: ");
                    errmsg = string.Concat(errmsg, ex.Source);

                    MessageBox.Show(errmsg, "Error");
                }
            }
         

            public void GridViewExcelWithoutUnvisiableColumn(DataGridView grid)
            {
                Excel.Application oXL; // 1
                Excel.Workbooks oWBs;
                Excel.Workbook oWB;
                Excel.Worksheet oSheet;
                //edubill 테스트
                Dictionary<int, int> ExcelIndexWithColumnIndexDictionary = new Dictionary<int, int>();


                try
                {
                    //start Excel and get Application object.
                    oXL = new Excel.Application();
                    //oXL.Visible = true;
                    //테스트
                    oWBs = oXL.Workbooks;
                    //Get a new workbook

                    oWB = (Excel.Workbook)(oWBs.Add(Missing.Value));
                    oSheet = (Excel.Worksheet)oWB.ActiveSheet;

                    //Add table headers going cell by cell.
                    int k = 0;
                    int u = grid.Columns.Count;
                    int l = grid.Columns.Count / 26;
                    int m = 0;
                    int ExcelIndex = 0;
                    for (int i = 0; i < grid.ColumnCount; i++)
                    {
                        if (grid.Columns[i].Visible == true)
                        {
                            ExcelIndexWithColumnIndexDictionary.Add(ExcelIndex, i);
                            ExcelIndex++;
                        }
                    }


                    string[] colHeader = new string[ExcelIndexWithColumnIndexDictionary.Count];
                    for (int i = 0; i < colHeader.Length; i++)
                    {
                        oSheet.Cells[1, i + 1] = grid.Columns[ExcelIndexWithColumnIndexDictionary[i]].HeaderText;

                        k = i + 65;
                        colHeader[i] = headercolumn(i);
                        //if (i % 26 == 0)
                        //    m++;
                        //if (i < 26)
                        //    colHeader[i] = Convert.ToString((char)k);
                        //else
                        //    colHeader[i] = Convert.ToString('A')+Convert.ToString((char)(k - 26));
                    }
                    //Format A1:D1 as bold, vertical alignment = center.
                    oSheet.get_Range("A1", colHeader[colHeader.Length - 1] + "1").Font.Bold = true;
                    oSheet.get_Range("A1", colHeader[colHeader.Length - 1] + "1").VerticalAlignment =
                        Excel.XlVAlign.xlVAlignCenter;

                    //Create an array to multiple values at once.
                    object[,] saNames = new object[grid.RowCount, ExcelIndexWithColumnIndexDictionary.Count];


                    string tp;
                    for (int i = 0; i < grid.RowCount; i++)
                    {
                        for (int j = 0; j < ExcelIndexWithColumnIndexDictionary.Count; j++)
                        {
                            tp = grid.Rows[i].Cells[ExcelIndexWithColumnIndexDictionary[j]].ValueType.Name;

                            if (tp == "String") // 3333-01-01형태의 날짜 필터하기 위함(숫자로 변환 방지)
                            {
                                if (grid.Rows[i].Cells[j].Value != null)
                                    saNames[i, j] = "'" + grid.Rows[i].Cells[ExcelIndexWithColumnIndexDictionary[j]].Value.ToString();
                            }
                            else
                            {
                                if (grid.Rows[i].Cells[j].Value != null)
                                    saNames[i, j] = grid.Rows[i].Cells[ExcelIndexWithColumnIndexDictionary[j]].Value;
                            }
                        }
                    }

                    //Fill A2:B6 with an array of values(First and Last Names).
                    //oSheet.get_Range("A2","B6").Value2 = saNames;
                    oSheet.get_Range(colHeader[0] + "2", colHeader[colHeader.Length - 1] +
                        (grid.RowCount + 1)).Value2 = saNames;

                    oXL.Visible = true;

                    oXL.UserControl = true;
                    releaseObject(oSheet);
                    releaseObject(oWB);
                    releaseObject(oWBs);
                    releaseObject(oXL);
                    //oXL.Quit();
                }
                catch (Exception ex)
                {
                    string errmsg;
                    errmsg = "Error: ";
                    errmsg = string.Concat(errmsg, ex.Message);
                    errmsg = string.Concat(errmsg, "Line: ");
                    errmsg = string.Concat(errmsg, ex.Source);

                    MessageBox.Show(errmsg, "Error");
                }
            }

            private void releaseObject(object obj)
            {
                try
                {
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(obj);
                    obj = null;
                }
                catch (Exception ex)
                {
                    obj = null;
                    throw ex;
                }
            }
        }

        public class excel_function
        {
            //쓰기

        }
        #endregion
    }
}
