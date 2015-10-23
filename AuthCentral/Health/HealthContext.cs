using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.Health
{
    internal class HealthContext
    {
        public const string Bad = "red";
        public const string Warning = "yellow";
        public const string Good = "green";

        private static string _currentStatus = Good;
        private static object _currentStatusLock = new object();

        public static string CurrentStatus {
            get
            {
                return _currentStatus;
            }
            set
            {
                if(value != _currentStatus)
                {
                    lock (_currentStatusLock)
                    {
                        _currentStatus = value;
                    }
                }
           }
        }


    }
}
