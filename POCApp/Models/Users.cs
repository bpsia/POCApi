using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCApp.Models
{
    public class Users
    {
        public Users()
        {

        }
        public int Id { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public string Role { get; set; }
    }
    public class Customers
    {
        public int Id { get; set; }

        public string CustomerNumber { get; set; }

        public string CustomerName { get; set; }

        public string CustomerType { get; set; }

        public double TotalAmountOfSales { get; set; }

        public string Timestamp { get; set; }
    }
}
