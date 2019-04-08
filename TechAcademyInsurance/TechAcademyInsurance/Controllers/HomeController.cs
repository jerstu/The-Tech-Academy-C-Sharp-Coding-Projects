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
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Insurance; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StartQuote(string firstName, string lastName, string email, DateTime dob)
        {
            Customer customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                DOB = dob,
                ID = 0

            };

            // CHECK IF NEW CUSTOMER BASED ON EMAIL
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string queryString = @"SELECT ID FROM Customers WHERE Email = @email";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@email", SqlDbType.VarChar);
                command.Parameters["@email"].Value = customer.Email;
                connection.Open();
                // Executes Query and returns ID of existing customer
                // If no results return customer.ID remains 0
                customer.ID = Convert.ToInt32(command.ExecuteScalar());
            }


            // If the previous query didn't set ID as nonzero
            // We have a new customer to add to the DB
            if (customer.ID == 0)
            {
                // ADD CUSTOMER TO DATABASE AND GET CUSTOMER ID BACK
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string queryString = @"Insert into Customers (FirstName, LastName, Email, DOB) VALUES (@FirstName, @LastName, @Email, @DOB); SELECT SCOPE_IDENTITY()";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add("@FirstName", SqlDbType.VarChar);
                    command.Parameters.Add("@LastName", SqlDbType.VarChar);
                    command.Parameters.Add("@Email", SqlDbType.VarChar);
                    command.Parameters.Add("@DOB", SqlDbType.DateTime);

                    command.Parameters["@FirstName"].Value = customer.FirstName;
                    command.Parameters["@LastName"].Value = customer.LastName;
                    command.Parameters["@Email"].Value = customer.Email;
                    command.Parameters["@DOB"].Value = customer.DOB;

                    connection.Open();

                    // Executes Insert AND returns ID field of new record                            
                    customer.ID = Convert.ToInt32(command.ExecuteScalar());

                    connection.Close();
                }
            }
            return View("StartQuote", customer);
        }
    


        [HttpPost]
        public ActionResult GetQuote(int customerID, DateTime dob, bool dui, int tickets, int year, string make, string model, bool fullCoverage)
        {
            // CustomerID and DOB are passed from StartQuote to View to GetQuote
            // These are the only Customer props that a Quote needs
            // This avoids storing some kind of session on server
            // Bad practice, but works

            
            Quote quote = new Quote()
            {
                CustomerID = customerID,
                QuoteDateTime = DateTime.Now,
                CarYear = year,
                CarMake = make,
                CarModel = model,
                DUI = dui,
                Tickets = tickets,
                FullCoverage = fullCoverage,
            };

            // after quote object is created, BuildQuote() takes in the DOB and calculates the quote
            quote.BuildQuote(dob);

            // Add the Quote to the DB
            string queryString = @"Insert into Quotes (CustomerID, QuoteDateTime, CarYear, CarMake, CarModel, DUI, Tickets, FullCoverage, QuotePrice) VALUES (@CustomerID, @QuoteDateTime, @CarYear, @CarMake, @CarModel, @DUI, @Tickets, @FullCoverage, @QuotePrice)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@CustomerID", SqlDbType.Int);
                command.Parameters.Add("@QuoteDateTime", SqlDbType.DateTime);
                command.Parameters.Add("@CarYear", SqlDbType.Int);
                command.Parameters.Add("@CarMake", SqlDbType.VarChar);
                command.Parameters.Add("@CarModel", SqlDbType.VarChar);
                command.Parameters.Add("@DUI", SqlDbType.Bit); 
                command.Parameters.Add("@Tickets", SqlDbType.Int);
                command.Parameters.Add("@FullCoverage", SqlDbType.Bit);
                command.Parameters.Add("@QuotePrice", SqlDbType.Float);

                command.Parameters["@CustomerID"].Value = quote.CustomerID;
                command.Parameters["@QuoteDateTIme"].Value = quote.QuoteDateTime;
                command.Parameters["@CarYear"].Value = quote.CarYear;
                command.Parameters["@CarMake"].Value = quote.CarMake;
                command.Parameters["@CarModel"].Value = quote.CarModel;
                command.Parameters["@DUI"].Value = quote.DUI;
                command.Parameters["@Tickets"].Value = quote.Tickets;
                command.Parameters["@FullCoverage"].Value = quote.FullCoverage;
                command.Parameters["@QuotePrice"].Value = quote.QuotePrice;                

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            return View("ShowQuote", quote);          
        }
    }
}