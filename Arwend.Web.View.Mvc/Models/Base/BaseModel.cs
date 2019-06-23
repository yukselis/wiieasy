using Arwend.Web.View.Mvc.Attributes.DataAnnotations;
using Arwend.Web.View.Mvc.Models.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Arwend.Web.View.Mvc.Models.Base
{
    public abstract class BaseModel : ISensible
    {
        public BaseModel()
        {
            this.Breadcrumbs = new List<BreadcrumbsItemModel>();
            this.AlertMessage = new AlertMessageModel();
            this.CreateSharedContext();
        }
        protected virtual void CreateSharedContext() { 
        
        }
        
        public IList<BreadcrumbsItemModel> Breadcrumbs { get; set; }
        public AlertMessageModel AlertMessage { get; set; }
        public SharedContext Context { get; set; }   
    }
}
