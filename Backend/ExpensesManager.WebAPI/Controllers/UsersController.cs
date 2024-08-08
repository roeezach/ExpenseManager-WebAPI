using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using ExpensesManager.DB.Models;
using ExpensesManager.Services.Contracts;
using ExpensesManager.Integrations.SplitWiseModels;
using System.Threading.Tasks;
using ExpensesManager.Services;
using Microsoft.EntityFrameworkCore.Query;
using ExpensesManager.Services.Services;

namespace ExpensesManager.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService m_UsersService;
        private readonly ICategoryService m_CategoryService;

        public UsersController(IUsersService usersService, ICategoryService categoryService)
        {
            m_UsersService = usersService;
            m_CategoryService = categoryService;
        }

        [HttpPost("SignUp")]
        public IActionResult SignUp(DB.Models.Users user)
        {
            try
            {
                AuthenticatedUsers result = m_UsersService.SignUp(user);
                if (result == null)
                {
                    return StatusCode(500, "User service returned null.");
                }
                Categories defaultCategories = m_CategoryService.CreateBasicCategoriesForRegisteredUsers(result.UserID);
                Console.WriteLine("Default category for the new user have been created");
                return Created("authenticated user: ", result);
            }
            catch (UsernameExistException e)
            {
                return StatusCode(409, e.Message);
            }
        }

        [HttpPost("SignIn")]
        public IActionResult SignIn(DB.Models.Users user)
        {
            try
            {
                var result = m_UsersService.SignIn(user);
                return Ok(result);
            }
            catch (InvalidUsernamePasswordException e)
            {
                return StatusCode(401, e.Message);
            }
        }

        [HttpGet("GetUser")]
        public IActionResult GetUser(string username)
        {
            var result = m_UsersService.GetUserByUsername(username);
            return Ok(result);                       
        }

        [HttpDelete]
        public IActionResult DeleteUser(string username)
        {
            var user = m_UsersService.GetUserByUsername(username);
            if (user != null)
            {
                m_UsersService.DeleteUserByUserId(user.UserID);
                return Ok();
            }
            return NotFound();
        }
    }
}