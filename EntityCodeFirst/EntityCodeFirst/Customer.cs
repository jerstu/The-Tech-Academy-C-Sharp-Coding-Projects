using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityCodeFirst
{
    public class Customer
    {
        public int ID { get; set; } = 0;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }

        public bool BuildCustomer()
        {
            Console.Write("First Name: ");
            FirstName = Console.ReadLine();
            if (FirstName == "admin") return true;
            Console.Write("Last Name: ");
            LastName = Console.ReadLine();
            Console.Write("Email: ");
            Email = Console.ReadLine();
            Console.Write("DOB MM/DD/YYYY: ");
            DOB = Convert.ToDateTime(Console.ReadLine());

            return false;
        }
    }
}
