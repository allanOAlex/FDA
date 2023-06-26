using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TB.Application.Abstractions.IRepositories;
using TB.Domain.Models;
using TB.Persistence.MySQL.MySQL;

namespace TB.Infrastructure.Implementations.Repositories
{
    internal sealed class EarningRepository : IBaseRepository<Earning>, IEarningRepository
    {
        private readonly MyDBContext context;
        public EarningRepository(MyDBContext Context)
        {
            context = Context;
        }


        public Task<Earning> Create(Earning entity)
        {
            throw new NotImplementedException();
        }

        public Task<Earning> Delete(Earning entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<Earning>> FindAll()
        {
            return await Task.FromResult(context.Earnings!.OrderByDescending(e => e.Id).AsNoTracking());
        }

        public Task<IQueryable<Earning>> FindByCondition(Expression<Func<Earning, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Earning?> FindById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<Earning> Update(Earning entity)
        {
            throw new NotImplementedException();
        }
    }
}
