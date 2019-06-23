using System.Web.Mvc;
namespace Arwend.Web.View.Mvc
{
    public class HttpApplication : Arwend.Web.HttpApplication
    {
        protected virtual void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
        }
    }
}
