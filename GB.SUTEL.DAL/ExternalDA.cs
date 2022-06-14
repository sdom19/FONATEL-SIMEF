using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SUTEL.DAL.Communication;
using GB.SUTEL.Shared;
using GB.SUTEL.ExceptionHandler;

namespace GB.SUTEL.DAL
{
    /// <summary>
    /// External WS data access example
    /// </summary>
    /// <createdby>
    /// <name>Walter Montes, Marvin Zumbado</name>
    /// <contact>walter.montes@grupobabel.com; walmon93@hotmail.com, marvinzzz@gmail.com, marvin.zumbado@grupobabel.com</contact>
    /// </createdby>
    /// <created>January 13, 2014</created>
    public class ExternalDA : LocalContextualizer
    {
        WSHelper proxyHelper;
        public ExternalDA(ApplicationContext appContext)
            : base(appContext)
        {
            proxyHelper = new WSHelper(appContext);
        }
        public ServiceReference1.Facade_SampleEntity GetFromWS()
        {
            ServiceReference1.Facade_SampleEntity result = null;
            return proxyHelper.SafeExecution(() =>
            {
                try
                {
                    ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
                    var response = client.GetData(1);
                    if (response.IsFaulted)
                    {
                        throw AppContext.ExceptionBuilder.BuildException(response.ErrorMessage);
                    }
                    result = response.Value;
                    return result;
                }
                catch (WSException)
                {
                    // since its a WSEXception its safe
                    throw;
                }
                catch (Exception ex)
                {
                    throw AppContext.ExceptionBuilder.BuildException("error msg", ex);
                }
            });
        }
    }
}
