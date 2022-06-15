using GB.SUTEL.ExceptionHandler;

namespace GB.SUTEL.Shared
{   /// <summary>
    /// Gives the application a context
    /// </summary>
    /// <createdby>
    /// <name>Walter Montes, Marvin Zumbado</name>
    /// <contact>walter.montes@grupobabel.com; walmon93@hotmail.com, marvinzzz@gmail.com, marvin.zumbado@grupobabel.com</contact>
    /// </createdby>
    /// <created>January 13, 2014</created>
    public class ApplicationContext
    {
        /// <summary>
        /// Current Logged User
        /// </summary>
        public string CurrentUser { get; private set; }
        /// <summary>
        /// Current Machine Name (server)
        /// </summary>
        public string MachineName { get; private set; }
        /// <summary>
        /// Layer where the error ocurred
        /// </summary>
        public ExceptionType Layer { get; private set; }
        /// <summary>
        /// Current Application Name
        /// </summary>
        public string ApplicationName { get; private set; }

        /// <summary>
        /// Makes easy to build exceptions
        /// </summary>
        public ExceptionBuilder ExceptionBuilder { get; set; }

        /// <summary>
        /// Init, first to be created
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="applicationName"></param>
        public ApplicationContext(string userName, string applicationName)
        {
            this.MachineName = System.Environment.MachineName;
            this.CurrentUser = userName;
            this.ApplicationName = applicationName;
        }
        /// <summary>
        /// Especific for certain layer
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="applicationName"></param>
        /// <param name="layer"></param>
        public ApplicationContext(string userName, string applicationName, ExceptionType layer)
        {
            this.MachineName = System.Environment.MachineName;
            this.CurrentUser = userName;
            this.Layer = layer;
            this.ApplicationName = applicationName;

            ExceptionBuilder = new ExceptionBuilder(CurrentUser, System.Environment.MachineName, ApplicationName, Layer);
        }
        /// <summary>
        /// Must be used when is beign propagated
        /// </summary>
        /// <param name="appContext"></param>
        public ApplicationContext(ApplicationContext appContext)
        {
            this.CurrentUser = appContext.CurrentUser;
            this.MachineName = appContext.MachineName;
            this.Layer = appContext.Layer;
            this.ApplicationName = appContext.ApplicationName;
            ExceptionBuilder = new ExceptionBuilder(CurrentUser, System.Environment.MachineName, ApplicationName, appContext.Layer);
        }
        /// <summary>
        /// Must be used when is beign propagated
        /// </summary>
        /// <param name="appContext"></param>
        /// <param name="layer"></param>
        public ApplicationContext(ApplicationContext appContext, ExceptionType layer)
        {
            CurrentUser = appContext.CurrentUser;
            MachineName = appContext.MachineName;
            Layer = layer;
            ApplicationName = appContext.ApplicationName;
            ExceptionBuilder = new ExceptionBuilder(CurrentUser, System.Environment.MachineName, ApplicationName, Layer);
        }
    }
}
