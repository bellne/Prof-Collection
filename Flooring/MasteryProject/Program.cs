using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using FlooringProgram.BLL;
using MasteryProject;

namespace FlooringProgram
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string modeChoice = ConfigurationManager.AppSettings["mode"];
            FlooringProgramManager _manager = FPManagerFactory.CreateFlooringProgramManager(modeChoice);
            FlooringProgramUI ui = new FlooringProgramUI();
            ui.ShowMenu(_manager);
        }
    }
}
