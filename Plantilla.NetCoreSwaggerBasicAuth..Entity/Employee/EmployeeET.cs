using System;
using System.Collections.Generic;
using System.Text;

namespace Plantilla.NetCoreSwaggerBasicAuth.Entity.Employee
{
    public class EmployeeET
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
    }
}
