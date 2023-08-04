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
        public DbSet<AdminRequest> AdminRequest => Set<AdminRequest>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Definizione chiave primaria per la classe AdminRequest
            modelBuilder.Entity<AdminRequest>().HasKey(ar => ar.IdRequest);
            modelBuilder.Entity<AdminRequest>(entity =>
            {
                entity.ToTable("AdminRequest", tb => tb.HasTrigger("betacomio"));
            });
        }
        
    }
}
