using System.Collections.Generic;
using System.Threading.Tasks;
using Commander.Models;

namespace Commander.Data
{
    public interface ICategoryService
    {
        bool SaveChanges();
        Task<IEnumerable<Category>> GetAllCategoty();
        Task<Category> GetCategoryById(int id);
        void CreateCategory(Category cat);
        void UpdateCategory(Category cat);

        void DeleteCategory(Category cat);
        Task<IEnumerable<Category>> GetCategotyByCondition(ConditionModel condition);

    }
}
