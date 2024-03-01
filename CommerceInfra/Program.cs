// See https://aka.ms/new-console-template for more information
using MongoDB.Driver;

Console.WriteLine("Hello, World!");

try
{
    var testConnect = new MongoClient("mongodb://127.0.0.1:27017/shopDEV");
    Console.WriteLine($"The total connect : {testConnect.Settings.MaxConnecting - 1}");
    Console.WriteLine("Connection is OK");
} catch (Exception ex)
{
    throw new AggregateException(ex);
} finally
{
    GC.Collect();
}
