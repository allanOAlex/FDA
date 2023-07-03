using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TB.Application.Abstractions.IRepositories;
using TB.Domain.Models;
using TB.Persistence.MySQL.MySQL;
using TB.Shared.Requests.Logging;
using TB.Shared.Responses.Logging;

namespace TB.Infrastructure.Implementations.Repositories
{
    internal sealed class LoggingRepository : IBaseRepository<Log>, ILoggingRepository
    {
        private readonly IConfiguration configuration;
        private readonly MyDBContext context;

        public LoggingRepository(IConfiguration Configuration, MyDBContext Context)
        {
            configuration = Configuration;
            context = Context;
        }

        public Task<Log> Create(Log entity)
        {
            throw new NotImplementedException();
        }

        public Task<Log> Delete(Log entity)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<Log>> FindAll()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<Log>> FindByCondition(Expression<Func<Log, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Log> FindByConnectionId(Log log)
        {
            throw new NotImplementedException();
        }

        public Task<Log> FindByCorrelationId(Log log)
        {
            throw new NotImplementedException();
        }

        public Task<Log?> FindById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<Log> Update(Log entity)
        {
            throw new NotImplementedException();
        }
    }
}
