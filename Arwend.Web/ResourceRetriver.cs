namespace Arwend.Web
{
	public class ResourceRetriver
	{
		private System.Web.UI.Page Page;
        public ResourceRetriver(System.Web.UI.Page Page)
        {
            this.Page = Page;
        }

		public virtual string GetResourceURL(string imageName, string nameSpace = "Arwend")
		{
			return this.Page.ClientScript.GetWebResourceUrl(this.GetType(), nameSpace + "." + imageName);
		}
	}
}
