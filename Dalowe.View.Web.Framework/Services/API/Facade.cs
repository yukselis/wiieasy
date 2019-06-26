using Dalowe.Domain.Base;

namespace Dalowe.View.Web.Framework.Services.API
{
    public class Facade
    {
        private AccountApi _oAccount;
        private LogApi _oLog;
        private VisaApi _oVisa;
        private ManagementApi _oManagement;
        private NotificationApi _oNotificationApi;

        public Facade(Services.Facade services)
        {
            Services = services;
        }

        public Services.Facade Services { get; }

        public AccountApi Account => _oAccount ?? (_oAccount = new AccountApi(this));
        
        public LogApi Log => _oLog ?? (_oLog = new LogApi(this));

        public VisaApi Visa => _oVisa ?? (_oVisa = new VisaApi(this));

        public ManagementApi Management => _oManagement ?? (_oManagement = new ManagementApi(this));

        public NotificationApi Notification => _oNotificationApi ?? (_oNotificationApi = new NotificationApi(this));

        public void Add<TEntity>(TEntity entity) where TEntity : Entity
        {
            Account.Add(entity);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : Entity
        {
            Account.Update(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : Entity
        {
            Account.Delete(entity);
        }
    }
}