using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringProgram.Models.Interfaces
{
    public interface IOrderRepository
    {
        int AddOrder(Order newOrder);
        void EditOrder(Order editedOrder);
        List<Order> SearchOrderByLastName(string UserInput);
        Order SearchOrderByOrderID(int OrderId);
        List<Order> GetOrdersList();
        void UpdateStatusToPreparingData(Order editedOrder);
    }
}
