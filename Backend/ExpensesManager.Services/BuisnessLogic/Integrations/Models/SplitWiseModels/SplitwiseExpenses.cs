using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExpensesManager.Integrations.SplitWiseModels
{
    [JsonObjectAttribute]
    public class SplitwiseExpenses
    {
        public List<SplitwiseExpense> Expenses { get; set; }
        
        public SplitwiseExpenses ParseResponseToExpensesObj(string jsonAsString)
        {
            SplitwiseExpenses splitwiseExpenses = (SplitwiseExpenses)JsonConvert.DeserializeObject(jsonAsString, typeof(SplitwiseExpenses));

            return splitwiseExpenses;
        }
    }
}
