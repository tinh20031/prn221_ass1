using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class CategoryDAO
    {
        private readonly SalesManagementDbContext _context;

        private static CategoryDAO? _instance = null;

        public CategoryDAO()
        {
            if (_context is null)
            {
                _context = new SalesManagementDbContext();
            }
        }

        public static CategoryDAO Instance { 
            get
            {
                if (_instance is null)
                {
                    _instance = new CategoryDAO();
                }

                return _instance;
            }
        }

        public List<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }
    }
}
