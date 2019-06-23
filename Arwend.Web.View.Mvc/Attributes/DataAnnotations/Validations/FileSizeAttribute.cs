using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Arwend.Web.View.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FileSizeAttribute : ValidationAttribute
    {
        private int nMaxSize;

        public FileSizeAttribute(int maxSize)
        {
            this.nMaxSize = maxSize;
        }

        public FileSizeAttribute()
        {
        }
        private int MaxSize
        {
            get
            {
                if (this.nMaxSize <= 0)
                    this.nMaxSize = ConfigurationManager.UploadLimit * 1024;
                return nMaxSize;
            }
        }
        public override bool IsValid(object value)
        {
            if (value == null) return true;

            return (value as HttpPostedFileBase).ContentLength <= this.MaxSize;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Dosya boyutu {0} KB değerini aşmamalıdır.", this.MaxSize);
        }
    }
}
