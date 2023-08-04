namespace betacomio.Data
{
    // Interfaccia IAuthRepository per la gestione dell'autenticazione e della registrazione
    public interface IAuthRepository
    {
        // Metodo per la registrazione di un nuovo utente
        // Parametri:
        //   - userCred: Le credenziali dell'utente (username e password)
        //   - password: La password dell'utente in chiaro
        //   - user: L'oggetto utente contenente altre informazioni (es. nome, email, ecc.)
        // Restituisce:
        //   - Un oggetto ServiceResponse contenente l'esito dell'operazione e l'Id dell'utente registrato
        Task<ServiceResponse<int>> Register(UserCred userCred, string password, User user);

        // Metodo per l'autenticazione dell'utente
        // Parametri:
        //   - username: Il nome utente dell'utente che sta effettuando il login
        //   - password: La password dell'utente in chiaro
        // Restituisce:
        //   - Un oggetto ServiceResponse contenente l'esito dell'operazione e un token JWT se il login è avvenuto con successo
        Task<ServiceResponse<string>> Login(string username, string password);

        // Metodo per verificare l'esistenza di un utente nel sistema
        // Parametri:
        //   - username: Il nome utente dell'utente da verificare
        // Restituisce:
        //   - Un valore booleano che indica se l'utente esiste o meno nel sistema
        Task<bool> UserExists(string username);
    }
}
