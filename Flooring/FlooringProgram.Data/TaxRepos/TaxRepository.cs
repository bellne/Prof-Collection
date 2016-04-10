using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.Models;
using FlooringProgram.Models.Interfaces;
using System.IO;

namespace FlooringProgram.Data.TaxRepos
{
    public class TaxRepository : ITaxRate

    {
        private string _taxData = "tax_rates.csv";

        public Tax GetTaxRate(string StateAbbreviation)
        {
            Tax stateTaxinfo = new Tax();
            List<Tax> stateTaxList = GetTaxRateList();
            
            foreach(var state in stateTaxList)
            {
                if(StateAbbreviation == state.StateAbbreviation)
                {
                    stateTaxinfo = state;
                }
            }

            return stateTaxinfo;
        }

        public string GetStateTaxRateOnly2 (string stateAbbrev)
        {
            List<Tax> stateTaxList = GetTaxRateList();
            string taxRateString = "";

            foreach (var state in stateTaxList)
            {
                if (stateAbbrev == state.StateAbbreviation)
                {
                    taxRateString = state.TaxRate;
                }
            }

            return taxRateString;
        }

        public List<Tax> GetTaxRateList()
        {
            if (!File.Exists(_taxData))
            {
                File.Create(_taxData).Close();
            }

            List<Tax> taxList = new List<Tax>();
            var allLines = File.ReadAllLines(_taxData);

            for (int i = 1; i < allLines.Count(); i++)
            {
                var fields = allLines[i].Split(',');
                if(fields.Count() == 5)
                {
                    Tax stateTaxInfo = new Tax()
                    {
                        StateAbbreviation = fields[0],
                        TaxRate = fields[1],
                        FuelTaxRate = fields[2]
                    };
                    var removeDecmialPoint = new string[] {"."};
                    foreach (var item in removeDecmialPoint)
                    {
                        stateTaxInfo.TaxRate = stateTaxInfo.TaxRate.Replace(item, string.Empty);
                        stateTaxInfo.TaxRate = stateTaxInfo.TaxRate.Insert(0,".0");
                    }
                    taxList.Add(stateTaxInfo);
                }
            }
            return taxList;
        }
    }
}
