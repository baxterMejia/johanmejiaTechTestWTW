namespace Repository.Order
{
    using DataAccess.Models;
    using Microsoft.EntityFrameworkCore;
    using Repository.Interfaces;
    using System;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class OrderRepository : IOrderRepository
    {
        private readonly ProductContext _dbContext;

        public OrderRepository(ProductContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GetOrders()
        {
            var orders = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .AsNoTracking()
                .ToListAsync();

            return JsonSerializer.Serialize(orders);
        }

        public async Task<string> GetOrderById(int id)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return JsonSerializer.Serialize(new { });

            return JsonSerializer.Serialize(order);
        }

        public async Task<string> CreateOrder(string orderJson)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var payload = JsonSerializer.Deserialize<Order>(orderJson, options);
            if (payload == null)
            {
                return JsonSerializer.Serialize(new
                {
                    error = "Invalid JSON payload."
                });
            }

            if (payload.OrderItems == null || payload.OrderItems.Count == 0)
            {
                return JsonSerializer.Serialize(new
                {
                    error = "Order must contain at least one item."
                });
            }

            // Validar productos
            var productIds = payload.OrderItems.Select(i => i.ProductId).ToList();
            var products = await _dbContext.Products
                .Where(p => productIds.Contains(p.ProductId))
                .ToListAsync();

            if (products.Count != payload.OrderItems.Count)
            {
                return JsonSerializer.Serialize(new
                {
                    error = "One or more products do not exist."
                });
            }


            var order = new Order
            {
                CustomerName = payload.CustomerName,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();


            foreach (var item in payload.OrderItems)
            {
                var product = products.First(p => p.ProductId == item.ProductId);

                var newItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                };

                _dbContext.OrderItems.Add(newItem);
            }

            await _dbContext.SaveChangesAsync();


            var created = await _dbContext.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(i => i.Product)
            .FirstAsync(o => o.Id == order.Id);
         
            var dto = new
            {
                created.Id,
                created.CustomerName,
                created.CreatedAt,
                Items = created.OrderItems.Select(i => new
                {
                    i.ProductId,
                    i.Quantity,
                    i.UnitPrice,
                    ProductName = i.Product.Name
                })
            };

            return JsonSerializer.Serialize(dto);
        }
    }
}
