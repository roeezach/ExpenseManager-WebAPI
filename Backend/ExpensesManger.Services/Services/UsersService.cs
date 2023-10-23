using System;
using ExpensesManger.Services.Contracts;
using System.Threading.Tasks;
using ExpensesManager.Services;
using ExpensesManager.DB;
using ExpensesManager.DB.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using ExpensesManager.BuisnessLogic.Core;

namespace ExpensesManger.Services.Services
{
    public class UsersService : IUsersService
    {
        private readonly AppDbContext m_AppDbContext;
        private readonly IPasswordHasher m_PasswordHasher;
        private readonly IConfiguration m_configuration;

        public UsersService(AppDbContext context, IPasswordHasher passwordHasher, IConfiguration configuration)
        {
            m_AppDbContext = context;
            m_PasswordHasher = passwordHasher;
            m_configuration = configuration;
        }

        public AuthenticatedUsers SignIn(Users users)
        {
            var dbUser = m_AppDbContext.Users.FirstOrDefault(u=> u.Username == users.Username);
            
            if(dbUser == null || m_PasswordHasher.VerifyHashedPassword(dbUser.Password,users.Password) == PasswordVerificationResult.Failed)
            {
                throw new InvalidUsernamePasswordException("invalid username or password");
            }

            return new AuthenticatedUsers
            {
                Username = users.Username,
                Token = JwtGenerator.GenerateUserToken(users.Username, m_configuration),
                UserID = dbUser.UserID
            };
        }

        public AuthenticatedUsers SignUp(Users user)
        {
            Users? checkUser = m_AppDbContext.Users.FirstOrDefault(u => u.Username.Equals(user.Username));
            if(checkUser != null)
            {
                throw new UsernameExistException("Username already exist");
            }
            
            user.Password = m_PasswordHasher.HashPassword(user.Password);
            user.UserID = Utils.GenerateRandomID();
            m_AppDbContext.Add(user);
            m_AppDbContext.SaveChanges();
            
            return new AuthenticatedUsers
            {   
                Username = user.Username,
                Token = JwtGenerator.GenerateUserToken(user.Username, m_configuration),
                UserID = user.UserID
            };
        }
    }
}