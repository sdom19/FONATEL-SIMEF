﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;

namespace GB.SIMEF.DAL
{
    public class DetalleReglasValicionDAL : BitacoraDAL
    {
        private SIMEFContext db;
        #region Metodos Consulta Base de Datos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public List<DetalleReglaValidacion> ObtenerDatos(DetalleReglaValidacion objeto)
        {
            List<DetalleReglaValidacion> ListaDetalles = new List<DetalleReglaValidacion>();
            using (db = new SIMEFContext())
            {
                ListaDetalles = db.Database.SqlQuery<DetalleReglaValidacion>
                    ("execute spObtenerDetalleReglasValidacion @IdDetalleReglaValidacion, @IdRegla, @IdTipo, @IdOperador, @IdDetalleIndicador, @IdIndicador",
                        new SqlParameter("@IdDetalleReglaValidacion", objeto.IdDetalleReglaValidacion),
                        new SqlParameter("@IdRegla", objeto.IdRegla),
                        new SqlParameter("@IdTipo", objeto.IdTipo),
                        new SqlParameter("@IdOperador", objeto.IdOperador),
                        new SqlParameter("@IdDetalleIndicador", objeto.IdDetalleIndicador),
                        new SqlParameter("@IdIndicador", objeto.IdIndicador)
                    ).ToList();

                ListaDetalles = ListaDetalles.Select(x => new DetalleReglaValidacion()
                {
                    id = Utilidades.Encriptar(x.IdDetalleReglaValidacion.ToString()),
                    IdDetalleReglaValidacion = x.IdDetalleReglaValidacion,
                    IdRegla = x.IdRegla,
                    IdTipo = x.IdTipo,
                    tipoReglaValidacion = ObtenerTipoRegla(x.IdTipo),
                    IdOperador = x.IdOperador,
                    operadorArismetico = ObtenerOperador(x.IdOperador),
                    idIndicadorVariableString = x.idIndicadorVariableString,
                    IdDetalleIndicador = x.IdDetalleIndicador,
                    IdIndicador = x.IdIndicador,
                    NombreVariable = ObtenerVariable(x.IdDetalleIndicador).NombreVariable,

                    //reglaAtributosValidos = ObtenerReglaAtributosValidos(x.IdDetalleReglaValidacion),
                    //reglaComparacionConstante = ObtenerReglaContraConstante(x.IdDetalleReglaValidacion),
                    //reglaSecuencial = ObtenerReglaSecuencial(x.IdReglasValidacionTipo),
                    //reglaIndicadorSalida = ObtenerReglaIndicadorSalida(x.IdDetalleReglaValidacion),
                    //reglaIndicadorEntrada = ObtenerReglaIndicadorEntrada(x.IdDetalleReglaValidacion),
                    //reglaIndicadorEntradaSalida = ObtenerReglaIndicadorEntradaSalida(x.IdDetalleReglaValidacion),
                    Estado = x.Estado
                }).ToList();
            }
            return ListaDetalles;
        }

        /// <summary>
        /// Valida si existen Reglas de Validación en indicadores
        /// Michael Hernández Cordero
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public List<string> ValidarDatos(DetalleReglaValidacion objeto)
        {
            List<string> listaValicion = new List<string>();
            using (db = new SIMEFContext())
            {
                listaValicion = db.Database.SqlQuery<string>
                    ("exec spValidarRegla @IdRegla",
                       new SqlParameter("@IdRegla", objeto.IdRegla)
                    ).ToList();
            }

            return listaValicion;
        }

        /// <summary>
        /// Creación y modificación
        /// </summary>
        /// <param name="objDetalleReglaValidacion"></param>
        /// <returns></returns>
        public List<DetalleReglaValidacion> ActualizarDatos(DetalleReglaValidacion objeto)
        {
            List<DetalleReglaValidacion> ListaDetalleReglaValidacion = new List<DetalleReglaValidacion>();

            using (db = new SIMEFContext())
            {
                ListaDetalleReglaValidacion = db.Database.SqlQuery<DetalleReglaValidacion>
                ("execute spActualizarDetalleReglaValidacion @pIdDetalleReglaValidacion,@pIdRegla,@pIdTipo,@pIdOperador,@pIdDetalleIndicador,@pIdIndicador,@pEstado",
                    new SqlParameter("@pIdDetalleReglaValidacion", objeto.IdDetalleReglaValidacion),
                    new SqlParameter("@pIdRegla", objeto.IdRegla),
                    new SqlParameter("@pIdTipo", objeto.IdTipo),
                    new SqlParameter("@pIdOperador", objeto.IdOperador),
                    new SqlParameter("@pIdDetalleIndicador", objeto.IdDetalleIndicador),
                    new SqlParameter("@pIdIndicador", objeto.IdIndicador),
                    new SqlParameter("@pEstado", objeto.Estado)
                ).ToList();

                ListaDetalleReglaValidacion = ListaDetalleReglaValidacion.Select(X => new DetalleReglaValidacion
                {
                    id = Utilidades.Encriptar(X.IdDetalleReglaValidacion.ToString()),
                    IdDetalleReglaValidacion = X.IdDetalleReglaValidacion,
                    IdRegla = X.IdRegla,
                    IdTipo = X.IdTipo,
                    IdOperador = X.IdOperador,
                    IdDetalleIndicador = X.IdDetalleIndicador,
                    IdIndicador = X.IdIndicador,
                    Estado = X.Estado

                }).ToList();

                return ListaDetalleReglaValidacion;
            }
        }

        private string ObtenerListadoTipoReglas(int IdRegla)
        {
            string tipoReglas = "";
            tipoReglas = db.Database.SqlQuery<string>
                ("exec spObtenerListadoTipoReglaXReglaValidacion @IdRegla",
                   new SqlParameter("@IdRegla", IdRegla)
                ).Single();

            return tipoReglas;
        }

        private DetalleIndicadorVariables ObtenerVariable(int id)
        {
            DetalleIndicadorVariables result = new DetalleIndicadorVariables();
            result =
            db.DetalleIndicadorVariables.Where(x => x.idDetalleIndicador == id).Single();
            return result;
        }

        private TipoReglaValidacion ObtenerTipoRegla(int id)
        {
            return
            db.TipoReglaValidacion.Where(x => x.IdTipo == id && x.Estado == true).FirstOrDefault();
        }

        private OperadorArismetico ObtenerOperador(int id)
        {
            return
            db.OperadorArismetico.Where(x => x.IdOperador == id && x.Estado == true).FirstOrDefault();
        }

        private ReglaAtributosValidos ObtenerReglaAtributosValidos(int id)
        {
            ReglaAtributosValidos regla =
                db.ReglaAtributosValidos.Where(x => x.IdDetalleReglaValidacion == id).FirstOrDefault();
            if (regla != null)
            {

            }
            return regla;
        }

        private ReglaComparacionConstante ObtenerReglaContraConstante(int id)
        {
            ReglaComparacionConstante regla =
                db.ReglaComparacionConstante.Where(x => x.IdDetalleReglaValidacion == id).FirstOrDefault();
            if (regla != null)
            {

            }
            return regla;
        }

        private ReglaSecuencial ObtenerReglaSecuencial(int id)
        {
            ReglaSecuencial regla =
                db.ReglaSecuencial.Where(x => x.IdDetalleReglaValidacion == id).FirstOrDefault();
            if (regla != null)
            {

            }
            return regla;
        }

        private ReglaIndicadorSalida ObtenerReglaIndicadorSalida(int id)
        {
            ReglaIndicadorSalida regla =
                db.ReglaIndicadorSalida.Where(x => x.IdDetalleReglaValidacion == id).FirstOrDefault();
            if (regla != null)
            {
                regla.IdCompara = 0;
            }
            return regla;
        }

        private ReglaIndicadorEntrada ObtenerReglaIndicadorEntrada(int id)
        {
            ReglaIndicadorEntrada regla =
                db.ReglaIndicadorEntrada.Where(x => x.IdDetalleReglaValidacion == id).FirstOrDefault();
            if (regla != null)
            {
                regla.IdCompara = 0;
            }
            return regla;
        }

        private ReglaIndicadorEntradaSalida ObtenerReglaIndicadorEntradaSalida(int id)
        {
            ReglaIndicadorEntradaSalida regla =
                db.ReglaIndicadorEntradaSalida.Where(x => x.IdDetalleReglaValidacion == id).FirstOrDefault();
            if (regla != null)
            {
                regla.IdCompara = 0;
            }
            return regla;
        }

        #endregion
    }
}
