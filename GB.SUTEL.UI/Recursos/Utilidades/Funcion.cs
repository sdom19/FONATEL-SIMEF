using System;

using GB.SUTEL.Entities;

using GB.SUTEL.UI.Helpers;

using System.Text;
using System.Web.Mvc;
using GB.SUTEL.DAL;
using System.Security.Principal;

using GB.SUTEL.Shared;

namespace GB.SUTEL.UI.Recursos.Utilidades
{
    public class Funcion
    {
        #region Contexto
        public ApplicationContext AppContext { get; private set; }
        // GET: Safe
        public Funcion()
        {
            WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();
            WindowsPrincipal currentPrincipal = new WindowsPrincipal(currentIdentity);
            string usuarioDominio = currentPrincipal.Identity.Name;

            AppContext = new ApplicationContext(usuarioDominio, "SUTEL - Captura Indicadores", ExceptionHandler.ExceptionType.Presentation);
        }
        #endregion

        #region Nivel

        public void nivelbit(ActionsBinnacle accion, Nivel nuevoNivel, Nivel anteriorNivel)
        {
            StringBuilder newData = new StringBuilder();
            StringBuilder oldData = new StringBuilder();
            String pantalla = "";
            String descripcion = "";
            pantalla = CBitacora.GetValueAsString(CBitacora.BitacoraOpciones.nivel);
            if (nuevoNivel != null)
            {
                newData.Append(" Id: ");
                newData.Append(nuevoNivel.IdNivel);
                newData.Append(" Descripción del Nivel: ");
                newData.Append(nuevoNivel.DescNivel);
               /* newData.Append(" Bandera de borrado:  ");
                newData.Append(nuevoNivel.Borrado); */
            }
            if (anteriorNivel != null)
            {
                oldData.Append(" Id:  ");
                oldData.Append(anteriorNivel.IdNivel);
                oldData.Append(" Descripción del Nivel: ");
                oldData.Append(anteriorNivel.DescNivel);
               /* oldData.Append(" Bandera de borrado:  ");
                oldData.Append(anteriorNivel.Borrado);*/
            }
            if (accion.Equals(ActionsBinnacle.Crear))
            {
                descripcion = "Proceso de Creación de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Editar))
            {
                descripcion = "Proceso de Edición de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Borrar))
            {
                descripcion = "Proceso de eliminación de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Consultar))
            {
                descripcion = "Proceso de consulta de " + pantalla;
            }
            else
            {
                descripcion = " ";
            }


            try
            {

                CBitacora.gRegistrarBitacora(AppContext, Convert.ToInt32(accion.GetHashCode()), pantalla,
                                                 descripcion, newData.ToString(), oldData.ToString());
            }
            catch (Exception)
            {



            }
        }

        #endregion

        #region Frecuencia

        public void frecuenciabit(ActionsBinnacle accion, Frecuencia nuevaFrecuencia, Frecuencia anteriorFrecuencia)
        {
            StringBuilder newData = new StringBuilder();
            StringBuilder oldData = new StringBuilder();
            String pantalla = "";
            String descripcion = "";
            pantalla = CBitacora.GetValueAsString(CBitacora.BitacoraOpciones.frecuencia);
            if (nuevaFrecuencia != null)
            {
                newData.Append("Id frecuencia: ");
                newData.Append(nuevaFrecuencia.IdFrecuencia);
                newData.Append(" Descripción de la Frecuencia: ");
                newData.Append(nuevaFrecuencia.NombreFrecuencia.Trim());
              //  newData.Append(" Bandera de borrado de la frecuencia: ");
               // newData.Append(nuevaFrecuencia.Borrado);
            }
            if (anteriorFrecuencia != null)
            {
                oldData.Append(" Id frecuencia: ");
                oldData.Append(anteriorFrecuencia.IdFrecuencia);
                oldData.Append(" Descripción de la Frecuencia: ");
                oldData.Append(anteriorFrecuencia.NombreFrecuencia.Trim());
               /* oldData.Append(" Bandera de borrado de la frecuencia: ");
                oldData.Append(anteriorFrecuencia.Borrado);*/
            }
            if (accion.Equals(ActionsBinnacle.Crear))
            {
                descripcion = "Proceso de creación de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Editar))
            {
                descripcion = "Proceso de edición de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Borrar))
            {
                descripcion = "Proceso de eliminación de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Consultar))
            {
                descripcion = "Proceso de consulta de " + pantalla;
            }
            else
            {
                descripcion = " ";
            }


            try
            {

                CBitacora.gRegistrarBitacora(AppContext, Convert.ToInt32(accion.GetHashCode()), pantalla,
                                                 descripcion, newData.ToString(), oldData.ToString());
            }
            catch (Exception)
            {



            }
        }

        #endregion

        #region Tipo de Indicador
        public void tipoindicadorbit(ActionsBinnacle accion, TipoIndicador newTipoIndicador, TipoIndicador oldTipoIndicador)
        {
            try
            {
                StringBuilder newData = new StringBuilder();
                StringBuilder oldData = new StringBuilder();
                String pantalla = "";
                String descripcion = "";
                pantalla = CBitacora.GetValueAsString(CBitacora.BitacoraOpciones.tipoIndicador);
                if (newTipoIndicador != null)
                {
                    newData.Append("Id tipo de indicador: ");
                    newData.Append(newTipoIndicador.IdTipoInd);
                    newData.Append(" Descripción: ");
                    newData.Append(newTipoIndicador.DesTipoInd.Trim());
                }
                if (oldTipoIndicador != null)
                {
                    oldData.Append("Id tipo de indicador: ");
                    oldData.Append(oldTipoIndicador.IdTipoInd);
                    oldData.Append(" Descripción: ");
                    oldData.Append(oldTipoIndicador.DesTipoInd.Trim());
                }
                if (accion.Equals(ActionsBinnacle.Crear))
                {
                    descripcion = "Proceso de creación de " + pantalla;
                }
                else if (accion.Equals(ActionsBinnacle.Editar))
                {
                    descripcion = "Proceso de edición de " + pantalla;
                }
                else if (accion.Equals(ActionsBinnacle.Borrar))
                {
                    descripcion = "Proceso de eliminación de " + pantalla;
                }

                else if (accion.Equals(ActionsBinnacle.Consultar))
                {
                    descripcion = "Proceso de consulta de " + pantalla;
                }
                else
                {
                    descripcion = " " + pantalla;
                }

                CBitacora.gRegistrarBitacora(AppContext, Convert.ToInt32(accion.GetHashCode()), pantalla,
                                                 descripcion, newData.ToString(), oldData.ToString());
            }
            catch (Exception)
            { }
        }

        #endregion

        #region Tipo de Indicador por servicio
        // Index Code
        #endregion

        #region IndicadorUIT
        public void indicadoruitbit(ActionsBinnacle accion, IndicadorUIT nuevaRegistro, IndicadorUIT anteriorRegistro)
        {

            StringBuilder newData = new StringBuilder();
            StringBuilder oldData = new StringBuilder();
            String pantalla = "";
            String descripcion = "";
            pantalla = CBitacora.GetValueAsString(CBitacora.BitacoraOpciones.indicadorUIT);
            if (nuevaRegistro != null)
            {
                newData.Append("Id de Indicador UIT: ");
                newData.Append(nuevaRegistro.IdIndicadorUIT);
                newData.Append("; Descripción del Indicador UIT: ");
                newData.Append(nuevaRegistro.DescIndicadorUIT);
               /* newData.Append("; Bandera de borrado del Indicador UIT: ");
                newData.Append(nuevaRegistro.Borrado);*/
            }
            if (anteriorRegistro != null)
            {
                oldData.Append(" Id de Indicador UIT: ");
                oldData.Append(anteriorRegistro.IdIndicadorUIT);
                oldData.Append("; Descripción del Indicador UIT: ");
                oldData.Append(anteriorRegistro.DescIndicadorUIT);
              //  oldData.Append("; Bandera de borrado del Indicador UIT: ");
               // oldData.Append(anteriorRegistro.Borrado);
            }
            if (accion.Equals(ActionsBinnacle.Crear))
            {
                descripcion = "Proceso de creación de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Editar))
            {
                descripcion = "Proceso de edición de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Borrar))
            {
                descripcion = "Proceso de eliminación de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Consultar))
            {
                descripcion = "Proceso de consulta de " + pantalla;
            }
            else
            {
                descripcion = " ";
            }


            try
            {

                CBitacora.gRegistrarBitacora(AppContext, Convert.ToInt32(accion.GetHashCode()), pantalla,
                                                 descripcion, newData.ToString(), oldData.ToString());
            }
            catch (Exception)
            {



            }
        }

        #endregion

        #region Agrupacion
        public void agrupacionbit(ActionsBinnacle accion, Agrupacion nuevaAgrupacion, Agrupacion anteriorAgrupacion)
        {
            StringBuilder newData = new StringBuilder();
            StringBuilder oldData = new StringBuilder();
            String pantalla = "";
            String descripcion = "";
            pantalla = CBitacora.GetValueAsString(CBitacora.BitacoraOpciones.agrupacion);
            if (nuevaAgrupacion != null)
            {
                newData.Append("Id de Agrupación: ");
                newData.Append(nuevaAgrupacion.IdAgrupacion);
                newData.Append("; Descripción de la agrupación: ");
                newData.Append(nuevaAgrupacion.DescAgrupacion);
               // newData.Append("; Bandera de borrado de la agrupación: ");
                //newData.Append(nuevaAgrupacion.Borrado);
            }
            if (anteriorAgrupacion != null)
            {
                oldData.Append("Id de Agrupación: ");
                oldData.Append(anteriorAgrupacion.IdAgrupacion);
                oldData.Append("; Descripción de la agrupación: ");
                oldData.Append(anteriorAgrupacion.DescAgrupacion);
               /* oldData.Append("; Bandera de borrado de la agrupación: ");
                oldData.Append(anteriorAgrupacion.Borrado);*/
            }
            if (accion.Equals(ActionsBinnacle.Crear))
            {
                descripcion = "Proceso de creación de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Editar))
            {
                descripcion = "Proceso de edición de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Borrar))
            {
                descripcion = "Proceso de eliminación de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Consultar))
            {
                descripcion = "Proceso de consulta de " + pantalla;
            }
            else
            {
                descripcion = " ";
            }


            try
            {

                CBitacora.gRegistrarBitacora(AppContext, Convert.ToInt32(accion.GetHashCode()), pantalla,
                                                 descripcion, newData.ToString(), oldData.ToString());
            }
            catch (Exception)
            {



            }
        }

        #endregion

        #region Detalle Agrupacion
        public void detalleagrupacionbit(ActionsBinnacle accion, DetalleAgrupacion nuevoDetalle, DetalleAgrupacion anteriorDetalle)
        {
            StringBuilder newData = new StringBuilder();
            StringBuilder oldData = new StringBuilder();
            String pantalla = "";
            String descripcion = "";
            pantalla = CBitacora.GetValueAsString(CBitacora.BitacoraOpciones.detalleAgrupacion);
            if (nuevoDetalle != null)
            {
                newData.Append("Id agrupacion: ");
                newData.Append(nuevoDetalle.IdDetalleAgrupacion);
                newData.Append(" Id de Operador: ");
                newData.Append(nuevoDetalle.IdOperador);
                newData.Append(" Id de Agrupacion: ");
                newData.Append(nuevoDetalle.IdAgrupacion);
                newData.Append(" Descripcion del Detalle Agrupación: ");
                newData.Append(nuevoDetalle.DescDetalleAgrupacion);
               /* newData.Append(" Bandera de borrado del detalle Agrupación: ");
                newData.Append(nuevoDetalle.Borrado);*/
            }
            if (anteriorDetalle != null)
            {
                oldData.Append("Id agrupacion: ");
                oldData.Append(anteriorDetalle.IdDetalleAgrupacion);
                oldData.Append(" Id de Operador: ");
                oldData.Append(anteriorDetalle.IdOperador);
                oldData.Append(" Id de Agrupacion: ");
                oldData.Append(anteriorDetalle.IdAgrupacion);
                oldData.Append(" Desscripcion del Detalle Agrupación: ");
                oldData.Append(anteriorDetalle.DescDetalleAgrupacion);
              /*  oldData.Append(" Bandera de borrado del detalle agrupación: ");
                oldData.Append(anteriorDetalle.Borrado);*/


            }
            if (accion.Equals(ActionsBinnacle.Crear))
            {
                descripcion = "Proceso de creación de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Editar))
            {
                descripcion = "Proceso de edición de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Borrar))
            {
                descripcion = "Proceso de eliminación de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Consultar))
            {
                descripcion = "Proceso de consulta de " + pantalla;
            }
            else
            {
                descripcion = " ";
            }


            try
            {

                CBitacora.gRegistrarBitacora(AppContext, Convert.ToInt32(accion.GetHashCode()), pantalla,
                                                 descripcion, newData.ToString(), oldData.ToString());
            }
            catch (Exception)
            {



            }
        }
        #endregion

        #region Indicador
        public void indicadorbit(ActionsBinnacle accion, Indicador newIndicador, Indicador oldIndicador)
        {


            try
            {
                StringBuilder newData = new StringBuilder();
                StringBuilder oldData = new StringBuilder();
                String pantalla = "";
                String descripcion = "";
                pantalla = CBitacora.GetValueAsString(CBitacora.BitacoraOpciones.indicador);
                if (newIndicador != null)
                {
                    newData.Append("Id indicador: ");
                    newData.Append(newIndicador.IdIndicador);
                    newData.Append(" Descripción: ");
                    newData.Append(newIndicador.DescIndicador);
                }
                if (oldIndicador != null)
                {
                    oldData.Append("Id indicador: ");
                    oldData.Append(oldIndicador.IdIndicador);
                    oldData.Append(" Descripción: ");
                    oldData.Append(oldIndicador.DescIndicador);
                }
                if (accion.Equals(ActionsBinnacle.Crear))
                {
                    descripcion = "Proceso de creación de " + pantalla;
                }
                else if (accion.Equals(ActionsBinnacle.Editar))
                {
                    descripcion = "Proceso de edición de " + pantalla;
                }
                else if (accion.Equals(ActionsBinnacle.Borrar))
                {
                    descripcion = "Proceso de eliminación de " + pantalla;
                }
                else if (accion.Equals(ActionsBinnacle.Consultar))
                {
                    descripcion = "Proceso de consulta de " + pantalla;
                }



                else if (accion.Equals(ActionsBinnacle.Index))
                {
                    descripcion = pantalla;
                }


                else
                {
                    descripcion = "Indicador Mantenimiento";
                }

                CBitacora.gRegistrarBitacora(AppContext, Convert.ToInt32(accion.GetHashCode()), pantalla,
                                                 descripcion, newData.ToString(), oldData.ToString());
            }
            catch (Exception)
            { }

        }


        #endregion

        #region IndicadorCruzado
        // Index y solucion con 1 linea
        #endregion

        #region Criterios

        public void criteriosbit(ActionsBinnacle accion, Criterio nuevoCriterio, Criterio anteriorCriterio)
        {
            StringBuilder newData = new StringBuilder();
            StringBuilder oldData = new StringBuilder();
            String pantalla = "";
            String descripcion = "";
            pantalla = CBitacora.GetValueAsString(CBitacora.BitacoraOpciones.criterio);
            if (nuevoCriterio != null)
            {
                newData.Append("Registro de un Criterio con Id: ");
                newData.Append(nuevoCriterio.IdCriterio);
                newData.Append(" Descripción del Criterio: ");
                newData.Append(nuevoCriterio.DescCriterio);
                newData.Append(" Dirección del Criterio: ");
                newData.Append(nuevoCriterio.IdDireccion);
                /*newData.Append(" Bandera de borrado del criterio: ");
                newData.Append(nuevoCriterio.Borrado);*/
            }
            if (anteriorCriterio != null)
            {

                oldData.Append("Registro de un Criterio con Id: ");
                oldData.Append(anteriorCriterio.IdCriterio);
                oldData.Append(" Descripción del Criterio: ");
                oldData.Append(anteriorCriterio.DescCriterio);
                oldData.Append(" Dirección del Criterio: ");
                oldData.Append(anteriorCriterio.IdDireccion.ToString());
                /*oldData.Append(" Bandera de borrado del criterio: ");
                oldData.Append(anteriorCriterio.Borrado);*/
            }
            if (accion.Equals(ActionsBinnacle.Crear))
            {
                descripcion = "Proceso de creación de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Editar))
            {
                descripcion = "Proceso de edición de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Borrar))
            {
                descripcion = "Proceso de eliminación de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Consultar))
            {
                descripcion = "Proceso de consulta de " + pantalla;
            }
            else
            {
                descripcion = " ";
            }


            try
            {

                CBitacora.gRegistrarBitacora(AppContext, Convert.ToInt32(accion.GetHashCode()), pantalla,
                                                 descripcion, newData.ToString(), oldData.ToString());
            }
            catch (Exception)
            {



            }
        }

        #endregion

        #region Servicios
        public void serviciosbit(ActionsBinnacle accion, Servicio newServicio, Servicio oldServicio)
        {
            try
            {
                StringBuilder newData = new StringBuilder();
                StringBuilder oldData = new StringBuilder();
                String pantalla = "";
                String descripcion = "";
                pantalla = CBitacora.GetValueAsString(CBitacora.BitacoraOpciones.servicio);
                if (newServicio != null)
                {
                    newData.Append("Id servicio: ");
                    newData.Append(newServicio.IdServicio);
                    newData.Append(" Descripción: ");
                    newData.Append(newServicio.DesServicio);
                }
                if (oldServicio != null)
                {
                    oldData.Append("Id servicio: ");
                    oldData.Append(oldServicio.IdServicio);
                    oldData.Append(" Descripción: ");
                    oldData.Append(oldServicio.DesServicio);
                }
                if (accion.Equals(ActionsBinnacle.Crear))
                {
                    descripcion = "Proceso de creación de " + pantalla;
                }
                else if (accion.Equals(ActionsBinnacle.Editar))
                {
                    descripcion = "Proceso de edición de " + pantalla;
                }
                else if (accion.Equals(ActionsBinnacle.Borrar))
                {
                    descripcion = "Proceso de eliminación de " + pantalla;
                }
                else if (accion.Equals(ActionsBinnacle.Consultar))
                {
                    descripcion = "Proceso de consulta de " + pantalla;
                }
                else
                {
                    descripcion = " ";
                }

                CBitacora.gRegistrarBitacora(AppContext, Convert.ToInt32(accion.GetHashCode()), pantalla,
                                                 descripcion, newData.ToString(), oldData.ToString());
            }
            catch (Exception)
            { }
        }


        #endregion

        #region Servicios Operador

        public void serviciosopbit(ActionsBinnacle accion, int[] IdServicios)
        {
            StringBuilder newData = new StringBuilder();
            StringBuilder oldData = new StringBuilder();
            String pantalla = "";
            String descripcion = "";
            String serviciosInvolucrados = "";
            pantalla = CBitacora.GetValueAsString(CBitacora.BitacoraOpciones.servioOperador);
            if (IdServicios.Length > 0)
            {
                newData.Append("Servicios involucrados en la acción: ");
                foreach (int item in IdServicios)
                {
                    serviciosInvolucrados += item.ToString() + ",";
                }

                newData.Append(serviciosInvolucrados);

            }

            if (accion.Equals(ActionsBinnacle.Crear))
            {
                descripcion = "Proceso de creación de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Editar))
            {
                descripcion = "Proceso de edición de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Borrar))
            {
                descripcion = "Proceso de eliminación de " + pantalla;
            }
            else
            {
                descripcion = "Proceso en " + pantalla;
            }


            try
            {

                CBitacora.gRegistrarBitacora(AppContext, Convert.ToInt32(accion.GetHashCode()), pantalla,
                                                 descripcion, newData.ToString(), oldData.ToString());
            }
            catch (Exception)
            {



            }
        }

        #endregion

        #region Servicios Indicador 
        // Solucion en el controlador
        #endregion

        #region Operador (Consultar Operadores)
        // Solucion en el controlador
        #endregion

        #region Registro de Indicador externo
        // Solucion en el controlador
        #endregion

        #region Registro de Indicadores
        // Solucion en el controlador, primero va al roll controller por permisos de usuario
        #endregion

        #region Solicitud
        // Solucion en el controlador
        #endregion

        #region Solicitud General
        // Solucion en el controlador
        #endregion

        #region Solicitud Operadores
        // Solucion en el controlador
        #endregion

        #region Fuentes Externas
        //Esta en el controlador
        #endregion

        #region Indicador Externo
        //Esta en el controlador
        #endregion

        #region Roll
        public void rollbit(ActionsBinnacle accion, Rol newRol, Rol oldRol)
        {
            try
            {
                StringBuilder newData = new StringBuilder();
                StringBuilder oldData = new StringBuilder();
                String pantalla = "";
                String descripcion = "";
                pantalla = CBitacora.GetValueAsString(CBitacora.BitacoraOpciones.rol);
                if (newRol != null)
                {
                    newData.Append("Id de rol: ");
                    newData.Append(newRol.IdRol);
                    newData.Append(" Descripción del rol: ");
                    newData.Append(newRol.NombreRol);
                }
                if (oldRol != null)
                {
                    oldData.Append(" Id de rol: ");
                    oldData.Append(oldRol.IdRol);
                    oldData.Append(" Descripción del rol: ");
                    oldData.Append(oldRol.NombreRol);
                }
                if (accion.Equals(ActionsBinnacle.Crear))
                {
                    descripcion = "Proceso de creación de " + pantalla;
                }
                else if (accion.Equals(ActionsBinnacle.Editar))
                {
                    descripcion = "Proceso de edición de " + pantalla;
                }
                else if (accion.Equals(ActionsBinnacle.Borrar))
                {
                    descripcion = "Proceso de eliminación de " + pantalla;
                }
                else
                {
                    descripcion = "Proceso en " + pantalla;
                }

                CBitacora.gRegistrarBitacora(AppContext, Convert.ToInt32(accion.GetHashCode()), pantalla,
                                                 descripcion, newData.ToString(), oldData.ToString());
            }
            catch (Exception)
            { }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Registro en bitacora
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="nuevoRegistro"></param>
        /// <param name="anteriorRegistro"></param>
        public void constructorbit (ActionsBinnacle accion, Constructor nuevoRegistro, Constructor anteriorRegistro)
        {
            StringBuilder newData = new StringBuilder();
            StringBuilder oldData = new StringBuilder();
            String pantalla = "";
            String descripcion = "";
            pantalla = CBitacora.GetValueAsString(CBitacora.BitacoraOpciones.constructor);
            try
            {
                if (nuevoRegistro != null)
                {
                    newData.Append("Id de Constructor: ");
                    newData.Append(nuevoRegistro.IdConstructor);
                    newData.Append(" ID del indicador: ");
                    newData.Append(nuevoRegistro.IdIndicador);

                    newData.Append(" ID Frecuencia del indicador: ");
                    newData.Append(nuevoRegistro.IdFrecuencia);
                    newData.Append(" ID Desglose del indicador: ");
                    newData.Append(nuevoRegistro.IdDesglose);
                   /* newData.Append(" Bandera de borrado de la agrupación: ");
                    newData.Append(nuevoRegistro.Borrado);*/
                }
                if (anteriorRegistro != null)
                {
                    oldData.Append("Id de Constructor: ");
                    oldData.Append(anteriorRegistro.IdConstructor);
                    oldData.Append(" ID del indicador: ");
                    newData.Append(anteriorRegistro.IdIndicador);

                    newData.Append(" ID Frecuencia del indicador: ");
                    newData.Append(anteriorRegistro.IdFrecuencia);
                    newData.Append(" ID Desglose del indicador: ");
                    newData.Append(anteriorRegistro.IdDesglose);
                 /*   newData.Append(" Bandera de borrado de la agrupación: ");
                    newData.Append(anteriorRegistro.Borrado);*/
                }
                if (accion.Equals(ActionsBinnacle.Crear))
                {
                    descripcion = "Proceso de creación de " + pantalla;
                }
                else if (accion.Equals(ActionsBinnacle.Editar))
                {
                    descripcion = "Proceso de edición de " + pantalla;
                }
                else if (accion.Equals(ActionsBinnacle.Borrar))
                {
                    descripcion = "Proceso de eliminación de " + pantalla;
                }
                else
                {
                    descripcion = "Proceso en " + pantalla;
                }




                CBitacora.gRegistrarBitacora(AppContext, Convert.ToInt32(accion.GetHashCode()), pantalla,
                                                 descripcion, newData.ToString(), oldData.ToString());
            }
            catch (Exception)
            {



            }
        }

        #endregion

        #region Index
        public void _index (string user, string pantalla, string descripcion ) { 
         
       SUTEL_IndicadoresEntities db = new SUTEL_IndicadoresEntities();

        db.Bitacora.Add(new Bitacora()
                {
                    Usuario = user,
                    FechaBitacora = DateTime.Now,
                    Pantalla = pantalla,
                    Descripcion = "Index en: "+descripcion,
                    Accion = 1,
                    RegistroAnterior = " ",
                    RegistroNuevo = " "
                });
                db.SaveChanges();
            }

        #endregion

        #region Constructor
        public void constructor(string user, string descripcion,int accion,string nuevo, string viejo)
        {

            SUTEL_IndicadoresEntities db = new SUTEL_IndicadoresEntities();

            db.Bitacora.Add(new Bitacora()
            {
                Usuario = user,
                FechaBitacora = DateTime.Now,
                Pantalla = "Constructor",
                Descripcion = descripcion,
                Accion = accion,
                RegistroAnterior = viejo,
                RegistroNuevo = nuevo
            });
            db.SaveChanges();
        }

        #endregion


        #region Fuentes
        public void fuentes(string user, string descripcion, int accion, string nuevo, string viejo)
        {

            SUTEL_IndicadoresEntities db = new SUTEL_IndicadoresEntities();

            db.Bitacora.Add(new Bitacora()
            {
                Usuario = user,
                FechaBitacora = DateTime.Now,
                Pantalla = "Fuentes Externas",
                Descripcion = descripcion,
                Accion = accion,
                RegistroAnterior = viejo,
                RegistroNuevo = nuevo
            });
            db.SaveChanges();
        }

        #endregion




        #region Fuentes
        public void frecuencia(string user, string descripcion, int accion, string nuevo, string viejo)
        {

            SUTEL_IndicadoresEntities db = new SUTEL_IndicadoresEntities();

            db.Bitacora.Add(new Bitacora()
            {
                Usuario = user,
                FechaBitacora = DateTime.Now,
                Pantalla = "Frecuencia",
                Descripcion = descripcion,
                Accion = accion,
                RegistroAnterior = viejo,
                RegistroNuevo = nuevo
            });
            db.SaveChanges();
        }

        #endregion



        #region Fuentes
        public void solicitud(string user, string descripcion, int accion, string nuevo, string viejo)
        {

            SUTEL_IndicadoresEntities db = new SUTEL_IndicadoresEntities();

            db.Bitacora.Add(new Bitacora()
            {
                Usuario = user,
                FechaBitacora = DateTime.Now,
                Pantalla = "Solicitud General Index",
                Descripcion = descripcion,
                Accion = accion,
                RegistroAnterior = viejo,
                RegistroNuevo = nuevo
            });
            db.SaveChanges();
        }

        #endregion



    }
}
