using BusinessObject;
using BusinessObject.Enum;
using DataAccess.DAO;
using DataAccess.DTOs.Order;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public void ConfirmOrder(ConfirmOrderDto confirmOrderDto)
        {
            var order = OrderDAO.Instance.GetOrderById(confirmOrderDto.OrderId);

            TypeAdapterConfig<ConfirmOrderDto, Order>.NewConfig().IgnoreNullValues(true);

            confirmOrderDto.Freight = OrderDAO.Instance.CalculateOrderTotal(confirmOrderDto.OrderId);
            confirmOrderDto.Adapt(order);

            OrderDAO.Instance.SaveChanges();
        }

        public Order CreateOrder(CreateOrderDto createOrderDto)
        {
            var order = createOrderDto.Adapt<Order>();
            OrderDAO.Instance.AddOrder(order);
            OrderDAO.Instance.SaveChanges();

            return order;
        }

        public void DeleteOrder(int orderId)
        {
            OrderDAO.Instance.DeleteOrder(orderId);
            OrderDAO.Instance.SaveChanges();
        }

        public Order? GetOrderByStatus(int memberId, OrderStatus orderStatus)
        {
            return OrderDAO.Instance.GerOrderByStatus(memberId, orderStatus);
        }

        public GetFullOrderInfoDto GetOrderDetailsById(int id)
        {
            var order = OrderDAO.Instance.GetOrderDetailsById(id);

            if (order is null)
            {
                throw new Exception("Order not found");
            }

            return order.Adapt<GetFullOrderInfoDto>();
        }

        public List<GetOrderDto> GetOrders(DateTime? startDate = null, DateTime? endDate = null, int? memberId = null)
        {
            var orders = OrderDAO.Instance.GetOrders(startDate, endDate, memberId);
            return orders.Adapt<List<GetOrderDto>>();
        }

        public int SaveChanges()
        {
            return OrderDAO.Instance.SaveChanges();
        }

        public void UpdateOrder(int orderId, UpdateOrderDto updateOrderDto)
        {
            var order = OrderDAO.Instance.GetOrderById(orderId);

            if (order is null)
            {
                return;
            }


            TypeAdapterConfig<Order, UpdateOrderDto>.NewConfig().IgnoreNullValues(true);
            updateOrderDto.Adapt(order);
            OrderDAO.Instance.SaveChanges();
        }

        public void UpdateOrderFreight(int orderId)
        {
            OrderDAO.Instance.UpdateOrderFreight(orderId);
        }

        public void UpdateOrderStatus(int orderId, OrderStatus orderStatus)
        {
            OrderDAO.Instance.UpdateOrderStatus(orderId, orderStatus);
            OrderDAO.Instance.SaveChanges();
        }
    }
}
