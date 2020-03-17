using Microsoft.AspNetCore.JsonPatch.Operations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Options
{
    public class FileUploadOperation : IOperationFilter
    {

        public void Apply(Swashbuckle.AspNetCore.Swagger.Operation operation, OperationFilterContext context)
        {
            if (operation.OperationId.ToLower() =="apivalueloadpost")
            {
                operation.Parameters.Clear();
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "UploadedFile",
                    In ="formData",
                    Description = "Upload File",
                    Required = true,
                    Type ="file"
                });
                operation.Consumes.Add("multipart/form-data");
            }
        }
    }
    public class SwaggerOptions
    {
        public string JsonRoute { get; set; }
        public string Description { get; set; }
        public string UIEndPoint { get; set; }
    }
}
