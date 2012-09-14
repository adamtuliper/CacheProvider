using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using CacheProvider.Models;

/*
 * Developed by Adam Tuliper   @AdamTuliper
 * completedevelopment.blogspot.com
 * adam.tuliper@gmail.com
 * Please retain credits if you use this code, use though without restrictions.
 */
namespace CacheProvider
{
    class Program
    {
        static void Main(string[] args)
        {
            //IoC in Console apps we'll treat this as the composition root
            var builder = new ContainerBuilder();
            builder.RegisterType<MemoryCacheProvider>().As<ICacheProvider>();

            using (var container = builder.Build())
            {
                var cache = container.Resolve<ICacheProvider>();

                var customer = new Customer
                    {
                        CustomerId = 656,
                        FirstName = "Mary",
                        LastName = "Doe",
                        Address = "555 Main St.",
                        City = "Austin",
                        State = "TX",
                        Zip = "73301",
                        EmailAddress = "mary@internal"
                    };

                string customerId = customer.CustomerId.ToString();

                //Add to the cache
                Console.WriteLine("Adding customer to the cache"); 
                cache.Add<Customer>(customerId, customer, 30);

                //Ensure its still in the cache
                var cachedCustomer = cache.Get<Customer>(customerId);
                cache.Exists(customerId);
                Console.WriteLine(string.Format("{0} has been retrieved from the cache", cachedCustomer.FirstName));

                //Remove from the cache
                cache.Remove(customerId);

                Console.WriteLine(string.Format("{0} exists still in cache (should be false): {1}", customerId, cache.Exists(customerId)));

                //Now add again with a one second time, lets ensure it expires.
                Console.WriteLine("Adding to the cache and sleeping for a few seconds to expire...");
                cache.Add(customerId, customer, 2);
                Thread.Sleep(1000);
                Console.WriteLine("..");
                Thread.Sleep(1000);
                Console.WriteLine("..");
                Thread.Sleep(1000);

                Console.WriteLine(string.Format("{0} exists still in cache (should be false as it expired): {1}", customerId, cache.Exists(customerId)));
            }
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();

        }
    }
}
