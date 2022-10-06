using Newtonsoft.Json;
using static ExpensesManager.Integrations.SplitWiseModels.SplitwiseExpense;

namespace ExpensesManager.Integrations.SplitWiseModels
{
    [JsonObjectAttribute]

    public class ChangeExpense
    {

        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public Picture picture { get; set; }
        public bool custom_picture { get; set; }
    }
}
