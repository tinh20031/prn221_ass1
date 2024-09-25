using BusinessObject;
using DataAccess.DAO;
using DataAccess.DTOs.Product;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public void AddProduct(CreateProductDto createProductDto)
        {
            ProductDAO.Instance.AddProduct(createProductDto.Adapt<Product>());
            ProductDAO.Instance.SaveChanges();
        }

        public void DeleteProduct(int productId)
        {
            ProductDAO.Instance.DeleteProduct(productId);
            ProductDAO.Instance.SaveChanges();
        }

        public GetProductDetailsDto? GetProductDetailsById(int productId)
        {
            return ProductDAO.Instance.GetProductDetailsById(productId).Adapt<GetProductDetailsDto>();
        }

        public List<GetProductDto> GetProducts(string keyword = "", decimal? startPrice = default, decimal? endPrice = default)
        {
            return ProductDAO.Instance.GetProducts(keyword, startPrice, endPrice).Adapt<List<GetProductDto>>();
        }

        public void UpdateProduct(Product product)
        {
            ProductDAO.Instance.UpdateProduct(product);
            ProductDAO.Instance.SaveChanges();
        }
    }
}
