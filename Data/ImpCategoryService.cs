using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Commander.Models;
using System.Threading.Tasks;

namespace Commander.Data
{
    public class ImpCategoryService : ICategoryService
    {
        private readonly NorthwindContext db;
        public ImpCategoryService(NorthwindContext _db)
        {
            db = _db;
        }

        public void CreateCategory(Category cat)
        {
            if (cat == null)
            {
                throw new ArgumentNullException(nameof(cat));
            }
            cat.CategoryId = db.Category.Max(c => c.CategoryId) + 1;
            db.Category.Add(cat);
        }

        public void DeleteCategory(Category cat)
        {
            if (cat == null)
            {
                throw new ArgumentNullException(nameof(cat));
            }
            db.Category.Remove(cat);
        }

        public async Task<IEnumerable<Category>> GetAllCategoty()
        {
            IEnumerable<Category> data;
            try
            {
                data = db.Category.ToList();
                return await Task.FromResult(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Category> GetCategoryById(int id)
        {
            Category cat = new Category();
            try
            {
                cat = db.Category.FirstOrDefault(c => c.CategoryId == id);
                return await Task.FromResult(cat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveChanges()
        {
            return (db.SaveChanges() >= 0);
        }

        public void UpdateCategory(Category cat)
        {
            //* Nothing 
            //* ไม่ต้องทำอะไรเนื่องจากเรารับค่ามาอยู่ใน Model Category อยู่แล้ว 
            //* เราจึงสามารถนำเอาค่าที่ได้ ส่ง save ได้เลย
            //*
        }

    }
}
