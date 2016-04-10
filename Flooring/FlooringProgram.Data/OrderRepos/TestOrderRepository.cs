using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.Models;
using FlooringProgram.Models.Interfaces;

namespace FlooringProgram.Data.OrderRepos
{
    public class TestOrderRepository : IOrderRepository
    {
        public void AddOrder(Order newOrder)
        {
            throw new NotImplementedException();
        }

        public void EditOrder(Order editedOrder)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetOrdersList()
        {
            throw new NotImplementedException();
        }

        public void SearchOrder(string UserInput)
        {
            throw new NotImplementedException();
        }

        public List<Order> SearchOrderByLastName(string UserInput)
        {
            throw new NotImplementedException();
        }

        public Order SearchOrderByOrderID(int OrderId)
        {
            throw new NotImplementedException();
        }

        public void UpdateStatusToPreparingData(Order editedOrder)
        {
            throw new NotImplementedException();
        }

        int IOrderRepository.AddOrder(Order newOrder)
        {
            throw new NotImplementedException();
        }
    }
}
