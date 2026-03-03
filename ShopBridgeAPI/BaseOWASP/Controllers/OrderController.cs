using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.Text.Json;

namespace ShopBridgeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var result = await _service.GetOrders();
            return Content(result, "application/json");
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetOrderById(id);
            return Content(result, "application/json");
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
        {
            // Serialización directa, sin mapper
            var json = JsonSerializer.Serialize(dto);

            var result = await _service.CreateOrder(json);
            return Content(result, "application/json");
        }
    }
}
