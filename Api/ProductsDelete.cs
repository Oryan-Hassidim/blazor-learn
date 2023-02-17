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

    public ProductsDelete(IProductData productData)
    {
        this.productData = productData;
    }

    [FunctionName("ProductsDelete")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "products/{productId:int}")] HttpRequest req,
        int productId,
        ILogger log)
    {
        if (!Auth.Parse(req).IsInRole("admin"))
            return new BadRequestObjectResult("not admin access");

        var result = await productData.DeleteProduct(productId);

        return result ? new OkResult() : new BadRequestResult();
    }
}
