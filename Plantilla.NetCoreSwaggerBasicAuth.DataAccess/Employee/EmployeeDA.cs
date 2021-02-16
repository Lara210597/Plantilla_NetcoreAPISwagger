using Plantilla.NetCoreSwaggerBasicAuth.Entity.Employee;
using Plantilla.NetCoreSwaggerBasicAuth.Entity.ResponseGeneric;
using Plantilla.NetCoreSwaggerBasicAuth.Extensions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Plantilla.NetCoreSwaggerBasicAuth.DataAccess.Employee
{
    public class EmployeeDA
    {
        private readonly IConfiguration _configuration;

        public EmployeeDA(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public ResponseGeneric<EmployeeET> GetEmployee(EmployeeET model)
        {
            try
            {
                IEnumerable<EmployeeET> res = null;

                Dictionary<string, string> parameters = new Dictionary<string, string>();
               
                parameters.Add("@parametro1", model.Id.ToString());
                
                parameters.Add("@parametro2", model.Email.CleanParameter());

                string query = $@"SELECT Name,LastName FROM Employee WITH(NOLOCK) WHERE Id = '{model.Id}' ";

                //res = Extensions.Utils.Execute_Query<EmployeeET>(query, this._configuration["ConnectionStrings:CRM"]);

                //return new ResponseGeneric<EmployeeET>(res.First());

                EmployeeET employee = new EmployeeET()
                {
                    Id = 1,
                    Country = "CR",
                    Email = "test@test.com",
                    LastName = "lastname",
                    Name = "Maria"
                };

                return new ResponseGeneric<EmployeeET>(employee);


            }

            catch (Exception ex)
            {
                return new ResponseGeneric<EmployeeET>(new Exception($"{System.Reflection.MethodBase.GetCurrentMethod().Name} - JsonInput: {JsonConvert.SerializeObject(model)}", ex));
            }

        }

        public ResponseGeneric<IEnumerable<EmployeeET>> GetEmployees(EmployeeET model)
        {            
            try
            {
                IEnumerable<EmployeeET> res = null;

                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("@parametro1", model.Id.ToString());

                parameters.Add("@parametro2", model.Email.CleanParameter());
                
                res = Utils.Execute_Query<EmployeeET>("[dbo].[ect_Upsert_Employee]", parameters, this._configuration["ConnectionStrings:CRM"]);

                                return new ResponseGeneric<IEnumerable<EmployeeET>>(res);

                //List<EmployeeET> list_employee = new List<EmployeeET>();

                //for (int i = 0; i < 15; i++)
                //{
                //    EmployeeET employee = new EmployeeET()
                //    {
                //        Id = i,
                //        Country = "CR",
                //        Email = "test@test.com",
                //        LastName = "lastname",
                //        Name = "Maria"
                //    };

                //    list_employee.Add(employee);
                //}

                //return new ResponseGeneric<IEnumerable<EmployeeET>>(list_employee.ToList());

            }
            catch (Exception ex)
            {
                return new ResponseGeneric<IEnumerable<EmployeeET>>(new Exception($"{System.Reflection.MethodBase.GetCurrentMethod().Name} - JsonInput: {JsonConvert.SerializeObject(model)}", ex));
            }
            
        }
        public ResponseGeneric<EmployeeET> UpsertEmployee(EmployeeET model)
        {            
            try
            {
                IEnumerable<EmployeeET> res = null;

                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("@parametro1", model.Id.ToString());

                parameters.Add("@parametro2", model.Email.CleanParameter());
                
                res = Utils.Execute_Query<EmployeeET>("[dbo].[ect_Upsert_Employee]", parameters, this._configuration["ConnectionStrings:CRM"]);

                return new ResponseGeneric<EmployeeET>(res.First());
                            
            }
            catch (Exception ex)
            {
                return new ResponseGeneric<EmployeeET>(new Exception($"{System.Reflection.MethodBase.GetCurrentMethod().Name} - JsonInput: {JsonConvert.SerializeObject(model)}", ex));
            }
            
        }


        public ResponseGeneric<int> GetNextIdentity(EmployeeET model)
        {
            try
            {
                IEnumerable<int> res = null;

                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("@parametro1", model.Id.ToString());

                parameters.Add("@parametro2", model.Email.CleanParameter());

                res = Utils.Execute_Query<int>("[dbo].[ect_Upsert_Employee]", parameters, this._configuration["ConnectionStrings:CRM"]);

                return new ResponseGeneric<int>(res.First());

            }
            catch (Exception ex)
            {
                return new ResponseGeneric<int>(new Exception($"{System.Reflection.MethodBase.GetCurrentMethod().Name} - JsonInput: {JsonConvert.SerializeObject(model)}", ex));
            }

        }




    }
}
