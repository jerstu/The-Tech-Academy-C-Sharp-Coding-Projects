using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechAcademyInsurance.Models;

namespace TechAcademyInsurance.Controllers
{
    public class AdminController : Controller
    {
        private string _connectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Insurance; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // GET: Admin
        public ActionResult Index()
        {
            // This method dumps the entire DB into one object and passes it to the view
            // Normally this would be bad, but there aren't many records
            // If there were a ton of records, it would be better to do batches

            // Admin is a list of customers and a list of quotes
            Admin admin = new Admin();

            // Get all customers into Admin object
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string queryString = "Select * From Customers";
                SqlCommand command = new SqlCommand(queryString, connection);   
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Customer customer = new Customer();
                    customer.ID = Convert.ToInt32(reader["ID"]);
                    customer.FirstName = reader["FirstName"].ToString();
                    customer.LastName = reader["LastName"].ToString();
                    customer.Email = reader["Email"].ToString();
                    customer.DOB = Convert.ToDateTime(reader["DOB"]);
                    admin.Customers.Add(customer);
                }
                connection.Close();
            }

            // Get all quotes into Admin object
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string queryString = "Select * From Quotes";
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Quote quote = new Quote();
                    quote.QuoteID = Convert.ToInt32(reader["QuoteID"]);
                    quote.CustomerID = Convert.ToInt32(reader["CustomerID"]);
                    quote.QuoteDateTime = Convert.ToDateTime(reader["QuoteDateTIme"]);
                    quote.CarYear = Convert.ToInt32(reader["CarYear"]);
                    quote.CarMake = reader["CarMake"].ToString();
                    quote.CarModel = reader["CarModel"].ToString();
                    quote.DUI = Convert.ToBoolean(reader["DUI"]);
                    quote.Tickets = Convert.ToInt32(reader["Tickets"]);
                    quote.FullCoverage = Convert.ToBoolean(reader["FullCoverage"]);
                    quote.QuotePrice = Convert.ToInt32(reader["QuotePrice"]);
                    admin.Quotes.Add(quote);
                    
                }
                connection.Close();
            }

            return View(admin);
        }
    }
}