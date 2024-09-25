using DataAccess.DTOs.OrderDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IOrderDetailRepository
    {
        List<GetOrderDetailWithProductDto> GetOrderDetailWithProducts(int orderId);

        void CreateOrderDetails(CreateOrderDetailDto createOrderDetailDto);
    }
}
