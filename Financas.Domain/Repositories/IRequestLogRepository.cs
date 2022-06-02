using Financas.Core;

namespace Financas.Domain.Repositories
{
    public interface IRequestLogRepository : IRepository<RequestLog>, ITransientDependency
    {
    }
}
