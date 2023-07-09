using CustomerManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagement.Helper
{
    public static class CustomersHelper
    {
        private const string FilePath = "customers.json"; // need to replace with absolute path where there are initial customers list
        public static List<Customer> GetCustomersFromFile()
        {
            var customers = new List<Customer>();
            if (System.IO.File.Exists(FilePath))
            {
                var json = System.IO.File.ReadAllText(FilePath);
                customers = JsonConvert.DeserializeObject<List<Customer>>(json);
            }

            return customers;
        }

        public static void SaveCustomersToFile(List<Customer> customers)
        {
            var json = JsonConvert.SerializeObject(customers);
            System.IO.File.WriteAllText(FilePath, json);
        }

        public static bool ValidateCustomer(Customer customer, List<Customer> customers)
        {
            if (customer.Id <= 0 || customer.Age <= 0 || string.IsNullOrWhiteSpace(customer.FirstName) ||
                string.IsNullOrWhiteSpace(customer.LastName))
            {
                return false;
            }

            if (customer.Age <= 18)
            {
                return false;
            }

            if (customers.Any(c => c.Id == customer.Id))
            {
                return false;
            }

            return true;
        }

        public static void InsertCustomer(Customer customer, List<Customer> customers)
        {
            int index = 0;
            while (index < customers.Count &&
                   (string.Compare(customers[index].LastName, customer.LastName, StringComparison.OrdinalIgnoreCase) < 0 ||
                    (string.Compare(customers[index].LastName, customer.LastName, StringComparison.OrdinalIgnoreCase) == 0 &&
                     string.Compare(customers[index].FirstName, customer.FirstName, StringComparison.OrdinalIgnoreCase) < 0)))
            {
                index++;
            }

            customers.Insert(index, customer);
        }
    }
}
