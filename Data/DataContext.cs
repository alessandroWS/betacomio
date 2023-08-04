namespace betacomio.Data
{
    // Classe DataContext che estende DbContext per fornire il contesto del database
    public class DataContext : DbContext
    {
        // Costruttore della classe che richiede le opzioni di configurazione del contesto
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        // DbSet per accedere alla tabella "Orders" nel database
        public DbSet<Order> Orders => Set<Order>();

        // DbSet per accedere alla tabella "Users" nel database
        public DbSet<User> Users => Set<User>();
        public DbSet<AdminRequest> AdminRequests => Set<AdminRequest>();
    }
}
