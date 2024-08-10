using ExpensesManager.DB;
using ExpensesManager.DB.Models;
using ExpensesManager.Services.Contracts;
using Newtonsoft.Json;

namespace ExpensesManager.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private AppDbContext appDbContext;
        public CategoryService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public Categories CreateCategory(Categories category)
        {
            appDbContext.Categories.Add(category);
            appDbContext.SaveChanges();

            return category;
        }

        public Categories CreateBasicCategoriesForRegisteredUsers(int userID)
        {
            string mappedCategoriesJson = Constants.DEFAULT_CATEGORIES;

            Categories defaultCategory = new()
            {
                UserID = userID,
                MappedCategoriesJson = mappedCategoriesJson
            };

            Console.WriteLine($"the default categories are {defaultCategory}");
            appDbContext.Categories.Add(defaultCategory);
            appDbContext.SaveChanges();
            return defaultCategory;
        }

        public void DeleteCategory(Categories deletedCategory)
        {
            appDbContext.Remove(deletedCategory);
            appDbContext.SaveChanges();
        }

        public Categories EditCategory(Categories category)
        {
            Categories? editedCategory = appDbContext.Categories.FirstOrDefault(c => c.UserID == category.UserID);
            editedCategory.MappedCategoriesJson = category.MappedCategoriesJson;
            appDbContext.SaveChanges();

            return editedCategory;
        }

        public List<Categories> GetCategories()
        {
            return appDbContext.Categories.ToList();
        }
        public Categories GetUserCategories(int userID)
        {
            return appDbContext.Categories.FirstOrDefault(c => c.UserID == userID);
        }

    }
}
