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
