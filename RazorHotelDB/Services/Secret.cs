using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

namespace RazorHotelDB.Services
{
    public class Secret
    {
        private static string _connectionStringLocal = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=HotelDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;"; //@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=HotelDbtest2;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        private static string _connectionStringSimply = @"Data Source = mssql14.unoeuro.com; Initial Catalog = saiv_zealand_dk_db_database; User ID = saiv_zealand_dk; Password=9DmGw2tprkfH6ndEhyxa;Connect Timeout = 30; Encrypt=True;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False";

        public static string ConnectionString
        {
            get { return _connectionStringSimply; }
        }

    }
}
