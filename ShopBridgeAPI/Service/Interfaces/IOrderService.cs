using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IOrderService
    {
        Task<string> GetOrders();
        Task<string> GetOrderById(int id);
        Task<string> CreateOrder(string orderJson);
    }
}
