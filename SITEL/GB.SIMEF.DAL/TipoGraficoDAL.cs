using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GB.SIMEF.DAL
{
    public class TipoGraficoDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 04/04/2023
        /// Jenifer Mora Badilla
        /// Función que retorna todos los tipos  de graficos registrados en estado activo.
        /// Se puede filtrar por el ID del objecto
        /// </summary>
        /// <returns></returns>
        public List<GraficoInforme> ObtenerDatos(GraficoInforme pGraficoInforme)
        {
            List<GraficoInforme> listaGraficoInforme = new List<GraficoInforme>();

            using (db = new SIMEFContext())
            {
                if (pGraficoInforme.IdGraficoInforme != 0)
                {
                    listaGraficoInforme = db.GraficoInforme.Where(x => x.IdGraficoInforme == pGraficoInforme.IdGraficoInforme && x.Estado == true).ToList();
                }
                else
                {
                    listaGraficoInforme = db.GraficoInforme.Where(x => x.Estado == true).ToList();
                }
            }

            listaGraficoInforme = listaGraficoInforme.Select(x => new GraficoInforme()
            {
                id = Utilidades.Encriptar(x.IdGraficoInforme.ToString()),
                Nombre = x.Nombre,
                Estado = x.Estado
            }).ToList();

            return listaGraficoInforme;
        }
    }
}
