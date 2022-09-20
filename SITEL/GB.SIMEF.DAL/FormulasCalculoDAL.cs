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
            List<FormulasCalculo> Listaformulas = new List<FormulasCalculo>();
            using (db = new SIMEFContext())
            {
                Listaformulas = db.Database.SqlQuery<FormulasCalculo>
                ("execute dbo.spActualizarFormulaCalculo " +
                " @IdFormula,@Codigo, @Nombre,@IdIndicador, @IdIndicadorVariable,@Descripcion,@NivelCalculoTotal,@IdFrecuencia,@UsuarioModificacion,@UsuarioCreacion,@IdEstado",
                     new SqlParameter("@IdFormula", objFormula.idFormula),
                     new SqlParameter("@Codigo", objFormula.Codigo),
                     new SqlParameter("@Nombre", objFormula.Nombre),
                     new SqlParameter("@IdIndicador", objFormula.IdIndicador),
                     new SqlParameter("@IdIndicadorVariable", objFormula.IdIndicadorVariable),
                     new SqlParameter("@Descripcion", objFormula.Descripcion),
                     new SqlParameter("@NivelCalculoTotal", objFormula.NivelCalculoTotal),
                     new SqlParameter("@IdFrecuencia", objFormula.IdFrecuencia),
                     new SqlParameter("@UsuarioCreacion", string.IsNullOrEmpty(objFormula.UsuarioCreacion) ? DBNull.Value.ToString() : objFormula.UsuarioCreacion),
                     new SqlParameter("@UsuarioModificacion", string.IsNullOrEmpty(objFormula.UsuarioModificacion) ? DBNull.Value.ToString() : objFormula.UsuarioModificacion),
                     new SqlParameter("@IdEstado", objFormula.IdEstado)

                    ).ToList();

              
            }
            return Listaformulas;
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
                     new SqlParameter("@IdFormula", pformulasCalculo.idFormula)
                    ).ToList();


                listaFormulasCalculo = listaFormulasCalculo.Select(x => new FormulasCalculo()
                {
                    id = Utilidades.Encriptar(x.idFormula.ToString()),
                    idFormula=x.idFormula,
                    Codigo=x.Codigo,
                    Nombre=x.Nombre,
                    Descripcion=x.Descripcion,
                    IdEstado=x.IdEstado,
                    NivelCalculoTotal=x.NivelCalculoTotal,
                    IdFrecuencia=x.IdFrecuencia,
                    IdIndicador=x.IdIndicador,
                    IdIndicadorVariable=x.IdIndicadorVariable,
                    EstadoRegistro=db.EstadoRegistro.Where(i=>i.idEstado==x.IdEstado).Single(),
                    FechaCreacion=x.FechaCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                }).ToList();
            }

            return listaFormulasCalculo;
        }

  
    }
}
