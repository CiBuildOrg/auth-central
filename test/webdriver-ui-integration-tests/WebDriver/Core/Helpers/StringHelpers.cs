using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.Webdriver.Core.Helpers
{
    public class StringHelpers
    {
        public static string GetValueFromParens(string given)
        {
            given = given.Remove(
                0
                , given.IndexOf('(') + 1);
            given = given.Substring(0, given.IndexOf(')'));
            return given;
        }
    }
}
