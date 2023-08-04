namespace betacomio.Controllers
{
    [ApiController] // Attributo che indica che questo è un controller API
    [Route("[controller]")] // Attributo per specificare il percorso di base delle richieste per questo controller, in questo caso, il nome del controller viene usato come percorso
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;

        // Costruttore della classe che richiede una dipendenza dell'interfaccia IAuthRepository
        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        // Metodo per registrare un nuovo utente
        // Metodo HTTP: POST
        // Percorso: /Auth/Register
        [HttpPost("Register")] // Attributo per specificare il percorso dell'endpoint di questo metodo
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserCredRegisterDto request)
        {
            // Chiama il metodo Register dell'IAuthRepository per gestire la registrazione del nuovo utente
            var response = await _authRepo.Register(
                new UserCred { Username = request.Username }, request.Password, new User { }
            );

            // Se la registrazione non ha avuto successo, restituisce una risposta HTTP con lo status 400 (BadRequest) e i dettagli dell'errore
            if (!response.Success)
            {
                return BadRequest(response);
            }

            // Se la registrazione ha avuto successo, restituisce una risposta HTTP con lo status 200 (OK) e l'ID del nuovo utente registrato
            return Ok(response);
        }

        // Metodo per effettuare il login di un utente esistente
        // Metodo HTTP: POST
        // Percorso: /Auth/Login
        [HttpPost("Login")] // Attributo per specificare il percorso dell'endpoint di questo metodo
        public async Task<ActionResult<ServiceResponse<int>>> Login(UserCredLoginDto request)
        {
            // Chiama il metodo Login dell'IAuthRepository per gestire l'autenticazione dell'utente
            var response = await _authRepo.Login(request.Username, request.Password);

            // Se l'autenticazione non ha avuto successo, restituisce una risposta HTTP con lo status 400 (BadRequest) e i dettagli dell'errore
            if (!response.Success)
            {
                return BadRequest(response);
            }

            // Se l'autenticazione ha avuto successo, restituisce una risposta HTTP con lo status 200 (OK) e il token di accesso generato per l'utente autenticato
            return Ok(response);
        }
    }
}
