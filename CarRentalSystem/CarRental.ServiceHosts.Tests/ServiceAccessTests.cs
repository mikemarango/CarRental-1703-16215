using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using CarRental.Business.Contracts.ServiceContracts;

namespace CarRental.ServiceHosts.Tests
{
    [TestClass]
    public class ServiceAccessTests
    {
        [TestMethod]
        public void test_inventory_manager_as_service()
        {
            ChannelFactory<IInventoryService> channelFactory =
                new ChannelFactory<IInventoryService>(string.Empty);

            IInventoryService proxy = channelFactory.CreateChannel();

            (proxy as ICommunicationObject).Open();

            channelFactory.Close();
        }

        [TestMethod]
        public void test_rental_manager_as_service()
        {
            ChannelFactory<IRentalService> channelFactory =
                new ChannelFactory<IRentalService>(string.Empty);

            IRentalService proxy = channelFactory.CreateChannel();

            (proxy as ICommunicationObject).Open();

            channelFactory.Close();
        }

        [TestMethod]
        public void test_account_manager_as_service()
        {
            ChannelFactory<IAccountService> channelFactory =
                new ChannelFactory<IAccountService>(string.Empty);

            IAccountService proxy = channelFactory.CreateChannel();

            (proxy as ICommunicationObject).Open();

            channelFactory.Close();
        }

    }
}
