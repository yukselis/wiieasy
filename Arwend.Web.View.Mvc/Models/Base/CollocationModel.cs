using System;

namespace Arwend.Web.View.Mvc.Models.Base
{
    public class CollocationModel
    {
        public OrderWay OrderWay { get; set; }
        public bool Enabled { get; set; }
    }
    public enum OrderWay : int
    {
        Ascending = 0,
        Descending = 1
    }
}
