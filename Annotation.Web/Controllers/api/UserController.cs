using Annotation.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Annotation.Web.Controllers.api
{
    public class UserController : ApiController {
        public ProfileModel Get() {
            
            return new ProfileModel() {
                //Created
            };
        }
    }
}
