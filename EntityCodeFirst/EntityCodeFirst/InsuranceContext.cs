using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityCodeFirst
{
    public class InsuranceContext: DbContext
    {
        public InsuranceContext(): base()
        {
            Database.SetInitializer<InsuranceContext>(new DropCreateDatabaseIfModelChanges<InsuranceContext>());

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Quote> Quotes { get; set; }

    }
}
