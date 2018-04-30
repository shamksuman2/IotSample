using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace SimulatedDevice
{
    class Program
    {
        private static DeviceClient deviceClient;
        private static string iotHubUri = "iothubspike.azure-devices.net";
        private static string deviceKey = "ya5SviUeLQ4k4XJjekMTM0yka4lT4hXQTJ7msvCZ0kI=";
        private static string deviceId = "devicespike";

        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n");
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey), TransportType.Mqtt);
            deviceClient.ProductInfo = "HappyPath-simulated-device";
            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }

        private static async void SendDeviceToCloudMessagesAsync()
        {
            double minTemperature = 20;
            double minHumidity = 60;
            int messageId = 1;
            Random rand = new Random();

            while (true)
            {
                double currentTemparature = minHumidity + rand.NextDouble() * 15;
                double currentHumidity = minHumidity + rand.NextDouble() * 20;

                var telematryDataPoint = new
                {
                    messageId = messageId++,
                    deviceId = deviceId,
                    temparature = currentTemparature,
                    humidity = currentHumidity
                };

                var messageString = JsonConvert.SerializeObject(telematryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                message.Properties.Add("temparatureAlert", (currentTemparature > 30) ? "true": "false");
                await deviceClient.SendEventAsync(message);

                Console.WriteLine($"{DateTime.Now} > Sending message {messageString}");
                await Task.Delay(1000);
            }
        }
    }
}
