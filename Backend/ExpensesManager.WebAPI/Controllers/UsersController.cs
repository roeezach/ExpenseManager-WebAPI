using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using ExpensesManager.DB.Models;
using ExpensesManger.Services.Contracts;
using ExpensesManager.Integrations.SplitWiseModels;
using System.Threading.Tasks;
using ExpensesManger.Services;

namespace ExpensesManager.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UsersController :  ControllerBase
    {
        private readonly IUsersService m_UsersService;

        public UsersController(IUsersService usersService)
        {
            m_UsersService = usersService;
        }

        [HttpPost("SignUp")]
        public IActionResult SignUp(DB.Models.Users user)
        {
            try
            {
                var result = m_UsersService.SignUp(user);
                return Created("authenticated user: ", result);
            }
            catch(UsernameExistException e)
            {
                return StatusCode(409, e.Message);                
            }
        }

        [HttpPost("SignIn")]
        public  IActionResult SignIn(DB.Models.Users user)
        {
            try
            {
                var result = m_UsersService.SignIn(user);
                return Ok(result);
            }
            catch(InvalidUsernamePasswordException e)
            {
                return StatusCode(401, e.Message);
            }
        }

    }
}