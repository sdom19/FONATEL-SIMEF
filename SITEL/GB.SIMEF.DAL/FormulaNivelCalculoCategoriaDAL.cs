﻿using GB.SIMEF.Entities;
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
                            IdFormula = pIdFormula,
                            IdCategoria = pListadoNivelesCalculo[i].IdCategoria,
                        });

                    detallesFormulas.Add(
                        new FormulaNivelCalculoCategoria()
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
                FormulaNivelCalculoCategoria detalleFormula = db.FormulaNivelCalculoCategoria
                    .Where(d => d.IdFormula == pIdFormula).FirstOrDefault();

                if (detalleFormula != null)
                {
                    db.FormulaNivelCalculoCategoria.Remove(detalleFormula);
                    db.SaveChanges();
                }
            }
        }
    }
}