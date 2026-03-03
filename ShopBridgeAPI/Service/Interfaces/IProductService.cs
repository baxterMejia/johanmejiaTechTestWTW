using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IProductService
    {
        Task<string> GetProductsPaged(string? name, int page, int pageSize);
        Task<string> GetProductById(int id);
        Task<string> CreateProduct(string productJson);
        Task<string> UpdateProduct(int id, string productJson);
        Task<string> DeleteProduct(int id);
    }
}
