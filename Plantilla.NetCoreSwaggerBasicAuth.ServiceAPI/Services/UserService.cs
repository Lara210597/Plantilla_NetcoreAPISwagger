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
        public bool ValidateCredentials(string username, string password)
        {
            return username.Equals("Exceltec_CRM") && password.Equals("Td9tRuVDPdW845rQ");
        }
    }
}
