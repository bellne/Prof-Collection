using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.Models;
using FlooringProgram.Models.Interfaces;
using System.IO;

namespace FlooringProgram.Data.ProductRepos
{
    public class ProductRepository : IProducts
    {
        private string _filename = "ProductList.csv";

      public List<Product> GetProductType(string ProductType)
        {
           List<Product> resultSet = GetProductList();
            List<Product> productTypeList = new List<Product>();

            foreach (Product item in resultSet)
            {
                if (item.ProductType.EndsWith(ProductType))
                {
                    productTypeList.Add(item);
                }
            }
                return productTypeList; 
        }

        public List<Product> GetProductBySku(string ProductSku)
        {
            List<Product> resultSet = GetProductList();
            List<Product> productPropertys = new List<Product>();

            foreach (Product item in resultSet)
            {
                if (item.Sku.Contains(ProductSku))
                {
                    productPropertys.Add(item);
                }
            }
            return productPropertys; 
        }

        public List<Product> GetProductList()
        {
            if (!File.Exists(_filename))
            {
                File.Create(_filename).Close();
            }

            var allLines = File.ReadAllLines(_filename);
            List<Product> resultSet = new List<Product>();

            for (int i  = 1; i < allLines.Count(); i++)
            {
                var fields = allLines[i].Trim().Split(';');
                if (fields.Count() == 8)
                {
                    Product existingProduct = new Product()
                    {
                        Id = int.Parse(fields[0]),
                        Sku = fields[3],
                        ProductName = fields[1],
                        ProductType = fields[2],
                        MaterialCostPerSquareFoot = decimal.Parse(fields[6]),
                        LaborCostPerSquareFoot = decimal.Parse(fields[5]),
                        ItemPrice = decimal.Parse(fields[7])
                    };
                    resultSet.Add(existingProduct);
                }
            }
            return resultSet;
        }
    }
}
