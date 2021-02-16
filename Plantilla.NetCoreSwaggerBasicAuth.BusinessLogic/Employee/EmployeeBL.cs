using Plantilla.NetCoreSwaggerBasicAuth.DataAccess.Employee;
using Plantilla.NetCoreSwaggerBasicAuth.Entity.Employee;
using Plantilla.NetCoreSwaggerBasicAuth.Entity.ResponseGeneric;
 
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plantilla.NetCoreSwaggerBasicAuth.BusinessLogic.Employee
{
    public class EmployeeBL : IEmployeeBL
    {
        private readonly EmployeeDA _employeDA;

        public EmployeeBL(IConfiguration configuration)
        {
            this._employeDA = new EmployeeDA(configuration);
        }

        public ResponseGeneric<IEnumerable<EmployeeET>> GetEmployees(EmployeeET model)
        {
            //logica negocio 
            //paso1
            //paso2

            return this._employeDA.GetEmployees(model);
        }

        public ResponseGeneric<EmployeeET> UpsertEmployee(EmployeeET model)
        {
            //logica negocio 
            //paso1
            //paso2

            return this._employeDA.UpsertEmployee(model);
        }

        public ResponseGeneric<EmployeeET> GetEmployee(EmployeeET model)
        {
            //logica negocio 
            //paso1
            //paso2

            return this._employeDA.GetEmployee(model);
        }


    }


}
