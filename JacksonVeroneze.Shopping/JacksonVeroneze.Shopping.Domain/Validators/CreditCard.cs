using System;
using System.Text.RegularExpressions;

namespace JacksonVeroneze.Shopping.Domain.Results
{
    public class CreditCard
    {
        public static bool IsValid(string cardNo, string expiryDate, string cvv)
        {
            Regex cardCheck = new Regex(@"^(1298|1267|4512|4567|8901|8933)([\-\s]?[0-9]{4}){3}$");
            Regex monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            Regex yearCheck = new Regex(@"^20[0-9]{2}$");
            Regex cvvCheck = new Regex(@"^\d{3}$");

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

            return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6));
        }
    }
}