using System.Collections.Generic;
using System.Web.Script.Serialization;
using System;
using Newtonsoft.Json;

namespace GB.SUTEL.Entities.Utilidades
{
    public class JSONResult<T>
    {
        public JSONResult()
        {
            this.ok = true;
            this.state = 200;
            this.strMensaje = string.Empty;
        }

        public string toJSON()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { state = this.state, ok = this.ok.ToString(), strMensaje = this.strMensaje, data = this.data});
        }

        public string toJSONLoopHandlingIgnore()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { state = this.state, ok = this.ok.ToString(), strMensaje = this.strMensaje, data = this.data }, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        public string errorServer(Exception ex)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { state = this.state, ok = this.ok.ToString(), strMensaje = this.strMensaje, data = this.data });
        }
        
        public T data { get; set; }
        public bool ok { get; set; }
        public int state { get; set; }
        public string strMensaje { get; set; }
    }
}
