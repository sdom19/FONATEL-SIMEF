//Clase Provicional, simulando los padres de los miembros en la clase Pantalla
namespace GB.SUTEL.Entities
{
    using System;
    using System.Collections.Generic;

    public partial class PANTALLAMENU
    {
        public PANTALLAMENU()
        {
            this.PANTALLAS = new HashSet<Pantalla>();
        }

        public string TITULO { get; set; }

        public virtual ICollection<Pantalla> PANTALLAS { get; set; }
    }
}
