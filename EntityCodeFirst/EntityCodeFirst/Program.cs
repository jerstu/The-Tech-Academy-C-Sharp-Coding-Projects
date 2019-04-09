using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityCodeFirst
{
    class Program
    {
        static void Main(string[] args)
        {

            Customer customer = new Customer();
            if (customer.BuildCustomer())
            {
                Admin();
                return;
            }
            // Check if customer exists
            // if query returns null, Add() new customer
            // else customer object is reassigned from DB
            using (var ctx = new InsuranceContext())
            {
                var customerQuery = ctx.Customers.FirstOrDefault(s => s.Email == customer.Email);
                if (customerQuery == null)
                {
                    ctx.Customers.Add(customer);
                    ctx.SaveChanges();
                }
                else
                {
                    customer = customerQuery;
                }
            }

            Console.WriteLine("Your Customer ID is {0}", customer.ID);

            Quote quote = new Quote();
            quote.NewQuote(customer);
            

            // Add quote to Database
            using (var ctx = new InsuranceContext())
            {
                ctx.Quotes.Add(quote);
                ctx.SaveChanges();
            }

            quote.ViewQuote(customer);
            Console.ReadLine();
        }

        public static void Admin()
        {
            using (var ctx = new InsuranceContext())
            {
                foreach (Quote quote in ctx.Quotes)
                {
                    var customer = ctx.Customers.Where(x => x.ID == quote.CustomerID).First();
                    quote.ViewQuote(customer);
                    Console.ReadLine();
                }

            }
        }

     

       
        
    }
}
