using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class HSBCContextSeed
    {
        public static async Task SeedAsync(HSBCContext orderContext, ILogger<HSBCContextSeed> logger)
        {            
            if (!orderContext.cadocuments.Any())
            {
                orderContext.cadocuments.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(HSBCContext).Name);
            }
        }

        private static IEnumerable<CAdocument> GetPreconfiguredOrders()
        {
            return new List<CAdocument>
            {
                new CAdocument() {
                    security = true 
                    //firstname = "Mehmet", 
                    //lastname = "Ozkaya", 
                    //emailaddress = "ezozkme@gmail.com", 
                    //addressline = "Bahcelievler", 
                    //country = "Turkey", 
                    //totalprice = "350"
                    }
            };
        }
    }
}
