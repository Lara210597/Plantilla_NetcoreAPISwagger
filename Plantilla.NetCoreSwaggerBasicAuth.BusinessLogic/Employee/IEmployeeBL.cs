using Plantilla.NetCoreSwaggerBasicAuth.Entity.Employee;
using Plantilla.NetCoreSwaggerBasicAuth.Entity.ResponseGeneric;
using System.Collections.Generic;
 

namespace Plantilla.NetCoreSwaggerBasicAuth.BusinessLogic.Employee
{
    public interface IEmployeeBL
    {
        ResponseGeneric<IEnumerable<EmployeeET>> GetEmployees(EmployeeET model);
        ResponseGeneric<EmployeeET> GetEmployee(EmployeeET model);
        ResponseGeneric<EmployeeET> UpsertEmployee(EmployeeET model);
    }
}