using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.Models;
using FlooringProgram.Models.Interfaces;

namespace FlooringProgram.Data.ClientRepos
{
    public class TestClientRepository : IClient
    {
        //creates a test client list that mimics the client .csv file
        private ClientList _testClientList = new ClientList();

        public int AddClient(Client newClient)
        {
            if (_testClientList.Clients.Count() > 0)
            {
                //selects max customer ID of all cust IDs in client list and adds 1
                newClient.CustomerID = _testClientList.Clients.Select(c => c.CustomerID).Max() + 1;
            }
            else
            {
                newClient.CustomerID = 1;
            }

            _testClientList.Clients.Add(newClient);

            return newClient.CustomerID;
        }

        public void EditClient(Client editedClient)
        {
            throw new NotImplementedException();
        }

        public void GetClientById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Client> GetClientLastName(string name)
        {
            throw new NotImplementedException();
        }

        public List<Client> GetClientList()
        {
            throw new NotImplementedException();
        }

        public List<Client> GetClientPhoneNumber(string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public void SearchClient(Client searchedClient)
        {
            throw new NotImplementedException();
        }

        Client IClient.GetClientById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
