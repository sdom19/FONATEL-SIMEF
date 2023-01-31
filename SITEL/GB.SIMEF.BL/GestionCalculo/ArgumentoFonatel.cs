using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.BL.GestionCalculo
{
    public class ArgumentoFonatel : IArgumento
    {
        /// <summary>
        /// 20/01/2023
        /// José Navarro Acuña
        /// Función que permite construir el predicado SQL del argumento
        /// </summary>
        /// <returns></returns>
        public string ConstruirPredicadoSQL(ArgumentoFormula pArgumentoFormula)
        {


            FormulasVariableDatoCriterio formulasVariableDatoCriterio = (FormulasVariableDatoCriterio)pArgumentoFormula;

            //formulasVariableDatoCriterio.IdAcumulacion;



            if (formulasVariableDatoCriterio.EsValorTotal)
            {

            }

            return "";
        }
    }
}
