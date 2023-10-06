using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext context, ILogger<OrderContextSeed> logger)
        {
            if (!context.Orders.Any())
            {
                context.Orders.AddRange(GetPreconfiguredOrders());
                await context.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order() 
                {
                    UserName = "kelvinlam", 
                    FirstName = "Kelvin", 
                    LastName = "Lam", 
                    EmailAddress = "kelvinlam_1986@yahoo.com.vn", 
                    AddressLine = "240/2 Le Thanh Ton street District 1", 
                    Country = "VietNam", 
                    TotalPrice = 350,
                    CVV = "123456",
                    CardName = "Minh",
                    CardNumber = "123456",
                    Expiration = "2024/10",
                    PaymentMethod = 1,
                    State = "HCM",
                    ZipCode = "79"
                }
            };
        }
    }
}
