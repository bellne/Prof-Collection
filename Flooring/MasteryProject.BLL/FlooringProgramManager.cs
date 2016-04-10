using System.Configuration;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram;
using FlooringProgram.Data.ClientRepos;
using FlooringProgram.Data.OrderRepos;
using FlooringProgram.Data.ProductRepos;
using FlooringProgram.Data.TaxRepos;
using FlooringProgram.Models.Interfaces;
using FlooringProgram.Models;


namespace FlooringProgram.BLL
{
    public class FlooringProgramManager
    {
        private IClient _clientRepo;
        private IOrderRepository _orderRepo;
        private IProducts _productRepo;
        private ITaxRate _taxRepo;

        public FlooringProgramManager(IClient clientRepo, IOrderRepository orderRepo, IProducts prodRepo, ITaxRate taxRepo)
        {
            _clientRepo = clientRepo;
            _orderRepo = orderRepo;
            _productRepo = prodRepo;
            _taxRepo = taxRepo;
        }

        public int AddEntryToClientList(Client client)
        {
            int newClientId = _clientRepo.AddClient(client);
            return newClientId;
        }
        
        public List<Order> SearchOrderByLastName(string input)
        {
            List<Order> orderList = _orderRepo.SearchOrderByLastName(input);
            return orderList;
        }

        public Order SearchOrderByID(int orderId)
        {
            Order orderProps = _orderRepo.SearchOrderByOrderID(orderId);
            return orderProps;
        }

        public void EditClientEntry(Client client)
        {
            _clientRepo.EditClient(client);
        }

        public List<Client> SearchClientListByName(string name)
        {
            List<Client> clientList = _clientRepo.GetClientLastName(name);
            return clientList;
        }

        public List<Client> SearchClientListByPhoneNumber(string phoneNumber)
        {
            List<Client> clientList = _clientRepo.GetClientPhoneNumber(phoneNumber);
            return clientList;
        }

        public void SearchClientListByCustomerId(int id)
        {
            _clientRepo.GetClientById(id);
        }
        
        public Client SelectClientForNewOrder(int id)
        {
           Client clientInfo = _clientRepo.GetClientById(id);
            return clientInfo;
        }

        public List<Product> SearchProductListByType(string type)
        {
            List<Product> productTypeList = _productRepo.GetProductType(type);
            return productTypeList;
        }

        public List<Product> GetProductBySku(string sku)
        {
            List<Product> productSkuList = _productRepo.GetProductBySku(sku);
            return productSkuList;            
        }

        public List<Product> GetProducts()
        {
            return _productRepo.GetProductList();
        }

        public List<Order> GetOrders()
        {
            return _orderRepo.GetOrdersList();
        }

        public int AddEntryToOrderList(Order order)
        {
            int newOrderNumber = _orderRepo.AddOrder(order);
            return newOrderNumber;
        }

        public void EditEntryToOrderList(Order order)
        {
            _orderRepo.EditOrder(order);
        }

        public void UpdateStatusToPreparingBLL(Order order)
        {
            _orderRepo.UpdateStatusToPreparingData(order);
        }

        public string GetStateTaxRateOnly(string stateAbbreviation)
        {
            string tax;            
            tax = _taxRepo.GetStateTaxRateOnly2(stateAbbreviation);
            return tax;
        }

        public Tax GetStateTax(string stateAbbreviation)
        {
            Tax tax = new Tax();
            tax = _taxRepo.GetTaxRate(stateAbbreviation);          
            return tax;
        }

        public decimal TotalMaterialCost(decimal cost, int area)
        {
            decimal TotalMaterialCost = cost * area;
            return TotalMaterialCost;
        }

        public decimal TotalLaborCost(decimal cost, int area)
        {
            decimal totalLaborCost = cost * area;
            return totalLaborCost;
        }

        public decimal TotalCost(decimal labor, decimal material, decimal taxRate)
        {
            decimal totalCostWithoutTax = labor + material;
            decimal Tax = totalCostWithoutTax * taxRate;
            return totalCostWithoutTax + Tax;
        }
    }
}
