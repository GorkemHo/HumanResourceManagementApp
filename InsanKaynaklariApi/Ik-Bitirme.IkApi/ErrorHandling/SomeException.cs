using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.IkApi.ErrorHandling
{
    public class SomeException : Exception
    {
        public SomeException() : base()
        {
        }
        public SomeException(string message) : base(message)
        {
        }
        public SomeException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
