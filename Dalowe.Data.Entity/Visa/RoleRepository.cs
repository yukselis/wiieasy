using Dalowe.Data.Visa;
using Dalowe.Domain.Visa;

namespace Dalowe.Data.Entity.Visa
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}