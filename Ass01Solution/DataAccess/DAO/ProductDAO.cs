using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class ProductDAO
    {
        private readonly SalesManagementDbContext _context;

        private static ProductDAO? _instance = null;

        public ProductDAO()
        {
            if (_context is null)
            {
                _context = new SalesManagementDbContext();
            }
        }


        public static ProductDAO Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new ProductDAO();
                }

                return _instance;
            }
        }

        public List<Product> GetProducts(string keyword = "", decimal? startPrice = default, decimal? endPrice = default)
        {
            var query = _context.Products.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.ProductName.ToLower().Contains(keyword.ToLower())
                    || p.ProductId.ToString() == keyword);
            }

            if (startPrice.HasValue)
            {
                query = query.Where(p => p.UnitPrice >= startPrice);
            }

            if (endPrice.HasValue)
            {
                query = query.Where(p => p.UnitPrice <= endPrice);
            }

            return query.ToList();
        }

        public Product? GetProductDetailsById(int productId)
        {
            return _context.Products
                    .AsNoTracking()
                    .Include(p => p.Category)
                    .SingleOrDefault(p => p.ProductId == productId);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public void DeleteProduct(int productId)
        {
            if (_context.OrderDetails.Any(od => od.ProductId == productId))
            {
                throw new Exception("Can't delete this product since it is already been purchased");
            }
            _context.Products.Where(p => p.ProductId == productId)
                            .ExecuteDelete();
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
        }


        


    }
}
