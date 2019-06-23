using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Arwend.Web.View.Mvc.Models.Base
{
    public class AlertMessageModel
    {
        public AlertMessageModel()
        {
            this.Options = new List<KeyValuePair<string, string>>();
        }

        public AlertMessageModel(string message, string type)
        {
            this.Type = type;
            this.Message = message;
            this.Show = true;
            this.Closeable = false;
            this.HasError = false;
        }

        [DefaultValue(true)]
        public bool Show { get; set; }

        public string Type { get; set; }

        [DefaultValue(false)]
        public bool Closeable { get; set; }

        public string Message { get; set; }

        public string Caption { get; set; }

        public List<KeyValuePair<string, string>> Options { get; set; }
        [DefaultValue(false)]
        public bool HasError { get; set; }

        public void Fail(string message)
        {
            this.HasError = true;
            this.Message = message;
        }
    }
}
