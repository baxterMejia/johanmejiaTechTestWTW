using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repository.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _dbContext;

        public ProductRepository(ProductContext productDbContext)
        {
            _dbContext = productDbContext;
        }

        public async Task<string> GetProductById(int id)
        {
            var product = await _dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == id);

            return JsonSerializer.Serialize(product);
        }

        public async Task<string> GetProductsPaged(string? name, int page, int pageSize)
        {
            var query = _dbContext.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(p => p.Name.Contains(name));

            var total = await query.CountAsync();

            var results = await query
                .OrderBy(p => p.ProductId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = new
            {
                page,
                pageSize,
                total,
                items = results
            };

            return JsonSerializer.Serialize(response);
        }

        public async Task<string> CreateProduct(string productJson)
        {
            var product = JsonSerializer.Deserialize<DataAccess.Models.Product>(productJson);
            if (product == null)
            {
                return JsonSerializer.Serialize(new
                {
                    error = "Invalid JSON payload."
                });
            }

            product.ProductId = 0;
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return JsonSerializer.Serialize(product);
        }

        public async Task<string> UpdateProduct(int id, string productJson)
        {
            var existing = await _dbContext.Products.FindAsync(id);
            if (existing == null)
            {
                return JsonSerializer.Serialize(new
                {
                    error = "Product not found."
                });
            }

            var updated = JsonSerializer.Deserialize<DataAccess.Models.Product>(productJson);
            if (updated == null)
            {
                return JsonSerializer.Serialize(new
                {
                    error = "Invalid JSON payload."
                });
            }

            existing.Name = updated.Name;
            existing.Price = updated.Price;
            existing.Stock = updated.Stock;
            existing.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return JsonSerializer.Serialize(existing);
        }

        public async Task<string> DeleteProduct(int id)
        {
            var existing = await _dbContext.Products
                .Include(p => p.OrderItems)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (existing == null)
            {
                return JsonSerializer.Serialize(new
                {
                    error = "Product not found."
                });
            }

            if (existing.OrderItems.Any())
            {
                return JsonSerializer.Serialize(new
                {
                    error = "Product is referenced in orders.",
                    status = 409
                });
            }

            _dbContext.Products.Remove(existing);
            await _dbContext.SaveChangesAsync();

            return JsonSerializer.Serialize(new
            {
                message = "Product deleted successfully."
            });
        }
    }
}
