using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Domain.Models;
using TB.Shared.Requests.Logging;
using TB.Shared.Responses.Logging;

namespace TB.Application.Abstractions.IRepositories
{
    public interface ILoggingRepository : IBaseRepository<Log>
    {
        Task<Log> FindByConnectionId(Log log);
        Task<Log> FindByCorrelationId(Log log);

    }
}
