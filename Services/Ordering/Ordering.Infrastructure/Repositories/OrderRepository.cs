﻿using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            var orderList = await _dbContext.orders
                                //.Where(o => o.UserName == userName)
                                .Where(o => o.username == userName)
                                .ToListAsync();
            return orderList;
        }
    }
}
