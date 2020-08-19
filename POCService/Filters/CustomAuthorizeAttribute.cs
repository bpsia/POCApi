using POCService.Context;
using POCService.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace POCService.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private const string BasicAuthResponseHeader = "WWW-Authenticate";
        private const string BasicAuthResponseHeaderValue = "Basic";
        readonly DatabaseContext Context = new DatabaseContext();

        public string UsersConfigKey { get; set; }
        public string RolesConfigKey { get; set; }

        protected CustomPrincipal CurrentUser
        {
            get { return Thread.CurrentPrincipal as CustomPrincipal; }
            set { Thread.CurrentPrincipal = value as CustomPrincipal; }
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                AuthenticationHeaderValue authValue = actionContext.Request.Headers.Authorization;

                if (authValue != null && !String.IsNullOrWhiteSpace(authValue.Parameter) && authValue.Scheme == BasicAuthResponseHeaderValue)
                {
                    Credentials parsedCredentials = ParseAuthorizationHeader(authValue.Parameter);

                    if (parsedCredentials != null)
                    {
                        var user = Context.Users.Where(u => u.Name == parsedCredentials.Username).FirstOrDefault();
                        bool VerifyPassword = HashSalt.VerifyPassword(parsedCredentials.Password, user.PasswordHash, user.PasswordSalt);
                        if (user != null && VerifyPassword)
                        {
                            List<string> list = new List<string>();
                            string[] userinroles = user.Role.Split(',');
                            foreach (var item in userinroles)
                            {
                                list.Add(item);
                            }

                            var roles = list.ToArray();
                            var authorizedUsers = ConfigurationManager.AppSettings[UsersConfigKey];
                            var authorizedRoles = ConfigurationManager.AppSettings[RolesConfigKey];

                            Users = String.IsNullOrEmpty(Users) ? authorizedUsers : Users;
                            Roles = String.IsNullOrEmpty(Roles) ? authorizedRoles : Roles;

                            CurrentUser = new CustomPrincipal(parsedCredentials.Username, roles);

                            if (!String.IsNullOrEmpty(Roles))
                            {
                                if (!CurrentUser.IsInRole(Roles))
                                {
                                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                                    actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                                    return;
                                }
                            }

                            if (!String.IsNullOrEmpty(Users))
                            {
                                if (!Users.Contains(CurrentUser.UserId.ToString()))
                                {
                                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                                    actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                                    return;
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                return;

            }
        }
        private Credentials ParseAuthorizationHeader(string authHeader)
        {
            string[] credentials = Encoding.ASCII.GetString(Convert.FromBase64String(authHeader)).Split(new[] { ':' });

            if (credentials.Length != 2 || string.IsNullOrEmpty(credentials[0]) || string.IsNullOrEmpty(credentials[1]))
                return null;

            return new Credentials() { Username = credentials[0], Password = credentials[1], };
        }
    }
    public class Credentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}