using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Api;

public class GetRoles
{

    private readonly Auth auth;

    public GetRoles(Auth auth)
    {
        this.auth = auth;
    }

    [FunctionName("GetRoles")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        var claims = auth.Parse(req);
        if (claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier && c.Value.StartsWith("oryan")) != null)
        {
            log.LogInformation("oryan user roles requested");
            return new OkObjectResult(new[] { "admin", "manager", "oryan", "all" });
        }
        log.LogInformation("other user roles requested");
        return new OkObjectResult(new[] { "regular" });
    }
}
