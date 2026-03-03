using Repository.Interfaces;
using Service.Interfaces;

namespace Service.Product
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<string> GetProductsPaged(string? name, int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return "{\"error\": \"page and pageSize must be greater than zero\"}";
            }

            return await _repo.GetProductsPaged(name, page, pageSize);
        }

        public async Task<string> GetProductById(int id)
        {
            if (id <= 0)
            {
                return "{\"error\": \"id must be greater than zero\"}";
            }

            return await _repo.GetProductById(id);
        }

        public async Task<string> CreateProduct(string productJson)
        {
            if (string.IsNullOrWhiteSpace(productJson))
            {
                return "{\"error\": \"Product JSON payload is required\"}";
            }

            return await _repo.CreateProduct(productJson);
        }

        public async Task<string> UpdateProduct(int id, string productJson)
        {
            if (id <= 0)
            {
                return "{\"error\": \"id must be greater than zero\"}";
            }

            if (string.IsNullOrWhiteSpace(productJson))
            {
                return "{\"error\": \"Product JSON payload is required\"}";
            }

            return await _repo.UpdateProduct(id, productJson);
        }

        public async Task<string> DeleteProduct(int id)
        {
            if (id <= 0)
            {
                return "{\"error\": \"id must be greater than zero\"}";
            }

            return await _repo.DeleteProduct(id);
        }
    }
}
