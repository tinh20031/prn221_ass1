using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class OrderDetailDAO
    {
        private static OrderDetailDAO? _instance = null;

        private readonly SalesManagementDbContext _context;

        public OrderDetailDAO()
        {
            if (_context is null)
            {
                _context = new SalesManagementDbContext();
            }
        }

        public static OrderDetailDAO Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new OrderDetailDAO();
                }

                return _instance;
            }
        }

        public List<OrderDetail> GetOrderDetailsWithProduct(int orderId)
        {
            return _context.OrderDetails.Where(od => od.OrderId == orderId)
                                        .Include(od => od.Product)
                                        .AsNoTracking()
                                        .ToList();
        }

        public void CreateOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }




    }
}
