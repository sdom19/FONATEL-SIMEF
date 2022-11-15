using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.DAL
{
    public class FormulasCalculoDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// Actualiza los datos e inserta por medio de merge
        /// 17/08/2022
        /// michael Hernández C
        /// </summary>
        /// <param name="objFormula"></param>
        /// <returns></returns>
        public List<FormulasCalculo> ActualizarDatos(FormulasCalculo objFormula)
        {
            List<FormulasCalculo> listaformulas = new List<FormulasCalculo>();
            using (db = new SIMEFContext())
            {
                listaformulas = db.Database.SqlQuery<FormulasCalculo>
                ("execute dbo.spActualizarFormulaCalculo " +
                " @IdFormula,@Codigo, @Nombre,@IdIndicador, @IdIndicadorVariable,@Descripcion,@NivelCalculoTotal,@IdFrecuencia,@UsuarioModificacion,@UsuarioCreacion,@IdEstado",
                     new SqlParameter("@IdFormula", objFormula.IdFormula),
                     new SqlParameter("@Codigo", objFormula.Codigo),
                     new SqlParameter("@Nombre", objFormula.Nombre),
                     objFormula.IdIndicador == 0 ?
                        new SqlParameter("@IdIndicador", DBNull.Value)
                        :
                        new SqlParameter("@IdIndicador", objFormula.IdIndicador),

                     objFormula.IdIndicadorVariable == 0 ?
                        new SqlParameter("@IdIndicadorVariable", DBNull.Value)
                        :
                        new SqlParameter("@IdIndicadorVariable", objFormula.IdIndicadorVariable),

                     string.IsNullOrEmpty(objFormula.Descripcion) ?
                        new SqlParameter("@Descripcion", DBNull.Value.ToString())
                        :
                        new SqlParameter("@Descripcion", objFormula.Descripcion),

                     new SqlParameter("@NivelCalculoTotal", objFormula.NivelCalculoTotal),

                     objFormula.IdFrecuencia == 0 ?
                        new SqlParameter("@IdFrecuencia", DBNull.Value)
                        :
                        new SqlParameter("@IdFrecuencia", objFormula.IdFrecuencia),

                     string.IsNullOrEmpty(objFormula.UsuarioCreacion) ?
                        new SqlParameter("@UsuarioCreacion", DBNull.Value)
                        :
                        new SqlParameter("@UsuarioCreacion", objFormula.UsuarioCreacion),

                     string.IsNullOrEmpty(objFormula.UsuarioModificacion) ?
                        new SqlParameter("@UsuarioModificacion", DBNull.Value)
                        :
                        new SqlParameter("@UsuarioModificacion", objFormula.UsuarioModificacion),

                     new SqlParameter("@IdEstado", objFormula.IdEstado)
                    ).ToList();

                listaformulas = listaformulas.Select(x => new FormulasCalculo()
                {
                    id = Utilidades.Encriptar(x.IdFormula.ToString()),
                    IdFormula = x.IdFormula
                }).ToList();
            }
            return listaformulas;
        }

        /// <summary>
        /// Listado de formulas 
        /// </summary>
        /// <returns></returns>
        public List<FormulasCalculo> ObtenerDatos(FormulasCalculo pformulasCalculo)
        {
            List<FormulasCalculo> listaFormulasCalculo = new List<FormulasCalculo>();

            using (db = new SIMEFContext())
            {
                listaFormulasCalculo = db.Database.SqlQuery<FormulasCalculo>
                    ("execute spObtenerFormulasCalculo  @IdFormula",
                     new SqlParameter("@IdFormula", pformulasCalculo.IdFormula)
                    ).ToList();

                listaFormulasCalculo = listaFormulasCalculo.Select(x => new FormulasCalculo()
                {
                    id = Utilidades.Encriptar(x.IdFormula.ToString()),
                    IdFormula = x.IdFormula,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    IdEstado = x.IdEstado,
                    NivelCalculoTotal = x.NivelCalculoTotal,
                    IdFrecuencia = x.IdFrecuencia,
                    IdIndicador = x.IdIndicador,
                    IdIndicadorVariable = x.IdIndicadorVariable,
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.idEstado == x.IdEstado).Single(),
                    FechaCreacion = x.FechaCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    FechaCalculo = x.FechaCalculo
                }).ToList();
            }

            return listaFormulasCalculo;
        }

        /// <summary>
        /// 11/10/2022
        /// José Navarro Acuña
        /// Función que busca y retorna una lista de fórmulas de cálculo donde el Indicador proporcionado este relacionado
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        public List<FormulasCalculo> ObtenerDependenciasIndicadorConFormulasCalculo(int pIdIndicador)
        {
            List<FormulasCalculo> lista = new List<FormulasCalculo>();

            using (db = new SIMEFContext())
            {
                lista = db.Database.SqlQuery<FormulasCalculo>
                    ("execute spObtenerDependenciasIndicadorConFormulasCalculo @pIdIndicador",
                     new SqlParameter("@pIdIndicador", pIdIndicador)
                    ).ToList();

                lista = lista.Select(x => new FormulasCalculo()
                {
                    id = Utilidades.Encriptar(x.IdFormula.ToString()),
                    IdFormula = x.IdFormula,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    IdEstado = x.IdEstado,
                    NivelCalculoTotal = x.NivelCalculoTotal,
                    IdFrecuencia = x.IdFrecuencia,
                    IdIndicador = x.IdIndicador,
                    IdIndicadorVariable = x.IdIndicadorVariable,
                    FechaCreacion = x.FechaCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    FechaCalculo = x.FechaCalculo
                }).ToList();
            }

            return lista;
        }

        /// <summary>
        /// 21/10/2022
        /// José Navarro Acuña
        /// Función que permite buscar y verificar por código y nombre la existencia de una fórmula de calculo en estado diferente de eliminado
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public FormulasCalculo VerificarExistenciaFormulaPorCodigoNombre(FormulasCalculo pFormulasCalculo)
        {
            FormulasCalculo formulasCalculo = null;

            using (db = new SIMEFContext())
            {
                formulasCalculo = db.FormulasCalculo.Where(x =>
                        (x.Nombre.Trim().ToUpper().Equals(pFormulasCalculo.Nombre.Trim().ToUpper()) || x.Codigo.Trim().ToUpper().Equals(pFormulasCalculo.Codigo.Trim().ToUpper())) &&
                        x.IdFormula != pFormulasCalculo.IdFormula &&
                        x.IdEstado != (int)EstadosRegistro.Eliminado
                    ).FirstOrDefault();
            }
            return formulasCalculo;
        }
    }
}
