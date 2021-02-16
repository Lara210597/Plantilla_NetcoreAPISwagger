using Microsoft.Extensions.Configuration;

namespace Plantilla.NetCoreSwaggerBasicAuth.Servicio
{
    /// <summary>
    /// 
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// Usuario para Basic Authentication
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ValidateCredentials(string username, string password, IConfiguration config)
        {
            return username.Equals(config["ApiCredentials:api_username"]) && password.Equals(config["ApiCredentials:api_userpassword"]);
        }
    }
}
