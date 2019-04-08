using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechAcademyInsurance.Models
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
        public List<string> Factors { get; set; } = new List<string>();

        public void BuildQuote(DateTime dob)
        {
            QuotePrice = 50;
            Factors.Add("Base Price: $50");
            if (Age(dob) < 18)
            {
                QuotePrice += 100;
                Factors.Add("Under 18 Years of Age: $100");
            }
            else if (Age(dob) < 25)
            {
                QuotePrice += 25;
                Factors.Add("Under 25 Years of Age: $25");
            }
            
            if (Age(dob) > 100)
            {
                QuotePrice += 25;
                Factors.Add("Over 100 years of Age: $25");
            }
            if (CarYear < 2000)
            {
                QuotePrice += 25;
                Factors.Add("Vehicle made before 2000: $25");
            }
            if (CarYear > 2015)
            {
                QuotePrice += 25;
                Factors.Add("Vehicle made after 2015: $25");
            }
            if (CarMake == "Porsche")
            {
                QuotePrice += 25;
                Factors.Add("European sports car penalty: $25");
                if (CarModel == "Carrera 911")
                {
                    QuotePrice += 25;
                    Factors.Add("High performance model: $25");
                }
            }
            if (Tickets > 0)
            {
                QuotePrice += (Tickets * 10);
                Factors.Add("Speeding tickets: $" + Tickets * 10);
            }
            if (DUI)
            {
                Factors.Add("DUI: $" + ((QuotePrice * 1.25f) - QuotePrice));
                QuotePrice = (QuotePrice * 1.25f);
            }
            if (FullCoverage)
            {
                Factors.Add("Full Coverage: $" + ((QuotePrice * 1.5f) - QuotePrice));
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

    }

    
}