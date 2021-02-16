using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plantilla.NetCoreSwaggerBasicAuth.FrontEnd.Common
{
    public class Shared
    {
        public IConfiguration _IConfiguration;

        public IHttpContextAccessor _IHttpContextAccessor;

        public IMemoryCache _IMemoryCache;
        
        public Shared(IConfiguration iConfiguration, IHttpContextAccessor context, IMemoryCache cache)
        {
            this._IConfiguration = iConfiguration;

            this._IHttpContextAccessor = context;

            this._IMemoryCache = cache;
        }

        public T EjecutarServicio<T>(string inputJson, string path)
        {
            string response = Extensions.RequestServicio(
                inputJson,
                path,
                RestSharp.Method.POST,
                this._IConfiguration["UrlServicios:CRM:UrlBase"],
                this._IConfiguration["UrlServicios:CRM:Authorization"]
                );

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response);            
        }

        public string EjecutarServicio(string inputJson, string path)
        {
            string response = Extensions.RequestServicio(
                inputJson,
                path,
                RestSharp.Method.POST,
                this._IConfiguration["UrlServicios:CRM:UrlBase"],
                this._IConfiguration["UrlServicios:CRM:Authorization"]
                );

            return response;
        }

        //public Ejemplo Ejemplo(string mol)
        //{
        //    string entryCache = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //    string modelJson;

        //    if (_IMemoryCache.TryGetValue(entryCache, out modelJson))
        //    {
        //        return Newtonsoft.Json.JsonConvert.DeserializeObject<Model>(modelJson);
        //    }

        //    var items = Mode("", $"metodo?pais={pais}");

        //    string serialized = Newtonsoft.Json.JsonConvert.SerializeObject(items);

        //    _IMemoryCache.Set(entryCache, serialized, TimeSpan.FromHours(CacheHourExpire()));

        //    return items;
        //}

    }
}
