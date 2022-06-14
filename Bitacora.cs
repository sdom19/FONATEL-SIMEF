using System;

public class Bitacora
{
    


        public void gBitacora(ActionsBinnacle accion, Agrupacion nuevaAgrupacion, Agrupacion anteriorAgrupacion)
        {
            StringBuilder newData = new StringBuilder();
            StringBuilder oldData = new StringBuilder();
            String pantalla = "";
            String descripcion = "";
            pantalla = CBitacora.GetValueAsString(CBitacora.BitacoraOpciones.agrupacion);
            if (nuevaAgrupacion != null)
            {
                newData.Append("Registro de una agrupación con Id de Agrupación: ");
                newData.Append(nuevaAgrupacion.IdAgrupacion);
                newData.Append("; Descripción de la agrupación: ");
                newData.Append(nuevaAgrupacion.DescAgrupacion);
                newData.Append("; Bandera de borrado de la agrupación: ");
                newData.Append(nuevaAgrupacion.Borrado);
            }
            if (anteriorAgrupacion != null)
            {
                oldData.Append("Registro de una agrupación con Id de Agrupación: ");
                oldData.Append(anteriorAgrupacion.IdAgrupacion);
                oldData.Append("; Descripción de la agrupación: ");
                oldData.Append(anteriorAgrupacion.DescAgrupacion);
                oldData.Append("; Bandera de borrado de la agrupación: ");
                oldData.Append(anteriorAgrupacion.Borrado);
            }
            if (accion.Equals(ActionsBinnacle.Insertar))
            {
                descripcion = "Proceso de inserción de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Modificar))
            {
                descripcion = "Proceso de modificación de " + pantalla;
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
}
