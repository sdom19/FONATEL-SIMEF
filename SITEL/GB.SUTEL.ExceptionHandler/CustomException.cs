using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.ExceptionHandler
{

    /// <summary>
    /// Custom Base Exception
    /// Every custom exception should inherit from this class
    /// </summary>
    /// <createdby>
    /// <name>Walter Montes, Marvin Zumbado</name>
    /// <contact>walter.montes@grupobabel.com; walmon93@hotmail.com, marvinzzz@gmail.com, marvin.zumbado@grupobabel.com</contact>
    /// </createdby>
    /// <created>January 13, 2014</created>
    public class CustomException : Exception
    {
        string CurrentUser { get; set; }
        public Guid Id { get; set; }
        ExceptionLevel Level { get; set; }
        ILogException logger = new Log();

        internal CustomException(ExceptionType exceptionType, string message, Exception exception,
            string currentUser, string currentMachineName, string applicationName)
            : base(message, exception)
        {
            Id = Guid.NewGuid();

            if (exception is Exception)
                Level = ExceptionLevel.Error;

            if (exception is CustomException)
                Level = ExceptionLevel.Manual;

            if (exception is AccessViolationException)
                Level = ExceptionLevel.Critical;

            if (exception is IndexOutOfRangeException || exception is NullReferenceException
                || exception is ArgumentException || exception is ArgumentNullException)
                Level = ExceptionLevel.DevelopmentError;

            if (exception is InvalidOperationException)
                Level = ExceptionLevel.Error;


            CurrentUser = currentUser;

            if (exception != null)
                logger.LogException(exceptionType, message, Id, Level, exception, currentUser, currentMachineName, applicationName);
        }
        internal CustomException(ExceptionType exceptionType, string message, string currentUser, string currentMachineName, string applicationName)
            : this(exceptionType, message, null, currentUser, currentMachineName, applicationName)
        {

        }
    }

    /// <summary>
    /// Defines what type of exception it is
    /// </summary>
    public enum ExceptionType
    {
        DataAccess,
        Business,
        WS,
        Presentation
    }
    public enum ExceptionLevel
    {
        Error,
        Information,
        Manual,
        DevelopmentError,
        Critical
    }
}
