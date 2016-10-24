using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateDeviceIdentity;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace SimulatedDevice
{
    class Program
    {
        static DeviceClient deviceClient;
        static string iotHubUri = "testIoTkkociuga.azure-devices.net";
        //static string deviceKey = "mgVVceoBpTdTYhW5Fu8SdGy3n4dlgLp5EXsQgFji72Q=";
        static string deviceKey = "IdilP4wgMKoZ15jbVsCLbBadCNpTr5beHmuayGrnsnU="; // thermometer0001
        static string connectionString = "HostName=testIoTkkociuga.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=R2909V1GqWYT4f+pTzJrnEH4mOK653/pIYO6F5fTHjA=";

        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n");
            //deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("myFirstDevice", deviceKey));
            //deviceClient = DeviceClient.CreateFromConnectionString(connectionString, "myFirstDevice");
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("thermometer0001", deviceKey));

            SendMessages();
            Console.ReadLine();
        }

        private static void SendMessages()
        {
            SendFirstDeviceToCloudMessagesAsync();
        }

        private static async void SendFirstDeviceToCloudMessagesAsync()
        {
            double avgTemp = 10;
            Random rand = new Random();

            int counter = 0;

            while (counter++ < 30)
            {
                double currentTemp = avgTemp + rand.NextDouble() * 4 - 2;

                var telemetryDataPoint = new
                {
                    deviceId = "thermometer0001",
                    temperature = currentTemp,
                    messageDateTime = DateTime.UtcNow
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.UtcNow, messageString);

                Task.Delay(3000).Wait();
            }
        }

        //private static async void SendFirstDeviceToCloudMessagesAsync()
        //{
        //    double avgWindSpeed = 10; // m/s
        //    Random rand = new Random();
        //    string s = "dsaf";

        //    while (true)
        //    {
        //        double currentWindSpeed = 9.0;

        //        var telemetryDataPoint = new
        //        {
        //            deviceId = "myFirstDevice",
        //            windSpeed = currentWindSpeed
        //        };
        //        var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
        //        var message = new Message(Encoding.ASCII.GetBytes(messageString));

        //        await deviceClient.SendEventAsync(message);
        //        Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

        //        Task.Delay(10000).Wait();
        //    }
        //}
    }
}
