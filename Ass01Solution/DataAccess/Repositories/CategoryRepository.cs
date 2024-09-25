using DataAccess.DAO;
using DataAccess.DTOs.Category;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public List<GetCategoryDto> GetCategories()
        {
            return CategoryDAO.Instance.GetCategories().Adapt<List<GetCategoryDto>>();
        }
    }
}
