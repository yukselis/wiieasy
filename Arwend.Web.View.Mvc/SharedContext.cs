using Arwend.Web.View.Mvc.Models.Base;
namespace Arwend.Web.View.Mvc
{
    public abstract class SharedContext
    {
        public bool IsAuthenticated { get { return this.CurrentUser != null; } }
        public BaseModel CurrentUser { get; set; }
    }
}
