using Microsoft.Extensions.Configuration;
using System;
namespace Plantilla.NetCoreSwaggerBasicAuth.API
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool ValidateCredentials(String username, String password, IConfiguration config);
    }
}
