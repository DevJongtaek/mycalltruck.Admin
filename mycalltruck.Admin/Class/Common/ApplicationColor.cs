using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;


namespace mycalltruck.Admin.Class.Common
{
    public class ApplicationColor : INotifyPropertyChanged
    {
        private static ApplicationColor _Instance = new ApplicationColor();
        public static ApplicationColor Instance
        {
            get { return _Instance; }
        }
        #region Properties
        private Color _PanelBorderBrush = Color.SkyBlue;
        private Color _PanelBackColor1 = Color.White;
        private Color _PanelBackColor2 = Color.Silver;
        private Color _GridBorderBursh = Color.SkyBlue;
        private Color _GridBackColor = Color.White;
        private Color _GridDefaultCellForeColor = Color.Black;
        private Color _GridDefaultCellBackColor = Color.White;
        private Color _GridAlternatingCellForeColor = Color.Black;
        private Color _GridAlternatingCellBackColor = Color.Lavender;
        private Color _GridSelectedCellForeColor = Color.White;
        private Color _GridSelectedCellBackColor = Color.SlateBlue;
        [XmlIgnore]
        public Color PanelBorderBrush
        {
            get
            {
                return _PanelBorderBrush;
            }
            set
            {
                if (_PanelBorderBrush != value)
                {
                    _PanelBorderBrush = value;
                    NotifyPropertyChanged("PanelBorderBrush");
                }
            }
        }
        [XmlElement("PanelBorderBrush")]
        public int PanelBorderBrushArgb
        {
            get
            {
                return PanelBorderBrush.ToArgb();
            }
            set
            {
                PanelBorderBrush = Color.FromArgb(value);
            }
        }
        [XmlIgnore]
        public Color PanelBackColor1
        {
            get
            {
                return _PanelBackColor1;
            }
            set
            {
                if (_PanelBackColor1 != value)
                {
                    _PanelBackColor1 = value;
                    NotifyPropertyChanged("PanelBackColor1");
                }
            }
        }
        [XmlElement("PanelBackColor1")]
        public int PanelBackColor1Argb
        {
            get
            {
                return PanelBackColor1.ToArgb();
            }
            set
            {
                PanelBackColor1 = Color.FromArgb(value);
            }
        }
        [XmlIgnore]
        public Color PanelBackColor2
        {
            get
            {
                return _PanelBackColor2;
            }
            set
            {
                if (_PanelBackColor2 != value)
                {
                    _PanelBackColor2 = value;
                    NotifyPropertyChanged("PanelBackColor2");
                }
            }
        }
        [XmlElement("PanelBackColor2")]
        public int PanelBackColor2Argb
        {
            get
            {
                return PanelBackColor2.ToArgb();
            }
            set
            {
                PanelBackColor2 = Color.FromArgb(value);
            }
        }
        [XmlIgnore]
        public Color GridBorderBursh
        {
            get
            {
                return _GridBorderBursh;
            }
            set
            {
                if (_GridBorderBursh != value)
                {
                    _GridBorderBursh = value;
                    NotifyPropertyChanged("GridBorderBursh");
                }
            }
        }
        [XmlElement("GridBorderBursh")]
        public int GridBorderBurshArgb
        {
            get
            {
                return GridBorderBursh.ToArgb();
            }
            set
            {
                GridBorderBursh = Color.FromArgb(value);
            }
        }
        [XmlIgnore]
        public Color GridBackColor
        {
            get
            {
                return _GridBackColor;
            }
            set
            {
                if (_GridBackColor != value)
                {
                    _GridBackColor = value;
                    NotifyPropertyChanged("GridBackColor");
                }
            }
        }
        [XmlElement("GridBackColor")]
        public int GridBackColorArgb
        {
            get
            {
                return GridBackColor.ToArgb();
            }
            set
            {
                GridBackColor = Color.FromArgb(value);
            }
        }
        [XmlIgnore]
        public Color GridDefaultCellForeColor
        {
            get
            {
                return _GridDefaultCellForeColor;
            }
            set
            {
                if (_GridDefaultCellForeColor != value)
                {
                    _GridDefaultCellForeColor = value;
                    NotifyPropertyChanged("GridDefaultCellForeColor");
                }
            }
        }
        [XmlElement("GridDefaultCellForeColor")]
        public int GridDefaultCellForeColorArgb
        {
            get
            {
                return GridDefaultCellForeColor.ToArgb();
            }
            set
            {
                GridDefaultCellForeColor = Color.FromArgb(value);
            }
        }
        [XmlIgnore]
        public Color GridDefaultCellBackColor
        {
            get
            {
                return _GridDefaultCellBackColor;
            }
            set
            {
                if (_GridDefaultCellBackColor != value)
                {
                    _GridDefaultCellBackColor = value;
                    NotifyPropertyChanged("GridDefaultCellBackColor");
                }
            }
        }
        [XmlElement("GridDefaultCellBackColor")]
        public int GridDefaultCellBackColorArgb
        {
            get
            {
                return GridDefaultCellBackColor.ToArgb();
            }
            set
            {
                GridDefaultCellBackColor = Color.FromArgb(value);
            }
        }
        [XmlIgnore]
        public Color GridAlternatingCellForeColor
        {
            get
            {
                return _GridAlternatingCellForeColor;
            }
            set
            {
                if (_GridAlternatingCellForeColor != value)
                {
                    _GridAlternatingCellForeColor = value;
                    NotifyPropertyChanged("GridAlternatingCellForeColor");
                }
            }
        }
        [XmlElement("GridAlternatingCellForeColor")]
        public int GridAlternatingCellForeColorArgb
        {
            get
            {
                return GridAlternatingCellForeColor.ToArgb();
            }
            set
            {
                GridAlternatingCellForeColor = Color.FromArgb(value);
            }
        }
        [XmlIgnore]
        public Color GridAlternatingCellBackColor
        {
            get
            {
                return _GridAlternatingCellBackColor;
            }
            set
            {
                if (_GridAlternatingCellBackColor != value)
                {
                    _GridAlternatingCellBackColor = value;
                    NotifyPropertyChanged("GridAlternatingCellBackColor");
                }
            }
        }
        [XmlElement("GridAlternatingCellBackColor")]
        public int GridAlternatingCellBackColorArgb
        {
            get
            {
                return GridAlternatingCellBackColor.ToArgb();
            }
            set
            {
                GridAlternatingCellBackColor = Color.FromArgb(value);
            }
        }
        [XmlIgnore]
        public Color GridSelectedCellForeColor
        {
            get
            {
                return _GridSelectedCellForeColor;
            }
            set
            {
                if (_GridSelectedCellForeColor != value)
                {
                    _GridSelectedCellForeColor = value;
                    NotifyPropertyChanged("GridSelectedCellForeColor");
                }
            }
        }
        [XmlElement("GridSelectedCellForeColor")]
        public int GridSelectedCellForeColorArgb
        {
            get
            {
                return GridSelectedCellForeColor.ToArgb();
            }
            set
            {
                GridSelectedCellForeColor = Color.FromArgb(value);
            }
        }
        [XmlIgnore]
        public Color GridSelectedCellBackColor
        {
            get
            {
                return _GridSelectedCellBackColor;
            }
            set
            {
                if (_GridSelectedCellBackColor != value)
                {
                    _GridSelectedCellBackColor = value;
                    NotifyPropertyChanged("GridSelectedCellBackColor");
                }
            }
        }
        [XmlElement("GridSelectedCellBackColor")]
        public int GridSelectedCellBackColorArgb
        {
            get
            {
                return GridSelectedCellBackColor.ToArgb();
            }
            set
            {
                GridSelectedCellBackColor = Color.FromArgb(value);
            }
        }
        #endregion
        #region Init & Write
        public void InitThis()
        {
            //XML 문서 경로
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folderPath = Path.Combine(folderPath, Application.ProductName);
            string fileName = "ApplicationColor.Xml";
            fileName = Path.Combine(folderPath, fileName);
            FileInfo xmlFile = new FileInfo(fileName);
            if (xmlFile.Exists)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ApplicationColor));
                XmlReader reader = XmlReader.Create(xmlFile.FullName);
                _Instance = (ApplicationColor)serializer.Deserialize(reader);
                reader.Close();
            }
        }
        public void Write()
        {
            //XML 문서 경로
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folderPath = Path.Combine(folderPath, Application.ProductName);
            string fileName = "ApplicationColor.Xml";
            fileName = Path.Combine(folderPath, fileName);
            FileInfo xmlFile = new FileInfo(fileName);
            if (Directory.Exists(folderPath) == false) Directory.CreateDirectory(folderPath);
            if (xmlFile.Exists) xmlFile.Delete();
            XmlSerializer serializer = new XmlSerializer(typeof(ApplicationColor));
            TextWriter writer = new StreamWriter(xmlFile.FullName);
            serializer.Serialize(writer, Instance);
            writer.Close();
        }
        #endregion
        #region INotifyPropertyChanged 멤버
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }
}
