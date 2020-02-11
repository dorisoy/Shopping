using System;
using System.Text.RegularExpressions;

namespace JacksonVeroneze.Shopping.Domain.Results
{
    public class CreditCard
    {
        public static bool IsValid(string cardNo, string expiryDate, string cvv)
        {
            Regex cardCheck = new Regex(@"4[0-9]{6,}|5[1-5][0-9]{5,}|222[1-9][0-9]{3,}|22[3-9][0-9]{4,}|2[3-6][0-9]{5,}|27[01][0-9]{4,}|2720[0-9]{3,}|3(?:0[0-5]|[68][0-9])[0-9]{4,}|6(?:011|5[0-9]{2})[0-9]{3,}|(?:2131|1800|35[0-9]{3})[0-9]{3,}|3[47][0-9]{5,}");
            Regex monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            Regex yearCheck = new Regex(@"^20[0-9]{2}$");
            Regex cvvCheck = new Regex(@"^\d{3}$");

            cardNo = cardNo.Replace(" ", "");

            if (!cardCheck.IsMatch(cardNo))
                return false;

            if (!cvvCheck.IsMatch(cvv))
                return false;

            string[] dateParts = expiryDate.Split('/');

            if (!monthCheck.IsMatch(dateParts[0]) || !yearCheck.IsMatch(dateParts[1]))
                return false;

            int year = int.Parse(dateParts[1]);
            int month = int.Parse(dateParts[0]);
            int lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month);
            var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

            return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(10));
        }
    }
}