using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace GB.SUTEL.Reglas
{
    public static class UtilMotor
    {
        public static void WriteToFile(string Message)
        {
            Task.Factory.StartNew(() => {

                try
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\SutelMotorReglas_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
                    if (!File.Exists(filepath))
                    {
                        // Create a file to write to.   
                        using (StreamWriter sw = File.CreateText(filepath))
                        {
                            sw.WriteLine(DateTime.Now.ToString() + "_" + Message);
                        }
                    }
                    else
                    {
                        using (StreamWriter sw = File.AppendText(filepath))
                        {
                            sw.WriteLine(DateTime.Now.ToString() + "_" + Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteEventLog(Message, ex.Message + " _ " + ex.StackTrace);
                }

            });


           
        }

        private static void WriteEventLog(string mensaje1, string mensaje2)
        {
            try
            {
                EventLog eventLog1 = new EventLog();
                // Turn off autologging
                // create an event source, specifying the name of a log that
                // does not currently exist to create a new, custom log
                if (!System.Diagnostics.EventLog.SourceExists("SutelMotorReglas"))
                {
                    System.Diagnostics.EventLog.CreateEventSource(
                        "SutelMotorReglas", "LogMotor");
                }
                // configure the event log instance to use this source name
                eventLog1.Source = "Motor";
                eventLog1.Log = mensaje1 + " _ " + mensaje2;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
