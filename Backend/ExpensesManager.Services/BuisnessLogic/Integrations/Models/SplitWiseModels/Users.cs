using Newtonsoft.Json;

namespace ExpensesManager.Integrations.SplitWiseModels
{
    [JsonObjectAttribute]
    public class Users
    {
        public User User { get; set; }
        public int User_ID { get; set; }
        public string Paid_Share { get; set; }
        public string Owed_Share { get; set; }
        public string Net_Balance { get; set; }
    }
}
