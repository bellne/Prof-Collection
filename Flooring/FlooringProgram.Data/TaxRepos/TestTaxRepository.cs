using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringProgram.Models;
using FlooringProgram.Models.Interfaces;

namespace FlooringProgram.Data.TaxRepos
{
    public class TestTaxRepository : ITaxRate
    {
        public string GetStateTaxRateOnly2(string stateAbbrev)
        {
            throw new NotImplementedException();
        }

        public Tax GetTaxRate(string StateAbbreviation)
        {
            throw new NotImplementedException();
        }
    }
}
