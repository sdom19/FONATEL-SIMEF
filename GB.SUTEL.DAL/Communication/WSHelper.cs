using System;
using System.ServiceModel;
using System.Threading.Tasks;

using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Shared;

namespace GB.SUTEL.DAL.Communication
{
    /// <summary>
    /// Secure web services calls
    /// </summary>
    /// <createdby>
    /// <name>Walter Montes, Marvin Zumbado</name>
    /// <contact>walter.montes@grupobabel.com; walmon93@hotmail.com, marvinzzz@gmail.com, marvin.zumbado@grupobabel.com</contact>
    /// </createdby>
    /// <created>January 13, 2014</created>
    internal class WSHelper : LocalContextualizer
    {
        public WSHelper(ApplicationContext appContext) : base(appContext) { }
        public void SafeExecution(Action action)
        {
            SafeExecution(() => { action(); return; });
        }

        public T SafeExecution<T>(Func<T> action)
        {
            try { return action(); }
            catch (WSException) { throw; }
            catch (CommunicationException tEx)
            {
                throw this.AppContext.ExceptionBuilder.BuildException("custom message", tEx);
            }
            catch (TimeoutException tEx)
            {
                throw this.AppContext.ExceptionBuilder.BuildException("custom message", tEx);
            }
        }
    }
}
