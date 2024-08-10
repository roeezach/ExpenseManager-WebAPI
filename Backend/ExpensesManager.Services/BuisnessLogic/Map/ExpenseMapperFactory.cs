using System;
using ExpensesManager.Services;
using ExpensesManager.Services.BuisnessLogic.Map;
using ExpensesManager.Services.BuisnessLogic.Map.Common;
using ExpensesManager.Services.BuisnessLogic.Map.ExpenseMappers;
using ExpensesManager.Services.Contracts;

namespace ExpensesManager.Services.BuisnessLogic.Map
{
    public class ExpenseMapperFactory
    {
        private readonly ICategoryService _categoryService;
        private readonly Dictionary<BankTypes.FileTypes, Func<ExpenseMapper>> m_factories;

        public ExpenseMapperFactory(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            m_factories = new Dictionary<BankTypes.FileTypes, Func<ExpenseMapper>>
            {
                { BankTypes.FileTypes.Hapoalim, () => new HapoalimExpenseMapper() },
                { BankTypes.FileTypes.Habinleuimi, () => new HabinleuimiExpenseMapper() },
                { BankTypes.FileTypes.Max, () => new MaxExpenseMapper() }
            };
        }

        public ExpenseMapper CreateExpenseMapper(BankTypes.FileTypes fileType)
        {
            if (m_factories.TryGetValue(fileType, out var factory))
            {
                return factory();
            }

            throw new NotSupportedException("Invalid bank type.");
        }

        public T GetMapper<T>(IServiceProvider serviceProvider) where T : class
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
