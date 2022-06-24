using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.ExceptionHandler
{
    /// <summary>
    /// Web Service Exception
    /// </summary>
    /// <createdby>
    /// <name>Walter Montes, Marvin Zumbado</name>
    /// <contact>walter.montes@grupobabel.com; walmon93@hotmail.com, marvinzzz@gmail.com, marvin.zumbado@grupobabel.com</contact>
    /// </createdby>
    /// <created>January 13, 2014</created>
    public class WSException : CustomException
    {
        internal WSException(string message, Exception exception, string currentUser, string currentMachineName, string applicationName)
            : base(ExceptionType.WS, message, exception, currentUser, currentMachineName, applicationName)
        {

        }
        internal WSException(string message, string currentUser, string currentMachineName, string applicationName)
            : base(ExceptionType.WS, message, null, currentUser, currentMachineName, applicationName)
        {

        }
    }
}
