using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using GB.SUTEL.BL.FormCumplimientoPorcenBL;
using GB.SUTEL.Entities.FormCumplimientoPorcenEnti;
using System.Timers;
using System.IO;
using System.Threading;

namespace GB.SUTEL.Reglas
{
    public partial class Service1 : ServiceBase
    {
        //Timer timer = new Timer(); // name space(using System.Timers;) 
        //public readonly UtilMotor oUtil;
        private Thread executeThread;
        public Service1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Se inicia el servicio en modo debug
        /// </summary>
        public void Debug()
        {
            ActivaMotor();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                UtilMotor.WriteToFile("Iniciando servicio " + DateTime.Now);
                executeThread = new Thread(new ThreadStart(procesoTimer));
                executeThread.Start();
            }
            catch (Exception ex)
            {
            }
        }

        public void procesoTimer()
        {

            try
            {
                try
                {
                    while (true)
                    {
                        UtilMotor.WriteToFile("Revisión Periódica " + DateTime.Now);
                        ActivaMotor();
                        Thread.Sleep(3600 * 1000);//1 hora a milisegundos --Alternativa qeu no funcionó TimeSpan.FromHours(1)
                    }
                }
                catch (Exception e)
                {
                    UtilMotor.WriteToFile(e.Message + "  " + DateTime.Now);
                }
            }
            catch (ThreadAbortException tx)
            {
            }

        }
        public void ActivaMotor()
        {
            Orquestador procesoCalc = new Orquestador();
            procesoCalc.EjecutarMotor();
        }

        protected override void OnStop()
        {
            UtilMotor.WriteToFile("Servicio detenido a las " + DateTime.Now);
        }
    }
}
