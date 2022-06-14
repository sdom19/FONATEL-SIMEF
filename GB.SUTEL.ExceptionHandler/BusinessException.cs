using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.ExceptionHandler
{
    /// <summary>
    ///Business Exception    /// 
    /// </summary>
    /// <createdby>
    /// <name>Walter Montes, Marvin Zumbado</name>
    /// <contact>walter.montes@grupobabel.com; walmon93@hotmail.com, marvinzzz@gmail.com, marvin.zumbado@grupobabel.com</contact>
    /// </createdby>
    /// <created>January 13, 2014</created>
    public class BusinessException : CustomException
    {
        internal BusinessException(string message, Exception exception, string currentUser, string currentMachineName, string applicationName)
            : base(ExceptionType.Business, message, exception, currentUser, currentMachineName, applicationName)
        {

        }
        public BusinessException(string message, string currentUser, string currentMachineName, string applicationName)
            : base(ExceptionType.Business, message, null, currentUser, currentMachineName, applicationName)
        {

        }
    }
}
