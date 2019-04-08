using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechAcademyInsurance.Models
{
    public class Admin
    {
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public List<Quote> Quotes { get; set; } = new List<Quote>();
    }
}