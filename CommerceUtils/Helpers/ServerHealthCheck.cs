using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceUtils.Helpers
{
    public class ServerHealthCheck : IServerHealthCheck
    {
        private readonly int TimeSleep = 5000;
        public int GetConnectionNumber(MongoClient client)
        {
            return client.Settings.MaxConnecting - 1;
        }

        public void IsOverloadServer(MongoClient client)
        {
            char stop = Console.ReadKey().KeyChar;
            if (stop == '\n') return;
            //1. Get the number of current active connection
            int activeConnection = GetConnectionNumber(client);
            //2. Get the number of core of CPU
            int numOfCPUCore = GetCPUCore();
            //3. Get the memory usage in physical RAM for this current process
            long memoryUsageInRAM = GetMemoryUsed();
            Console.WriteLine($"Memory used: {memoryUsageInRAM/1024/1024} MB\n " +
                $"Current active connection : {activeConnection}");
            if (activeConnection > (numOfCPUCore * 4))
            {
                Console.WriteLine("Server is overloaded");
            }
            Thread.Sleep(TimeSleep);
            Console.WriteLine("Press Enter key to exit monitoring process, exit now?");
            IsOverloadServer(client);
        }

        private static long GetMemoryUsed()
        {
            long result;
            using (Process current = Process.GetCurrentProcess())
            {
                current.Refresh();
                result = current.WorkingSet64;
            }
            return result;
        }

        private static int GetCPUCore()
        {
            return Environment.ProcessorCount;
        }
    }
}
