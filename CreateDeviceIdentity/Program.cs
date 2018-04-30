using System;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace CreateDeviceIdentity
{
    class Program
    {
        private static RegistryManager registryManager;
        private static string connnectionString = "HostName=iothubspike.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=Um0WZ8l+W4SBv//fjMUEkD16MMQ70ens38geihI5hjs=";

        static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connnectionString);
            AddDevice().Wait();
            Console.ReadLine();
        }

        private static async Task AddDevice()
        {
            string deviceId = "devicespike";
            Device device;

            try
            {
                device =await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
