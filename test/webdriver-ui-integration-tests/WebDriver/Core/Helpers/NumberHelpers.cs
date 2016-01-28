using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.Webdriver.Core.Helpers
{
    public class NumberHelpers
    {
        public static decimal SanitizePriceFromStrings(string uncleanString)
        {
            var allowedValues = "1234567890.";
            string cleanString = new string(uncleanString.Where(c => allowedValues.Contains(c)).ToArray());
            return decimal.Parse(cleanString);
        }

        public static int GetNumberFromParens(string given)
        {
            int i;
            given = given.Remove(
                0
                , given.IndexOf('(') + 1);
            int.TryParse(given.Substring(0, given.IndexOf(')')), out i);
            return i;
        }
    }
}
