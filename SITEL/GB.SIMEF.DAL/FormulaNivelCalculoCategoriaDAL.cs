using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;


namespace GB.SIMEF.DAL
{
    public class FormulaNivelCalculoCategoriaDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 24/10/2022
        /// José Navarro Acuña
        /// Permite insertar un detalle de nivel de cálculo a una fórmula
        /// </summary>
        /// <param name="pFormulaNivelCalculoCategoria"></param>
        /// <returns></returns>
        public List<FormulasNivelCalculoCategoria> InsertarFormulaNivelCalculoCategoria(int pIdFormula, List<FormulasNivelCalculoCategoria> pListadoNivelesCalculo)
        {
            List<FormulasNivelCalculoCategoria> detallesFormulas = new List<FormulasNivelCalculoCategoria>();

            using (db = new SIMEFContext())
            {
                for (int i = 0; i < pListadoNivelesCalculo.Count; i++)
                {
                    FormulasNivelCalculoCategoria detalle = db.FormulasNivelCalculoCategoria.Add(
                        new FormulasNivelCalculoCategoria()
                        {
                            IdFormula = pIdFormula,
                            IdCategoria = pListadoNivelesCalculo[i].IdCategoria,
                        });

                    detallesFormulas.Add(
                        new FormulasNivelCalculoCategoria()
                        {
                            id = Utilidades.Encriptar(detalle.IdFormulaNivel.ToString()),
                            IdFormulaString = Utilidades.Encriptar(pIdFormula.ToString()),
                            IdCategoriaString = Utilidades.Encriptar(detalle.IdCategoria.ToString())
                        });
                }
                db.SaveChanges();
            }

            return detallesFormulas;
        }

        /// <summary>
        /// 24/10/2022
        /// José Navarro Acuña
        /// Eliminar los detalles de nivel de cálculo de una fórmula por medio la fórmula y la categoria
        /// </summary>
        /// <param name="detalleFecha"></param>
        public void EliminarFormulaNivelCalculoCategoriaPorIDFormula(int pIdFormula)
        {
            using (db = new SIMEFContext())
            {
                List<FormulasNivelCalculoCategoria> detallesFormula = db.FormulasNivelCalculoCategoria
                    .Where(d => d.IdFormula == pIdFormula).ToList();

                if (detallesFormula.Count > 0)
                {
                    for (int i = 0; i < detallesFormula.Count; i++)
                    {
                        db.FormulasNivelCalculoCategoria.Remove(detallesFormula[i]);
                    }
                    db.SaveChanges();
                }
            }
        }
    }
}
