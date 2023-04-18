using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
    public class TextoPantallaController : Controller
    {
        [HttpGet]
        [Route("ObtenerTextoPantalla/{idPantalla}")]
        public async Task<List<RespuestaTextoPantallaSIGITEL>> Get(int idPantalla)
        {
            return await ObtenerTextoPantalla(idPantalla);
        }

        internal async Task<List<RespuestaTextoPantallaSIGITEL>> ObtenerTextoPantalla(int idPantalla)
        {
            var SqlQuery = "execute pa_ObtenerTextoPantallaSIGITEL @IdCatalogoPantallaSIGITEL";

            List<RespuestaTextoPantallaSIGITEL> lista = null;

            using (var connection = new SqlConnection(Connection.SIGITELSIMEFDatabase))
            {
                lista = (await connection.QueryAsync<RespuestaTextoPantallaSIGITEL>(SqlQuery, new { IdCatalogoPantallaSIGITEL = idPantalla })).ToList();
            }
            return lista;
        }
    }
}
