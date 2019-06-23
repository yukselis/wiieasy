using Dalowe.Data.Visa;
using Dalowe.Domain.Visa;

namespace Dalowe.Data.Entity.Visa
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}