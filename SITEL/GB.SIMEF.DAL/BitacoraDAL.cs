using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.Entities;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.DAL
{
    public class BitacoraDAL
    {
        private SIMEFContext db;
        public void RegistrarError()
        {
            using (db=new SIMEFContext())
            {

            }
        }

        public void RegistrarBitacora(int accion, string usuario, string pantalla, string codigo )
        {
            Bitacora bitacora = new Bitacora()
            {
                Accion = accion,
                Usuario = usuario,
                Pantalla = pantalla,
                Codigo=codigo 
            };
            using (db = new SIMEFContext())
            {
                db.Bitacora.Add(bitacora);
                db.SaveChanges();
            }
        }




        /// <summary>
        /// Metodo que carga la bitacora por fecha, se toman 200 registros en modo desc 
        /// fecha 03-08-2022
        /// Michael Hernandez
        /// </summary>
        /// <returns>Lista</returns>
        public List<Bitacora> ObtenerDatos(Bitacora bitacora)
        {

            List<Bitacora> ListaBitacora= new List<Bitacora>();
            using (db = new SIMEFContext())
            {
                ListaBitacora = db.Database.SqlQuery<Bitacora>
                    ("execute spObtenerBitacora @fechaDesde, @fechaHasta ",
                     new SqlParameter("@fechaDesde", string.IsNullOrEmpty(bitacora.FechaDesde) ? DBNull.Value.ToString(): bitacora.FechaDesde),
                     new SqlParameter("@fechaHasta", string.IsNullOrEmpty(bitacora.FechaHasta) ? DBNull.Value.ToString() : bitacora.FechaHasta)
                    ).ToList();

                ListaBitacora = ListaBitacora.Select(x => new Bitacora()
                {
                    idBitacora=x.idBitacora,
                    Accion=x.Accion,
                    Codigo=x.Codigo,
                    Pantalla=x.Pantalla,
                    Usuario=x.Usuario,
                    Fecha=x.Fecha,
                    AccionNombre= Enum.GetName(typeof(Accion), x.Accion)
            }).ToList();
            }
            return ListaBitacora;
        }

    }
}
