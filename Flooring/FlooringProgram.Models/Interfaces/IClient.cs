using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringProgram.Models.Interfaces
{
    public interface IClient
    {
        int AddClient(Client newClient);
        void EditClient(Client editedClient);
        List<Client> GetClientList();
        List<Client> GetClientLastName(string name);
        Client GetClientById(int id);
        List<Client> GetClientPhoneNumber(string phoneNumber);
    }
}
