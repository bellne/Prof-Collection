using System;
using System.Collections.Generic;
using System.Linq;
using FlooringProgram.BLL;
using FlooringProgram.Models;
using FlooringProgram;
using FlooringProgram.Data.ClientRepos;
using FlooringProgram.Data.OrderRepos;
using System.Text.RegularExpressions;
using System.IO;
using MasteryProject;
using System.Configuration;
using FlooringProgram.Data.ProductRepos;
using FlooringProgram.Data.TaxRepos;

namespace FlooringProgram
{
    public class FlooringProgramUI
    {
        string _salesMode = "## SALES MODE ##";
        string _floorStore = "The Floor Store";
        string _clientName = "";
        bool _counter;
        private List<Product> _productProps = new List<Product>();

        //string modeChoice = ConfigurationManager.AppSettings["mode"];
        //FlooringProgramManager _manager = FPManagerFactory.CreateFlooringProgramManager(modeChoice);

        public void ShowMenu(FlooringProgramManager _manager)
        {
            string choice;
            int iChoice = 0;
            bool isValidChoice = false;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition((Console.WindowWidth - _salesMode.Length) / 2, Console.CursorTop);
                Console.WriteLine(_floorStore + "\n");
                Console.ResetColor(); 
                Console.WriteLine("1. Sales");
                Console.WriteLine("2. Operations\n");
                Console.WriteLine("Enter Selection: ");

                choice = Console.ReadLine();
                bool isANumber = int.TryParse(choice, out iChoice);

                if (isANumber && iChoice < 3 && iChoice > 0)
                {
                    isValidChoice = true;
                }
                else
                {
                    Console.WriteLine("Invalid Entry. Press Enter to choose option 1 or 2.");
                    Console.ReadLine();
                }
            } while (!isValidChoice);

            switch (iChoice)
            {
                case 1:
                    ShowSalesMenu(_manager);
                    break;
                case 2:
                    ShowOperationsMenu();
                    break;
                default:
                    throw new Exception("Run for your life!");
            }
        }

        //Displays the main sales menu screen
        public void ShowSalesMenu(FlooringProgramManager _manager)
        {
            string choice;
            int iChoice = 0;
            bool isValidChoice = false;
            do
            {
                Header();
                DisplayClientName();

                Console.WriteLine("1. Create Order");
                Console.WriteLine("2. Search Order");
                Console.WriteLine("3. Product Search");
                Console.WriteLine("4. Main Menu");
                Console.WriteLine("5. Clear Current Client");
                Console.WriteLine("\nEnter Selection: ");

                choice = Console.ReadLine();
                bool isANumber = int.TryParse(choice, out iChoice);

                if (isANumber && iChoice < 6 && iChoice > 0)
                {
                    isValidChoice = true;
                }
                else
                {
                    Console.WriteLine("Invalid Entry. Enter a option 1, 2, 3, 4, or 5 to continue.");
                    Console.ReadLine();
                }
            } while (!isValidChoice);

            switch (iChoice)
            {
                case 1:
                    ShowCreateOrderMenu(_manager);
                    break;
                case 2:
                    SearchOrderMenu(_manager);
                    break;
                case 3:
                    List <Product> productProp = ProductSearch(_manager);
                    bool counter = true;
                    AddToOrderFromProductSearch(productProp, counter, _manager);

                    break;
                case 4:
                    ShowMenu(_manager);
                    break;
                case 5:
                    _clientName = "";
                    ShowSalesMenu(_manager);
                    break;
                default:
                    throw new Exception("Run for your life!");
            }
        }

        //shows the base menu that allows user to search for existing orders
        public void SearchOrderMenu(FlooringProgramManager _manager)
        {
            bool isValidChoice = false;
            string choice;
            int iChoice = 0;
            do
            {
                Header();
                DisplayClientName();

                Console.WriteLine("---Order Search---");
                Console.WriteLine("1. Search by Last Name");
                Console.WriteLine("2. Search by Order ID");
                Console.WriteLine("3. Main Menu");
                Console.WriteLine("\nEnter Selection: ");

                choice = Console.ReadLine();
                bool isANumber = int.TryParse(choice, out iChoice);

                if (isANumber && iChoice < 4 && iChoice > 0)
                {
                    isValidChoice = true;
                }
                else
                {
                    Console.WriteLine("Invalid Entry. Please enter an option 1, 2, or 3.");
                    Console.ReadLine();
                }
            } while (!isValidChoice);

            Console.Clear();
            switch (iChoice)
            {
                case 1:
                    SearchOrdersByName(_manager);
                    break;
                case 2:
                    SearchByOrderNumber(_manager);
                    break;
                case 3:
                    ShowSalesMenu(_manager);
                    break;
                default:
                    throw new Exception("Pizza is good!");
            }
        }

        public Order SelectOrderID(int selection, FlooringProgramManager _manager)
        {
            bool validChoice = false;
            do
            {                                   
                Order OrderIdSearchReturn = _manager.SearchOrderByID(selection); //grabs a specific order using it's id and returns it
                if (OrderIdSearchReturn != null)
                {
                    Order orderProps = OrderIdSearchReturn;
                    Header();
                    _clientName = orderProps.CustomerLast;
                    DisplayClientName();
                    Console.WriteLine("\n---Order Summary---");
                    Console.WriteLine("Order ID:              {0}", orderProps.OrderNumber);
                    Console.WriteLine("Customer Name:         {0} {1}", orderProps.CustomerFirst, orderProps.CustomerLast);
                    Console.WriteLine("Customer Phone Number: {0}", orderProps.CustomerPhoneNum);
                    Console.WriteLine("Company Name:          {0}", orderProps.CompanyName);
                    Console.WriteLine("Address:               {0}",orderProps.Address);
                    Console.WriteLine("City:                  {0}",orderProps.City);
                    Console.WriteLine("State:                 {0}",orderProps.State);
                    Console.WriteLine("State Tax:             {0}",_manager.GetStateTax(orderProps.State).TaxRate);
                    Console.WriteLine("Product Name:          {0}", orderProps.ProductName);
                    Console.WriteLine("Product SKU:           {0}", orderProps.ProductSku);
                    Console.WriteLine("Area (SQF):            {0}", orderProps.Area);
                    Console.WriteLine("Total Labor Cost:      {0}", orderProps.TotalLaborCost);
                    Console.WriteLine("Total Material Cost:   {0}", orderProps.TotalMaterialCost);
                    Console.WriteLine("Total Cost:            {0}", orderProps.TotalCost);
                    Console.WriteLine("Status:                {0}", orderProps.Status);
                    validChoice = true;
                    return orderProps;
                }
                else
                {
                    return null;
                }
            } while (!validChoice);
        }

        private void SearchOrdersByName(FlooringProgramManager _manager)
        {
            Header();
            DisplayClientName();
            string enteredName = GetUserInputName();
            List<Order> LastNameOrderList = _manager.SearchOrderByLastName(enteredName); //grabs all orders matching the last name that the user enters

            //let user back out of search
            if (enteredName.ToLower() == "m")
            {
                ShowSalesMenu(_manager);
            }
            else if (enteredName.ToLower() == "s")
            {
                SearchOrderMenu(_manager);
            }

            if (LastNameOrderList.Count() < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No results found. Press Enter to try again.");
                Console.ResetColor();
                Console.ReadLine();
                SearchOrdersByName(_manager);
            }
            else
            {
                Header();
                DisplayClientName();
                Console.WriteLine("Order #   First Name \t   Last Name \t\t Address \t\t Phone Number \t Product Name \t Product sku \t Total Cost");

                foreach (Order item in LastNameOrderList)
                {
                    Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}", item.OrderNumber.ToString().PadRight(10), item.CustomerFirst.PadRight(17), 
                        item.CustomerLast.PadRight(22), item.Address.PadRight(24), item.CustomerPhoneNum.PadRight(16), item.ProductName.PadRight(16), item.ProductSku.PadRight(16), 
                        item.TotalCost.ToString().PadRight(10));
                }
                
                Console.WriteLine("-------------------");
                SearchByOrderNumber(_manager);  
            }
        }

        public void SearchByOrderNumber(FlooringProgramManager _manager)
        {
            string iChoice;
            bool validChoice = false;
            int orderNumber;
            Order selectedOrder = new Order();

            Console.WriteLine("\nEnter Client's Order ID # or 'R' to return to search menu:\n");

            do
            {
                string orderNumberSelection = Console.ReadLine();

                if(orderNumberSelection.ToLower() == "r")
                {
                    SearchOrderMenu(_manager);
                }

                if (int.TryParse(orderNumberSelection, out orderNumber))
                {
                    selectedOrder = SelectOrderID(orderNumber, _manager); //grabs a specific order by the order id that the user enters
                    if(selectedOrder == null)
                    {
                        InvalidEntry();
                        continue;
                    }
                    //Console.WriteLine(selectedOrder.OrderNumber);  //don't think we need this? -kb
                    validChoice = true;
                }
                else
                {
                    InvalidEntry();
                }

            } while (!validChoice);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nMake Selection: \n'E' to edit Order");
            Console.ResetColor();
            Console.WriteLine("'R' to return to 'search order' menu");
            Console.WriteLine("'M' to return to 'main menu'");
            if (selectedOrder.Status == OrderStatus.Saved)
            {
                Console.WriteLine("'S' to submit Order\n");
            }

            do
            {
                iChoice = Console.ReadLine().ToLower();
                if (iChoice == "e" || iChoice == "r"|| iChoice == "m" || iChoice == "s")
                {
                    validChoice = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid entry. Try again.");
                    Console.ResetColor();
                }
            } while (validChoice);

            switch (iChoice)
            {
                case "e":
                    EditOrder(selectedOrder, _manager); 
                    break;
                case "r":
                    SearchOrderMenu(_manager);
                    break;
                case "m":
                    ShowSalesMenu(_manager);
                    break;
                case "s":
                    selectedOrder.Status = OrderStatus.Preparing;
                    UpdateStatusToPreparing(selectedOrder, _manager); //writes the new order with the 'preparing' status to the csv file
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Order #:{0}", selectedOrder.OrderNumber.ToString().PadRight(5));
                    Console.WriteLine("Order status: {0}", selectedOrder.Status);
                    Console.ResetColor();
                    Console.WriteLine("Press Any Key to return to Main Menu");
                    Console.ReadKey();
                    ShowSalesMenu(_manager);
                    break;
                default:
                    throw new Exception("Ludacris Speed!");
            }
        }

        public void UpdateStatusToPreparing(Order selectedOrder, FlooringProgramManager _manager)
        {
            _manager.UpdateStatusToPreparingBLL(selectedOrder);
        }

        public void DisplayOrderInfo(Order order, FlooringProgramManager _manager)
        {
            Console.WriteLine("    Order ID:              {0}", order.OrderNumber);
            Console.WriteLine("1.  Customer First Name:   {0}", order.CustomerFirst);
            Console.WriteLine("2.  Customer Last Name:    {0}", order.CustomerLast);
            Console.WriteLine("3.  Customer Phone Number: {0}", order.CustomerPhoneNum);
            Console.WriteLine("4.  Company Name:          {0}", order.CompanyName);
            Console.WriteLine("5.  Address:               {0}", order.Address);
            Console.WriteLine("6.  City:                  {0}", order.City);
            Console.WriteLine("7.  State:                 {0}", order.State);
            Console.WriteLine("      State Tax:           {0}", _manager.GetStateTax(order.State).TaxRate);
            Console.WriteLine("8. Order info: ");
            Console.WriteLine("      Product Name:        {0}", order.ProductName);
            Console.WriteLine("      Product SKU:         {0}", order.ProductSku);
            Console.WriteLine("      Area (SQF):          {0}", order.Area);
            Console.WriteLine("      Total Labor Cost:    {0}", order.TotalLaborCost);
            Console.WriteLine("      Total Material Cost: {0}", order.TotalMaterialCost);
            Console.WriteLine("      Total Cost:          {0}", order.TotalCost);
            Console.WriteLine("      Status:              {0}", order.Status);
        }

        public void EditOrder(Order order, FlooringProgramManager _manager)
        {
            Header();
            _clientName = order.CustomerLast;
            DisplayClientName();
            DisplayOrderInfo(order, _manager);
            EditOrderMainMenuOptions();
            string userEntry = Console.ReadLine();

            if(userEntry.ToLower() == "s")
            {
                SearchOrderMenu(_manager);
            }
            else if(userEntry.ToLower() == "m")
            {
                ShowMenu(_manager);
            }

            switch (userEntry)
            {
                case "1":
                    Console.Clear();
                    Header();
                    DisplayClientName();
                    Console.WriteLine("Customer First Name: {0}", order.CustomerFirst);
                    Console.WriteLine("Enter New First Name: ");
                    string customerFirst = Console.ReadLine();
                    order.CustomerFirst = customerFirst;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Order updated! Press enter to continue.");
                    Console.ResetColor();
                    Console.ReadLine();
                    Console.Clear();
                    EditOrder(order, _manager);
                    break;
                case "2":
                    Console.Clear();
                    Header();
                    DisplayClientName();
                    Console.WriteLine("Customer Last Name:{0}", order.CustomerLast);
                    Console.WriteLine("Enter New Last Name:");
                    string customerLast = Console.ReadLine();
                    order.CustomerLast = customerLast;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Order updated! Press enter to continue.");
                    Console.ResetColor();
                    Console.ReadLine();
                    Console.Clear();
                    EditOrder(order, _manager);
                    break;
                case "3":
                    Console.Clear();
                    Header();
                    Console.WriteLine("Customer Phone Number: {0}", order.CustomerPhoneNum);
                    Console.WriteLine("Enter New Phone Number: ");
                    string customerPhoneNum = Console.ReadLine();
                    order.CustomerPhoneNum = customerPhoneNum;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Order updated! Press enter to continue.");
                    Console.ResetColor();
                    Console.ReadLine();
                    Console.Clear();
                    EditOrder(order, _manager);
                    break;
                case "4":
                    Console.Clear();
                    Header();
                    DisplayClientName();
                    Console.WriteLine("Company Name: {0}", order.CompanyName);
                    Console.WriteLine("Enter New Company Name: ");
                    string companyName = Console.ReadLine();
                    order.CompanyName = companyName;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Order updated! Press enter to continue.");
                    Console.ResetColor();
                    Console.ReadLine();
                    Console.Clear();
                    EditOrder(order, _manager);
                    _manager.EditEntryToOrderList(order);
                    break;
                case "5":
                    Console.Clear();
                    Header();
                    DisplayClientName();
                    Console.WriteLine("Address: {0}", order.Address);
                    Console.WriteLine("Enter New Address: ");
                    string address = Console.ReadLine();
                    order.Address = address;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Order updated! Press enter to continue.");
                    Console.ResetColor();
                    Console.ReadLine();
                    Console.Clear();
                    EditOrder(order, _manager);
                    _manager.EditEntryToOrderList(order);
                    break;
                case "6":
                    Console.Clear();
                    Header();
                    DisplayClientName();
                    Console.WriteLine("City: {0}", order.City);
                    Console.WriteLine("Enter New City: ");
                    string city = Console.ReadLine();
                    order.City = city;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Order updated! Press enter to continue.");
                    Console.ResetColor();
                    Console.ReadLine();
                    Console.Clear();
                    EditOrder(order, _manager);
                    _manager.EditEntryToOrderList(order);
                    break;
                case "7":
                    Console.Clear();
                    Header();
                    DisplayClientName();
                    Console.WriteLine("State: {0}", order.State);
                    Console.WriteLine("Enter New State: ");
                    string state = Console.ReadLine().ToUpper();
                    order.State = state;
                    string taxRate2 = _manager.GetStateTaxRateOnly(state);
                    order.StateTax.TaxRate = taxRate2;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Order updated! Press enter to continue.");
                    Console.ResetColor();
                    Console.ReadLine();
                    Console.Clear();
                    _manager.EditEntryToOrderList(order);
                    EditOrder(order, _manager);
                    break;
                case "8":
                    Console.Clear();
                    Order updatedProduct = updateProductInfo(order, _manager); //takes user back through the product selection process
                    string stateAbbrev = order.State;
                    string taxRate = _manager.GetStateTaxRateOnly(stateAbbrev);

                    //all of the product info fields must be updated in order to keep the information accurate
                    order.Area = updatedProduct.Area;                
                    order.ProductName = updatedProduct.ProductName;
                    order.ProductSku = updatedProduct.ProductSku;
                    order.TotalMaterialCost = updatedProduct.TotalMaterialCost;
                    order.TotalLaborCost = updatedProduct.TotalLaborCost;
                    order.StateTax.TaxRate = taxRate;
                    order.TotalCost = updatedProduct.TotalCost;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Order updated! Press enter to continue.");
                    Console.ResetColor();
                    Console.ReadLine();
                    Console.Clear();
                    _manager.EditEntryToOrderList(order);
                    EditOrder(order, _manager);
                    break;
                default:
                    break;
            }
        }

        //this is used to update the product information for an order in 'saved' status
        public Order updateProductInfo(Order order, FlooringProgramManager _manager)
        {
            Order newOrder = new Order();
            string tax = order.StateTax.TaxRate;
            long tax1;
            Int64.TryParse(tax, out tax1);
            decimal tax2 = tax1;

            var productInfo = ProductSearch(_manager);

            Header();
            _clientName = order.CustomerLast;
            DisplayClientName();

            Console.Write("\nEnter the Area(SQF):");
            newOrder.Area = int.Parse(Console.ReadLine());

            foreach (var item in productInfo)
            {
                newOrder.ProductName = item.ProductName;
                newOrder.ProductSku = item.Sku;
                newOrder.TotalMaterialCost = _manager.TotalMaterialCost(item.MaterialCostPerSquareFoot, newOrder.Area);
                newOrder.TotalLaborCost = _manager.TotalLaborCost(item.LaborCostPerSquareFoot, newOrder.Area);
                newOrder.StateTax = _manager.GetStateTax(order.State);
                newOrder.TotalCost = _manager.TotalCost(newOrder.TotalLaborCost, newOrder.TotalMaterialCost, tax2);
            }

            return newOrder;
        }

        public void EditOrderMainMenuOptions()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nEnter the number of the field to edit:");
            Console.ResetColor();
            Console.WriteLine("\nEnter 'S' to return to 'search order' menu or 'M' to return to main menu:");
        }

        public int SelectClientId(List<Client> clientList, FlooringProgramManager _manager)
        {
            Header();
            Console.WriteLine("\nClient Id\tFirst Name\t    Last Name\t\tAddress\t\t                    Phone Number\n");

            foreach (var client in clientList)
            {
                Console.WriteLine(" {0}{1}{2}{3}{4}", client.CustomerID.ToString().PadRight(15), client.FirstName.PadRight(20), client.LastName.PadRight(20),
                    client.Address.PadRight(36), client.Phone.PadRight(100));//NOTE FOR TY
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nEnter Client ID number:");
            Console.ResetColor();
            Console.WriteLine("\n \nEnter 'S' to return to search menu:");
            Console.WriteLine("\n \nEnter 'M' to return to Main Menu: \n");
            string clientId = Console.ReadLine();

            if (clientId.ToLower() == "m")
            {
                ShowSalesMenu(_manager);
            }
            else if (clientId.ToLower() == "s")
            {
                SearchForClientMenu(_manager);
            }

            int clientIdParsed;

            if (int.TryParse(clientId, out clientIdParsed))
            {
                return clientIdParsed;
            }
            else
            {
                Console.WriteLine("Please enter a vaild Id number. Press Enter to try again.");
                Console.ReadLine();
                SelectClientId(clientList, _manager);
                return 0;
            }
        }

        public List<Client> SearchByClientName(FlooringProgramManager _manager)
        {
            string enteredName = GetUserInputName();

            if (enteredName.ToLower() == "m")
            {
                ShowSalesMenu(_manager);
            }
            else if (enteredName.ToLower() == "s")
            {
                SearchForClientMenu(_manager);
            }
            //returns a list containing all clients matching the last name entered by the user
            List<Client> clientList = _manager.SearchClientListByName(enteredName);
            if (clientList.Count() < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No results found. Try again.");
                Console.ResetColor();
                SearchByClientName(_manager);

                return clientList;
            }
            else
            {
                return clientList;
            }
        }

        public void NameSearchMainMenuOptions()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nEnter client's name using the following format (last name,first initial) ===> (Ex: Johnson,B):");
            Console.ResetColor();
            Console.WriteLine("\n \nEnter 'S' to return to search menu:");
            Console.WriteLine("\n \nEnter 'M' to return to Main Menu: \n");
        }

        //prompts user for client name and validates input
        public string GetUserInputName()
        {
            Header();
            DisplayClientName();
            NameSearchMainMenuOptions();
            string enteredName = Console.ReadLine();

            //convert each char in input string to ascii code and check to make sure it's a capital/lowercase letter or a comma
            foreach (char character in enteredName)
            {
                int i = (int)character;
                if (!(i == 44 || (i >= 65 && i <= 90) || (i >= 97 && i <= 122)))
                {
                    Console.WriteLine("\n");
                    Console.WriteLine("Client entry may only contain letters and a comma. Press Enter to continue.");
                    Console.ReadLine();
                    Console.Clear();
                    GetUserInputName();
                }
            }
            return enteredName;
        }


        //NO VALIDATION
        public void CreateOrder(Client existingClient, FlooringProgramManager _manager)
        {
            List <Product> productInfo;
            List<Client> clientList = new List<Client>();
            var listOfOrders = _manager.GetOrders();
            bool validChoice = false;
            Order newOrder = new Order();
            if (existingClient == null)
            {
                SearchForClientMenu(_manager);
            }
            else
            {
                newOrder.CustomerFirst = existingClient.FirstName;
                newOrder.CustomerLast = existingClient.LastName;
                newOrder.CustomerPhoneNum = existingClient.Phone;
                newOrder.State = existingClient.StateAbbreviation;
                newOrder.Address = existingClient.Address;
                newOrder.City = existingClient.City;
                newOrder.CompanyName = existingClient.CompanyName;

            }
            var checkIfOrderWasAlreadyPicked = AddToOrderFromProductSearch(_productProps, _counter, _manager);
            if (checkIfOrderWasAlreadyPicked != null)
            {
                if (checkIfOrderWasAlreadyPicked.Count() == 1 && _counter == true)
                {
                    productInfo = checkIfOrderWasAlreadyPicked;
                }
                else
                {
                    productInfo = ProductSearch(_manager);
                }
            }
            else
            {
                productInfo = ProductSearch(_manager);
            }
            Header();
            _clientName = existingClient.LastName;
            DisplayClientName();

            Console.Write("\nEnter the Area(SQF):");
            newOrder.Area = int.Parse(Console.ReadLine());
            string taxRate = _manager.GetStateTaxRateOnly(newOrder.State);
        
            foreach (var item in productInfo)
            {
                newOrder.ProductName = item.ProductName;    
                newOrder.ProductSku = item.Sku;
                newOrder.TotalMaterialCost = _manager.TotalMaterialCost(item.MaterialCostPerSquareFoot, newOrder.Area);
                newOrder.TotalLaborCost = _manager.TotalLaborCost(item.LaborCostPerSquareFoot, newOrder.Area);
                newOrder.StateTax = _manager.GetStateTax(newOrder.State);
                newOrder.TotalCost = _manager.TotalCost(newOrder.TotalLaborCost, newOrder.TotalMaterialCost, decimal.Parse(taxRate));
            }

            Header();
            _clientName = existingClient.LastName;
            DisplayClientName();

            Console.WriteLine("\n---Verify Order---\n");
            Console.WriteLine("First Name:              {0}", newOrder.CustomerFirst);
            Console.WriteLine("Last Name:               {0}", newOrder.CustomerLast);
            Console.WriteLine("Phone Number:            {0}", newOrder.CustomerPhoneNum);
            Console.WriteLine("Company:                 {0}", newOrder.CompanyName);
            Console.WriteLine("Address:                 {0}", newOrder.Address);
            Console.WriteLine("City:                    {0}", newOrder.City);
            Console.WriteLine("State:                   {0}", newOrder.State);
            Console.WriteLine("State Tax:               {0}", newOrder.StateTax.TaxRate); 
            Console.WriteLine("Product Name:            {0}", newOrder.ProductName);
            Console.WriteLine("Product SKU:             {0}", newOrder.ProductSku);
            Console.WriteLine("Total Cost Per SQF:      {0}", newOrder.TotalMaterialCost.ToString());
            Console.WriteLine("Total Labor Cost Per SQF:{0}", newOrder.TotalLaborCost.ToString());
            Console.WriteLine("Total Cost:              {0}", newOrder.TotalCost.ToString());


            Console.WriteLine("\n------------------");
            Console.WriteLine("Enter 'S' to Submit Order, 'E' to Edit Order, or 'R' to Save Order and Exit");


            do
            {
                string userChoice = Console.ReadLine().ToLower();
                if (userChoice == "s" || userChoice == "e" || userChoice == "r")
                {
                    switch (userChoice)
                    {
                        case "s":
                            _productProps = null;
                            newOrder.Status = OrderStatus.Preparing;  
                            newOrder.OrderNumber = _manager.AddEntryToOrderList(newOrder);
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Order #:      {0}", newOrder.OrderNumber.ToString().PadRight(5));
                            Console.WriteLine("Order status: {0}", newOrder.Status);
                            Console.ResetColor();
                            Console.WriteLine("Press Any Key to return to Main Menu");
                            Console.ReadKey();
                            ShowSalesMenu(_manager);
                            break;

                        case "e":
                            _manager.EditEntryToOrderList(newOrder);
                            break;

                        case "r":
                            _productProps = null;
                            newOrder.Status = OrderStatus.Saved;
                            //newOrder.OrderNumber = _manager.AddEntryToOrderList(newOrder);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Order #:      {0}", newOrder.OrderNumber.ToString());
                            Console.WriteLine("Order status: {0}", newOrder.Status);
                            Console.ResetColor();
                            Console.ReadLine();                            
                            ShowSalesMenu(_manager);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Enter a valid choice");
                }
            } while (!validChoice);
        }

        public void ShowOperationsMenu()
        {

        }

        public void ShowCreateOrderMenu(FlooringProgramManager _manager)
        {
            string choice;
            int iChoice = 0;
            bool isValidChoice = false;

            do
            {
                Header();
                DisplayClientName();
                Console.WriteLine("1. Search for Client");
                Console.WriteLine("2. Add Client");
                Console.WriteLine("3. Main Menu\n");
                Console.WriteLine("Enter Selection: ");

                choice = Console.ReadLine();
                bool isANumber = int.TryParse(choice, out iChoice);

                if (isANumber && iChoice <= 3 && iChoice > 0)
                {
                    isValidChoice = true;
                }
                else
                {
                    Console.WriteLine("Invalid Entry. Press Enter to try again.");
                    Console.ReadLine();
                }
            } while (!isValidChoice);

            switch (iChoice)
            {
                case 1:
                    SearchForClientMenu(_manager);
                    break;
                case 2:
                    ShowAddClientScreens(_manager);
                    break;
                case 3:
                    ShowSalesMenu(_manager);
                    break;
                default:
                    throw new Exception("Ludacris Speed!");
            }
        }

        public void SearchForClientMenu(FlooringProgramManager _manager)
        {
            string choice;
            int iChoice = 0;
            bool isValidChoice = false;

            do
            {
                Header();
                DisplayClientName();

                Console.WriteLine("1. Search by name");
                Console.WriteLine("2. Search by phone number");
                Console.WriteLine("3. Main menu\n");
                Console.WriteLine("Enter Selection: ");

                choice = Console.ReadLine();
                bool isANumber = int.TryParse(choice, out iChoice);

                if (isANumber && iChoice <= 3 && iChoice > 0)
                {
                    isValidChoice = true;
                }
                else
                {
                    Console.WriteLine("Invalid Entry");
                }
            } while (!isValidChoice);

            switch (iChoice)
            {
                case 1:
                    Console.Clear();
                    List<Client> clientList = SearchByClientName(_manager);
                    int clientId = SelectClientId(clientList, _manager);
                    Client clientInfo = _manager.SelectClientForNewOrder(clientId);
                    _clientName = clientInfo.LastName;
                    CreateOrder(clientInfo, _manager);
                    break;
                case 2:
                    Console.Clear();
                    List<Client> clientList2 = SearchByClientPhoneNumber(_manager);
                    int clientId2 = SelectClientId(clientList2, _manager);
                    Client clientInfo2 = _manager.SelectClientForNewOrder(clientId2);
                    CreateOrder(clientInfo2, _manager);
                    break;
                case 3:
                    Console.Clear();
                    ShowSalesMenu(_manager);
                    break;
                default:
                    throw new Exception("Ludacris Speed!");
            }

        }

        public List<Product> SearchForProductType(FlooringProgramManager _manager)
        {
            Header();
            DisplayClientName();

            Console.WriteLine("Enter the product type or press M to return to main menu.");
            var selectedProductType = Console.ReadLine().ToLower();

            if(selectedProductType == "m")
            {
                ShowSalesMenu(_manager);
            }

            Header();
            DisplayClientName();
            List<Product> productTypeList = _manager.SearchProductListByType(selectedProductType);

            if (productTypeList.Count == 0)
            {
                Console.WriteLine("Please enter a valid Product Type. Press Enter to continue.");
                Console.ReadLine();
                ProductSearch(_manager);
            }
            return productTypeList;
        }

        public List<Client> SearchByClientPhoneNumber(FlooringProgramManager _manager)
        {
            string pattern = " ";
            string replacement = "";

            Header();
            DisplayClientName();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nEnter client's phone number using the following format (xxx xxx xxxx):");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n \nEnter 'S' to return to search menu:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n \nEnter 'M' to return to Main Menu: \n");
            Console.ResetColor();

            string enteredNumber = Console.ReadLine();

            if (enteredNumber.ToLower() == "m")
            {
                ShowSalesMenu(_manager);
            }
            else if (enteredNumber.ToLower() == "s")
            {
                SearchForClientMenu(_manager);
            }

            enteredNumber = Regex.Replace(enteredNumber, pattern, replacement);
            List<Client> clientList = _manager.SearchClientListByPhoneNumber(enteredNumber);
            long parsedNumber = 2;
            bool result = Int64.TryParse(enteredNumber, out parsedNumber);

            if (!result)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid entry. Press Enter to try again.");
                Console.ResetColor();
                SearchByClientPhoneNumber(_manager);
                return clientList;
            }
            if (clientList.Count() < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No results found. Try again.");
                Console.ResetColor();
                SearchByClientPhoneNumber(_manager);
                return clientList;
            }
            else
            {
                return clientList;
            }
        }

        public List<Product> ProductSearch(FlooringProgramManager _manager)
        {
            bool validProductType;
            string selectedProductType;

            List<Product> ProductProperties = new List<Product>();

            Header();
            DisplayClientName();

            int selectedMenuChoice = 0;
            var listOfProducts = _manager.GetProducts();      
            bool validChoice = false;
            bool validMenuChoice = false;

            Console.WriteLine("\n---Availale Product Types---");

            Console.WriteLine("\n   Tile\n   Board\n   Carpet\n   Adhesive\n");
            //Console.WriteLine("\nEnter the product type, Press enter to see all products or press M to return to main menu:");
            //selectedProductType = Console.ReadLine().ToLower();

            do
            {
                validProductType = false;

                Console.WriteLine("\nEnter the product type or press M to return to main menu:");
                selectedProductType = Console.ReadLine().ToLower();

                if (selectedProductType == "m")
                {
                    ShowSalesMenu(_manager);
                }
                if(selectedProductType == "tile" || selectedProductType == "board" || selectedProductType == "carpet" || selectedProductType == "adhesive")
                {
                    validProductType = true;
                }
            } while (!validProductType);


            List < Product > productTypeList = _manager.SearchProductListByType(selectedProductType);

            int productCount = productTypeList.Count();
            int counter = 0;
            int counter2 = 0;
            string selectedSku;

            do
            {
                Header();
                DisplayClientName();
                bool headerCounter = true;
                foreach (var item in productTypeList)
                {
                    if (headerCounter)
                    {
                        Header();
                        DisplayClientName();
                        Console.WriteLine("Product ID   SKU          Name           Type                     Cost PSF        Labor Cost PSF");
                    }
                   
                    Console.WriteLine("{0}{1}{2}{3}${4:C}${5:C}", item.Id.ToString().PadRight(13), item.Sku.PadRight(13), item.ProductName.PadRight(15), item.ProductType.PadRight(25),
                        item.MaterialCostPerSquareFoot.ToString().PadRight(15), item.LaborCostPerSquareFoot.ToString().PadRight(100));

                    counter++;
                    counter2++;
                    headerCounter = false;
                    if (counter == 10 || counter2 == productCount)
                    {
                        headerCounter = true;
                        counter = 0;
                        Console.WriteLine("\n---------------------");
                        Console.WriteLine("Select a Product by SKU:");
                        Console.WriteLine("\n\nPress Enter to see more results");
                        selectedSku = Console.ReadLine();
                        ProductProperties = _manager.GetProductBySku(selectedSku);   //use sku to search for prods

                        if (ProductProperties.Count() < 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Please enter a valid SKU");
                            Console.ResetColor();
                        }
                        if (selectedSku == "")
                        {
                            if (counter2 == productCount)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("End of list. Pressing Enter will return you to product search screen");
                                Console.ResetColor();
                                Console.ReadLine();
                                ProductSearch(_manager);
                                Console.Clear();
                            }
                            Console.Clear();
                        }
                        else
                        {
                            do
                            {
                                 ///Bkmark for KB - product selected here ////////////////////////////////////               
                                if (ProductProperties.Count() != 0)
                                {
                                    Header();
                                    DisplayClientName();
                                    validChoice = true;

                                    Console.WriteLine("\n---Product---");
                                    foreach (var items in ProductProperties)
                                    {
                                        Console.WriteLine("\nName: {0}\nMaterial:{1}\nItem Price:{2}\nLabor Cost PSF:{3}\nMaterial Cost PSF:{4}\nSKU:{5}",
                                            items.ProductName, items.ProductType, items.ItemPrice, items.LaborCostPerSquareFoot, items.MaterialCostPerSquareFoot, items.Sku);
                                    }
                                    Console.WriteLine("\nChoose from the following options");
                                    Console.WriteLine("1. Provide Quote");
                                    Console.WriteLine("2. Add To Order");
                                    Console.WriteLine("3. New Product Search");
                                    Console.WriteLine("4. Main Menu");
                                    do
                                    {
                                        int.TryParse(Console.ReadLine(), out selectedMenuChoice);
                                        if (selectedMenuChoice < 0 && selectedMenuChoice > 4)
                                        {
                                            Console.WriteLine("Please make a valid selection");
                                        }
                                        else
                                        {
                                            validMenuChoice = true;
                                        }
                                    } while (!validMenuChoice);
                                }
                                else
                                {
                                    Console.WriteLine("Enter a SKU to select a product");
                                }
                            } while (!validChoice);
                        }
                        if (validMenuChoice)
                        {
                            switch (selectedMenuChoice)
                            {
                                case 1:
                                    ShowProductQuote(ProductProperties, _manager);
                                    break;
                                case 2:
                                    return ProductProperties;
                                case 3:
                                    ProductSearch(_manager);
                                    return ProductProperties;
                                    break;
                                case 4:
                                    ShowSalesMenu(_manager);
                                    break;
                            }
                        }
                    }
                }
            } while (counter < productCount && validMenuChoice == false);
            return ProductProperties;
        }

        private void ShowProductQuote(List<Product> propertyList, FlooringProgramManager _manager)
        {
            string areaWanted;
            int areaNum;
            bool validArea;
            bool validState;
            string state;
            Tax tax = new Tax();

            Header();

            do
            {
                validArea = true;

                Header();
                DisplayClientName();
                Console.WriteLine("Enter amount desired: (SQF)");
                areaWanted = Console.ReadLine();
                validArea = int.TryParse(areaWanted, out areaNum);

                if (!validArea)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nPlease enter a number value for the desired area of flooring.");
                    Console.ResetColor();
                }
            } while (!validArea);

            do
            {
                validState = true;

                Console.WriteLine("\nPlease enter a 2-letter state abbreviation: ");
                state = Console.ReadLine().ToUpper();

                if (state.Length != 2)
                {
                    validState = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid 2-letter state abbreviation.");
                    Console.ResetColor();
                }
                else
                {
                    tax = _manager.GetStateTax(state);

                    if (tax.TaxRate == null)
                    {
                        validState = false;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nInvalid 2-letter state abbreviation.");
                        Console.ResetColor();
                    }
                }
            } while (!validState);

            decimal costPerSqFoot = propertyList.Select(p => p.MaterialCostPerSquareFoot).First();
            decimal totalMaterialCost = _manager.TotalMaterialCost(costPerSqFoot, areaNum);
            decimal totalLaborCost = _manager.TotalLaborCost(costPerSqFoot, areaNum);

            decimal totalCost = _manager.TotalCost(totalLaborCost, totalMaterialCost, decimal.Parse(tax.TaxRate));

            Header();
            DisplayClientName();
            foreach (var item in propertyList)
            {
                Console.WriteLine("Product Quote:\n");
                Console.WriteLine("Name: {0}\nMaterial: {1}\nSKU: {2}\nItem Price: {3:C}\nLabor Cost PSF: {4:C}\nMaterial Cost PSF: {5:C}\nTotal Material Cost: {6:C}\nTotal Labor Cost: {7:C}",
                                            item.ProductName, item.ProductType, item.Sku, item.ItemPrice, item.LaborCostPerSquareFoot, item.MaterialCostPerSquareFoot, totalMaterialCost, totalLaborCost);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nTotal Cost:{0:C}", totalCost);
                Console.ResetColor();
            }

            Console.WriteLine("Press any key to return to main menu.");
            Console.ReadLine();
            ShowSalesMenu(_manager);
        }

        public void ShowAddClientScreens(FlooringProgramManager _manager)
        {
            bool isNotValid = true;
            bool commaPresent = true;
            Client newClient = new Client();

            Header();
            DisplayClientName();
            do
            {
                Header();
                DisplayClientName();
                Console.WriteLine("Please enter a Company Name:");
                newClient.CompanyName = Console.ReadLine();
                if (newClient.CompanyName.Contains(','))
                {
                    NoCommaAllowed();
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    commaPresent = false;
                }
            } while (commaPresent == true);
            commaPresent = true;

            do
            {
                Header();
                DisplayClientName();
                Console.WriteLine("Please enter a First Name:");
                newClient.FirstName = Console.ReadLine();
                if (newClient.FirstName.Contains(','))
                {
                    NoCommaAllowed();
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    commaPresent = false;
                }
            } while(commaPresent == true);
            commaPresent = true;

            do
            {
                Header();
                DisplayClientName();
                Console.WriteLine("Please enter a Last Name:");
                newClient.LastName = Console.ReadLine();
                if (newClient.LastName.Contains(','))
                {
                    NoCommaAllowed();
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    commaPresent = false;
                }
            } while (commaPresent == true);
            commaPresent = true;

            _clientName = newClient.LastName;

            do
            {
                Header();
                DisplayClientName();
                Console.WriteLine("Please enter a Street Address:");
                newClient.Address = Console.ReadLine();
                if (newClient.Address.Contains(','))
                {
                    NoCommaAllowed();
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    commaPresent = false;
                }
            } while (commaPresent == true);
            commaPresent = true;

            do
            {
                Header();
                DisplayClientName();
                Console.WriteLine("Please enter a City:");
                newClient.City = Console.ReadLine();
                if (newClient.City.Contains(','))
                {
                    NoCommaAllowed();
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    commaPresent = false;
                }
            } while (commaPresent == true);
            commaPresent = true;

            do
            {
                Header();
                DisplayClientName();
                Console.WriteLine("Please enter a two-letter state abbreviation: ");
                newClient.StateAbbreviation = Console.ReadLine();

                List<string> file = new List<string>(File.ReadAllLines("tax_rates.csv"));
                foreach(string line in file)
                {
                    if (newClient.StateAbbreviation.Length == 2 && line.StartsWith(newClient.StateAbbreviation.ToUpper()))
                    {
                        isNotValid = false;
                    }                       
                }

                //if (newClient.StateAbbreviation.Length == 2)
                //{
                //    isNotValid = false;
                //}
                //else

                if(isNotValid== true)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid two-letter state abbreviation. Press Enter to try again.");
                    Console.ResetColor();
                    Console.ReadLine();
                }
            } while (isNotValid);
            isNotValid = true;

            do
            {
                Header();
                DisplayClientName();
                Console.WriteLine("Please enter a postal code: ");
                newClient.PostalCode = Console.ReadLine();
                if (newClient.PostalCode.Length == 5)
                {
                    isNotValid = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid 5-digit Postal Code. Press Enter to try again");
                    Console.ResetColor();
                    Console.ReadLine();
                }
            } while (isNotValid);
            isNotValid = true;

            do
            {
                Header();
                DisplayClientName();
                Console.WriteLine("Please enter a country:");
                newClient.Country = Console.ReadLine();
                if (newClient.Country.Contains(','))
                {
                    NoCommaAllowed();
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    commaPresent = false;
                }
            } while (commaPresent == true);
            commaPresent = true;

            do
            {
                Header();
                DisplayClientName();
                Console.WriteLine("Please enter a Phone Number: ");
                string userInput = Console.ReadLine();
                newClient.Phone = new string(userInput.Where(char.IsDigit).ToArray());      //initializes new string which is an array of the chars that are digits
                if (newClient.Phone.Length == 10)
                {
                    isNotValid = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid 10-digit Phone Number. Press Enter to try again.");
                    Console.ResetColor();
                    Console.ReadLine();
                }
            } while (isNotValid);

            isNotValid = true;

            do
            {
                          
                Header();
                DisplayClientName();
                Console.WriteLine("Please enter an Email: ");
                newClient.Email = Console.ReadLine();

                if (newClient.Email.Contains(','))
                {
                    NoCommaAllowed();
                    Console.ReadLine();
                    Console.Clear();
                }

                else if (!(newClient.Email.Contains('@') && newClient.Email.Contains(".com")))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("A valid email address must contain an '@' and a '.com'.  Press enter to try again.");
                    Console.ResetColor();
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    commaPresent = false;
                }
            } while (commaPresent == true);

            do
            {
                Header();
                DisplayClientName();
                Console.WriteLine("Please enter a Fax Number: ");
                string userInput = Console.ReadLine();
                newClient.Fax = new string(userInput.Where(char.IsDigit).ToArray());
                if (newClient.Fax.Length == 10)
                {
                    isNotValid = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid 10-digit Fax Number. Press Enter to try again.");
                    Console.ResetColor();
                    Console.ReadLine();
                }
            } while (isNotValid);
            isNotValid = true;

            Header();
            DisplayClientName();

            //adds newClient to File and returns the new id which is generated in ClientRepository
            newClient.CustomerID = _manager.AddEntryToClientList(newClient);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Customer Id: {0}\n", newClient.CustomerID);
            Console.ResetColor();

            Console.WriteLine("Company Name: {0}", newClient.CompanyName);
            Console.WriteLine("First Name: {0}", newClient.FirstName);
            Console.WriteLine("Last Name: {0}", newClient.LastName);
            Console.WriteLine("Street Address: {0}", newClient.Address);
            Console.WriteLine("City: {0}", newClient.City);
            Console.WriteLine("State: {0}", newClient.StateAbbreviation);
            Console.WriteLine("Postal Code: {0}", newClient.PostalCode);
            Console.WriteLine("Country: {0}", newClient.Country);
            Console.WriteLine("Phone: {0}", newClient.Phone);
            Console.WriteLine("Email: {0}", newClient.Email);
            Console.WriteLine("Fax Number: {0}\n", newClient.Fax);

            Console.WriteLine("Press 'E' to edit client info. Press 'P' to place order: ");
            string userChoice = Console.ReadLine();
            switch (userChoice.ToLower())
            {
                case "e":
                    ShowEditClientScreens(newClient, _manager);
                    break;
                case "p":
                    CreateOrder(newClient, _manager);
                    ProductSearch(_manager);
                    break;
                default:
                    break;
            }
        }

        public void ShowEditClientScreens(Client editClient, FlooringProgramManager _manager)
        {
            Header();
            _clientName = editClient.LastName;
            DisplayClientName();
            Console.WriteLine("1. Company Name: {0}", editClient.CompanyName);
            Console.WriteLine("2. First Name: {0}", editClient.FirstName);
            Console.WriteLine("3. Last Name: {0}", editClient.LastName);
            Console.WriteLine("4. Street Address: {0}", editClient.Address);
            Console.WriteLine("5. City: {0}", editClient.City);
            Console.WriteLine("6. State: {0}", editClient.StateAbbreviation);
            Console.WriteLine("7. Postal Code: {0}", editClient.PostalCode);
            Console.WriteLine("8. Country: {0}", editClient.Country);
            Console.WriteLine("9. Phone: {0}", editClient.Phone);
            Console.WriteLine("10. Email: {0}", editClient.Email);
            Console.WriteLine("11. Fax Number: {0}\n", editClient.Fax);

            Console.WriteLine("Press a number to edit a field: ");
            int userChoice = int.Parse(Console.ReadLine());

            switch (userChoice)
            {
                case 1:
                    Console.WriteLine("Company Name: {0}", editClient.CompanyName);
                    Console.WriteLine("Enter new Company Name: ");
                    editClient.CompanyName = Console.ReadLine();
                    break;
                case 2:
                    Console.WriteLine("First Name: {0}", editClient.FirstName);
                    Console.WriteLine("Enter new First Name: ");
                    editClient.FirstName = Console.ReadLine();
                    break;
                case 3:
                    Console.WriteLine("Last Name: {0}", editClient.LastName);
                    Console.WriteLine("Enter new Last Name: ");
                    editClient.LastName = Console.ReadLine();
                    break;
                case 4:
                    Console.WriteLine("Street Address: {0}", editClient.Address);
                    Console.WriteLine("Enter new Street Address: ");
                    editClient.Address = Console.ReadLine();
                    break;
                case 5:
                    Console.WriteLine("City: {0}", editClient.City);
                    Console.WriteLine("Enter new City: ");
                    editClient.City = Console.ReadLine();
                    break;
                case 6:
                    Console.WriteLine("State: {0}", editClient.StateAbbreviation);
                    Console.WriteLine("Enter new State: ");
                    editClient.StateAbbreviation = Console.ReadLine();
                    break;
                case 7:
                    Console.WriteLine("Postal Code: {0}", editClient.PostalCode);
                    Console.WriteLine("Enter new Postal Code: ");
                    editClient.PostalCode = Console.ReadLine();
                    break;
                case 8:
                    Console.WriteLine("Country: {0}", editClient.Country);
                    Console.WriteLine("Enter new Country: ");
                    editClient.Country = Console.ReadLine();
                    break;
                case 9:
                    Console.WriteLine(" Phone: {0}", editClient.Phone);
                    Console.WriteLine("Enter new Phone Number: ");
                    editClient.Phone = Console.ReadLine();
                    break;
                case 10:
                    Console.WriteLine("Email: {0}", editClient.Email);
                    Console.WriteLine("Enter new Email: ");
                    editClient.Email = Console.ReadLine();
                    break;
                case 11:
                    Console.WriteLine("Fax Number: {0}\n", editClient.Fax);
                    Console.WriteLine("Enter new Fax Number: ");
                    editClient.Fax = Console.ReadLine();
                    break;
                default:
                    break;
            }

            _manager.EditClientEntry(editClient);

            Console.WriteLine("Press 'E' to edit client info. Press 'P' to place order: ");
            string menuChoice = Console.ReadLine();
            switch (menuChoice.ToLower())
            {
                case "e":
                    ShowEditClientScreens(editClient, _manager);
                    break;
                case "p":
                    CreateOrder(editClient, _manager);
                    break;
                default:
                    break;
            }
        }

        public List<Product> AddToOrderFromProductSearch(List<Product> productProps, bool counter, FlooringProgramManager _manager)
        {
           _counter = counter;
            if (productProps != null)
            {
                if (productProps.Count() == 1 && _counter == true)
                {
                    _productProps = productProps;
                    _counter = false;
                    ShowCreateOrderMenu(_manager);
                    return _productProps;
                }
                else
                {
                    _counter = true;
                    return _productProps;
                }
            }
            else
            {
                _counter = true;
                _productProps = null;
                _productProps = productProps;
                return _productProps;
            }

            

        }

        public void Header()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition((Console.WindowWidth - _salesMode.Length) / 2, Console.CursorTop);
            Console.WriteLine(_salesMode + "\n");
            Console.ResetColor();
        }

        public void InvalidEntry()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid entry. Try again.");
            Console.ResetColor();
        }

        public void NoCommaAllowed()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nThis field cannot contain a comma.  Press 'enter' to try again.");
            Console.ResetColor();
        }

        public void DisplayClientName()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Current Client: {0}\n", _clientName);
            Console.ResetColor();
        }
    }
}


