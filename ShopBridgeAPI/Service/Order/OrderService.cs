using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Order
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;

        public OrderService(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<string> GetOrders()
        {
            return await _repo.GetOrders();
        }

        public async Task<string> GetOrderById(int id)
        {
            if (id <= 0)
            {
                return "{\"error\": \"id must be greater than zero\"}";
            }

            return await _repo.GetOrderById(id);
        }

        public async Task<string> CreateOrder(string orderJson)
        {
            if (string.IsNullOrWhiteSpace(orderJson))
            {
                return "{\"error\": \"Order JSON payload is required\"}";
            }

            return await _repo.CreateOrder(orderJson);
        }
    }
}
