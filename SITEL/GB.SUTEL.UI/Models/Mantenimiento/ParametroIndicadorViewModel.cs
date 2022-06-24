using System;

namespace Mantenimiento
{
    public class ParametroIndicadorViewModel
    {

        public int IdParametroIndicador { get; set; }
        public string IdIndicador { get; set; }
        public string DescripcionIndicador { get; set; }
        public bool Visualiza { get; set; }
        public int? AnnoDesde { get; set; }
        public int? MesDesde { get; set; }
        public int AnnoPorOperador { get; set; }
        public int MesPorOperador { get; set; }
        public int AnnoPorTotal { get; set; }
        public int MesPorTotal { get; set; }
        public bool Publicar { get; set; }
        public System.DateTime FechaUltimaPublicacion { get; set; }
        public System.TimeSpan HoraUltimaPublicacion { get; set; }
        public string UsuarioUltimoPublicador { get; set; }
    }
}