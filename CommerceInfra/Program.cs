// See https://aka.ms/new-console-template for more information
using MongoDB.Driver;

Console.WriteLine("Hello, World!");

try
{
    var testConnect = new MongoClient("mongodb://127.0.0.1:27017/shopDEV");

    Console.WriteLine($"The total connect : {testConnect.Settings.MaxConnecting - 1}");
    Console.WriteLine("Connection is OK");

    var databaseSchema = testConnect.GetDatabase("shopDEV");
    var cursor = await databaseSchema.ListCollectionsAsync();
    var listTask = await cursor.ToListAsync();
    foreach ( var item in listTask)
    {
        Console.WriteLine(item);
    }
} catch (Exception ex)
{
    throw new AggregateException(ex);
} finally
{
    GC.Collect();
}
