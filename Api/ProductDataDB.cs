using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ShoppingList.Shared;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Api;

class ProductDataDB : IProductData
{
    private readonly MongoClient mongoClient;
    private readonly IMongoDatabase database;
    private readonly IMongoCollection<Product> products;

    public ProductDataDB(MongoClient mongoClient, IConfiguration configuration)
    {
        this.mongoClient = mongoClient ?? throw new ArgumentNullException();
        database = mongoClient.GetDatabase("main-db");
        products = database.GetCollection<Product>("Products");
    }

    public async Task<Product> AddProduct(Product product)
    {
        await products.InsertOneAsync(product);
        return product;
    }

    public async Task<bool> DeleteProduct(int id)
    {
        var res = await products.DeleteOneAsync(p => p.Id == id);
        return res.DeletedCount >= 1;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        var res = await products.FindAsync(_=>true);
        return res.ToList();
    }

    public async Task<Product> UpdateProduct(Product product)
    {
        return await products.FindOneAndReplaceAsync(p => p.Id == product.Id, product);
    }
}