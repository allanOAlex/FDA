using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TB.Application.Abstractions.IRepositories;
using TB.Domain.Models;
using TB.Persistence.SQLServer;

namespace TB.Infrastructure.Implementations.Repositories
{
    internal sealed class FinancialDataRepository : IFinancialDataRepository
    {
        private readonly DBContext context;

        public FinancialDataRepository(DBContext Context)
        {
            context = Context;
        }

        



    }
}
