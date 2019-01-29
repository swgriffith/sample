using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace griffith.function
{
    public static class hello
    {
        [FunctionName("hello")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function recieved a message.");

            string msg = req.Query["msg"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            msg = msg ?? data?.msg;

            //build message
            msg = "" + msg;
            
            string top = "";
            string bottom= "";
            for (int i = 0; i < msg.Length+2; i++)
            {
                top +="_";
                bottom+="-";
            }
            string output = $" {top}\n< {msg} >\n {bottom}\n \\\n  \\\n    __\n   /  \\\n   |  |\n   @  @\n   |  |\n   || |/\n   || ||\n   |\\_/|\n   \\___/ ";


            return output != null
                ? (ActionResult)new OkObjectResult(output)
                : new BadRequestObjectResult("Please pass a message on the query string or in the request body");
        }
    }
}
