using Microsoft.AspNetCore.JsonPatch.Operations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Options
{

        public class SwaggerOptions
        {
            public string JsonRoute { get; set; }
            public string Description { get; set; }
            public string UIEndPoint { get; set; }
        }
   
}
