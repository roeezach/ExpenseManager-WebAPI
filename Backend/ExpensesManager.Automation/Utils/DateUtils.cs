using System.Globalization;

namespace ExpensesManager.Testing;
public class DateUtils
{
    public static DateTime GetDate(string dateString)
    {
        if (DateTime.TryParseExact(dateString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            return date;
        }
        else
        {
            throw new ArgumentException("Invalid date string", nameof(dateString));
        }
    }
}
