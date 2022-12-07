﻿using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.BL
{
    public class ServicioSitelBL
    {
        private readonly ServicioSitelDAL servicioSitelDAL;

        public ServicioSitelBL()
        {
            servicioSitelDAL = new ServicioSitelDAL();
        }

        /// <summary>
        /// 24/11/2022
        /// José Navarro Acuña
        /// Función que retorna todos los servicio registrados en estado activo de SITEL
        /// Se puede aplicar un filtro para obtener un único elemento a traves del ID.
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<ServicioSitel>> ObtenerDatos(ServicioSitel pServicioSitel)
        {
            RespuestaConsulta<List<ServicioSitel>> resultado = new RespuestaConsulta<List<ServicioSitel>>();

            try
            {
                if (!string.IsNullOrEmpty(pServicioSitel.id))
                {
                    int.TryParse(Utilidades.Desencriptar(pServicioSitel.id), out int idDecencriptado);
                    pServicioSitel.IdServicio = idDecencriptado;
                }

                var result = servicioSitelDAL.ObtenerDatos(pServicioSitel);
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Constantes.Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }
    }
}