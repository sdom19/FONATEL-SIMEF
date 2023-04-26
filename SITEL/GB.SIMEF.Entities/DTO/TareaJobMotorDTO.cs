using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.Entities.DTO
{
    public class TareaJobMotorDTO
    {
        public string usuario { set; get; }
        public string aplicacion { set; get; }
        public string despacho { set; get; }
        public string periodicidad { set; get; }
        public bool iniciarAhora { set; get; }
        public DateTime? fechaInicio { set; get; }
        public ParametroTareaDTO[] parametros { set; get; }

        public bool error { set; get; }
        public string respuesta { set; get; } // se mapea con la clase RespuestaTareaJobMotorDTO
        public string estado { set; get; }

        public TareaJobMotorDTO() { }

        public TareaJobMotorDTO(string usuario, string aplicacion, string despacho, string periodicidad, bool iniciarAhora, DateTime? fechaInicio, ParametroTareaDTO[] parametros)
        {
            this.usuario = usuario;
            this.aplicacion = aplicacion;
            this.despacho = despacho;
            this.periodicidad = periodicidad;
            this.iniciarAhora = iniciarAhora;
            this.fechaInicio = fechaInicio;
            this.parametros = parametros;
        }

        public TareaJobMotorDTO(string periodicidad, DateTime? fechaInicio)
        {
            this.periodicidad = periodicidad;
            this.fechaInicio = fechaInicio;
        }
    }

    public class ParametroTareaDTO
    {
        public string nombre { set; get; }
        public string valor { set; get; }

        public ParametroTareaDTO(string nombre, string valor)
        {
            this.nombre = nombre;
            this.valor = valor;
        }
    }

    public class RespuestaTareaJobMotorDTO
    {
        public string FormulaEjecuta { get; set; }
        public bool EjecucionCorrecta { get; set; }
        public string Mensaje { get; set; }
    }
}
