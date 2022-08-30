using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace mycalltruck.Admin.Class.XML
{
    class FormStyle
    {
        private XElement _FormXML;
        private XDocument _Doc;
        public FormStyle(System.Windows.Forms.Form iForm, params System.Windows.Forms.DataGridView[] iGrids)
        {
            string _filePath = Path.Combine(XMLAccess.RootFolder, "FormStyle.xml");
            try
            {
                _Doc = XDocument.Load(_filePath);
            }
            catch (Exception)
            {
                _Doc = new XDocument(new XElement("Doc"));
                _Doc.Save(_filePath);
            }
            finally
            {
                _Doc.Changed += new EventHandler<XObjectChangeEventArgs>((object sender, XObjectChangeEventArgs e) => {
                    _Doc.Save(_filePath);
                });
                var qEle = _Doc.Root.Elements().Where(c => c.Attributes("Name").Count() > 0 && c.Attribute("Name").Value == iForm.Name);
                if (qEle.Count() > 0)
                {
                    _FormXML = qEle.First();
                }
                else
                {
                    _FormXML = new XElement("Form",
                            new XAttribute("Name", iForm.Name),
                            new XAttribute("Title", iForm.Text));
                    int _gridIndex = 1;
                    foreach (var grid in iGrids)
                    {
                        XElement _gridEle = new XElement("grid" + _gridIndex.ToString());
                        foreach (System.Windows.Forms.DataGridViewColumn column in grid.Columns)
                        {
                            string _BackColor = column.DefaultCellStyle.BackColor.ToArgb().ToString();
                            string _ForeColor = column.DefaultCellStyle.ForeColor.ToArgb().ToString();
                            System.Drawing.Font _Font = column.DefaultCellStyle.Font;
                            if (_BackColor == "0") _BackColor = grid.DefaultCellStyle.BackColor.ToArgb().ToString();
                            if (_ForeColor == "0") _ForeColor = grid.DefaultCellStyle.ForeColor.ToArgb().ToString();
                            if (_Font == null) _Font = grid.DefaultCellStyle.Font;
                            _gridEle.Add(
                                new XElement("Column",
                                    new XAttribute("Name", column.Name),
                                    new XAttribute("Visible", column.Visible.ToString()),
                                    new XAttribute("DisplayIndex", column.DisplayIndex.ToString()),
                                    new XAttribute("FontFamilyName", _Font.FontFamily.Name),
                                    new XAttribute("FontSize", _Font.Size.ToString()),
                                    new XAttribute("BackColor", _BackColor),
                                    new XAttribute("ForeColor", _ForeColor)));
                        }
                        _FormXML.Add(_gridEle);
                        _gridIndex++;
                    }
                    _Doc.Root.Add(_FormXML);
                }
            }
        }
       
        public FormStyle(System.Windows.Forms.Form iForm)
        {
            string _filePath = Path.Combine(XMLAccess.RootFolder, "FormStyle.xml");
            try
            {
                _Doc = XDocument.Load(_filePath);
            }
            catch (Exception)
            {
                _Doc = new XDocument(new XElement("Doc"));
                _Doc.Save(_filePath);
            }
            finally
            {
                _Doc.Changed += new EventHandler<XObjectChangeEventArgs>((object sender, XObjectChangeEventArgs e) =>
                {
                    _Doc.Save(_filePath);
                });
                var qEle = _Doc.Root.Elements().Where(c => c.Attributes("Name").Count() > 0 && c.Attribute("Name").Value == iForm.Name);
                if (qEle.Count() > 0)
                {
                    _FormXML = qEle.First();
                }
                else
                {
                    _FormXML = new XElement("Form",
                            new XAttribute("Name", iForm.Name),
                            new XAttribute("Title", iForm.Text));
                    _Doc.Root.Add(_FormXML);
                }
            }
        }
        public void SetFormStyle(System.Windows.Forms.Form iForm, params System.Windows.Forms.DataGridView[] iGrids)
        {
            try
            {
                //iForm.Text = _FormXML.Attribute("Title").Value;
                foreach (var grid in iGrids)
                {
                    var query = _FormXML.Elements(grid.Name);
                    if (query.Count() == 0) continue;
                    XElement eleGrid = query.First();
                    foreach (System.Windows.Forms.DataGridViewColumn column in grid.Columns)
                    {
                        var qColumn = eleGrid.Elements().Where(c => c.Attribute("Name").Value == column.Name);
                        if (qColumn.Count() == 0) continue;
                        XElement eleColumn = qColumn.First();
                        column.Visible = bool.Parse(eleColumn.Attribute("Visible").Value);
                        column.DisplayIndex = int.Parse(eleColumn.Attribute("DisplayIndex").Value);
                      
                        column.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(int.Parse(
                            eleColumn.Attribute("BackColor").Value));
                        column.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(int.Parse(
                            eleColumn.Attribute("ForeColor").Value));
                        column.DefaultCellStyle.Font = new System.Drawing.Font(
                            eleColumn.Attribute("FontFamilyName").Value, float.Parse(eleColumn.Attribute("FontSize").Value));
                        column.Width = int.Parse(eleColumn.Attribute("Width").Value);
                    }
                }
            }
            catch { }
        }
        public void WriteFormStyle(System.Windows.Forms.Form iForm, params System.Windows.Forms.DataGridView[] iGrids)
        {
            _FormXML = new XElement("Form",
                    new XAttribute("Name", iForm.Name),
                    new XAttribute("Title", iForm.Text));
            int _gridIndex = 1;
            foreach (var grid in iGrids)
            {
                XElement _gridEle = new XElement("grid" + _gridIndex.ToString());
                foreach (System.Windows.Forms.DataGridViewColumn column in grid.Columns)
                {
                    string _BackColor = column.DefaultCellStyle.BackColor.ToArgb().ToString();
                    string _ForeColor = column.DefaultCellStyle.ForeColor.ToArgb().ToString();
                   
                    System.Drawing.Font _Font = column.DefaultCellStyle.Font;
                    if (_BackColor == "0") _BackColor = grid.DefaultCellStyle.BackColor.ToArgb().ToString();
                    if (_ForeColor == "0") _ForeColor = grid.DefaultCellStyle.ForeColor.ToArgb().ToString();
                    if (_Font == null) _Font = grid.DefaultCellStyle.Font;
                    _gridEle.Add(
                        new XElement("Column",
                            new XAttribute("Name", column.Name),
                            new XAttribute("Visible", column.Visible.ToString()),
                            new XAttribute("DisplayIndex", column.DisplayIndex.ToString()),
                            new XAttribute("FontFamilyName", _Font.FontFamily.Name),
                            new XAttribute("FontSize", _Font.Size.ToString()),
                            new XAttribute("BackColor", _BackColor),
                            new XAttribute("ForeColor", _ForeColor),
                            new XAttribute("Width",column.Width)
                            
                            ));
                }
                _FormXML.Add(_gridEle);
                _gridIndex++;
            }
            var query = _Doc.Root.Elements().Where(c => c.Attribute("Name").Value == iForm.Name);
            if (query.Count() > 0) query.Remove();
            _Doc.Root.Add(_FormXML);
        }
        public void WriteFormStyle(System.Windows.Forms.Form iForm, params System.Windows.Forms.CheckBox[] iCheckBoxes)
        {
            string _filePath = Path.Combine(XMLAccess.RootFolder, "FormStyle.xml");
            try
            {
                _Doc = XDocument.Load(_filePath);
            }
            catch (Exception)
            {
                _Doc = new XDocument(new XElement("Doc"));
                _Doc.Save(_filePath);
            }
            finally
            {
                _Doc.Changed += new EventHandler<XObjectChangeEventArgs>((object sender, XObjectChangeEventArgs e) =>
                {
                    _Doc.Save(_filePath);
                });
                var qEle = _Doc.Root.Elements().Where(c => c.Attributes("Name").Count() > 0 && c.Attribute("Name").Value == iForm.Name);
                if (qEle.Count() > 0)
                {
                    _FormXML = qEle.First();
                }
                else
                {
                    _FormXML = new XElement("Form",
                            new XAttribute("Name", iForm.Name),
                            new XAttribute("Title", iForm.Text));
                    _Doc.Root.Add(_FormXML);
                }
                foreach (var checkBox in iCheckBoxes)
                {
                    XElement eleChk = new XElement(checkBox.Name);
                    eleChk.Add(new XAttribute("Checked", checkBox.Checked)
                    , new XAttribute("CheckState", checkBox.CheckState));
                    _FormXML.Add(eleChk);
                }
            }
        }
        public void SetFormStyle(System.Windows.Forms.Form iForm, params System.Windows.Forms.CheckBox[] iCheckBoxes)
        {
            try
            {
                foreach (var checkBox in iCheckBoxes)
                {
                    var query = _FormXML.Elements(checkBox.Name);
                    if (query.Count() == 0) continue;
                    XElement eleChk = query.First();
                    try
                    {
                        checkBox.CheckState = (System.Windows.Forms.CheckState)Enum.Parse(typeof(System.Windows.Forms.CheckState),
                            eleChk.Attribute("CheckState").Value);
                    }
                    catch { }
                    try
                    {
                        checkBox.Checked = Boolean.Parse(eleChk.Attribute("Checked").Value);
                    }
                    catch { }
                }
            }
            catch { }
        }


        public FormStyle(System.Windows.Forms.UserControl iForm, params System.Windows.Forms.DataGridView[] iGrids)
        {
            string _filePath = Path.Combine(XMLAccess.RootFolder, "FormStyle.xml");
            try
            {
                _Doc = XDocument.Load(_filePath);
            }
            catch (Exception)
            {
                _Doc = new XDocument(new XElement("Doc"));
                _Doc.Save(_filePath);
            }
            finally
            {
                _Doc.Changed += new EventHandler<XObjectChangeEventArgs>((object sender, XObjectChangeEventArgs e) => {
                    _Doc.Save(_filePath);
                });
                var qEle = _Doc.Root.Elements().Where(c => c.Attributes("Name").Count() > 0 && c.Attribute("Name").Value == iForm.Name);
                if (qEle.Count() > 0)
                {
                    _FormXML = qEle.First();
                }
                else
                {
                    _FormXML = new XElement("UserControl",
                            new XAttribute("Name", iForm.Name),
                            new XAttribute("Title", iForm.Text));
                    int _gridIndex = 1;
                    foreach (var grid in iGrids)
                    {
                        XElement _gridEle = new XElement("grid" + _gridIndex.ToString());
                        foreach (System.Windows.Forms.DataGridViewColumn column in grid.Columns)
                        {
                            string _BackColor = column.DefaultCellStyle.BackColor.ToArgb().ToString();
                            string _ForeColor = column.DefaultCellStyle.ForeColor.ToArgb().ToString();
                            System.Drawing.Font _Font = column.DefaultCellStyle.Font;
                            if (_BackColor == "0") _BackColor = grid.DefaultCellStyle.BackColor.ToArgb().ToString();
                            if (_ForeColor == "0") _ForeColor = grid.DefaultCellStyle.ForeColor.ToArgb().ToString();
                            if (_Font == null) _Font = grid.DefaultCellStyle.Font;
                            _gridEle.Add(
                                new XElement("Column",
                                    new XAttribute("Name", column.Name),
                                    new XAttribute("Visible", column.Visible.ToString()),
                                    new XAttribute("DisplayIndex", column.DisplayIndex.ToString()),
                                    new XAttribute("FontFamilyName", _Font.FontFamily.Name),
                                    new XAttribute("FontSize", _Font.Size.ToString()),
                                    new XAttribute("BackColor", _BackColor),
                                    new XAttribute("ForeColor", _ForeColor)));
                        }
                        _FormXML.Add(_gridEle);
                        _gridIndex++;
                    }
                    _Doc.Root.Add(_FormXML);
                }
            }
        }

        public FormStyle(System.Windows.Forms.UserControl iForm)
        {
            string _filePath = Path.Combine(XMLAccess.RootFolder, "FormStyle.xml");
            try
            {
                _Doc = XDocument.Load(_filePath);
            }
            catch (Exception)
            {
                _Doc = new XDocument(new XElement("Doc"));
                _Doc.Save(_filePath);
            }
            finally
            {
                _Doc.Changed += new EventHandler<XObjectChangeEventArgs>((object sender, XObjectChangeEventArgs e) =>
                {
                    _Doc.Save(_filePath);
                });
                var qEle = _Doc.Root.Elements().Where(c => c.Attributes("Name").Count() > 0 && c.Attribute("Name").Value == iForm.Name);
                if (qEle.Count() > 0)
                {
                    _FormXML = qEle.First();
                }
                else
                {
                    _FormXML = new XElement("UserControl",
                            new XAttribute("Name", iForm.Name),
                            new XAttribute("Title", iForm.Text));
                    _Doc.Root.Add(_FormXML);
                }
            }
        }
        public void SetFormStyle(System.Windows.Forms.UserControl iForm, params System.Windows.Forms.DataGridView[] iGrids)
        {
            try
            {
                //iForm.Text = _FormXML.Attribute("Title").Value;
                foreach (var grid in iGrids)
                {
                    var query = _FormXML.Elements(grid.Name);
                    if (query.Count() == 0) continue;
                    XElement eleGrid = query.First();
                    foreach (System.Windows.Forms.DataGridViewColumn column in grid.Columns)
                    {
                        var qColumn = eleGrid.Elements().Where(c => c.Attribute("Name").Value == column.Name);
                        if (qColumn.Count() == 0) continue;
                        XElement eleColumn = qColumn.First();
                        column.Visible = bool.Parse(eleColumn.Attribute("Visible").Value);
                        column.DisplayIndex = int.Parse(eleColumn.Attribute("DisplayIndex").Value);
                        column.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(int.Parse(
                            eleColumn.Attribute("BackColor").Value));
                        column.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(int.Parse(
                            eleColumn.Attribute("ForeColor").Value));
                        column.DefaultCellStyle.Font = new System.Drawing.Font(
                            eleColumn.Attribute("FontFamilyName").Value, float.Parse(eleColumn.Attribute("FontSize").Value));
                    }
                }
            }
            catch { }
        }
        public void WriteFormStyle(System.Windows.Forms.UserControl iForm, params System.Windows.Forms.DataGridView[] iGrids)
        {
            _FormXML = new XElement("UserControl",
                    new XAttribute("Name", iForm.Name),
                    new XAttribute("Title", iForm.Text));
            int _gridIndex = 1;
            foreach (var grid in iGrids)
            {
                XElement _gridEle = new XElement("grid" + _gridIndex.ToString());
                foreach (System.Windows.Forms.DataGridViewColumn column in grid.Columns)
                {
                    string _BackColor = column.DefaultCellStyle.BackColor.ToArgb().ToString();
                    string _ForeColor = column.DefaultCellStyle.ForeColor.ToArgb().ToString();
                    System.Drawing.Font _Font = column.DefaultCellStyle.Font;
                    if (_BackColor == "0") _BackColor = grid.DefaultCellStyle.BackColor.ToArgb().ToString();
                    if (_ForeColor == "0") _ForeColor = grid.DefaultCellStyle.ForeColor.ToArgb().ToString();
                    if (_Font == null) _Font = grid.DefaultCellStyle.Font;
                    _gridEle.Add(
                        new XElement("Column",
                            new XAttribute("Name", column.Name),
                            new XAttribute("Visible", column.Visible.ToString()),
                            new XAttribute("DisplayIndex", column.DisplayIndex.ToString()),
                            new XAttribute("FontFamilyName", _Font.FontFamily.Name),
                            new XAttribute("FontSize", _Font.Size.ToString()),
                            new XAttribute("BackColor", _BackColor),
                            new XAttribute("ForeColor", _ForeColor)));
                }
                _FormXML.Add(_gridEle);
                _gridIndex++;
            }
            var query = _Doc.Root.Elements().Where(c => c.Attribute("Name").Value == iForm.Name);
            if (query.Count() > 0) query.Remove();
            _Doc.Root.Add(_FormXML);
        }
        public void WriteFormStyle(System.Windows.Forms.UserControl iForm, params System.Windows.Forms.CheckBox[] iCheckBoxes)
        {
            string _filePath = Path.Combine(XMLAccess.RootFolder, "FormStyle.xml");
            try
            {
                _Doc = XDocument.Load(_filePath);
            }
            catch (Exception)
            {
                _Doc = new XDocument(new XElement("Doc"));
                _Doc.Save(_filePath);
            }
            finally
            {
                _Doc.Changed += new EventHandler<XObjectChangeEventArgs>((object sender, XObjectChangeEventArgs e) =>
                {
                    _Doc.Save(_filePath);
                });
                var qEle = _Doc.Root.Elements().Where(c => c.Attributes("Name").Count() > 0 && c.Attribute("Name").Value == iForm.Name);
                if (qEle.Count() > 0)
                {
                    _FormXML = qEle.First();
                }
                else
                {
                    _FormXML = new XElement("UserControl",
                            new XAttribute("Name", iForm.Name),
                            new XAttribute("Title", iForm.Text));
                    _Doc.Root.Add(_FormXML);
                }
                foreach (var checkBox in iCheckBoxes)
                {
                    XElement eleChk = new XElement(checkBox.Name);
                    eleChk.Add(new XAttribute("Checked", checkBox.Checked)
                    , new XAttribute("CheckState", checkBox.CheckState));
                    _FormXML.Add(eleChk);
                }
            }
        }
        public void SetFormStyle(System.Windows.Forms.UserControl iForm, params System.Windows.Forms.CheckBox[] iCheckBoxes)
        {
            try
            {
                foreach (var checkBox in iCheckBoxes)
                {
                    var query = _FormXML.Elements(checkBox.Name);
                    if (query.Count() == 0) continue;
                    XElement eleChk = query.First();
                    try
                    {
                        checkBox.CheckState = (System.Windows.Forms.CheckState)Enum.Parse(typeof(System.Windows.Forms.CheckState),
                            eleChk.Attribute("CheckState").Value);
                    }
                    catch { }
                    try
                    {
                        checkBox.Checked = Boolean.Parse(eleChk.Attribute("Checked").Value);
                    }
                    catch { }
                }
            }
            catch { }
        }
    }
}
