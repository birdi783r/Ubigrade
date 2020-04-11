using System;
using System.Collections.Generic;
using System.Text;

namespace Ubigrade.Library
{
    class SqlDataAccess
    {
        public static string GetConnectionString()
        {
            //return "Host=172.16.231.38;Username=postgres;Password=Ubigrade1;Database=ubigrade";
            return "Server=localhost; Port=5432; Database=ubineu; User Id=postgres; Password=1903;";
        }
    }
}
