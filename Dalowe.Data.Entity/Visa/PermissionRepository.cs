using Dalowe.Data.Visa;
using Dalowe.Domain.Visa;

namespace Dalowe.Data.Entity.Visa
{
    public class PermissionRepository : Repository<Permission>, IPermissionrRepository
    {
        public PermissionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}