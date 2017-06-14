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

            ServiceHost host = new ServiceHost(typeof(InventoryManager));
            host.Open();

            WriteLine(string.Empty);
            WriteLine("Press [Enter] to exit.");

            ReadKey();
        }
    }
}
