using System;
using System.Collections.Generic;
using System.Threading;
using CacheProvider.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CacheProvider.Tests
{
    [TestClass]
    public class CacheProviderTests
    {

        /// <summary>
        /// Since a MemoryProvider even if instantiated again in each test returns the same 
        /// instance, we can simply use a single instance here and not create a new one for each test.
        /// However we'll need to ensure we add different items to the cache to ensure we don't
        /// spoil other test cases, hence using Random(DateTime.Now.Millisecond).Next() for each CustomerId
        /// which is then the key in the cache each time.
        /// </summary>
        private ICacheProvider _cache = new MemoryCacheProvider();

        [TestMethod]
        public void Test_Add_Item_To_Cache_Get_Item_From_Cache_Pass()
        {
            //Arrange
            Customer customer = new Customer
            {
                CustomerId = new Random(DateTime.Now.Millisecond).Next(),
                FirstName = "Mary",
                LastName = "Doe",
                Address = "555 Main St.",
                City = "Austin",
                State = "TX",
                Zip = "73301",
                EmailAddress = "mary@internal"
            };

            string customerId = customer.CustomerId.ToString();

            //Act
            _cache.Add(customerId, customer, 1);

            var cachedCustomer = _cache.Get<Customer>(customerId);
            
            
            //Assert
            Assert.AreSame(customer, cachedCustomer);

        }

        [TestMethod]
        public void Test_Get_Nonexisting_Item_From_Cache_Pass()
        {
            var customer = _cache.Get<Customer>("fake key");
            Assert.IsNull(customer);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Test_Get_Item_From_Cache_Wrong_Cast_Fail()
        {
            _cache.Add("123",new Customer(),10);
            var list = _cache.Get<List<string>>("123");

        }

        [TestMethod]
        public void Test_Add_And_Get_Item_From_Cache_Expired_Pass()
        {
            //Arrange
            Customer customer = new Customer
            {
                CustomerId = new Random(DateTime.Now.Millisecond).Next(),
                FirstName = "Mary",
                LastName = "Doe",
                Address = "555 Main St.",
                City = "Austin",
                State = "TX",
                Zip = "73301",
                EmailAddress = "mary@internal"
            };

            string customerId = customer.CustomerId.ToString();

            //Act (add and remove from cache, then attempt to retrieve)
            _cache.Add(customerId, customer, 1);
            Thread.Sleep(1000);
            var cachedCustomer = _cache.Get<Customer>(customerId);

            //Assert - should be null as it's expired
            Assert.IsNull(cachedCustomer);
        }

        [TestMethod]
        public void Test_Add_And_Item_Exists_In_Cache_Pass()
        {
            //Arrange
            Customer customer = new Customer
            {
                CustomerId = new Random(DateTime.Now.Millisecond).Next(),
                FirstName = "Mary",
                LastName = "Doe",
                Address = "555 Main St.",
                City = "Austin",
                State = "TX",
                Zip = "73301",
                EmailAddress = "mary@internal"
            };

            string customerId = customer.CustomerId.ToString();

            //Act (add and remove from cache, then attempt to retrieve)
            _cache.Add(customerId, customer, 5);

            //Assert
            Assert.IsTrue(_cache.Exists(customerId));
        }

        [TestMethod]
        public void Test_Add_And_Remove_Item_From_Cache_Pass()
        {
            Customer customer = new Customer
            {
                CustomerId = new Random(DateTime.Now.Millisecond).Next(),
                FirstName = "Mary",
                LastName = "Doe",
                Address = "555 Main St.",
                City = "Austin",
                State = "TX",
                Zip = "73301",
                EmailAddress = "mary@internal"
            };

            string customerId = customer.CustomerId.ToString();

            //Act (add and remove from cache, then attempt to retrieve)
            _cache.Add(customerId, customer, 100);
            _cache.Remove(customerId);
            var cachedCustomer = _cache.Get<Customer>(customerId);

            //Assert - should be null
            Assert.IsNull(cachedCustomer);
        }
    }
}