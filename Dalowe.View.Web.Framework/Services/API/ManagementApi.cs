using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dalowe.Domain.Management;
using Dalowe.Domain.Visa;
using Dalowe.View.Web.Framework.Services.API.Base;

namespace Dalowe.View.Web.Framework.Services.API
{
    public class ManagementApi : BaseApi
    {
        public ManagementApi(Facade facade) : base(facade)
        {
        }

        #region Write Members

        public void SaveCampaign(Campaign campaign)
        {
            Add(campaign);
        }

        public void DeleteCampaign(Campaign campaign)
        {
            Delete(campaign);
        }

        public void UpdateCampaign(Campaign campaign)
        {
            Update(campaign);
        }

        public void SaveAccessNode(AccessNode accessNode)
        {
            Add(accessNode);
        }

        public void DeleteAccessNode(AccessNode accessNode)
        {
            Delete(accessNode);
        }

        public void UpdateAccessNode(AccessNode accessNode)
        {
            Update(accessNode);
        }

        #endregion

        #region Read Members

        public Campaign GetCampaign(long campaignId)
        {
            Campaign campaign = null;

            using (var transaction = DbTransaction())
            {
                var repository = transaction.Repository<Campaign>();

                try
                {
                    campaign = repository.Find(u => u.ID == campaignId);
                }
                catch (Exception exc)
                {
                    Facade.Log.CreateErrorLog(ModuleName, "GetCampaign", "", exc, "");
                }
            }

            return campaign;
        }

        public List<Campaign> GetCampaigns(int page = 0, int pageSize = 0)
        {
            List<Campaign> campaigns = null;
            try
            {
                campaigns = GetAll<Campaign>();
            }
            catch (Exception exc)
            {
                Facade.Log.CreateErrorLog(ModuleName, "GetCampaigns", "", exc, "");
            }

            return campaigns;
        }

        public AccessNode GetAccessNode(long id)
        {
            AccessNode accessNode = null;

            using (var transaction = DbTransaction())
            {
                var repository = transaction.Repository<AccessNode>();

                try
                {
                    accessNode = repository.Find(u => u.ID == id);
                }
                catch (Exception exc)
                {
                    Facade.Log.CreateErrorLog(ModuleName, "GetAccessNode", "", exc, "");
                }
            }

            return accessNode;
        }

        public List<AccessNode> GetAccessNodes(int page = 0, int pageSize = 0)
        {
            List<AccessNode> accessNodes = null;
            try
            {
                accessNodes = GetAll<AccessNode>();
            }
            catch (Exception exc)
            {
                Facade.Log.CreateErrorLog(ModuleName, "GetAccessNodes", "", exc, "");
            }

            return accessNodes;
        }

        #endregion
    }
}
