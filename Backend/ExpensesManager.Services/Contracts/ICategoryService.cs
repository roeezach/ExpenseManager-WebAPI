﻿using ExpensesManager.DB.Models;
using ExpensesManager.Services;
using ExpensesManager.Services.BuisnessLogic.Map;

namespace ExpensesManager.Services.Contracts
{
    public interface ICategoryService
    {
        Categories CreateCategory(Categories category);
        public Categories CreateBasicCategoriesForRegisteredUsers(int userID);
        void DeleteCategory(Categories deletedCategory);
        Categories EditCategory(Categories category);
        List<Categories> GetCategories();
        Categories GetUserCategories(int userID);

    }
}
