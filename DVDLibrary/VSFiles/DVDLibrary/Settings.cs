using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DVDLibrary
{
    public static class Settings
    {

        private static string _connectionString;

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = ConfigurationManager.ConnectionStrings["DVDLibrary"].ConnectionString;
                }

                return _connectionString;
            }
        }
    }
}