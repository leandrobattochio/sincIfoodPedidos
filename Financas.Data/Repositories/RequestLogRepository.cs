using Financas.Domain;
using Financas.Domain.Repositories;

namespace Financas.Data.Repositories
{
    public class RequestLogRepository : BaseRepository<RequestLog>, IRequestLogRepository
    {
        public RequestLogRepository(FinancasDbContext context) : base(context)
        {
        }
    }
}
