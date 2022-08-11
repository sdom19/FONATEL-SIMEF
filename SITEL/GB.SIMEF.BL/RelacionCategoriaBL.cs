﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class RelacionCategoriaBL : IMetodos<RelacionCategoria>
    {
        private readonly RelacionCategoriaDAL clsDatos;


        private RespuestaConsulta<List<RelacionCategoria>> ResultadoConsulta;
        string modulo = EtiquetasViewRelacionCategoria.RelacionCategoria;

        public RelacionCategoriaBL()
        {
            this.clsDatos = new RelacionCategoriaDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<RelacionCategoria>>();
        }

        public RespuestaConsulta<RelacionCategoria> ActualizarElemento(RelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<RelacionCategoria> CambioEstado(RelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<RelacionCategoria> ClonarDatos(RelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<RelacionCategoria>> EliminarElemento(RelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<RelacionCategoria> InsertarDatos(RelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Fecha 10/08/2022
        /// Francisco Vindas Ruiz
        /// Metodo para obtener la lista Relacion Categorias
        /// </summary>
        public RespuestaConsulta<List<RelacionCategoria>> ObtenerDatos(RelacionCategoria objRelacionCategoria)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                var resul = clsDatos.ObtenerDatos(objRelacionCategoria);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<RelacionCategoria> ValidarDatos(RelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }
    }
}
