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
            //Log that the function was triggered
            log.LogInformation("C# HTTP trigger function recieved a message.");

            //Pull message from they query string
            string msg = req.Query["msg"];

            //Check the body for the message and use from body if not found in the query string
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            msg = msg ?? data?.msg;

            //build message
            msg = "Msg: " + msg;
            

            //Build the response text
            string top = "";
            string bottom= "";
            for (int i = 0; i < msg.Length+2; i++)
            {
                top +="_";
                bottom+="-";
            }
            string output = $" {top}\n< {msg} >\n {bottom}\n \\\n  \\\n    __\n   /  \\\n   |  |\n   @  @\n   |  |\n   || |/\n   || ||\n   |\\_/|\n   \\___/ ";

            //Return OK or bad request object
            return output != null
                ? (ActionResult)new OkObjectResult(output)
                : new BadRequestObjectResult("Please pass a message on the query string or in the request body");
        }
    }
}
