using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace betacomio.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly DataContext2 _context2;
        private readonly AdventureWorksLt2019Context _adventure;
        public AuthRepository(DataContext context, DataContext2 context2 ,IConfiguration configuration, AdventureWorksLt2019Context adventure)
        {
            _adventure = adventure;
            _context2 = context2;
            _configuration = configuration;
            _context = context;
        }

        public async Task<ServiceResponse<string>> Login(string username, string password)
{
    var response = new ServiceResponse<string>();
    var customer = await _adventure.Customers.FirstOrDefaultAsync(x => x.EmailAddress.ToLower().Equals(username.ToLower()));
    var userCred = await _context2.UsersCred.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));

    if (customer is null && userCred is null)
    {
        response.Success = false;
        response.Message = "User not found.";
    }
    else if (customer is not null && userCred is not null)
    {
        response.Success = true;
        response.Message = "Login successful.";
        response.Data = CreateToken(userCred);
    }
    else if (customer is null && userCred is not null)
    {
        if (VerifyPasswordHash(password, userCred.PasswordHash, userCred.PasswordSalt))
        {
            response.Success = true;
            response.Message = "Login successful.";
            response.Data = CreateToken(userCred);
        }
        else
        {
            response.Success = false;
            response.Message = "Wrong password";
        }
    }

    return response;
}

        public async Task<ServiceResponse<int>> Register(UserCred userCred, string password, User user)
{
    var response = new ServiceResponse<int>();
    if (await UserExists(userCred.Username))
    {
        response.Success = false;
        response.Message = "User already exists";
        return response;
    }

    CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
    userCred.PasswordHash = passwordHash;
    userCred.PasswordSalt = passwordSalt;

    // Aggiungi l'oggetto `userCred` alla tabella `UsersCred` nel database
    _context2.UsersCred.Add(userCred);
    await _context2.SaveChangesAsync();

    // Ottieni l'Id generato per l'oggetto `userCred`
    var generatedId = userCred.Id;

    // Assegna l'Id generato a `user.Id`
    user.Id = generatedId;

    // Ottieni l'Username generato per l'oggetto `userCred`
    var generatedUsername = userCred.Username;

    // Assegna l'Username generato a `user.Username`
    user.Username = generatedUsername;

    // Aggiungi l'oggetto `user` alla tabella `Users` nel database
    _context.Users.Add(user);
    
    // Salva i cambiamenti nel contesto `_context`
    await _context.SaveChangesAsync();

    response.Data = generatedId;
    return response;
}


        public async Task<bool> UserExists(string username)
        {
            if(await _context2.UsersCred.AnyAsync(u => u.Username.ToLower()==username.ToLower()))
            {
                return true;
            }
            return false;
        }

         private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(UserCred userCred)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userCred.Id.ToString()),
                new Claim(ClaimTypes.Name, userCred.Username)
            };

//questa è la chiave segreta dal nostro file JSON delle impostazioni dell'app.
            var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;

//Controlliamo anche se questa cosa è nulla.
            if (appSettingsToken is null)
            {
                throw new Exception("AppSettings token is null!");
            }

            //istanza della classe di sicurezza simmetrica con la chiave segreta dell'appsetting
            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsToken));

            //con la chiave creiamo nuove credenziali di firma
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //questo oggetto ottiene le informazioni utilizzate per creare il token finale
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //attestazioni
                Subject = new ClaimsIdentity(claims),
                
                //data di scadenza 1gg
                Expires = DateTime.Now.AddDays(1),

                //credenziali di firma sull'oggetto creds
                SigningCredentials = creds
            };

            //gestore di token di sicurezza jwt
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            //
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            //serializzazione del token in un token jwt
            return tokenHandler.WriteToken(token);
        }
    }
}