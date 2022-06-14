using GB.SUTEL.DAL.Umbrales;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.UmbralesPesosRelativos;
using GB.SUTEL.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.BL.UmbralesPesosRelativos
{
    public class UmbralesPesosRelativosBL
    {
        /// <summary>
        /// Metodo retorna un listado de las direcciones almacendas en tabla Direcciones
        /// </summary>
        /// <returns></returns>
        public List<Direccion> GetLisDirecciones() { return new UmbralesPesosRelativosAD(new ApplicationContext("", "SUTEL - Captura de Indicadores")).GetLisDirecciones(); }
        /// <summary>
        /// Metodo retorna un listado de las Servicios almacendas en tabla Servicios
        /// </summary>
        /// <returns></returns>
        public List<Servicio> GetLisServicio() { return new UmbralesPesosRelativosAD(new ApplicationContext("", "SUTEL - Captura de Indicadores")).GetLisServicios(); }

        /// <summary>
        /// Consulta los inidicador asociados aun servicio retorna un objeto ServiIndicadoresEnti
        /// </summary>
        /// <param name="IdServicio">IdServicio</param>
        /// <returns>Retorna una lista de tipo ServiIndicadorEnti</returns>
        public List<ServiIndicadorEnti> GetLisIndicadorXservicio(int IdServicio, int IdDireccion) { return new UmbralesPesosRelativosAD(new ApplicationContext("", "SUTEL - Captura de Umbrales")).GetLisIndicadorXservicio(IdServicio, IdDireccion); }


        /// <summary>
        /// Consulta los inidicador asociados aun servicio retorna un objeto ServiIndicadoresEnti ppr rango de fechas
        /// </summary>
        /// <param name="IdServicio">IdServicio</param>
        /// <returns>Retorna una lista de tipo ServiIndicadorEnti</returns>
        public List<ServiIndicadorEnti> GetLisIndicadorXservicio(int IdServicio, DateTime FechaInic, DateTime FechaFin, int IdDireccion) { return new UmbralesPesosRelativosAD(new ApplicationContext("", "SUTEL - Captura de Umbrales")).GetLisIndicadorXservicio(IdServicio, FechaInic, FechaFin, IdDireccion); }


        public List<ServiIndicadorEnti> GetLisIndicadorXservicio(int IdServicio, string Usuario, int IdDireccion) { return new UmbralesPesosRelativosAD(new ApplicationContext("", "SUTEL - Captura de Umbrales")).GetLisIndicadorXservicio(IdServicio, Usuario, IdDireccion); }


        /// <summary>
        /// Metodo para crear los pesos relativo y umbrales pertenicientes a un aundicador
        /// </summary>
        /// <param name="Entidad">ServiIndicadorEnti</param>
        /// <returns>Retorna en caso de caso de realizar un insert de un nevo registro el valor 1, en caso de ser atualizacion de datos 2</returns>
        public int CrearIndicadorUmbral(ServiIndicadorEnti Entidad) { return new UmbralesPesosRelativosAD(new ApplicationContext("", "SUTEL - Captura de Indicadores")).CrearIndicadorUmbral(Entidad); }


        public List<Usuario> GetUser() { return new UmbralesPesosRelativosAD(new ApplicationContext("", "SUTEL - Captura de Indicadores")).GetUsuarios(); }
    }
}
