using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TB.Application.Abstractions.IRepositories;
using TB.Domain.Models;
using TB.Persistence.SQLServer;

namespace TB.Infrastructure.Implementations.Repositories
{
    internal sealed class FinancialDataRepository : IBaseRepository<StockPrice>, IFinancialDataRepository
    {
        private readonly DBContext context;

        public FinancialDataRepository(DBContext Context)
        {
            context = Context;
        }

        public async Task<StockPrice> Create(StockPrice entity)
        {
            await context.FinancialData!.AddAsync(entity);
            return entity;
        }

        public Task<StockPrice> Delete(StockPrice entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<StockPrice>> FindAll()
        {
            return await Task.FromResult(context.FinancialData!.OrderByDescending(e => e.Id).AsNoTracking());

        }

        public Task<IQueryable<StockPrice>> FindByCondition(Expression<Func<StockPrice, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<StockPrice?> FindById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<StockPrice> Update(StockPrice entity)
        {
            throw new NotImplementedException();
        }
    }
}
