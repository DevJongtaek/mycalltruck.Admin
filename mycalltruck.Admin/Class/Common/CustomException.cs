using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mycalltruck.Admin.Class.Common
{
    class CustomException : Exception
    {
        public CustomException(String Message) : base(Message)
        {
        }
    }
}
