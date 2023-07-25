using System;
using ExpensesManager.Services;
using ExpensesManger.Services.BuisnessLogic.Map.Common;
using ExpensesManger.Services.BuisnessLogic.Map.ExpenseMappers;
using ExpensesManger.Services.Contracts;

namespace ExpensesManger.Services.BuisnessLogic.Map
{
    public class ExpenseMapperFactory : IExpenseMapperFactory
    {
        private readonly ICategoryService _categoryService;

        public ExpenseMapperFactory(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ExpenseMapper GetBankMapper(BankTypes.FileTypes fileType)
        {
            switch (fileType)
            {
                case BankTypes.FileTypes.Habinleuimi:
                    return new HabinleuimiExpenseMapper();
                case BankTypes.FileTypes.Hapoalim:
                    return new HapoalimExpenseMapper();
                case BankTypes.FileTypes.Max:
                    return new MaxExpenseMapper();
                default:
                    throw new NotSupportedException("Invalid bank type.");
            }
        }

        public static T GetMapper<T>(IServiceProvider serviceProvider) where T : class
        {
            var categoryService = serviceProvider.GetService(typeof(ICategoryService)) as ICategoryService;

            if (typeof(T) == typeof(CategoryExpenseMapper))
            {
                return new CategoryExpenseMapper(categoryService) as T;
            }

            throw new NotSupportedException($"Mapper type {typeof(T).Name} is not supported.");
        }
    }
}
