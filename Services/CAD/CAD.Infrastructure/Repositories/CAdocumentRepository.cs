using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class CAdocumentRepository : RepositoryBase<CAdocument>, ICAdocumentRepository
    {
        public CAdocumentRepository(HSBCContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<CAdocument>> GetOrdersByUserName(string userName)
        {
            var orderList = await _dbContext.cadocuments
                                .Where(o => o.businesspartner == userName)
                                .ToListAsync();
            return orderList;
        }
    }
}
