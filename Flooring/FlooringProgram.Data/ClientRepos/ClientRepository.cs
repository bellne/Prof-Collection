using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.Models.Interfaces;
using FlooringProgram.Models;
using System.IO;

namespace FlooringProgram.Data.ClientRepos
{
  
    public class ClientRepository : IClient
    {
        private string _filename = "CustomerList.csv";

        public int AddClient(Client newClient)
        {
            if (!File.Exists(_filename))
            {
                File.Create(_filename).Close();
            }
            var allClients = GetClientList();
            var newCustomerId = 1;
            if(allClients.Count() > 0)
            {
                newCustomerId = allClients.Select(x => x.CustomerID).Max() + 1;
            }
            newClient.CustomerID = newCustomerId;
            allClients.Add(newClient);
            List<string> allLines = new List<string>();

            File.AppendAllText("CustomerList.csv", string.Format("\n{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", newClient.CustomerID, newClient.CompanyName, newClient.FirstName, newClient.LastName, newClient.Address, newClient.City, newClient.StateAbbreviation, newClient.PostalCode, newClient.Country, newClient.Phone, newClient.Email, newClient.Fax));

            return newCustomerId;
        }

        public void EditClient(Client editedClient)
        {
            List<string> file = new List<string>(File.ReadAllLines("CustomerList.csv"));
            int lastLineIndex = file.Count();
            file.RemoveAt(lastLineIndex-1);
            File.WriteAllLines("CustomerList.csv", file.ToArray());

            File.AppendAllText("CustomerList.csv", string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", editedClient.CustomerID, editedClient.CompanyName, editedClient.FirstName, editedClient.LastName, editedClient.Address, editedClient.City, editedClient.StateAbbreviation, editedClient.PostalCode, editedClient.Country, editedClient.Phone, editedClient.Email, editedClient.Fax));
            return;
        }

        public List<Client> GetClientLastName(string name)
        {
            var fields = name.Trim().Split(',');
            List<Client> lastNameList = new List<Client>();
            List<Client> clientList = GetClientList();
            bool foundClient = false;

            if (foundClient == false)
            {
                foreach (var client in clientList)
                {
                    if ((client.LastName.ToLower() == fields[0].ToLower() && client.FirstName[0].ToString().ToLower() == fields[1].ToLower().First().ToString()))
                    {
                        lastNameList.Add(client);

                        if (lastNameList.Count() > 0)
                        {
                            foundClient = true;
                        }
                    }
                }
            }
            if (foundClient == false)
            {
                foreach (var client in clientList)
                {
                    if (client.LastName.ToLower().Contains(fields[0].ToLower()))
                    {
                        lastNameList.Add(client);
                    }
                }
            }

            return lastNameList;
        }

        public List<Client> GetClientPhoneNumber(string phoneNumber)
        {
            List<Client> phoneNumberList = new List<Client>();
            List<Client> clientList = GetClientList();
            bool foundClient = false;

            if (foundClient == false)
            {
                foreach (var client in clientList)
                {
                    if ((client.Phone == phoneNumber))
                    {
                        phoneNumberList.Add(client);

                        if (phoneNumberList.Count() > 0)
                        {
                            foundClient = true;
                        }
                    }
                }
            }
            if (foundClient == false)
            {
                foreach (var client in clientList)
                {
                    if (client.Phone.Contains(phoneNumber))
                    {
                        phoneNumberList.Add(client);
                    }
                }
            }

            return phoneNumberList;
        }

        public Client GetClientById(int id)
        {
            var resultSet = GetClientList();

            return resultSet.SingleOrDefault(x => x.CustomerID == id);
        }

        public List<Client> GetClientList()
        {
            if (!File.Exists(_filename))
            {
                File.Create(_filename).Close();
            }

            var allLines = File.ReadAllLines(_filename);
            List<Client> resultSet = new List<Client>();

            for (int i = 0; i < allLines.Count(); i++)
            {
                var fields = allLines[i].Split(',');
                if (fields.Count() == 12)
                {
                    Client existingClient = new Client()
                    {
                        CustomerID = int.Parse(fields[0]),
                        CompanyName = fields[1],
                        FirstName = fields[2],
                        LastName = fields[3],
                        Address = fields[4],
                        City = fields[5],
                        StateAbbreviation = fields[6],
                        PostalCode = fields[7],
                        Country = fields[8],
                        Phone = fields[9],
                        Email = fields[10],
                        Fax = fields[11]
                    };
                    resultSet.Add(existingClient);
                }
            }
            return resultSet;
        }
    }
}
