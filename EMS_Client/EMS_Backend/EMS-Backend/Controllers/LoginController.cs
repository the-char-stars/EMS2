using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EMS_Backend.Data;

namespace EMS_Backend.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    { 
        // POST api/values
        [HttpPost]
        public ActionResult<string> Post([FromBody]string value)
        {
            return "hello there";
        }
    }
}
