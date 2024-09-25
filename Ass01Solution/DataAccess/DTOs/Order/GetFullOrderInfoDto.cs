using DataAccess.DTOs.Member;
using DataAccess.DTOs.OrderDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Order
{
    public class GetFullOrderInfoDto
    {
        public int OrderId { get; set; }

        public int MemberId { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public decimal? Freight { get; set; }

        public string Status { get; set; } = null!;

        public GetMemberDto Member { get; set; } = null!;

        public ICollection<GetOrderDetailWithProductDto> OrderDetails { get; set; } = [];
    }
}
