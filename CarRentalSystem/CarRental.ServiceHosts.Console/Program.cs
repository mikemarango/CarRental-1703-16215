using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using static System.Console;
using CarRental.Business.Managers.Managers;

namespace CarRental.ServiceHosts.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("Starting services...");
            WriteLine(string.Empty);

            ServiceHost hostInventoryManager = new ServiceHost(typeof(InventoryManager));
            ServiceHost hostRentalManager = new ServiceHost(typeof(RentalManager));
            ServiceHost hostAccountManager = new ServiceHost(typeof(AccountManager));

            StartService(hostInventoryManager, "InventoryManager");
            StartService(hostRentalManager, "RentalManger");
            StartService(hostAccountManager, "AccountManger");


            WriteLine(string.Empty);
            WriteLine("Press [Enter] to exit.");
            ReadKey();

            StopService(hostInventoryManager, "InventoryManager");
            StopService(hostRentalManager, "RentalManager");
            StopService(hostAccountManager, "AccountManager");
        }

        static void StartService(ServiceHost host, string serviceDescription)
        {
            host.Open();
            WriteLine($"Service {serviceDescription} started");

            foreach (var endPoint in host.Description.Endpoints)
            {
                WriteLine($"Listening on endpoint:");
                WriteLine($"Address: {endPoint.Address.Uri.ToString()}");
                WriteLine($"Binding: {endPoint.Binding.Name}");
                WriteLine($"Contract: {endPoint.Contract.ConfigurationName}");
            }

            WriteLine();
        }

        static void StopService(ServiceHost host, string serviceDescription)
        {
            host.Close();
            WriteLine("Service {0} stopped.", serviceDescription);
        }
    }
}
