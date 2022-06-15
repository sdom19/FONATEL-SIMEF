using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.ExceptionHandler
{
    /// <summary>
    /// User Interface Exception
    /// </summary>
    /// <createdby>
    /// <name>Walter Montes, Marvin Zumbado</name>
    /// <contact>walter.montes@grupobabel.com; walmon93@hotmail.com, marvinzzz@gmail.com, marvin.zumbado@grupobabel.com</contact>
    /// </createdby>
    /// <created>January 13, 2014</created>
    public class UIException : CustomException
    {
        internal UIException(string message, Exception exception, string currentUser, string currentMachineName, string applicationName)
            : base(ExceptionType.Presentation, message, exception, currentUser, currentMachineName, applicationName)
        {

        }
        internal UIException(string message, string currentUser, string currentMachineName, string applicationName)
            : base(ExceptionType.Presentation, message, null, currentUser, currentMachineName, applicationName)
        {

        }
    }
}
