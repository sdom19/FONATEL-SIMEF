using GB.SUTEL.Shared;
using GB.SUTEL.Entities;

namespace GB.SUTEL.BL
{
    /// <summary>
    /// Locally initialize Data access files
    /// </summary>
    /// <createdby>
    /// <name>Walter Montes, Marvin Zumbado</name>
    /// <contact>walter.montes@grupobabel.com; walmon93@hotmail.com, marvinzzz@gmail.com, marvin.zumbado@grupobabel.com</contact>
    /// </createdby>
    /// <created>January 13, 2014</created>
    public class LocalContextualizer : ContextualizedClass
    {
        /// <summary>
        /// Let's give it even more context!
        /// </summary>
        /// <param name="appContext"></param>
        public LocalContextualizer(ApplicationContext appContext)
        {
            base.AppContext = new ApplicationContext(appContext, ExceptionHandler.ExceptionType.Business);
        }
    }
}
