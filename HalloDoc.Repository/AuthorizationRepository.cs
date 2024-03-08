

//using System.Web.Mvc;
//using System.Security.Claims;
//using System.IdentityModel.Tokens.Jwt;
//using Microsoft.AspNetCore.Routing;
//using HalloDoc.Repository.IRepository;
//namespace HalloDoc.Repository
//{
//    [AttributeUsage(AttributeTargets.All)]
//    public class CustomAuthorize : Attribute, IAuthorizationFilter
//    {

//        private readonly string _role;
//        public CustomAuthorize(string role="")
//        {
//            this._role = role;  
//        }
//        public void OnAuthorization(AuthorizationContext filterContext)
//        {
//            var jwtService = filterContext.HttpContext.RequestServices.GetService<IJwtService>();
//            if(jwtService == null)
//            {
//                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login" }));
//                return;
//            }
//            var request=filterContext.HttpContext.Request;
//            var token = request.Cookies["jwt"];
//            if (token == null || !jwtService.ValidateToken(token, out JwtSecurityToken jwtToken)) { filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login" })); }

//            var roleClaim = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);
//            if(roleClaim == null) {
//                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login" }));
//                return;
//            }
//            if(string.IsNullOrWhiteSpace(_role)||roleClaim.Value!=_role)
//            {
//                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "AccessDenied" }));
//            }
//        }

//    }
//}



using HalloDoc.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace Services.Implementation
{
    public class AuthorizationRepository : Attribute, IAuthorizationFilter, IAuthorizationRepository
    {
        private readonly string _role;

        public AuthorizationRepository(string role = "")
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var jwtservice = context.HttpContext.RequestServices.GetService<IJwtService>();
            if (jwtservice == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login" }));
                return;
            }

            var request = context.HttpContext.Request;
            var token = request.Cookies["jwt"];

            if (token == null || !jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login" }));
                return;
            }

            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");

            if (roleClaim == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login" }));
                return;
            }

            if (string.IsNullOrEmpty(_role) || roleClaim.Value != _role)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login" }));
            }


        }
    }

}
