using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;

namespace GB.SIMEF.DAL
{
    public class DefinicionIndicadorDAL:BitacoraDAL
    {
        private  SIMEFContext db;
        #region Metodos Consulta Base de Datos
        /// <summary>
        /// Metodo que carga los registros de Definiciones según parametros
        /// fecha 03-08-2022
        /// Michael Hernandez
        /// </summary>
        /// <returns>Lista</returns>
        public List<DefinicionIndicador> ObtenerDatos(DefinicionIndicador objDefinicion)
        {



            var listaIndicador = ObtenerIndicadores();
            List<DefinicionIndicador> ListaDefiniciones = new List<DefinicionIndicador>();
            using (db = new SIMEFContext())
            {
                ListaDefiniciones = db.Database.SqlQuery<DefinicionIndicador>
                    ("execute spObtenerDefinicionesIndicador @idIndicador, @idEstado",
                        new SqlParameter("@idIndicador", objDefinicion.idIndicador),
                        new SqlParameter("@idEstado", objDefinicion.idEstado)
                    ).ToList();
            }
            ListaDefiniciones = ListaDefiniciones.Select(x => new DefinicionIndicador()
            {
                Definicion = x.Definicion,
                idIndicador = x.idIndicador,
                Fuente = x.Fuente,
                Notas = x.Notas,
                idEstado = x.idEstado,
                id=Utilidades.Encriptar(x.idIndicador.ToString()),
                Indicador = listaIndicador
                .Where(i => i.id==Utilidades.Encriptar(x.idIndicador.ToString()) ).Single(),
                NombreIndicador = listaIndicador
               .Where(i => i.id == Utilidades.Encriptar(x.idIndicador.ToString())).Single().Nombre,
            }).ToList();






            return ListaDefiniciones;
        }
        #endregion


        /// <summary>
        /// Michael Hernández C
        /// Actualiza e inserta 
        /// </summary>
        /// <param>objDefinicion</param>

        public List<DefinicionIndicador> ActualizarDatos(DefinicionIndicador objDefinicion)
        {
            List<DefinicionIndicador> Definiciones = new List<DefinicionIndicador>();

            using (db = new SIMEFContext())
            {
                Definiciones = db.Database.SqlQuery<DefinicionIndicador>
                ("execute " +
                "dbo.spActualizarDefinicionIndicador @Fuente,@Notas,@idIndicador,@idEstado,@Definicion",
                     new SqlParameter("@Fuente", objDefinicion.Fuente),
                     new SqlParameter("@Notas", objDefinicion.Notas),
                     new SqlParameter("@idIndicador", objDefinicion.idIndicador),
                     new SqlParameter("@idEstado", objDefinicion.idEstado),
                     new SqlParameter("@Definicion", objDefinicion.Definicion)
                    ).ToList();

            
            }
            var listaIndicador = ObtenerIndicadores();
            Definiciones = Definiciones.Select(x => new DefinicionIndicador()
            {
                Definicion = x.Definicion,
                idIndicador = x.idIndicador,
                Fuente = x.Fuente,
                Notas = x.Notas,
                idEstado = x.idEstado,
                id = Utilidades.Encriptar(x.idIndicador.ToString()),
                Indicador = listaIndicador
               .Where(i => i.id == Utilidades.Encriptar(x.idIndicador.ToString())).Single(),
                NombreIndicador  = listaIndicador
               .Where(i => i.id == Utilidades.Encriptar(x.idIndicador.ToString())).Single().Nombre,
            }).ToList();





            return Definiciones;
        }


        private List<Indicador> ObtenerIndicadores()
        {
            IndicadorFonatelDAL indicadorDal = new IndicadorFonatelDAL();

            return indicadorDal.ObtenerDatos(new Indicador()).ToList();


        }


    }
}
