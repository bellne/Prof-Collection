using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.Models;
using FlooringProgram.Models.Interfaces;

namespace FlooringProgram.Data.ProductRepos
{
    public class TestProductRepository : IProducts
    {
        public Product GetProductBySku(string ProductSku)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetProductList()
        {
            throw new NotImplementedException();
        }

        public Product GetProductType(string ProductType)
        {
            throw new NotImplementedException();
        }

        List<Product> IProducts.GetProductBySku(string ProductSku)
        {
            throw new NotImplementedException();
        }

        List<Product> IProducts.GetProductType(string ProductType)
        {
            throw new NotImplementedException();
        }


    }
}
