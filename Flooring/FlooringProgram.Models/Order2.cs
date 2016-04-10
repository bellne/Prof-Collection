using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringProgram.Models
{
    class Order2
    {
        
            public int OrderNumber { get; set; }
            public string CustomerFirst { get; set; }
            public string CompanyName { get; set; }
            public string CustomerPhoneNum { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public Tax StateTax { get; set; }
            public decimal TaxRate { get; set; }
            public string StateAbbreviation { get; set; }
            public string ProductName { get; set; }
            public string ProductSku { get; set; }
            public decimal TotalMaterialCost { get; set; }
            public decimal TotalLaborCost { get; set; }
            public decimal TotalCost { get; set; }
            public OrderStatus Status { get; set; }
            public string CustomerLast { get; set; }
            public int Area { get; set; }
    }
}
