using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
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

        public Bitacora RegistrarBitacora(int accion, string usuario, string pantalla, string codigo, string valorActual="", string ValorAnterior="", string ValorInicial="" )
        {
            Bitacora bitacora = new Bitacora()
            {
                Accion = accion,
                Usuario = usuario,
                Pantalla = pantalla,
                Codigo = codigo.ToUpper(),
                ValorInicial=ValorInicial,
                ValorActual=valorActual,
                ValorAnterior=ValorAnterior
                
            };
            using (db = new SIMEFContext())
            {
                db.Bitacora.Add(bitacora);
                db.SaveChanges();
            }
            return bitacora;
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
                    ("execute pa_ObtenerBitacora @fechaDesde, @fechaHasta, @Pantalla, @Acciones, @Usuario ",
                     new SqlParameter("@fechaDesde", string.IsNullOrEmpty(bitacora.FechaDesde) ? DBNull.Value.ToString(): bitacora.FechaDesde),
                     new SqlParameter("@fechaHasta", string.IsNullOrEmpty(bitacora.FechaHasta) ? DBNull.Value.ToString() : bitacora.FechaHasta),
                     new SqlParameter("@Pantalla", string.IsNullOrEmpty(bitacora.Pantalla) ? DBNull.Value.ToString() : bitacora.Pantalla),
                     new SqlParameter("@Acciones", string.IsNullOrEmpty(bitacora.AccionNombre) ? DBNull.Value.ToString() : bitacora.AccionNombre),
                     new SqlParameter("@Usuario", string.IsNullOrEmpty(bitacora.Usuario) ? DBNull.Value.ToString() : bitacora.Usuario)
                    ).ToList();

                ListaBitacora = ListaBitacora.Select(x => new Bitacora()
                {
                    idBitacora = x.idBitacora,
                    Accion = x.Accion,
                    Codigo = x.Codigo,
                    Pantalla = x.Pantalla,
                    Usuario = x.Usuario,
                    Fecha = x.Fecha,
                    ValorActual = x.ValorActual,
                    ValorInicial = x.ValorInicial,
                    ValorAnterior = x.ValorAnterior,

                    ValorDiferencial = (x.Accion == (int)Accion.Editar || x.Accion == (int)Accion.Activar || x.Accion == (int)Accion.Desactivar) ? Utilidades.jsonDiff(x.ValorAnterior, x.ValorActual) : string.Empty,
                    AccionNombre = Enum.GetName(typeof(Accion), x.Accion)
                }).ToList();
            }
            return ListaBitacora;
        }

    }
}
