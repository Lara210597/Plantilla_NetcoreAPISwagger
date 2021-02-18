using Microsoft.Extensions.Configuration;

namespace Plantilla.NetCoreSwaggerBasicAuth.API
{
    /// <summary>
    /// 
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool ValidateCredentials(string username, string password, IConfiguration config)
        {           
            return username.Equals(config["BasicAuthenticationCredentials:username"]) && password.Equals(config["BasicAuthenticationCredentials:password"]);
        }
    }
}
