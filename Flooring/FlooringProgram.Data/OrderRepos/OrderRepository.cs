using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.Models;
using FlooringProgram.Models.Interfaces;
using FlooringProgram.Data.TaxRepos;
using System.IO;

namespace FlooringProgram.Data.OrderRepos
{
    public class OrderRepository : IOrderRepository
    {
        private string _filename = "OrderList.csv";
        private static List<Order> _orderList;

        public int AddOrder(Order newOrder)
        {
            if (!File.Exists(_filename))
            {
                File.Create(_filename).Close();
            }
            var allClients = GetOrdersList();
            var orderId = 1;
            if (allClients.Count() > 0)
            {
                orderId = allClients.Select(x => x.OrderNumber).Max() + 1;
            }
            newOrder.OrderNumber = orderId;
            allClients.Add(newOrder);
            List<string> allLines = new List<string>();

            File.AppendAllText("OrderList.csv", string.Format("\n{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}", 
                newOrder.OrderNumber, newOrder.CustomerFirst, newOrder.CustomerLast, newOrder.CompanyName, newOrder.CustomerPhoneNum, 
                newOrder.Address, newOrder.City, newOrder.State, newOrder.StateTax.TaxRate, newOrder.ProductName, newOrder.ProductSku, 
                newOrder.Area, newOrder.TotalMaterialCost, newOrder.TotalLaborCost, newOrder.TotalCost, newOrder.Status));

            return orderId;
        }

        //public static List<Order> LoadOrders()
        //{
        //    _orderList = (
        //        from e in XDocument.Load("OrderList.csv").
        //        Root.Elements("order")
        //        select new Order
        //        {
        //            OrderNumber = (string)e.Element("ordernumber"),
        //            CustomerFirst = (string)e.Element("customerfirst"),
        //            CompanyName = (string)e.Element("companyname"),
        //            CustomerPhoneNum = (string)e.Element("phonenumber"),
        //            Address = (string)e.Element("address"),
        //            City = (string)e.Element("city"),
        //            State = (string)e.Element("state"),
        //            StateTax = (
        //                from o in e.Elements("statetax").Elements("tax")
        //                select new Tax
        //                {
        //                    StateAbbreviation = (string)e.Element("state"),
        //                    TaxRate = (string)e.Element("taxrate"),
        //                    FuelTaxRate = (string)e.Element("fueltaxrate")
        //                })
        //                .ToArray(),
        //            ProductName = (string)e.Element("productname"),
        //            ProductSku = (string)e.Element("productsku"),
        //            TotalMaterialCost = (decimal)e.Element("totalmaterialcost"),
        //            TotalLaborCost = (decimal)e.Element("totallaborcost"),
        //            TotalCost = (decimal)e.Element("totalcost"),
        //            Status = (OrderStatus)e.Element("orderstatus"),
        //            CustomerLast = (string)e.Element("customerlast"),
        //            Area = (int)e.Element("area")
        //        }).ToList();
        //} 

        public void EditOrder(Order editedOrder)
        {
            var orderList = GetOrdersList();
            Order order;
            int totalCount = 0;
            int neededCount = 0;
            List<string> file = new List<string>(File.ReadAllLines("OrderList.csv"));

            foreach (var i in orderList)
            {
                totalCount++;

                if(i.OrderNumber == editedOrder.OrderNumber)
                {
                    neededCount = totalCount -1;
                    order = i;
                }
            };

            file.RemoveAt(neededCount);

            File.WriteAllLines("OrderList.csv", file.ToArray());

            File.AppendAllText("OrderList.csv", string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}",
                editedOrder.OrderNumber, editedOrder.CustomerFirst, editedOrder.CustomerLast, editedOrder.CompanyName, editedOrder.CustomerPhoneNum,
                editedOrder.Address, editedOrder.City, editedOrder.State, editedOrder.StateTax.TaxRate, editedOrder.ProductName, editedOrder.ProductSku,
                editedOrder.Area, editedOrder.TotalMaterialCost, editedOrder.TotalLaborCost, editedOrder.TotalCost, editedOrder.Status));
        }

        public void UpdateStatusToPreparingData(Order editedOrder)
        {
            var orderList = GetOrdersList();
            Order order;
            int totalCount = 0;
            int neededCount = 0;
            List<string> file = new List<string>(File.ReadAllLines("OrderList.csv"));

            foreach (var i in orderList)
            {
                totalCount++;

                if (i.OrderNumber == editedOrder.OrderNumber)
                {
                    neededCount = totalCount - 1;
                    order = i;
                }
            };

            file.RemoveAt(neededCount);

            File.WriteAllLines("OrderList.csv", file.ToArray());

            File.AppendAllText("OrderList.csv", string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}",
                editedOrder.OrderNumber, editedOrder.CustomerFirst, editedOrder.CustomerLast, editedOrder.CompanyName, editedOrder.CustomerPhoneNum,
                editedOrder.Address, editedOrder.City, editedOrder.State, editedOrder.StateTax.TaxRate, editedOrder.ProductName, editedOrder.ProductSku,
                editedOrder.Area, editedOrder.TotalMaterialCost, editedOrder.TotalLaborCost, editedOrder.TotalCost, editedOrder.Status));
        }

        public List<Order> GetOrdersList()
        {
            if (!File.Exists(_filename))
            {
                File.Create(_filename).Close();
            }

            var allLines = File.ReadAllLines(_filename);
            List<Order> ordersList = new List<Order>();

            for (int i = 0;  i < allLines.Count(); i++)
            {
                var fields = allLines[i].Split(',');
                if (fields.Count() == 16)
                {
                    Order exsitingOrder = new Order()
                    {
                        OrderNumber = int.Parse(fields[0]),
                        Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), fields[15]),
                        TotalMaterialCost = decimal.Parse(fields[12]),
                        TotalLaborCost = decimal.Parse(fields[13]),
                        StateTax = StateTaxRate(fields[8]),
                        TotalCost = decimal.Parse(fields[14]),
                        City = fields[6],
                        Address = fields[5],
                        CustomerFirst = fields[1], 
                        CustomerLast = fields[2],
                        CompanyName = fields[3],
                        CustomerPhoneNum = fields[4],
                        Area = int.Parse(fields[11]),
                        ProductName = fields[9],
                        ProductSku = fields[10],
                        State = fields[7]                       
                     };
                    ordersList.Add(exsitingOrder);

                }

            }
            return ordersList;
        }

        public List<Order> SearchOrderByLastName(string UserInput)
        {
            var fields = UserInput.Trim().Split(',');
            bool validChoice = false;
            List<Order> lastNameOrderList = new List<Order>();
            List<Order> orderList = GetOrdersList();
            if (validChoice == false)
            {


                foreach (var order in orderList)
                {
                    if ((order.CustomerLast == fields[0].ToLower() && order.CustomerFirst[0].ToString().ToLower() == fields[1].ToLower().First().ToString()) || 
                        (order.CustomerLast.ToLower().Contains(fields[0].ToLower())))
                    {
                        lastNameOrderList.Add(order);
                        if (lastNameOrderList.Count() > 0)
                        {
                            validChoice = true;
                        }
                    }
                }
            }
            if (validChoice == false)
            {
                foreach (var order  in orderList)
                {
                    if (order.CustomerLast.ToLower().Contains(fields[0].ToLower()))
                    {
                        lastNameOrderList.Add(order);
                    }
                }
            }

            return lastNameOrderList;
        }

        public Order SearchOrderByOrderID(int OrderId)
        {
            var resultSet = GetOrdersList();

            //var order = from i in resultSet
            //            where i.OrderNumber == OrderId
            //            select new { TaxRate = i.StateTax.TaxRate, StateAbbrev = i.StateTax.StateAbbreviation,
            //            i.Address, i.Area, i.City, i.CompanyName, i.CustomerFirst, i.CustomerLast, i.CustomerPhoneNum,
            //            i.OrderNumber, i.ProductName, i.ProductSku, i.State, i.Status, i.TotalCost, i.TotalLaborCost,
            //            i.TotalMaterialCost};

            return resultSet.SingleOrDefault(x => x.OrderNumber == OrderId);
        }

        public Tax StateTaxRate(string stateAbv)
        {
            TaxRepository asd = new TaxRepository();
            return asd.GetTaxRate(stateAbv);
        }
    }
}

