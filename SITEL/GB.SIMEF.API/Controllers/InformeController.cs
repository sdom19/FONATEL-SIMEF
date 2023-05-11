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
        /// <summary>
        /// Método para obtener json con la informacion guardada
        /// </summary>
        /// <param name="tabla">Nombre de tabla</param>
        [HttpGet]
        [Route("~/api/Informe/{tabla}")]
        public async Task<JObject> Get(string tabla)
        {
            InformeNombreTabla informe = ObtenerInformeNombreTabla(tabla);
            var resultado = new JObject();
            return
            await Task.Run(() => {
                if (informe != null)
                {
                    List<string> l = ObtenerInformeValor(informe.IdInformeNombreTabla);
                    string json = "[" + String.Join(",", l) + "]";
                    JArray filas = JArray.Parse(json);

                    resultado.Add(tabla, filas);
                }
                return resultado;
            });
        }

        /// <summary>
        /// Método para obtener json con la informacion guardada de varias tablas
        /// </summary>
        /// <param name="nn">Nombre de tablas</param>
        [HttpGet]
        [ArrayInput("nn")]
        [Route("~/Informes/{nn?}")]
        public JObject Informes(string nn = "")
        {
            //Se crea el objeto que se retornara
            var resultado = new JObject();

            string[] lista = nn.Split(",");

            //Se comprueba que se tiene datos recibidos
            if (lista.Length > 0)
            {
                //Se recorren datos recibidos
                foreach (var item in lista)
                {
                    InformeNombreTabla informe = ObtenerInformeNombreTabla(item);

                    if (informe != null)
                    {
                        List<string> l = ObtenerInformeValor(informe.IdInformeNombreTabla);
                        string json = "[" + String.Join(",", l) + "]";
                        JArray filas = JArray.Parse(json);

                        resultado.Add(item, filas);
                    }
                }
            }
            //Se retorna los valores
            return resultado;
        }

        /// <summary>
        /// Método para obtener el informe nombre tabla
        /// </summary>
        /// <param name="tabla">Nombre de tabla</param>
        /// <returns>Lista de InformeNombreTabla</returns>
        internal InformeNombreTabla ObtenerInformeNombreTabla(string tabla)
        {
            var SqlQuery = "execute pa_ObtenerInformeNombreTabla @nombre";

            InformeNombreTabla obj = null;

            using (var connection = new SqlConnection(Connection.SIGITELDatabase))
            {
                obj = connection.Query<InformeNombreTabla>(SqlQuery, new { nombre = tabla }).Select(x => new InformeNombreTabla
                {
                    IdInformeNombreTabla = x.IdInformeNombreTabla,
                    NombreTabla = x.NombreTabla,
                    Estado = x.Estado,
                }).FirstOrDefault();

            }
            return obj;
        }

        /// <summary>
        /// Método para obtener los json para el BI
        /// </summary>
        /// <param name="IdTabla">Nombre de tabla</param>
        /// <returns>Lista de json en string</returns>
        internal List<string> ObtenerInformeValor(int IdTabla)
        {
            var SqlQuery = "execute pa_ObtenerInformeValor @idTabla";

            List<string> lista = null;

            using (var connection = new SqlConnection(Connection.SIGITELDatabase))
            {
                lista = connection.Query<string>(SqlQuery, new { idTabla = IdTabla }).ToList();
            }
            return lista;
        }

    }
}
