using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Api;

public class ProductsDelete
{
    private readonly IProductData productData;
    private readonly Auth auth;

    public ProductsDelete(IProductData productData, Auth auth)
    {
        this.productData = productData;
        this.auth = auth;
    }

    [FunctionName("ProductsDelete")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "products/{productId:int}")] HttpRequest req,
        int productId,
        ILogger log)
    {
        if (!auth.Parse(req).IsInRole("admin"))
        {
            log.LogInformation("delete blocked");
            return new BadRequestObjectResult("not admin access");
        }

        var result = await productData.DeleteProduct(productId);
        log.LogInformation($"delete {productId}: {result}");

        return result ? new OkResult() : new BadRequestResult();
    }
}
