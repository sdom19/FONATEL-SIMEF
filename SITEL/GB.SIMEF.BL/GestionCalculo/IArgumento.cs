using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.BL.GestionCalculo
{
    /// <summary>
    /// 20/01/2023
    /// José Navarro Acuña
    /// Interface que define cómo construir un predicado SQL apartir de un argumento.
    /// Esta interfaz forma parte de una implemetación del patrón funcional Strategy.
    /// </summary>
    public interface IArgumento
    {
        string ConstruirPredicadoSQL(ArgumentoFormula pArgumentoFormula, FormulasCalculo pFormulasCalculo);
    }
}
