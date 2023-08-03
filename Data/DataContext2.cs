using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace betacomio.Data
{
    public class DataContext2 : DbContext
    {
        public DataContext2(DbContextOptions<DataContext2> options) : base(options)
        {

        }
        
        public DbSet<UserCred> UsersCred => Set<UserCred>();
    }
}