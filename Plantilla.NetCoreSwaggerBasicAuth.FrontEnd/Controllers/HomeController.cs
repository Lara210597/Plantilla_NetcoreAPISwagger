using Plantilla.NetCoreSwaggerBasicAuth.Entity.Employee;
using Plantilla.NetCoreSwaggerBasicAuth.Entity.ResponseGeneric;
using Plantilla.NetCoreSwaggerBasicAuth.FrontEnd.Common;
using Plantilla.NetCoreSwaggerBasicAuth.FrontEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Plantilla.NetCoreSwaggerBasicAuth.FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        public Shared _shared;

        public IMemoryCache _cache;

        public HomeController(IConfiguration configuration, IHttpContextAccessor context, IMemoryCache cache)
        {
            _shared = new Shared(configuration, context, cache);
        }
    
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    
        public IActionResult ListdoEmployee(EmployeeET model)
        {
            try
            {
                return Json(_shared.EjecutarServicio<ResponseGeneric<IEnumerable<EmployeeET>>>(JsonConvert.SerializeObject(model), "Employee/GetEmployees"));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
