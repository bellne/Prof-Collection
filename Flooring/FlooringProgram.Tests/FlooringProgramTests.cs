using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FlooringProgram.Models;
using FlooringProgram.Data.ClientRepos;
using FlooringProgram.Data.OrderRepos;
using FlooringProgram.Data.ProductRepos;
using FlooringProgram.Data.TaxRepos;
using FlooringProgram.BLL;
using MasteryProject;

namespace FlooringProgram.Tests
{
    [TestFixture]
    public class FlooringProgramTests
    {

        ClientRepository _clientRepo = new ClientRepository();
        OrderRepository _orderRepo = new OrderRepository();
        ProductRepository _productRepo = new ProductRepository();
        TaxRepository _taxRepo = new TaxRepository();
        //FlooringProgramManager _manager = new FlooringProgramManager();
        FlooringProgramManager _manager = FPManagerFactory.CreateFlooringProgramManager("PROD");

        // Test 1
        [TestCase("Bell,N", "NathanBell")]
        [TestCase("Copeland,B", "BryanCopeland")]
        [TestCase("balak,z", "zachbalak")]
        public void SearchClientListByName(string name, string expected)
        { 
            string actual = "";
            List<Client> result = _clientRepo.GetClientLastName(name);
            foreach(var client in result)
            {
                actual = client.FirstName + client.LastName;
            }

            Assert.AreEqual(expected, actual);
        }

        // Test 2
        [TestCase("Jones", "Jones")]
        [TestCase("Way", "Way")]
        [TestCase("Walton", "Walton")]
        public void SearchOrderByLastName(string lastName, string expected)
        {
            List<Order> resultSet = _orderRepo.SearchOrderByLastName(lastName);
            string actual = resultSet[0].CustomerLast;

            Assert.AreEqual(expected, actual);
        }

        // Test 3
        [TestCase("lamenate_tile", "nb-3X011")]
        [TestCase("bamboo_board", "kZ-2f792")]
        [TestCase("office_carpet", "LM-1r969")]
        public void SearchProductListByType(string test, string expected)
        {
            List<Product> productTypeList = _productRepo.GetProductType(test);
            string actual = productTypeList[0].Sku;

            Assert.AreEqual(expected, actual);
        }

        // Test 4
        [TestCase("CA", ".0825")]
        [TestCase("MN", ".06875")]
        [TestCase("WV", ".06")]
        public void GetStateTax(string stateAbbreviation, string expected)
        {
            Tax tax = _taxRepo.GetTaxRate(stateAbbreviation);
            string actual = tax.TaxRate;
            Assert.AreEqual(expected, actual);
        }

        // Test 5
        [TestCase(14.75, 400, 5900.00)]
        [TestCase(2.29, 2500, 5725.00)]
        [TestCase(8.64, 113, 976.32)]
        public void TotalMaterialCost(decimal cost, int area, decimal expected)
        {
            decimal actual;
            actual = _manager.TotalMaterialCost(cost, area);

        }

        // Test 6
        [TestCase(6.49, 204, 1323.96)]
        [TestCase(19.21, 4925, 94609.25)]
        [TestCase(.31, 4, 1.24)]
        public void TotalLaborCost(decimal cost, int area, decimal expected)
        {
            decimal actual;
            actual = _manager.TotalLaborCost(cost, area);

            Assert.AreEqual(expected, actual);
        }

        // Test 7
        [TestCase("AL", ".04")]    
        [TestCase("OR", ".00")]       
        [TestCase("MO", ".04225")]            
        public void GetStateTaxRateOnly(string stateAbbreviation, string expected)
        {           
            string actual = _manager.GetStateTaxRateOnly(stateAbbreviation);
            Assert.AreEqual(expected, actual);
        }
        
    }
    }
