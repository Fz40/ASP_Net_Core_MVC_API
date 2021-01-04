using System.Collections.Generic;
using Commander.Models;

namespace Commander.Data
{
    public interface ICategoryService
    {
        bool SaveChanges();
        IEnumerable<Category> GetAllCategoty();

        Category GetCategoryById(int id);

        void CreateCategory(Category cat);
        void UpdateCategory(Category cat);
        void DeleteCategory(Category cat);

    }
}