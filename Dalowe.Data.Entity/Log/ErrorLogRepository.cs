using Dalowe.Data.Log;
using Dalowe.Domain.Log;

namespace Dalowe.Data.Entity.Log
{
    public class ErrorLogRepository : Repository<ErrorLog>, IErrorLogRepository
    {
        public ErrorLogRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}