using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
namespace GB.SIMEF.BL
{
    public class CategoriasDesagregacionBL
    {
        private readonly CategoriasDesagregacionDAL clsDatos;

        private Bitacora ResultadoConsulta;

        public CategoriasDesagregacionBL()
        {
            clsDatos = new CategoriasDesagregacionDAL();
            ResultadoConsulta = new Bitacora();
        }
        public Bitacora Obtener(CategoriasDesagregacion objCategoria)
        {   
            try
            {
                ResultadoConsulta.objetoRespuesta = "Consultar Categorias";
                ResultadoConsulta.Accion = 1;
                var resul = clsDatos.ObtenerDatos(objCategoria);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = true;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

    }
}
