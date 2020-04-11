using System;
using System.Collections.Generic;
using System.Text;

namespace Ubigrade.Library
{
    class SqlDataAccess
    {
        public static string GetConnectionString()
        {
            return "Host=172.16.231.38;Username=postgres;Password=Ubigrade1;Database=ubigrade";
        }
    }
}
