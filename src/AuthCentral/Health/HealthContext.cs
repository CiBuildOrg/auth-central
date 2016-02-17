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

        private static string _idsDbStatus = Good;
        private static object _idsDbStatusLock = new object();

        private static string _idmDbStatus = Good;
        private static object _idmDbStatusLock = new object();

        public static string CurrentStatus {
            get
            {
                if (IdmDbStatus == Bad || IdsDbStatus == Bad)
                {
                    return Bad;
                }
                else if (IdmDbStatus == Warning || IdsDbStatus == Warning)
                {
                    return Warning;
                }
                else
                {
                    return Good;
                }
            }
        }

        public static string IdmDbStatus
        {
            get
            {
                return _idmDbStatus;
            }
            set
            {
                if (value != _idmDbStatus)
                {
                    lock (_idmDbStatusLock)
                    {
                        _idmDbStatus = value;
                    }
                }
            }
        }
        public static string IdsDbStatus
        {
            get
            {
                return _idsDbStatus;
            }
            set
            {
                if (value != _idsDbStatus)
                {
                    lock (_idsDbStatusLock)
                    {
                        _idsDbStatus = value;
                    }
                }
            }
        }

    }
}
