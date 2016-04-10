using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringProgram.Models.Interfaces
{
    public interface IProducts
    {
        List<Product> GetProductType(string ProductType);
        List<Product> GetProductList();
        List<Product> GetProductBySku(string ProductSku);
    }
}
