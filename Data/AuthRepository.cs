
namespace betacomio.Data
{
    // Implementa l'interfaccia IAuthRepository per fornire i servizi di autenticazione e registrazione
    public class AuthRepository : IAuthRepository
    {
        // Dichiarazione delle dipendenze utilizzate all'interno della classe
        private readonly DataContext _context; // Oggetto per accedere al contesto del database DataContext
        private readonly IConfiguration _configuration; // Oggetto per accedere alle configurazioni dell'applicazione
        private readonly DataContext2 _context2; // Oggetto per accedere al contesto del database DataContext2
        private readonly AdventureWorksLt2019Context _adventure; // Oggetto per accedere al contesto del database AdventureWorksLT2019

        // Costruttore della classe, viene utilizzato per iniettare le dipendenze necessarie
        public AuthRepository(DataContext context, DataContext2 context2, IConfiguration configuration, AdventureWorksLt2019Context adventure)
        {
            _adventure = adventure; // Inizializza l'oggetto per accedere al contesto del database AdventureWorksLT2019
            _context2 = context2; // Inizializza l'oggetto per accedere al contesto del database DataContext2
            _configuration = configuration; // Inizializza l'oggetto per accedere alle configurazioni dell'applicazione
            _context = context; // Inizializza l'oggetto per accedere al contesto del database DataContext
        }

        // Metodo per eseguire l'autenticazione dell'utente e restituire un token JWT in caso di successo
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            // Creazione dell'oggetto di risposta del servizio
            var response = new ServiceResponse<string>();

            // Cerca il cliente corrispondente all'username nel database AdventureWorksLT2019
            var customer = await _adventure.Customers.FirstOrDefaultAsync(x => x.EmailAddress.ToLower().Equals(username.ToLower()));

            // Cerca le credenziali utente corrispondenti all'username nel database DataContext2
            var userCred = await _context2.UsersCred.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));

            // Verifica se l'utente esiste sia come cliente che come utente del sistema
            if (customer is null && userCred is null)
            {
                // Se l'utente non esiste né come cliente né come utente del sistema, restituisce un messaggio di errore
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (customer is not null && userCred is not null)
            {
                // Se l'utente esiste sia come cliente che come utente del sistema, esegue il login e restituisce un token JWT
                response.Success = true;
                response.Message = "Login successful.";
                response.Data = CreateToken(userCred);
            }
            else if (customer is null && userCred is not null)
            {
                // Se l'utente esiste solo come utente del sistema, verifica la password fornita
                if (VerifyPasswordHash(password, userCred.PasswordHash, userCred.PasswordSalt))
                {
                    // Se la password è corretta, esegue il login e restituisce un token JWT
                    response.Success = true;
                    response.Message = "Login successful.";
                    response.Data = CreateToken(userCred);
                }
                else
                {
                    // Se la password non è corretta, restituisce un messaggio di errore
                    response.Success = false;
                    response.Message = "Wrong password";
                }
            }
            // Restituisce l'oggetto di risposta del servizio contenente il risultato dell'autenticazione
            return response;
        }

        // Metodo per registrare un nuovo utente e restituire il suo ID
        public async Task<ServiceResponse<int>> Register(UserCred userCred, string password, User user)
        {
            // Creazione dell'oggetto di risposta del servizio
            var response = new ServiceResponse<int>();

            // Verifica se l'utente esiste già nel sistema controllando le credenziali utente
            if (await UserExists(userCred.Username))
            {
                // Se l'utente esiste già, restituisce un messaggio di errore
                response.Success = false;
                response.Message = "User already exists";
                return response;
            }

            // Crea l'hash della password fornita dall'utente
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            userCred.PasswordHash = passwordHash;
            userCred.PasswordSalt = passwordSalt;

            // Aggiungi le credenziali utente alla tabella UsersCred nel database DataContext2
            _context2.UsersCred.Add(userCred);
            await _context2.SaveChangesAsync();

            // Ottieni l'ID generato per le credenziali utente
            var generatedId = userCred.Id;

            // Assegna l'ID generato all'ID dell'utente nel sistema
            user.Id = generatedId;

            // Ottieni l'username generato per le credenziali utente
            var generatedUsername = userCred.Username;

            // Assegna l'username generato all'username dell'utente nel sistema
            user.Username = generatedUsername;

            // Aggiungi l'utente alla tabella Users nel database DataContext
            _context.Users.Add(user);

            // Salva i cambiamenti nel contesto DataContext
            await _context.SaveChangesAsync();

            // Imposta il dato di risposta con l'ID generato
            response.Data = generatedId;
            return response;
        }

        // Metodo per verificare se l'utente esiste nel sistema controllando le credenziali utente
        public async Task<bool> UserExists(string username)
        {
            return await _context2.UsersCred.AnyAsync(u => u.Username.ToLower() == username.ToLower());
        }

        // Metodo privato per creare l'hash della password utilizzando HMACSHA512
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        // Metodo privato per verificare la corrispondenza dell'hash della password fornita con l'hash nel database
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        // Metodo privato per creare un token JWT per l'utente
        private string CreateToken(UserCred userCred)
        {
            // Creazione delle attestazioni (claims) per il token JWT
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userCred.Id.ToString()),
                new Claim(ClaimTypes.Name, userCred.Username),
                new Claim("IsAdmin", userCred.IsAdmin.ToString())
                
            };

            // Ottiene la chiave segreta dal file JSON delle impostazioni dell'applicazione
            var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;

            // Controlla se la chiave segreta è nulla
            if (appSettingsToken is null)
            {
                throw new Exception("AppSettings token is null!");
            }

            // Crea una nuova chiave di sicurezza simmetrica con la chiave segreta ottenuta dall'appSettings
            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsToken));

            // Crea le credenziali di firma per il token JWT utilizzando la chiave
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Crea un oggetto SecurityTokenDescriptor con le informazioni utilizzate per creare il token finale
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), // Attestazioni (claims) da includere nel token
                Expires = DateTime.Now.AddDays(1), // Data di scadenza del token (1 giorno da adesso)
                SigningCredentials = creds // Credenziali di firma per il token
            };

            // Crea un gestore di token di sicurezza JWT
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            // Crea il token JWT utilizzando le informazioni fornite nel tokenDescriptor
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            // Serializza il token in una stringa JWT
            return tokenHandler.WriteToken(token);
        }
    }
}
