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
    internal sealed class StockPriceRepository : IBaseRepository<StockPrice>, IStockPriceRepository
    {
        private readonly MyDBContext context;


        public StockPriceRepository(MyDBContext Context)
        {
            context = Context;   
        }

        public async Task<StockPrice> Create(StockPrice entity)
        {
            await context.StockPrices!.AddAsync(entity);
            return entity;
        }

        public Task<StockPrice> Delete(StockPrice entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<StockPrice>> FindAll()
        {
            return await Task.FromResult(context.StockPrices!.OrderByDescending(e => e.Id).AsNoTracking());
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
