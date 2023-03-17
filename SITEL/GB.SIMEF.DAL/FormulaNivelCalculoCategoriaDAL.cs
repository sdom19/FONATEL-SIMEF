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
        /// 13/02/2023
        /// José Navarro Acuña
        /// Función que permite obtener las categorias asociadas al nivel de cálculo de una fórmula
        /// </summary>
        /// <param name="pIdFormula"></param>
        /// <returns></returns>
        public List<FormulaNivelCalculoCategoria> ObtenerDatos(int pIdFormula)
        {
            List<FormulaNivelCalculoCategoria> detallesFormulas = new List<FormulaNivelCalculoCategoria>();

            using (db = new SIMEFContext())
            {
                detallesFormulas = db.FormulaNivelCalculoCategoria.Where(x => x.IdFormulaCalculo == pIdFormula).ToList();
                detallesFormulas = detallesFormulas.Select(x => new FormulaNivelCalculoCategoria()
                {
                    id = Utilidades.Encriptar(x.IdFormulaNivelCalculoCategoria.ToString()),
                    IdFormulaString = Utilidades.Encriptar(pIdFormula.ToString()),
                    IdCategoriaString = Utilidades.Encriptar(x.IdCategoriaDesagregacion.ToString())
                }).ToList();
            }

            return detallesFormulas;
        }

        /// <summary>
        /// 24/10/2022
        /// José Navarro Acuña
        /// Permite insertar un detalle de nivel de cálculo a una fórmula
        /// </summary>
        /// <param name="pFormulaNivelCalculoCategoria"></param>
        /// <returns></returns>
        public List<FormulaNivelCalculoCategoria> InsertarFormulaNivelCalculoCategoria(int pIdFormula, List<FormulaNivelCalculoCategoria> pListadoNivelesCalculo)
        {
            List<FormulaNivelCalculoCategoria> detallesFormulas = new List<FormulaNivelCalculoCategoria>();

            using (db = new SIMEFContext())
            {
                for (int i = 0; i < pListadoNivelesCalculo.Count; i++)
                {
                    FormulaNivelCalculoCategoria detalle = db.FormulaNivelCalculoCategoria.Add(
                        new FormulaNivelCalculoCategoria()
                        {
                            IdFormulaCalculo = pIdFormula,
                            IdCategoriaDesagregacion = pListadoNivelesCalculo[i].IdCategoriaDesagregacion,
                        });

                    detallesFormulas.Add(
                        new FormulaNivelCalculoCategoria()
                        {
                            id = Utilidades.Encriptar(detalle.IdFormulaNivelCalculoCategoria.ToString()),
                            IdFormulaString = Utilidades.Encriptar(pIdFormula.ToString()),
                            IdCategoriaString = Utilidades.Encriptar(detalle.IdCategoriaDesagregacion.ToString())
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
                List<FormulaNivelCalculoCategoria> detallesFormula = db.FormulaNivelCalculoCategoria
                    .Where(d => d.IdFormulaCalculo == pIdFormula).ToList();

                if (detallesFormula.Count > 0)
                {
                    for (int i = 0; i < detallesFormula.Count; i++)
                    {
                        db.FormulaNivelCalculoCategoria.Remove(detallesFormula[i]);
                    }
                    db.SaveChanges();
                }
            }
        }
    }
}
