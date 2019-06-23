using Dalowe.View.Web.Framework.Models.Base;

namespace Dalowe.View.Web.Framework
{
    public interface IViewModelFactory
    {
        T Create<T>() where T : SharedContext, new();
        void Set<T>(T model) where T : SharedContext, new();
    }
}