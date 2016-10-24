using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ValueProviders.Providers;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace CreateDeviceIdentity
{
    class Program
    {
        static RegistryManager registryManager;
        static string connectionString = "HostName=testIoTkkociuga.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=R2909V1GqWYT4f+pTzJrnEH4mOK653/pIYO6F5fTHjA=";

        static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            AddDeviceAsync().Wait();
            Console.ReadLine();
        }
        private static async Task AddDeviceAsync()
        {
            string[] devices = {"myFirstDevice", "thermometer0001"};

            foreach (var deviceId in devices)
            {
                Device device;
                try
                {
                    device = await registryManager.AddDeviceAsync(new Device(deviceId));
                }
                catch (DeviceAlreadyExistsException)
                {
                    device = await registryManager.GetDeviceAsync(deviceId);
                }

                if (device.Authentication.SymmetricKey.PrimaryKey.Contains("+"))
                {
                    var removeDeviceAsync = registryManager.RemoveDeviceAsync(deviceId);
                    continue;
                }

                Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
            }
        }
    }
}
