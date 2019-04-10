using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityCodeFirst
{
    public class Quote
    {
        public int QuoteID { get; set; }
        public int CustomerID { get; set; }
        public DateTime QuoteDateTime { get; set; }
        public int CarYear { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public bool DUI { get; set; }
        public int Tickets { get; set; }
        public bool FullCoverage { get; set; }
        public float QuotePrice { get; set; }
        public string Factors { get; set; }

        public virtual Customer Customer { get; set; }

        public Quote()
        {

        }

        public void NewQuote(Customer customer)
        {
            // Gather Quote Info
            Console.Write("Car Year: ");
            CarYear = Convert.ToInt32(Console.ReadLine());
            Console.Write("Car Make: ");
            CarMake = Console.ReadLine();
            Console.Write("Car Model: ");
            CarModel = Console.ReadLine();
            Console.Write("Have you ever had a dui? true/false: ");
            DUI = Convert.ToBoolean(Console.ReadLine());
            Console.Write("How many speeding tickets have you had? ");
            Tickets = Convert.ToInt32(Console.ReadLine());
            Console.Write("Would you like full coverage? true/false: ");
            FullCoverage = Convert.ToBoolean(Console.ReadLine());
            QuoteDateTime = DateTime.Now;
            CustomerID = customer.ID;
            BuildQuote(customer);
        }

        public void BuildQuote(Customer customer)
        {
            QuotePrice = 50;
            Factors = "Base Price: $50";
            if (Age(customer.DOB) < 18)
            {
                QuotePrice += 100;
                Factors += "\nUnder 18 Years of Age: $100";
            }
            else if (Age(customer.DOB) < 25)
            {
                QuotePrice += 25;
                Factors += "\nUnder 25 Years of Age: $25";
            }

            if (Age(customer.DOB) > 100)
            {
                QuotePrice += 25;
                Factors += "\nOver 100 years of Age: $25";
            }
            if (CarYear < 2000)
            {
                QuotePrice += 25;
                Factors += "\nVehicle made before 2000: $25";
            }
            if (CarYear > 2015)
            {
                QuotePrice += 25;
                Factors += "\nVehicle made after 2015: $25";
            }
            if (CarMake == "Porsche")
            {
                QuotePrice += 25;
                Factors += "\nEuropean sports car penalty: $25";
                if (CarModel == "Carrera 911")
                {
                    QuotePrice += 25;
                    Factors += "\nHigh performance model: $25";
                }
            }
            if (Tickets > 0)
            {
                QuotePrice += (Tickets * 10);
                Factors += "\nSpeeding tickets: $" + Tickets * 10;
            }
            if (DUI)
            {
                Factors += "\nDUI: $" + ((QuotePrice * 1.25f) - QuotePrice);
                QuotePrice = (QuotePrice * 1.25f);
            }
            if (FullCoverage)
            {
                Factors += "\nFull Coverage: $" + ((QuotePrice * 1.5f) - QuotePrice);
                QuotePrice = QuotePrice * 1.5f;
            }
        }

        private static int Age(DateTime dob)
        {
            int years = DateTime.Now.Year - dob.Year;

            if ((dob.Month > DateTime.Now.Month) || (dob.Month == DateTime.Now.Month && dob.Day > DateTime.Now.Day))
                years--;

            return years;
        }

        public void ViewQuote(Customer Customer)
        {
            using (var ctx = new InsuranceContext())
            {
                
            }
            Console.WriteLine("Quote # {0} issued on {1} at {2}", QuoteID, QuoteDateTime.Date.ToShortDateString(), QuoteDateTime.ToShortTimeString());
            Console.WriteLine("{0} {1} {2}", CarYear, CarMake, CarModel);
            Console.WriteLine("Cust ID: {0} {1} {2}\n\t{3} {4}", Customer.ID, Customer.FirstName, Customer.LastName,
                                Customer.Email, Customer.DOB.ToShortDateString());
            if (DUI) Console.WriteLine("Has DUI!");
            if (Tickets > 0) Console.WriteLine("Speeding Tickets: {0}", Tickets);
            Console.WriteLine(Factors);
            Console.WriteLine("Monthy Premium: ${0}", QuotePrice);
        }

    }




}
