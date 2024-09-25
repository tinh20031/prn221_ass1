using BusinessObject;
using DataAccess.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IProductRepository
    {
        List<GetProductDto> GetProducts(string keyword = "", decimal? startPrice = default, decimal? endPrice = default);

        GetProductDetailsDto? GetProductDetailsById(int productId);

        void AddProduct(CreateProductDto createProductDto);

        void DeleteProduct(int productId);

        void UpdateProduct(Product product);

    }
}
