namespace betacomio.Data
{
    // Classe DataContext2 che estende DbContext per fornire il contesto del secondo database
    public class DataContext2 : DbContext
    {
        // Costruttore della classe che richiede le opzioni di configurazione del contesto
        public DataContext2(DbContextOptions<DataContext2> options) : base(options)
        {

        }

        // DbSet per accedere alla tabella "UsersCred" nel secondo database
        public DbSet<UserCred> UsersCred => Set<UserCred>();
    }
}
