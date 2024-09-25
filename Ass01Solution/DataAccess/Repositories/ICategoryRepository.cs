using DataAccess.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface ICategoryRepository
    {
        List<GetCategoryDto> GetCategories();
    }
}
