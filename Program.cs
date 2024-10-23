using Integration.Common;
using Integration.Service;

namespace Integration;

public abstract class Program
{
    public static void Main(string[] args)
    {
        var service = new ItemIntegrationService();

        //ThreadPool.QueueUserWorkItem(_ => service.SaveItem("a"));
        //ThreadPool.QueueUserWorkItem(_ => service.SaveItem("b"));
        //ThreadPool.QueueUserWorkItem(_ => service.SaveItem("c"));

        //Thread.Sleep(500);

        //ThreadPool.QueueUserWorkItem(_ => service.SaveItem("a"));
        //ThreadPool.QueueUserWorkItem(_ => service.SaveItem("b"));
        //ThreadPool.QueueUserWorkItem(_ => service.SaveItem("c"));

        List<string> sendList = new()
        {
            "aa",
            "bb",
            "aa",
            "cc",
            "cc",
            "bb",
            "dd",
            "bb",
        };

        Parallel.ForEach(sendList, new ParallelOptions() { MaxDegreeOfParallelism = 8 }, row =>
        {
            service.SaveItem(row);
        });

        //Thread.Sleep(5000);

        Console.WriteLine("Everything recorded:");

        service.GetAllItems().ForEach(Console.WriteLine);

        Console.ReadLine();
    }
}