using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class CorreoDal
    {
        public CorreoDal(string para, string cc, string html, string asunto)
        {
            this.Para = para;
            this.Copia = cc;
            this.Html = html;
            this.Asunto = asunto;
        }













        private SIMEFContext db;
        public string Html { get; set; }

        public string Para { get; set; }

        public string Copia { get; set; }

        public string Asunto { get; set; }

        public int EnviarCorreo()
        {
            int resultado = 0;

                using (db = new SIMEFContext())
                {
                    resultado = db.Database.SqlQuery<int>
                         ("execute spEnvioCorreo @Para, @CC,@titulo,@html",
                          new SqlParameter("@Para", string.IsNullOrEmpty(this.Para) ? DBNull.Value.ToString() : this.Para),
                          new SqlParameter("@CC", string.IsNullOrEmpty(this.Copia) ? DBNull.Value.ToString() : this.Copia),
                          new SqlParameter("@titulo", string.IsNullOrEmpty(this.Asunto) ? DBNull.Value.ToString() : this.Asunto),
                          new SqlParameter("@html", string.IsNullOrEmpty(this.Html) ? DBNull.Value.ToString() : this.Html)
                         ).Single();
                }
            return resultado;
        }
    }
}
