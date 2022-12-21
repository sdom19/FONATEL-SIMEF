using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using System.Configuration;

namespace GB.SIMEF.Excel
{
    class Program
    {
        static void Main(string[] args)
        {

            RespuestaConsulta<List<DatoHistorico>> result = null;
            DirectoryInfo di = new DirectoryInfo(ConfigurationManager.AppSettings["RutaCarpeta"]);
            DatosHistoricosBL historicoBl = new DatosHistoricosBL("Carga Historico", "Excel");
            foreach (var archivo in di.GetFiles())
            {
                DataSet ds;
                DatoHistorico datoHistorio = null;
                string ruta = archivo.FullName;
                using var stream = new FileStream(ruta, FileMode.Open, FileAccess.Read, FileShare.ReadWrite) { };

                var sw = new Stopwatch();
                sw.Start();

                using IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
                {
                    var openTiming = sw.ElapsedMilliseconds;
                    // reader.IsFirstRowAsColumnNames = firstRowNamesCheckBox.Checked;
                    ds = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        UseColumnDataType = false,
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                    });
                    int cantidadIndicadores = ds.Tables.Count;

                    for (int tabla = 0; tabla < cantidadIndicadores; tabla++)
                    {
                        datoHistorio = new DatoHistorico();
                        datoHistorio.NombrePrograma = ds.Tables[tabla].TableName;
                        datoHistorio.CantidadFilas = ds.Tables[tabla].Rows.Count;
                        datoHistorio.CantidadColumnas = ds.Tables[tabla].Columns.Count;
                        datoHistorio.DetalleDatoHistoricoColumna = new List<DetalleDatoHistoricoColumna>();
                        datoHistorio.DetalleDatoHistoricoFila = new List<DetalleDatoHistoricoFila>();


                        for (int columna = 0; columna < datoHistorio.CantidadColumnas; columna++)
                        {
                            datoHistorio.DetalleDatoHistoricoColumna.Add(new DetalleDatoHistoricoColumna()
                            {
                                Nombre = ds.Tables[tabla].Rows[0][columna].ToString(),
                                NumeroColumna = columna
                            });

                            for (int fila = 1; fila < datoHistorio.CantidadFilas; fila++)
                            {
                                datoHistorio.DetalleDatoHistoricoFila.Add(new DetalleDatoHistoricoFila()
                                {
                                    Atributo = ds.Tables[tabla].Rows[fila][columna].ToString(),
                                    NumeroFila = fila,
                                    NumeroColumna = columna
                                });
                            }
                        }
                         result= historicoBl.ActualizarElemento(datoHistorio);
                        stream.Close();

                    }

                }
                if (result.HayError == 0)
                {
                    Console.WriteLine("Se Carga el indicador " + datoHistorio.NombrePrograma + " Cantidad de columnas " + datoHistorio.CantidadColumnas + " cantidad de filas " + datoHistorio.CantidadFilas);
                    System.IO.File.Delete(ruta);
                }
                else
                {
                    Console.WriteLine("Error en el indicador " + datoHistorio.NombrePrograma + " error" + result.MensajeError);
                }
            }
        }
        


        private static IList<string> GetTablenames(DataTableCollection tables)
        {
            var tableList = new List<string>();
            foreach (var table in tables)
            {
                tableList.Add(table.ToString());
            }

            return tableList;
        }
    }
}
