using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plantilla.NetCoreSwaggerBasicAuth.FrontEnd.Common
{
    public static class Extensions
    {
        private static int _retry = 5;

        public static string RequestServicio(string json_model, string path, Method method, string url_base,  string authorization)
        {
            string NameEntry = System.Reflection.MethodBase.GetCurrentMethod().Name;

            int Contador = 0;

            while (Contador <= _retry)
            {
                try
                {
                    var client = new RestClient($"{url_base}{path}");

                    client.Timeout = -1;

                    var request = new RestRequest(method);

                    request.AddHeader("Authorization", authorization);

                    request.AddHeader("Content-Type", "application/json-patch+json");

                    request.AddParameter("application/json-patch+json", json_model, ParameterType.RequestBody);

                    IRestResponse response = client.Execute(request);

                    if (response.Content.Contains("fechaEjecucion"))
                    {
                        return response.Content;
                    }

                    throw new Exception($"Error ejecutando {NameEntry} --- {response.ErrorMessage} --- {response.Content}");
                }
                catch (Exception ex)
                {
                    if (Contador == _retry)
                    {
                        throw ex;
                    }

                    Contador++;
                }
            }

            return string.Empty;
        }



    }
}
