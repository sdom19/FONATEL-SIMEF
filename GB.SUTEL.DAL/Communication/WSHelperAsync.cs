using System;
using System.ServiceModel;
using System.Threading.Tasks;

using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Shared;

namespace GB.SUTEL.DAL.Communication
{
    /// <summary>
    /// Secure async web services calls
    /// </summary>
    /// <createdby>
    /// <name>Walter Montes, Marvin Zumbado</name>
    /// <contact>walter.montes@grupobabel.com; walmon93@hotmail.com, marvinzzz@gmail.com, marvin.zumbado@grupobabel.com</contact>
    /// </createdby>
    /// <created>January 13, 2014</created>
    internal class WSHelperAsync : LocalContextualizer
    {
        public WSHelperAsync(ApplicationContext appContext) : base(appContext) { }
        public async Task SafeExecution(Action action)
        {
            await SafeExecution(() => { action(); return; });
        }

        public async Task<T> SafeExecution<T>(Func<Task<T>> action)
        {
            try { return await action(); }
            catch (WSException) { throw; }
            catch (CommunicationException tEx)
            {
                throw this.AppContext.ExceptionBuilder.BuildWSException("custom message", tEx);
            }
            catch (TimeoutException tEx)
            {
                throw this.AppContext.ExceptionBuilder.BuildWSException("custom message", tEx);
            }
        }
    }
}
