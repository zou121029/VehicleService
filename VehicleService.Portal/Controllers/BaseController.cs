using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleService.Portal.Filters;

namespace VehicleService.Portal.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public abstract class BaseController : Controller
    {
    }
}
