using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Commander.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LinqKit;

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
        public async Task<IEnumerable<Category>> GetCategotyByCondition(CategoryConditionModel condition)
        {
            IEnumerable<Category> data;
            try
            {   
                data = db.Category
                .AsNoTracking()
                .AsExpandable();
                if(condition.id != 0)
                {
                   data = data.Where(c => c.CategoryId.Equals(condition.id));
                }
                if(condition.Name != null && condition.Name.Any())
                {
                   data = data.Where(c => c.CategoryName.Contains(condition.Name));
                }
                if(condition.Description != null && condition.Description.Any())
                {
                   data = data.Where(c => c.Description.Contains(condition.Description));
                }

            //*
            //* ตัวอย่างการใช้งานการค้นหาแบบหลายเงื่อนไข 
            //* ใช้ function ของ LINQKit
            //*
            // if(mySearchFilter.IsActive)
            //     result = result.Where(p => p.ActiveOnly);

            // if(mySearchFilter.CategoryIds != null && mySearchFilter.CategoryIds.Any())
            //     result = result.Where(p => mySearchFilter.CategoryIds.Contains(p.CategoryId));

            // if(mySearchFilter.Keywords != null && mySearchFilter.Keywords.Any())
            // {
            //     //Crate the builder
            //     var predicate = PredicateBuilder.New();

            //     //Loop through the keywords
            //     foreach(var item in mySearchFilter.Keywords)
            //     {
            //         var currentKeyword = item;
            //         predicate = predicate.Or (p => p.ProductName.Contains (currrentKeyword));
            //         predicate = predicate.Or (p => p.Description.Contains (currrentKeyword));
            //     }
            //     result = result.Where(predicate);
            //*

                data = data.ToList();
                return await Task.FromResult(data);
            }
            catch (Exception ex)
            {
                throw ex;           
            }
        }
        

    }
}
