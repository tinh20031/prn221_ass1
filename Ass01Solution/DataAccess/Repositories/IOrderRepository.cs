using BusinessObject;
using BusinessObject.Enum;
using DataAccess.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IOrderRepository
    {
        Order CreateOrder(CreateOrderDto createOrderDto);

        Order? GetOrderByStatus(int memberId, OrderStatus orderStatus);

        void UpdateOrderFreight(int orderId);

        void UpdateOrderStatus(int orderId, OrderStatus orderStatus);

        int SaveChanges();

        List<GetOrderDto> GetOrders(DateTime? startDate = default, DateTime? endDate = default, int? memberId = null);

        void ConfirmOrder(ConfirmOrderDto confirmOrderDto);

        GetFullOrderInfoDto? GetOrderDetailsById(int id);

        void UpdateOrder(int orderId, UpdateOrderDto updateOrderDto);

        void DeleteOrder(int orderId);
    }
}
