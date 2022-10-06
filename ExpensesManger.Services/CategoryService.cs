using ExpensesManager.DB;
using ExpensesManager.DB.Models;


namespace ExpensesManger.Services
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
