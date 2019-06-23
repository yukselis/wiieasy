using Dalowe.View.Web.Framework.Models.Visa;

namespace Dalowe.View.Web.Framework.Models.Base
{
    public class SharedContext : Arwend.Web.View.Mvc.SharedContext
    {
        public new UserModel CurrentUser
        {
            get => (UserModel)base.CurrentUser;
            set => base.CurrentUser = value;
        }
    }
}