using AutoMapper;
using Dalowe.View.Web.Framework.Models.Base;
using Dalowe.View.Web.Framework.Models.Visa;

namespace Dalowe.View.Web.Framework
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly Client _Client;

        public ViewModelFactory(Client client)
        {
            _Client = client;
        }

        public T Create<T>() where T : SharedContext, new()
        {
            var model = new T();
            Set(model);

            return model;
        }

        public void Set<T>(T model) where T : SharedContext, new()
        {
            if (_Client != null && _Client.IsAuthenticated)
                model.CurrentUser = Mapper.Map<UserModel>(_Client.CurrentUser);
        }
    }
}