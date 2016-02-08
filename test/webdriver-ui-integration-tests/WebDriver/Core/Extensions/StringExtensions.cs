using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Core.Extensions
{
    public static class StringExtensions
    {
        public static string GetValueFromParens(this string given)
        {
            given = given.Remove(
                0
                , given.IndexOf('(') + 1);
            given = given.Substring(0, given.IndexOf(')'));
            return given;
        }
    }
}
