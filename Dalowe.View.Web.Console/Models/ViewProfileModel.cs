using Dalowe.View.Web.Framework.Models.Account;
using Dalowe.View.Web.Framework.Models.Visa;

namespace Dalowe.View.Web.Console.Models
{
    public class ViewProfileModel : UserModel
    {
        public ViewProfileModel()
        {
            PasswordModel = new LocalPasswordModel();
        }

        public LocalPasswordModel PasswordModel { get; set; }
    }
}