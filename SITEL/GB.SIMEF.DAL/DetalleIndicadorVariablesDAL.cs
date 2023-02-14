using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class DetalleIndicadorVariablesDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 01/09/2022
        /// José Navarro Acuña
        /// Función que permite obtener los detalles variables dato de un indicador
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public List<DetalleIndicadorVariables> ObtenerDatos(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            List<DetalleIndicadorVariables> listaDetalles = new List<DetalleIndicadorVariables>();

            using (db = new SIMEFContext())
            {
                listaDetalles = db.Database.SqlQuery<DetalleIndicadorVariables>
                    ("execute spObtenerDetallesIndicadorVariables @pIdDetalleIndicador,@pIdIndicador ",
                     new SqlParameter("@pIdDetalleIndicador", pDetalleIndicadorVariables.idDetalleIndicador),
                     new SqlParameter("@pIdIndicador", pDetalleIndicadorVariables.idIndicador)
                    ).ToList();

                listaDetalles = listaDetalles.Select(x => new DetalleIndicadorVariables()
                {
                    id = Utilidades.Encriptar(x.idDetalleIndicador.ToString()),
                    idIndicadorString = Utilidades.Encriptar(x.idIndicador.ToString()),
                    NombreVariable = x.NombreVariable,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado
                }).ToList();
            }
            return listaDetalles;
        }

        /// <summary>
        /// 14/02/2022
        /// José Navarro Acuña
        /// Función que permite obtener los detalles variables dato de un indicador que no estan en uso por formulas de calculo.
        /// A su vez, permite hacer una excepcion por una fórmula (es decir, incluir el registro de una fórmula, esto es útil cuando se edita)
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <param name="pIdFormula"></param>
        /// <returns></returns>
        public List<DetalleIndicadorVariables> ObtenerVariablesSinUsoEnFormula(DetalleIndicadorVariables pDetalleIndicadorVariables, int pIdFormula)
        {
            List<DetalleIndicadorVariables> listaDetalles = new List<DetalleIndicadorVariables>();

            using (db = new SIMEFContext())
            {
                listaDetalles = db.Database.SqlQuery<DetalleIndicadorVariables>
                    ("execute spObtenerVariablesSinUsoEnFormula @pIdIndicador, @pIdFormula",
                     new SqlParameter("@pIdIndicador", pDetalleIndicadorVariables.idIndicador),
                     new SqlParameter("@pIdFormula", pIdFormula)
                    ).ToList();

                listaDetalles = listaDetalles.Select(x => new DetalleIndicadorVariables()
                {
                    id = Utilidades.Encriptar(x.idDetalleIndicador.ToString()),
                    idIndicadorString = Utilidades.Encriptar(x.idIndicador.ToString()),
                    NombreVariable = x.NombreVariable,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado
                }).ToList();
            }
            return listaDetalles;
        }

        /// <summary>
        /// 12/09/2022
        /// José Navarro Acuña
        /// Función que insertar o actualizar un detalle de variable de un indicador
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public List<DetalleIndicadorVariables> ActualizarDatos(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            List<DetalleIndicadorVariables> listaDetalles = new List<DetalleIndicadorVariables>();

            using (db = new SIMEFContext())
            {
                listaDetalles = db.Database.SqlQuery<DetalleIndicadorVariables>
                    ("execute spActualizarIndicadorVariable @pIdDetalleIndicador, @pIdIndicador, @pNombreVariable, @pDescripcion, @pEstado ",
                     new SqlParameter("@pIdDetalleIndicador", pDetalleIndicadorVariables.idDetalleIndicador),
                     new SqlParameter("@pIdIndicador", pDetalleIndicadorVariables.idIndicador),
                     new SqlParameter("@pNombreVariable", pDetalleIndicadorVariables.NombreVariable),
                     new SqlParameter("@pDescripcion", pDetalleIndicadorVariables.Descripcion),
                     new SqlParameter("@pEstado", pDetalleIndicadorVariables.Estado)
                    ).ToList();
            }

            listaDetalles = listaDetalles.Select(x => new DetalleIndicadorVariables()
            {
                id = Utilidades.Encriptar(x.idDetalleIndicador.ToString()),
                NombreVariable = x.NombreVariable,
                Descripcion = x.Descripcion,
                Estado = x.Estado
            }).ToList();

            return listaDetalles;
        }
    }
}
