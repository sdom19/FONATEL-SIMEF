using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System;
using System.Data.SqlClient;

namespace GB.SUTEL.ExceptionHandler
{
    /// <summary>
    /// Implementation of ILogExceptions, to register errors remotely or locally
    /// </summary>
    /// <createdby>
    /// <name>Walter Montes, Marvin Zumbado</name>
    /// <contact>walter.montes@grupobabel.com; walmon93@hotmail.com, marvinzzz@gmail.com, marvin.zumbado@grupobabel.com</contact>
    /// </createdby>
    /// <created>January 13, 2014</created>
    internal class Log : ILogException
    {
        public void LogException(ExceptionType exceptionType, string message, Guid id, ExceptionLevel level, Exception cEx
            , string currentUser, string currentMachineName, string applicationName)
        {
            string innerException = cEx.InnerException == null ? "InnerException no registrado" : cEx.InnerException.ToString();
            string stackTrace = cEx.StackTrace == null ? "Stacktrace no registrado" : cEx.StackTrace.ToString();
            try
            {
                //throw new Exception("custom ex");
                if (!LogToDB(exceptionType.ToString(), level.ToString(), cEx.Message, innerException, stackTrace, id, currentUser, currentMachineName, applicationName))
                {
                    // we tried to log it to db, but we cant reach it
                    // inside LogToDB we log the communication error locally, but outside that NEW error, we still need to log the original exception
                    LogExceptionLocally(applicationName, String.Format("Id: {3} | Mensaje: {0} | InnerException: {1} | Stacktrace: {2}", cEx.Message, innerException, stackTrace, id));
                }
            }
            catch (Exception ex)
            {
                // log the NEW Exception
                LogExceptionLocally(applicationName, String.Format("Id: {3} | Mensaje: {0} | InnerException: {1} | Stacktrace: {2}", cEx.Message,
                    ex.InnerException == null ? "InnerException no registrado" : ex.InnerException.ToString(), ex.StackTrace == null ? "" : ex.StackTrace.ToString(), Guid.NewGuid()));
                // log the original Exception
                LogExceptionLocally(applicationName, String.Format("Id: {3} | Mensaje: {0} | InnerException: {1} | Stacktrace: {2}", cEx.Message, innerException, stackTrace, id));
            }
        }
        bool LogToDB(string exceptionType, string exceptionLevel, string message, string innerException, string stackTrace, Guid id, 
            string currentUser, string currentMachineName, string applicationName)
        {
            var result = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SUTEL_IndicadoresEntities"].ConnectionString))
                {
                    conn.Open();

                    SqlCommand command = conn.CreateCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "pa_RegistrarExcepcion";

                    command.Parameters.Add("@IDEXCEPCION", System.Data.SqlDbType.UniqueIdentifier).Value = id;
                    command.Parameters.Add("@NOMBREAPLICACION", System.Data.SqlDbType.VarChar).Value = applicationName;
                    command.Parameters.Add("@NOMBRESERVIDOR", System.Data.SqlDbType.VarChar).Value = currentMachineName;
                    command.Parameters.Add("@NombreUsuario", System.Data.SqlDbType.VarChar).Value = currentUser;
                    command.Parameters.Add("@NIVELEXCEPCION", System.Data.SqlDbType.VarChar).Value = exceptionLevel;
                    command.Parameters.Add("@CAPA", System.Data.SqlDbType.VarChar).Value = exceptionType;
                    command.Parameters.Add("@MENSAJE", System.Data.SqlDbType.VarChar).Value = message;
                    command.Parameters.Add("@STACKTRACE", System.Data.SqlDbType.VarChar).Value = stackTrace;
                    command.Parameters.Add("@INNEREXCEPTION", System.Data.SqlDbType.VarChar).Value = innerException;

                    if (command.ExecuteNonQuery() != -1)
                    {
                        result = false;
                    }
                }

            }
            catch (Exception ex)
            {
                // if code reaches this point, its because there is a NEW error, reaching the database, so we log it and set the flag
                // 'result' to false, because we need to log the ORIGINAL error locally, just like this one
                result = false;
                LogExceptionLocally(applicationName, String.Format("Id: {3} | Mensaje: {0} | InnerException: {1} | Stacktrace: {2}", ex.Message,
                    ex.InnerException == null ? "InnerException no registrado" : ex.InnerException.ToString(), ex.StackTrace == null ? "" : ex.StackTrace.ToString(), Guid.NewGuid()));
                // log locally!!!
            }
            return result;
        }
        void LogExceptionLocally(string applicationName, string exception)
        {
            try
            {
                // Check to see fi the log for AspNetError exists on the machine
                //          If not, create it
                if ((!(EventLog.SourceExists(applicationName))))
                {
                    EventLog.CreateEventSource(applicationName, applicationName);
                }

                // Now insert your exception information into the AspNetError event log
                EventLog logEntry = new EventLog();
                logEntry.Source = applicationName;
                logEntry.WriteEntry(exception, EventLogEntryType.Error);
            }
            catch (System.Exception)
            {
            }

        }
    }
}
