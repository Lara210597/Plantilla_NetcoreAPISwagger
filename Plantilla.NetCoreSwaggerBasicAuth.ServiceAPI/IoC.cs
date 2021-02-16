using Plantilla.NetCoreSwaggerBasicAuth.BusinessLogic.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plantilla.NetCoreSwaggerBasicAuth.Servicio
{
 
    /// <summary>
    /// 
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static void IoC_Injection(IServiceCollection services)
        {
            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IEmployeeBL, EmployeeBL>();
            //agregar...
            
        }

    }
}
