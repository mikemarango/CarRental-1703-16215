using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using static System.Console;
using CarRental.Business.Managers.Managers;
using System.Timers;
using CarRental.Business.Entities;
using System.Transactions;
using System.Security.Principal;
using System.Threading;
using Core.Common.Core;
using CarRental.Business.Bootstrapper;

namespace CarRental.ServiceHosts.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            GenericPrincipal principal = new GenericPrincipal(
                new GenericIdentity("Miguel"), new string[] { "CarRentalAdmin" });

            Thread.CurrentPrincipal = principal;

            ObjectBase.Container = MEFLoader.Init();

            WriteLine("Starting services...");
            WriteLine(string.Empty);

            ServiceHost hostInventoryManager = new ServiceHost(typeof(InventoryManager));
            ServiceHost hostRentalManager = new ServiceHost(typeof(RentalManager));
            ServiceHost hostAccountManager = new ServiceHost(typeof(AccountManager));

            StartService(hostInventoryManager, "InventoryManager");
            StartService(hostRentalManager, "RentalManger");
            StartService(hostAccountManager, "AccountManger");

            System.Timers.Timer timer = new System.Timers.Timer(10000);
            timer.Elapsed += OnTimerElapsed;
            timer.Start();

            WriteLine("Reservation monitor started.");

            WriteLine(string.Empty);
            WriteLine("Press [Enter] to exit.");
            ReadKey();

            timer.Stop();

            WriteLine("Reservation monitor stopped");

            StopService(hostInventoryManager, "InventoryManager");
            StopService(hostRentalManager, "RentalManager");
            StopService(hostAccountManager, "AccountManager");
        }

        private static void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            WriteLine($"Looking for dormant reservations at {DateTime.UtcNow.ToString()}");

            RentalManager rentalManager = new RentalManager();

            Reservation[] reservations = rentalManager.GetDeadReservations();

            if (reservations != null)
            {
                foreach (Reservation reservation in reservations)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            rentalManager.CancelReservation(reservation.ReservationId);
                            WriteLine($"Canceling reservation {reservation.ReservationId}");
                            scope.Complete();

                        }
                        catch (Exception ex)
                        {
                            WriteLine($"There was an error when attempting to cancel reservation {reservation.ReservationId}.", ex);
                        }
                    }
                }
            }
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
