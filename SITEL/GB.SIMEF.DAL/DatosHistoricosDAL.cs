using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class DatosHistoricosDAL : BitacoraDAL
    {
        private BDHistoricoContext db;

       /// <summary>
       /// Listado de años 
       /// </summary>
       /// <returns></returns>
        public List<DatoHistorico> ObtenerDatos( DatoHistorico objDato)
        {

           

            List<DatoHistorico> listaDatosHistoricos = new List<DatoHistorico>();

            using (db = new BDHistoricoContext())
            {

                listaDatosHistoricos = db.Database.SqlQuery<DatoHistorico>("exec spObtenerDatoHistorico  @idHistorico,@codigo,@arrayid",
                      new SqlParameter("@idHistorico", objDato.IdHistorico),
                      new SqlParameter("@codigo", string.IsNullOrEmpty(objDato.Codigo) ? DBNull.Value.ToString() : objDato.Codigo),
                     new SqlParameter("@arrayid", string.IsNullOrEmpty(objDato.id) ? DBNull.Value.ToString() : objDato.id))
                    .ToList();

                listaDatosHistoricos = listaDatosHistoricos.Select(x => new DatoHistorico()
                {
                    Codigo = x.Codigo,
                    CantidadColumnas = x.CantidadColumnas,
                    CantidadFilas = x.CantidadFilas,
                    NombrePrograma = x.NombrePrograma,
                    IdHistorico = x.IdHistorico,
                    FechaCarga = x.FechaCarga,
                    DetalleDatoHistoricoColumna = db.DetalleDatoHistoricoColumna.Where(i => i.IdHistorico == x.IdHistorico).ToList(),
                    DetalleDatoHistoricoFila=db.DetalleDatoHistoricoFila.Where(i=>i.IdDatoHistorico==x.IdHistorico).ToList(),
                    id=Utilidades.Encriptar(x.IdHistorico.ToString())
                }).ToList();
            }



          
            return listaDatosHistoricos;
        }



        public DatoHistorico AgregarDatos(DatoHistorico objHistorico)
        {
           DatoHistorico DatosHistoricosResult = new DatoHistorico();
            using (db = new BDHistoricoContext())
            {
                var DatosHistoricos = db.DatoHistorico.Add(objHistorico);
                db.SaveChanges();
                DatosHistoricosResult = DatosHistoricos;
                int idDatoHistorico = objHistorico.IdHistorico;



                DatosHistoricos.DetalleDatoHistoricoColumna= objHistorico.DetalleDatoHistoricoColumna.Select(x => new DetalleDatoHistoricoColumna()
                {
                    IdHistorico = idDatoHistorico,
                    NumeroColumna =x.NumeroColumna,
                    Nombre=x.Nombre
                }).ToList();


                foreach (var itemColumna in DatosHistoricos.DetalleDatoHistoricoColumna)
                {
                    var DetalleDatoHistoricoColumna = AgregarDatoColumna(itemColumna);
                    var listadoFilas = objHistorico.DetalleDatoHistoricoFila.Select(x => new DetalleDatoHistoricoFila()
                    {
                        IdDetalleColumna = DetalleDatoHistoricoColumna.IdDetalleDato,
                        NumeroFila = x.NumeroFila,
                        NumeroColumna = x.NumeroColumna,
                        Atributo = x.Atributo,
                        IdDatoHistorico= idDatoHistorico
                    }).Where(I=>I.NumeroColumna==DetalleDatoHistoricoColumna.NumeroColumna).ToList();

                    foreach (var itemfila in listadoFilas)
                    {
                        var detalleFila=AgregarDatoFila(itemfila);
                        DatosHistoricos.DetalleDatoHistoricoFila.Add(detalleFila);
                    }
                }
            }
            return DatosHistoricosResult;
        }


        private DetalleDatoHistoricoColumna AgregarDatoColumna(DetalleDatoHistoricoColumna objHistorico)
        {
            DateTime tipoFecha;

            if(DateTime.TryParse(objHistorico.Nombre,out tipoFecha))
            {
                objHistorico.Nombre = Utilidades.fechaColumna(tipoFecha);
            }
           
            var DatoHistoricoColumna = db.DetalleDatoHistoricoColumna.Add(objHistorico);
            db.SaveChanges();
            return DatoHistoricoColumna;
        }



        private DetalleDatoHistoricoFila AgregarDatoFila(DetalleDatoHistoricoFila objHistorico)
        {
            DateTime tipoFecha;
            if (DateTime.TryParse(objHistorico.Atributo, out tipoFecha))
            {
                objHistorico.Atributo = Utilidades.fechaSinHora(tipoFecha);
            }

            var datoHistoricoFila = db.DetalleDatoHistoricoFila.Add(objHistorico);
            db.SaveChanges();
            return datoHistoricoFila;
        }




    }
}
