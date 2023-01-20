using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.DAL
{
    public class FormulasCalculoDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 17/08/2022
        /// José Navarro Acuña
        /// Actualiza los datos e inserta por medio de merge
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public List<FormulasCalculo> ActualizarDatos(FormulasCalculo pFormulasCalculo)
        {
            List<FormulasCalculo> listaformulas = new List<FormulasCalculo>();
            using (db = new SIMEFContext())
            {
                listaformulas = db.Database.SqlQuery<FormulasCalculo>
                ("execute dbo.spActualizarFormulaCalculo " +
                " @pIdFormula, @pCodigo, @pNombre, @pIdIndicador, @pIdIndicadorVariable, @pFechaCalculo, @pDescripcion, @pIdFrecuencia, @pNivelCalculoTotal, @pUsuarioModificacion, @pUsuarioCreacion, @pIdEstado",
                     new SqlParameter("@pIdFormula", pFormulasCalculo.IdFormula),
                     new SqlParameter("@pCodigo", pFormulasCalculo.Codigo),
                     new SqlParameter("@pNombre", pFormulasCalculo.Nombre),
                     pFormulasCalculo.IdIndicador == 0 ?
                        new SqlParameter("@pIdIndicador", DBNull.Value)
                        :
                        new SqlParameter("@pIdIndicador", pFormulasCalculo.IdIndicador),

                     pFormulasCalculo.IdIndicadorVariable == 0 ?
                        new SqlParameter("@pIdIndicadorVariable", DBNull.Value)
                        :
                        new SqlParameter("@pIdIndicadorVariable", pFormulasCalculo.IdIndicadorVariable),

                     pFormulasCalculo.FechaCalculo == null ?
                        new SqlParameter("@pFechaCalculo", DBNull.Value)
                        :
                        new SqlParameter("@pFechaCalculo", pFormulasCalculo.FechaCalculo),

                     string.IsNullOrEmpty(pFormulasCalculo.Descripcion) ?
                        new SqlParameter("@pDescripcion", DBNull.Value.ToString())
                        :
                        new SqlParameter("@pDescripcion", pFormulasCalculo.Descripcion),

                     new SqlParameter("@pNivelCalculoTotal", pFormulasCalculo.NivelCalculoTotal),

                     pFormulasCalculo.IdFrecuencia == 0 ?
                        new SqlParameter("@pIdFrecuencia", DBNull.Value)
                        :
                        new SqlParameter("@pIdFrecuencia", pFormulasCalculo.IdFrecuencia),

                     string.IsNullOrEmpty(pFormulasCalculo.UsuarioCreacion) ?
                        new SqlParameter("@pUsuarioCreacion", DBNull.Value)
                        :
                        new SqlParameter("@pUsuarioCreacion", pFormulasCalculo.UsuarioCreacion),

                     string.IsNullOrEmpty(pFormulasCalculo.UsuarioModificacion) ?
                        new SqlParameter("@pUsuarioModificacion", DBNull.Value)
                        :
                        new SqlParameter("@pUsuarioModificacion", pFormulasCalculo.UsuarioModificacion),

                     new SqlParameter("@pIdEstado", pFormulasCalculo.IdEstado)
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
        /// 20/01/2023
        /// José Navarro Acuña
        /// Función que permite actualizar la etiqueta formula del objeto formula de calculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public FormulasCalculo ActualizarEtiquetaFormula(FormulasCalculo pFormulasCalculo)
        {
            FormulasCalculo _formula = new FormulasCalculo();

            using (db = new SIMEFContext())
            {
                db.FormulasCalculo.Attach(pFormulasCalculo);
                db.Entry(pFormulasCalculo).Property(x => x.Formula).IsModified = true;
                db.SaveChanges();
            }
            return _formula;
        }

        /// <summary>
        /// Listado de formulas 
        /// Michael Hernández C
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
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    IdEstado = x.IdEstado,
                    NivelCalculoTotal = x.NivelCalculoTotal,
                    IdFrecuenciaString = Utilidades.Encriptar(x.IdFrecuencia.ToString()),
                    IdIndicadorSalidaString = Utilidades.Encriptar(x.IdIndicador.ToString()),
                    IdVariableDatoString = Utilidades.Encriptar(x.IdIndicadorVariable.ToString()),
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.idEstado == x.IdEstado).Single(),
                    FechaCreacion = x.FechaCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    FechaCalculo = x.FechaCalculo,
                    Formula = x.Formula
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
