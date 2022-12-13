using Dapper;
using GB.SIMEF.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIMEF.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GB.SIMEF.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class InformeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        [Route("~/api/Informe/{tabla}")]
        public JObject Get(string tabla)
        {
            InformeNombreTabla informe = ObtenerInformeNombreTabla(tabla);
            var resultado = new JObject();
            if (informe != null)
            {
                JArray filas = new JArray();

                int cantidadFilas = informe.InformeFilasValor.Select(x => x.IdFilaValor).Max();

                for (int i = 1; i <= cantidadFilas; i++)
                {
                    dynamic jsonObject = new JObject();
                    foreach (var item in informe.InformeFilasValor.Where(x => x.IdFilaValor == i))
                    {
                        string encabezado = informe.InformeEncabezadoTablas.Where(x => x.IdEncabezado == item.IdEncabezado).FirstOrDefault().NombreEncabezado;
                        jsonObject.Add(encabezado, item.Valor);

                    }
                    filas.Add(jsonObject);
                }

                resultado.Add(tabla, filas);
            }

            return resultado;
        }

        /// <summary>
        /// Método para obtener el informe nombre tabla
        /// </summary>
        /// <param name="tabla">Nombre de tabla</param>
        /// <returns>Lista de InformeNombreTabla</returns>
        internal InformeNombreTabla ObtenerInformeNombreTabla(string tabla)
        {
            var SqlQuery = "execute FONATEL.spObtenerInformeNombreTabla @nombre";

            InformeNombreTabla obj = null;

            using (var connection = new SqlConnection(Connection.DWSIGITEL))
            {
                obj = connection.Query<InformeNombreTabla>(SqlQuery, new { nombre = tabla }).Select(x => new InformeNombreTabla
                {
                    IdTabla = x.IdTabla,
                    NombreTabla = x.NombreTabla,
                    Estado = x.Estado,
                    InformeFilasValor = ObtenerInformeFilasValor(x.IdTabla),
                    InformeEncabezadoTablas = ObtenerInformeEncabezadoTabla(x.IdTabla)
                }).FirstOrDefault();

            }
            return obj;
        }

        internal List<InformeFilasValor> ObtenerInformeFilasValor(int IdTabla)
        {
            var SqlQuery = "execute FONATEL.spObtenerInformeFilasValor @id";

            List<InformeFilasValor> lista = null;

            using (var connection = new SqlConnection(Connection.DWSIGITEL))
            {
                lista = connection.Query<InformeFilasValor>(SqlQuery, new { id = IdTabla }).ToList();

            }
            return lista;
        }

        internal List<InformeEncabezadoTabla> ObtenerInformeEncabezadoTabla(int IdTabla)
        {
            var SqlQuery = "execute FONATEL.spObtenerInformeEncabezadoTabla @id";

            List<InformeEncabezadoTabla> lista = null;

            using (var connection = new SqlConnection(Connection.DWSIGITEL))
            {
                lista = connection.Query<InformeEncabezadoTabla>(SqlQuery, new { id = IdTabla }).ToList();

            }
            return lista;
        }
    }
}
