using BusinessObject;
using DataAccess.DAO;
using DataAccess.DTOs.OrderDetail;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public void CreateOrderDetails(CreateOrderDetailDto createOrderDetailDto)
        {
            var orderDetail = createOrderDetailDto.Adapt<OrderDetail>();

            OrderDetailDAO.Instance.CreateOrderDetail(orderDetail);
            OrderDetailDAO.Instance.SaveChanges();
        }

        public List<GetOrderDetailWithProductDto> GetOrderDetailWithProducts(int orderId)
        {
            return OrderDetailDAO.Instance.GetOrderDetailsWithProduct(orderId).Adapt<List<GetOrderDetailWithProductDto>>();
        }
    }
}
