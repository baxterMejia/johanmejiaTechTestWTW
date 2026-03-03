using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    /// <summary>
    /// Repository for order operations.
    /// All operations return JSON as a string.
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// Retrieves all orders.
        /// Must include associated items using eager loading.
        /// </summary>
        /// <returns>JSON containing the list of orders and their items.</returns>
        Task<string> GetOrders();

        /// <summary>
        /// Retrieves an order by its identifier.
        /// </summary>
        /// <param name="id">Order identifier.</param>
        /// <returns>JSON with the order data and items, or an empty object if not found.</returns>
        Task<string> GetOrderById(int id);

        /// <summary>
        /// Creates a new order.
        /// The JSON must include order data and the list of items.
        /// Expected payload example:
        /// {
        ///   "customerId": 123,
        ///   "orderDate": "2024-01-15",
        ///   "items": [
        ///       { "productId": 1, "quantity": 2 },
        ///       { "productId": 5, "quantity": 1 }
        ///   ]
        /// }
        /// </summary>
        /// <param name="orderJson">JSON containing the order information and items.</param>
        /// <returns>JSON with the created order, calculated totals, or validation errors.</returns>
        Task<string> CreateOrder(string orderJson);
    }
}
