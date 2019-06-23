namespace Dalowe.View.Web.Framework.Services
{
    public class Facade
    {
        private API.Facade oAPI;

        public Facade(Client client)
        {
            Client = client;
        }

        public Client Client { get; }

        public API.Facade API => oAPI ?? (oAPI = new API.Facade(this));
    }
}