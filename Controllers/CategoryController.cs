using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Commander.Models;
using Commander.Data;
using AutoMapper;
using Commander.Dtos;
using Microsoft.AspNetCore.JsonPatch;

namespace Commander.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {


        //! เป็นการช้งาน Dependency injection (DI) โดยการไม่เรียกใช้งาน new 
        //! เหมือนบรรทัดที่ 20
        //* พิมพ์ ctor เพื่อ เพิ่ม function ชื่อเดียวกับ controller
        //* public CategoryController(Parameters)
        //* {

        //* }

        private readonly ICategoryService Impcat;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService ICat, IMapper mapper)
        {
            Impcat = ICat;
            _mapper = mapper;
        }

        //*private readonly ImpCategoryService Impcat = new ImpCategoryService();

        //Get api/Category
        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetAllCategory()
        {
            var _ImpCat = Impcat.GetAllCategoty();
            return Ok(_mapper.Map<IEnumerable<CategoryReadDto>>(_ImpCat));
        }

        //Get api/Category/{id}
        [HttpGet("{id}", Name = "GetCategoryById")]
        public ActionResult<CategoryReadDto> GetCategoryById(int id)
        {
            var _ImpCat = Impcat.GetCategoryById(id);
            if (_ImpCat != null)
            {
                return Ok(_mapper.Map<CategoryReadDto>(_ImpCat));
            }
            else
            {
                return NotFound();
            }

        }
        //Post api/Category
        [HttpPost]
        public ActionResult<CategoryReadDto> CreateCategory(CategoryCreateDto categoryCreateDto)
        {
            var categoryModel = _mapper.Map<Category>(categoryCreateDto);
            Impcat.CreateCategory(categoryModel);
            Impcat.SaveChanges();

            //* _mapper.Map(Source,Destination) 
            //* แม็พจาก อ็อบเจ็กต์ต้นทาง ไปยัง อ็อบเจ็กต์ปลายทาง ใหม่ ด้วยอ็อพชันการแม็พที่ให้มา

            var categoryReadDto = _mapper.Map<CategoryReadDto>(categoryModel);

            return CreatedAtRoute(nameof(GetCategoryById), new { Id = categoryReadDto.CategoryId }, categoryReadDto);


        }

        //Put api/Category/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateCategory(int id, CategoryUpdateDto categoryUpdateDto)
        {
            var categoryModelFromImpCat = Impcat.GetCategoryById(id);
            if (categoryModelFromImpCat == null)
            {
                return NotFound();
            }

            //* _mapper.Map(Source,Destination) 
            //* แม็พจาก อ็อบเจ็กต์ต้นทาง ไปยัง อ็อบเจ็กต์ปลายทาง ที่มีอยู่ ด้วยอ็อพชันการแม็พที่ให้มา

            _mapper.Map(categoryUpdateDto, categoryModelFromImpCat);
            Impcat.UpdateCategory(categoryModelFromImpCat);
            Impcat.SaveChanges();

            return NoContent();

        }

        //Patch api/Category/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialUpdateCategory(int id, JsonPatchDocument<CategoryUpdateDto> patchDoc)
        {
            var categoryModelFromImpCat = Impcat.GetCategoryById(id);
            if (categoryModelFromImpCat == null)
            {
                return NotFound();
            }
            var CategoryToPacth = _mapper.Map<CategoryUpdateDto>(categoryModelFromImpCat);
            patchDoc.ApplyTo(CategoryToPacth, ModelState);
            if (!TryValidateModel(CategoryToPacth))
            {
                return NotFound();
            }

            //* _mapper.Map(Source,Destination) 
            //* แม็พจาก อ็อบเจ็กต์ต้นทาง ไปยัง อ็อบเจ็กต์ปลายทาง ที่มีอยู่ ด้วยอ็อพชันการแม็พที่ให้มา

            _mapper.Map(CategoryToPacth, categoryModelFromImpCat);
            Impcat.UpdateCategory(categoryModelFromImpCat);
            Impcat.SaveChanges();

            return NoContent();

        }

        //Delete api/Category/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCategory(int id)
        {
            var categoryModelFromImpCat = Impcat.GetCategoryById(id);
            if (categoryModelFromImpCat == null)
            {
                return NotFound();
            }
            Impcat.DeleteCategory(categoryModelFromImpCat);
            Impcat.SaveChanges();

            return NoContent();

        }

    }
}