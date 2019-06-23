using Dalowe.Data.Log;
using Dalowe.Domain.Log;

namespace Dalowe.Data.Entity.Log
{
    public class ActionLogRepository : Repository<ActionLog>, IActionLogRepository
    {
        public ActionLogRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}