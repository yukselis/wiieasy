using System;
using System.Web.Mvc;

namespace Arwend.Web.View.Mvc.Attributes
{
    public class AllowAuthenticatedAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            throw new NotImplementedException();
        }
    }
}
