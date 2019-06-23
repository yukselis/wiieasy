namespace Arwend.Web.View.Mvc.Models.Base.Interfaces
{
    public interface IAuditable
    {
        AuditModel History { get; set; }
    }
}
