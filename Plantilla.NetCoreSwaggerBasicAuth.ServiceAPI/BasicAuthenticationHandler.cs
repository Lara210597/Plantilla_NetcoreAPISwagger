﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Plantilla.NetCoreSwaggerBasicAuth.Servicio;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Plantilla.NetCoreSwaggerBasicAuth.Servicio
{
    /// <summary>
    /// 
    /// </summary>
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        readonly IUserService _userService;

        readonly IConfiguration _config;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        public BasicAuthenticationHandler(
            IUserService userService,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock, 
            IConfiguration configuration)
            : base(options, logger, encoder, clock)
        {
            _userService = userService;
            _config = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Authenticate"] = "Basic";
            return base.HandleChallengeAsync(properties);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
        
            string username = null;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
                username = credentials.FirstOrDefault();
                var password = credentials.LastOrDefault();

                if (!_userService.ValidateCredentials(username, password, _config))
                    throw new ArgumentException("Invalid credentials");
            }
            catch(Exception ex)
            {
                return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");
            }

            var claims = new[] {
                new Claim(ClaimTypes.Name, username)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}