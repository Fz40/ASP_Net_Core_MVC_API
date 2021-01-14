using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Commander.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using Commander.Encode;
using Newtonsoft.Json;
using System.Transactions;

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
            //*
            //* ตัวอย่างการใช้งาน begin transaction committ rollback
            //* ในกรณีนี้เราใช้ EF เป็นตัวจัดการ เราจึงประกาศใช้ TransactionScope
            //* และใช้ tranScope.Complete เพื่อจบงาน โดยที่ไม่ต้องใช้ Committ หรือ RollBack
            //* ซึ่งถ้าเราใช้ EF มันจะทำทั้ง 2 ฟังชั่น (เมตตอด) ให้อัตโนมัติ
            //* 
            using (TransactionScope tranScope = new TransactionScope())
                try
                {
                    var res = db.SaveChanges() >= 0;
                    tranScope.Complete();
                    return (res);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            //*
            //* ตัวอย่างการใช้งาน BeginTransaction Commit Rollback แบบปกติสำหรับใช้กับ LINQ
            //*
            // using (dbDataContext db = new dbDataContext())
            // using (var dbContextTransaction = db.Database.BeginTransaction()) 
            // {
            //     try
            //     { 
            //         var user = new User(){ID = 1, Name = "Nick"};
            //         db.Users.Add(user);
            //         db.SaveChanges();
            //         dbContextTransaction.Commit(); 
            //     } 
            //     catch (Exception) 
            //     { 
            //         dbContextTransaction.Rollback(); 
            //     }
            // } 

        }

        public void UpdateCategory(Category cat)
        {
            //* Nothing 
            //* ไม่ต้องทำอะไรเนื่องจากเรารับค่ามาอยู่ใน Model Category อยู่แล้ว 
            //* เราจึงสามารถนำเอาค่าที่ได้ ส่ง save ได้เลย
            //*
        }
        public async Task<IEnumerable<Category>> GetCategotyByCondition(ConditionModel condition)
        {
            IEnumerable<Category> data;
            try
            {
                var decrypt = EncryptDecryptService.DecryptAes(condition.encrypt);
                var _condition = JsonConvert.DeserializeObject<ConditionModel>(decrypt);
                data = db.Category.Where(c => c.CategoryId > 0);
                // .AsNoTracking()
                // .AsExpandable();
                if (_condition.id != 0)
                {
                    data = data.Where(c => c.CategoryId.Equals(_condition.id));
                }
                if (_condition.Name != null && _condition.Name.Any())
                {
                    data = data.Where(c => c.CategoryName.ToLower().Contains(_condition.Name.ToLower()));
                }
                if (_condition.Description != null && _condition.Description.Any())
                {
                    data = data.Where(c => c.Description.ToLower().Contains(_condition.Description.ToLower()));
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
