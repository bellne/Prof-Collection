using FlooringProgram.BLL;
using FlooringProgram.Models.Interfaces;
using FlooringProgram.Data.ClientRepos;
using FlooringProgram.Data.OrderRepos;
using FlooringProgram.Data.ProductRepos;
using FlooringProgram.Data.TaxRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasteryProject
{
    public class FPManagerFactory
    {
        public static FlooringProgramManager CreateFlooringProgramManager(string modeChoice)
        {

            if (modeChoice == "TEST")
            {
                IClient testClientRepo = new TestClientRepository();
                IOrderRepository testOrderRepo = new TestOrderRepository();
                IProducts testProdRepo = new TestProductRepository();
                ITaxRate testTaxRepo = new TestTaxRepository();

                return new FlooringProgramManager(testClientRepo, testOrderRepo, testProdRepo, testTaxRepo);
            }
            else
            {
                IClient clientRepo = new ClientRepository();
                IOrderRepository orderRepo = new OrderRepository();
                IProducts prodRepo = new ProductRepository();
                ITaxRate taxRepo = new TaxRepository();

                return new FlooringProgramManager(clientRepo, orderRepo, prodRepo, taxRepo);
            }
        }

    }
}

