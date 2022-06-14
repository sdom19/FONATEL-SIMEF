using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.ExceptionHandler
{
    /// <summary>
    /// Interface for Exceptions Logger
    /// </summary>
    /// <createdby>
    /// <name>Walter Montes, Marvin Zumbado</name>
    /// <contact>walter.montes@grupobabel.com; walmon93@hotmail.com, marvinzzz@gmail.com, marvin.zumbado@grupobabel.com</contact>
    /// </createdby>
    /// <created>January 13, 2014</created>
    public interface ILogException
    {
        void LogException(ExceptionType exceptionType, string message, Guid id, ExceptionLevel level, Exception cEx
            , string currentUser, string currentMachineName, string applicationName);
    }
}
