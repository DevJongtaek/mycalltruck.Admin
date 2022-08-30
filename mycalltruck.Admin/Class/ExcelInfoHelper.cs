using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mycalltruck.Admin.Class
{
    class ExcelInfoHelper
    {
        

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

        internal class SourceModel
        {
            public String ColumnIndex { get; set; }
            public int RowIndex { get; set; }
            public String Value { get; set; }
        }
    }
}
