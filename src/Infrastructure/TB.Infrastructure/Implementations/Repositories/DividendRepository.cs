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
    internal sealed class DividendRepository : IBaseRepository<Dividend>, IDividendRepository
    {
        private readonly MyDBContext context;


        public DividendRepository(MyDBContext Context)
        {
            context = Context;
            
        }


        public Task<Dividend> Create(Dividend entity)
        {
            throw new NotImplementedException();
        }

        public Task<Dividend> Delete(Dividend entity)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<Dividend>> FindAll()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<Dividend>> FindByCondition(Expression<Func<Dividend, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Dividend?> FindById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<Dividend> Update(Dividend entity)
        {
            throw new NotImplementedException();
        }
    }
}
