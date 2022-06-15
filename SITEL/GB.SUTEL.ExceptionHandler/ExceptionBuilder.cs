using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.ExceptionHandler
{
    /// <summary>
    /// Builds exceptions based on existing data
    /// </summary>
    /// <createdby>
    /// <name>Walter Montes, Marvin Zumbado</name>
    /// <contact>walter.montes@grupobabel.com; walmon93@hotmail.com, marvinzzz@gmail.com, marvin.zumbado@grupobabel.com</contact>
    /// </createdby>
    /// <created>January 13, 2014</created>>
    public class ExceptionBuilder
    {
        string CurrentUser { get; set; }
        string MachineName { get; set; }
        ExceptionType Layer { get; set; }
        public string ApplicationName { get; private set; }

        public ExceptionBuilder(string currentUser, string machineName, string applicationName, ExceptionType layer)
        {
            this.MachineName = machineName;
            this.CurrentUser = currentUser;
            this.Layer = layer;
            this.ApplicationName = applicationName;
        }

        public CustomException BuildException(string message, Exception exception)
        {
            switch (Layer)
            {
                case ExceptionType.Business:
                    return BuildBusinessException(message, exception);
                case ExceptionType.DataAccess:
                    return BuildDataAccessException(message, exception);
                case ExceptionType.Presentation:
                    return BuildUIException(message, exception);
                case ExceptionType.WS:
                    return BuildWSException(message, exception);
                default:
                    throw new NotImplementedException();
            }
        }
        public CustomException BuildException(string message)
        {
            switch (Layer)
            {
                case ExceptionType.Business:
                    return new BusinessException(message, CurrentUser, MachineName, ApplicationName);
                case ExceptionType.DataAccess:
                    return new DataAccessException(message, CurrentUser, MachineName, ApplicationName);
                case ExceptionType.Presentation:
                    return new UIException(message, CurrentUser, MachineName, ApplicationName);
                case ExceptionType.WS:
                    return new WSException(message, CurrentUser, MachineName, ApplicationName);
                default:
                    throw new NotImplementedException();
            }
        }

        public BusinessException BuildBusinessException(string message, Exception exception)
        {
            return new BusinessException(message, exception, CurrentUser, MachineName, ApplicationName);
        }
        public DataAccessException BuildDataAccessException(string message, Exception exception)
        {
            return new DataAccessException(message, exception, CurrentUser, MachineName, ApplicationName);
        }
        public WSException BuildWSException(string message, Exception exception)
        {
            return new WSException(message, exception, CurrentUser, MachineName, ApplicationName);
        }
        public UIException BuildUIException(string message, Exception exception)
        {
            return new UIException(message, exception, CurrentUser, MachineName, ApplicationName);
        }

    }
}
