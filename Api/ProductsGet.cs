using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Api;

//public class MongoDbFunc
//{

//    [Function("MongoDbFunc")]
//    public MultiResponse Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
//        FunctionContext executionContext)
//    {
//        var logger = executionContext.GetLogger("HttpExample");
//        logger.LogInformation("C# HTTP trigger function processed a request.");

//        var message = "Welcome to Azure Functions!";

//        var response = req.CreateResponse(HttpStatusCode.OK);
//        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
//        response.WriteString(message);

//        // Return a response to both HTTP trigger and Azure Cosmos DB output binding.
//        return new MultiResponse()
//        {
//            Document = new MyDocument
//            {
//                id = System.Guid.NewGuid().ToString(),
//                message = message
//            },
//            HttpResponse = response
//        };
//    }
//}

public class ProductsGet
{
    private readonly IProductData productData;

    public ProductsGet(IProductData productData)
    {
        this.productData = productData;
    }

    [FunctionName("ProductsGet")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products")] HttpRequest req)
    {
        var products = await productData.GetProducts();
        return new OkObjectResult(products);
    }
}