using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Ubigrade.Application.Controllers.API_Controller
{
    public abstract class BaseAPIController : ControllerBase
    {
        public string conString = ConfigurationManager.ConnectionStrings["UbiServer"].ConnectionString;
        AppSettingsReader reader = new AppSettingsReader();
       
        public BaseAPIController()
        {
        
        }
    }
}
