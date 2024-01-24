using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using Task1.DoNotChange;

namespace Task1.Tests
{
    [TestFixture]
    public class Tests
    {
        [TestCase(6250, ExpectedResult = 0)]
        [TestCase(0, ExpectedResult = 6)]
        [TestCase(-1, ExpectedResult = 10)]
        [TestCase(1, ExpectedResult = 5)]
        public int GetCustomersByTotalOrderSum_Should_ReturnsCustomersCount_Acording_With_Limit(decimal limit)
        {
            return LinqTask.GetCustomersByTotalOrderSum(DataSource.Customers, limit).Count();
        }

        [Test]
        public void GetCustomersByTotalOrderSum_NullSource_Should_ThrowsArgumentNullException()
        {
            Assert.That(() => LinqTask.GetCustomersByTotalOrderSum(null, 42).ToList(), Throws.ArgumentNullException);
        }

        [Test]
        public void GetTheFirstOrderDateFromAllCustomers_NullSource_Should_ThrowsArgumentNullException()
        {
            Assert.That(() => LinqTask.GetTheFirstOrderDateFromAllCustomers(null).ToList(), Throws.ArgumentNullException);
        }

        [Test]
        public void GetTheFirstOrderDateFromAllCustomers_Should_Return_AllCustomers_With_The_First_Order_Date()
        {
            var result = LinqTask.GetTheFirstOrderDateFromAllCustomers(DataSource.Customers).ToList();
            Assert.That(() => result.Count, Is.EqualTo(6));
            foreach (var (customer, orderDate) in result)
            {
                Assert.That(orderDate, Is.Not.Null);
            }
        }

        [Test]
        public void GetTheFirstOrderDateFromAllCustomersOrdered_Should_Return_AllCustomers_With_The_First_Order_Date()
        {
            var result = LinqTask.GetTheFirstOrderDateFromAllCustomersOrdered(DataSource.Customers).ToList();
            Assert.That(() => result.Count, Is.EqualTo(6));
            Assert.That(result[0].orderDate.ToString("dd/MM/yyyy"), Is.EqualTo(FindCustomerOrdersMinDate(result[0].customer).ToString("dd/MM/yyyy")));
        }

        [Test]
        public void GetAllCustomersWithSupplierByCity_Should_Return_CustomersAndSuppliers()
        {
            var result = LinqTask.GetAllCustomersWithSupplierByCity(DataSource.Customers, DataSource.Suppliers).ToList();

            Assert.That(() => result.Count, Is.EqualTo(DataSource.Customers.Count));
            foreach (var (customer, suppliers) in result)
            {
                foreach (var supplier in suppliers)
                {
                    StringAssert.AreEqualIgnoringCase(customer.City, supplier.City);
                    StringAssert.AreEqualIgnoringCase(customer.Country, supplier.Country);
                }
            }
        }

        [Test]
        public void GetAllCustomersWithSupplierByCity_NullCustomer_Should_ThrowsArgumentNullException()
        {
            Assert.That(() => LinqTask.GetAllCustomersWithSupplierByCity(null, null).ToList(), Throws.ArgumentNullException);
        }

        [Test]
        public void GetCustomersWithIssueInLocationParameters_Should_Return_AllCustomers_With_Incorrect_Data_In_Location_Parameters()
        {
            var result = LinqTask.GetCustomersWithIssueInLocationParameters(DataSource.Customers).ToList();
            Assert.That(() => result.Count, Is.EqualTo(5));
            Assert.That(result[0].Phone, Is.EqualTo("555-7788"));
        }

        [Test]
        public void GetProductsGroupedByCategory_Should_Return_AllCustomers_With_Incorrect_Data_In_Location_Parameters()
        {
            var result = LinqTask.GetProductsGroupedByCategory(DataSource.Products).ToList();
            Assert.That(() => result.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetAverageProfitabilityByCity_Should_Return_All_Cities_With_product_Average()
        {
            var result = LinqTask.GetAverageProfitabilityByCity(DataSource.Customers).ToList();
            Assert.That(() => result.Count, Is.EqualTo(6));
        }

        [Test]
        public void GetAllCitiesOrderdByName_Should_Return_All_Cities_In_a_Single_Line_Ordered()
        {
            var result = LinqTask.GetAllCitiesOrderedByName(DataSource.Customers);
            Assert.That(result, Is.EqualTo("-USA-Berlin-London-Warszawa-Sao Paulo-Mexico D.F."));
        }

        private static DateTime FindCustomerOrdersMinDate(Customer customer)
        {
            var min = DateTime.MaxValue;
            foreach (var order in customer.Orders)
            {
                if (order.OrderDate < min)
                {
                    min = order.OrderDate;
                }
            }

            return min;
        }
    }
}
