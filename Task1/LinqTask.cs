using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.VisualBasic;
using Task1.DoNotChange;

namespace Task1
{
    public static class LinqTask
    {
        public static IEnumerable<Customer> GetCustomersByTotalOrderSum(IEnumerable<Customer> customers, decimal limit)
        {
            if (customers == null)
                throw new ArgumentNullException();

            var result = from customer in customers
                         where customer.Orders.Sum(order => order.Total) > limit
                         select customer;

            return result;
        }

        public static IEnumerable<(Customer customer, DateTime orderDate)> GetTheFirstOrderDateFromAllCustomers(IEnumerable<Customer> customers)
        {
            if (customers == null)
                throw new ArgumentNullException();

            var result = from customer in customers
                         where customer.Orders.Any()
                         let firstOrderDate = customer.Orders.Min(order => order.OrderDate)
                         select (customer, firstOrderDate);
            return result;
        }

        public static IEnumerable<(Customer customer, DateTime orderDate)> GetTheFirstOrderDateFromAllCustomersOrdered(IEnumerable<Customer> customers)
        {
            if (customers == null)
                throw new ArgumentNullException();

            var result = from customer in customers
                         where customer.Orders.Any()
                         let firstOrderDate = customer.Orders.Min(order => order.OrderDate)
                         orderby firstOrderDate.Year,
                             firstOrderDate.Month,
                             customer.Orders.Sum(order => order.Total) descending,
                             customer.CompanyName
                         select
                             (
                             customer,
                             firstOrderDate
                             );
            return result;
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> GetAllCustomersWithSupplierByCity(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            if (customers == null)
                throw new ArgumentNullException();

            var result = from customer in customers
                         join supplier in suppliers on customer.City equals supplier.City into customerSuppliers
                         select (customer, customerSuppliers);

            return result;
        }

        public static IEnumerable<Customer> GetCustomersWithIssueInLocationParameters(
            IEnumerable<Customer> customers)
        {
            if (customers == null)
                throw new ArgumentNullException();

            var result = from customer in customers
                         where !IsAValidPostalCode(customer.PostalCode)
                               || customer.Region == string.Empty
                               || !ContainsParenthesis(customer.Phone)
                         select customer;

            return result;
        }

        private static bool IsAValidPostalCode(string postalCode)
        {
            var result = postalCode.All(char.IsDigit);
            return result;
        }

        private static bool ContainsParenthesis(string phoneNumber)
        {
            return phoneNumber.Contains("(") && phoneNumber.Contains(")");
        }

        public static IEnumerable<object> GetProductsGroupedByCategory(
            IEnumerable<Product> products)
        {
            if (products == null)
                throw new ArgumentNullException();

            var result = from product in products
                         group product by new { product.Category, availble = product.UnitsInStock > 0 } into grouped
                         orderby grouped.Min(p => p.UnitPrice)
                         select new
                         {
                             Category = grouped.Key.Category,
                             InStock = grouped.Key.availble
                         };

            return result;
        }

        public static IEnumerable<object> GetAverageProfitabilityByCity(IEnumerable<Customer> customers)
        {
            if (customers == null)
                throw new ArgumentNullException();

            var result = from customer in customers
                         group customer by customer.City into groupedByCity
                         select new
                         {
                             City = groupedByCity.Key,
                             AverageProfitability = groupedByCity.Average(c => c.Orders.Sum(o => o.Total)),
                             AverageRate = groupedByCity.Average(c => c.Orders.Count())
                         };

            return result;
        }

        public static string GetAllCitiesOrderedByName(IEnumerable<Customer> customers)
        {
            if (customers == null)
                throw new ArgumentNullException();

            var result = from customer in customers
                         group customer by new { customer.City, customer.Country } into groupedByCity
                         orderby groupedByCity.Key.City.Length, groupedByCity.Key.Country
                         select groupedByCity.Key.City;

            return result.Aggregate(string.Empty, (current, city) => current + $"-{city}");
        }
    }
}