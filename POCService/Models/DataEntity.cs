using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POCService.Models
{
    public class Customers
    {
        [Key]
        public int Id { get; set; }

        public int CustomerNumber { get; set; }

        public string CustomerName { get; set; }

        public string CustomerType { get; set; }

        public double TotalAmountOfSales { get; set; }

        public string Timestamp { get; set; }
    }
    public class Users
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public string Role { get; set; }
    }
    public class SalesCustomers
    {
        [Key]
        public int Id { get; set; }

        public int CustomerNumber { get; set; }

        public string CustomerName { get; set; }

        public string CustomerType { get; set; }

        public double TotalAmountOfSales { get; set; }

        public string Timestamp { get; set; }
    }
}