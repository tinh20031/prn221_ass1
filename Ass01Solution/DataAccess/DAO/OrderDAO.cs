using BusinessObject;
using BusinessObject.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class OrderDAO
    {
        private static OrderDAO? _instance = null;

        private readonly SalesManagementDbContext _context;

        public OrderDAO()
        {
            if (_context is null)
            {
                _context = new SalesManagementDbContext();
            }
        }

        public decimal CalculateOrderTotal(int orderId)
        {
            decimal total = _context.OrderDetails.Where(od => od.OrderId == orderId)
                                            .Sum(od => od.UnitPrice * od.Quantity);

            return total;                   
        }

        public Order? GetOrderDetailsById(int id)
        {
            return _context.Orders.AsNoTracking()
                                .Include(o => o.Member)
                                .Include(o => o.OrderDetails)
                                .ThenInclude(od => od.Product)
                                .SingleOrDefault(o => o.OrderId == id);
        }

        public void DeleteOrder(int orderId)
        {
            _context.OrderDetails.Where(od => od.OrderId == orderId)
                                .ExecuteDelete();
            _context.Orders.Where(o => o.OrderId == orderId)
                        .ExecuteDelete();
        }

        public void UpdateOrderFreight(int orderId)
        {
            decimal newFreight = _context.OrderDetails.Where(od => od.OrderId == orderId)
                                                  .Sum(od => od.UnitPrice * od.Quantity);

            _context.Orders.Where(o => o.OrderId == orderId)
                            .ExecuteUpdate(setter => setter.SetProperty(o => o.Freight, newFreight));
                            
        }

        public Order? GetOrderById(int id, bool hasTrackings = true)
        {
            return hasTrackings ? _context.Orders.SingleOrDefault(o => o.OrderId == id)
                                : _context.Orders.AsNoTracking().SingleOrDefault(o => o.OrderId == id);
        }

        public List<Order> GetOrders(DateTime? startDate = default, DateTime? endDate = default, int? memberId = null)
        {
            var query = _context.Orders.AsNoTracking()
                                       .AsQueryable();

            if (memberId.HasValue)
            {
                query = query.Where(o => o.MemberId == memberId)
                             .Include(o => o.Member);
            }

            if (startDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= startDate);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= endDate);
            }


            return query.ToList();
        }

        public static OrderDAO Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new OrderDAO();
                }

                return _instance;
            }
        }

        public Order? GerOrderByStatus(int memberId, OrderStatus orderStatus)
        {
            return _context.Orders.FirstOrDefault(o => o.MemberId == memberId && o.Status == orderStatus.ToString());
        }

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void UpdateOrderStatus(int orderId, OrderStatus orderStatus)
        {
            _context.Orders.Where(o => o.OrderId == orderId)
                            .ExecuteUpdate(setter => setter.SetProperty(o => o.Status, orderStatus.ToString()));
        }



    }
}
