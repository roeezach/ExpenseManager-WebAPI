using ExpensesManger.Services;
using ExpensesManager.DB.Models;
using Microsoft.AspNetCore.Mvc;
using ExpensesManger.Services.BuisnessLogic.Core;

namespace ExpensesManager.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService m_CategoryService;

        public CategoryController(ICategoryService categoryService)
        {
            m_CategoryService = categoryService;
        }

        [HttpGet(Name = "GetCategories")]
        public IActionResult GetCategories()
        {
            return Ok(m_CategoryService.GetCategories());
        }

        [HttpGet("{userID:int}")]
        public IActionResult GetUserCategories(int userID)
        {
            return Ok(m_CategoryService.GetUserCategories(userID));
        }

        [HttpPost]
        public IActionResult CreateCategory(Categories catrgory)
        {
            Categories categoryItem = m_CategoryService.CreateCategory(catrgory);
            return CreatedAtRoute("GetCategories", new { requestID = Utils.GenerateRandomID()}, categoryItem);
        }

        [HttpDelete]
        public IActionResult DeleteCategory(Categories catrgory)
        {
            m_CategoryService.DeleteCategory(catrgory);
            return Ok();
        }

        [HttpPut]
        public IActionResult EditCategory(Categories category)
        {
            return Ok(m_CategoryService.EditCategory(category));
        }
    }
}
