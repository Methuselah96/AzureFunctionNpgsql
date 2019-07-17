using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FunctionCaller
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();

        static async Task Main()
        {
            var tasks = Enumerable.Range(0, 1000).Select(async i =>
            {
                await client.GetAsync("https://functionappnpgsql.azurewebsites.net/api/Function1");
                Console.WriteLine(i);
            });

            await Task.WhenAll(tasks);
        }
    }
}
