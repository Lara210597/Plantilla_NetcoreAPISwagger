using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Plantilla.NetCoreSwaggerBasicAuth.BusinessLogic.Employee;
using Plantilla.NetCoreSwaggerBasicAuth.Entity.ResponseGeneric;
using System.Collections.Generic;
using Plantilla.NetCoreSwaggerBasicAuth.Entity.Employee;

namespace Plantilla.NetCoreSwaggerBasicAuth.API.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]    
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class EmployeeController : ControllerBase
    {        
        private readonly IEmployeeBL _employeeBL;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeBL"></param>
        public EmployeeController(IEmployeeBL employeeBL)
        {
            this._employeeBL = employeeBL;   
        }

 

        /// <summary>
        /// Obtiene listado de Employee 
        /// </summary>
        /// <remarks>        
        /// <pre>Ejemplo de Input para probar
        /// </pre>
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseGeneric<IEnumerable<EmployeeET>> GetEmployees([FromBody] EmployeeET model)
        {            
            return this._employeeBL.GetEmployees(model);
        }

       /// <summary>
       /// Obtiene Emplyee por Id
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
        [HttpPost]
        public ResponseGeneric<EmployeeET> GetEmployee([FromBody] EmployeeET model)
        {
          

            return this._employeeBL.GetEmployee(model);
        }
        
    }
}
