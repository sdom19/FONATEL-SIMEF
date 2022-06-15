using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Epplus
using System.Diagnostics;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;

using System.Drawing;

//SQL 
using System.Data.SqlClient;

//Necesaria para poder obtener ConfigurationManager
using System.Configuration;
using System.Threading;
using OfficeOpenXml.DataValidation;

namespace SutelSolution
{
    class Principal
    {

        //Quitar
        static string uRLSaveFormatXLS = "C:\\Sutel\\Repositorio";
     
      
      public static void Main(string[] args)
        {
            try
            {
                //Se obtiene el string de conexión a la base de datos 
                String connString = ConfigurationManager.ConnectionStrings["Name"].ConnectionString;

                BLArchivoExcel blArchivoExcel = new BLArchivoExcel();

                blArchivoExcel.CrearIndicadoresExcel(connString);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();

            }
          
        }


        //EL SIGUIENTE BLOQUE DE CÓDIGO ES PARA PRUEBAS DE ESTADÍSTICA!!!! NO TIENE NADA QUE VER CON EL CONSTRUCTOR DEL EXCEL.

        //List<double> listaValores = new List<double>();
        //listaValores.Add(470000);
        //listaValores.Add(490000);
        //listaValores.Add(500000);
        //listaValores.Add(493820);
        //listaValores.Add(445032);
        //listaValores.Add(501293);

        //listaValores.Add(8020);
        //listaValores.Add(10000);
        //listaValores.Add(2000);
        //listaValores.Add(5002);
        //listaValores.Add(4939);
        //listaValores.Add(3040);

        //double media = listaValores.Sum() / listaValores.Count;

        //double desviacionEstandar = ObtenerDesviacionEstandar(listaValores, media);

        //var superior = media + (desviacionEstandar * 2);
        //var inferior = media - (desviacionEstandar * 2);

      //public static double ObtenerDesviacionEstandar(List<double> valoresIniciales, double media)
      //{


      //    List<double> valoresInicialesMenosMedia = new List<double>();
      //    List<double> valoresInicialesMenosMediaAlCuadrado = new List<double>();
      //    double sumatoriaValoresIniciales = new double();
      //    double sumatoriaValoresInicialesMenosMediaAlCuadrado = new double();

      //    sumatoriaValoresIniciales = valoresIniciales.Sum();

      //    foreach (var item in valoresIniciales)
      //    {
      //        valoresInicialesMenosMedia.Add(item - media);
      //    }

      //    foreach (var item in valoresInicialesMenosMedia)
      //    {
      //        valoresInicialesMenosMediaAlCuadrado.Add((item * item));
      //    }

      //    sumatoriaValoresInicialesMenosMediaAlCuadrado = valoresInicialesMenosMediaAlCuadrado.Sum();

      //    // se usa la fórmula Muestral NO la Poblacional. Por eso el -1

      //    var varianza = sumatoriaValoresInicialesMenosMediaAlCuadrado / (valoresInicialesMenosMediaAlCuadrado.Count - 1);

      //    var desviacionEstandar = Math.Sqrt(varianza);

      //    return desviacionEstandar;

      //}



     
      
   
    }
}
