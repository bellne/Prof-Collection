using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringProgram.Models
{
    public class ClientList
    {
        public List<Client> Clients { get; set; }

        public ClientList()
        {
            Clients = new List<Client>();
        }
    }
}
