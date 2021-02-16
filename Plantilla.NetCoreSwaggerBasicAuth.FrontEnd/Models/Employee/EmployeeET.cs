using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plantilla.NetCoreSwaggerBasicAuth.FrontEnd.Models.Employee
{
   
    public class EmployeeET
    {
        public Model[] model { get; set; }
        public object exception { get; set; }
        public DateTime fechaEjecucion { get; set; }
    }

    public class Model
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public string country { get; set; }
        public string email { get; set; }
    }




}
