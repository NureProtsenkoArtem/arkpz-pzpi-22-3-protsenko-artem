//Код до рефакторингу:

public class TokenService(IOptions<JwtOptions> options) : ITokenService
{
    private readonly JwtOptions _options = options.Value;

    public Task<Tuple<string, string>> GenerateTokens(User user)
    {
        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken(user);

        return Task.FromResult(Tuple.Create(accessToken, refreshToken));
    }

    private string GenerateAccessToken(User user)
    {
        var claims = new[]
        {
            new Claim("UserId", user.UserId.ToString()),
            new Claim("type", "access"),
            new Claim("Email", user.Email),
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.AccessSecretKey)),
            SecurityAlgorithms.HmacSha256);

        var accessToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.Now.AddHours(_options.AccessExpiresDuration));

        return new JwtSecurityTokenHandler().WriteToken(accessToken);
    }

    private string GenerateRefreshToken(User user)
    {
        var claims = new[]
        {
            new Claim("UserId", user.UserId.ToString()),
            new Claim("type", "refresh"),
            new Claim("Email", user.Email),
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.RefreshSecretKey)),
            SecurityAlgorithms.HmacSha256);

        var refreshToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.Now.AddDays(_options.RefreshExpiresDuration));

        return new JwtSecurityTokenHandler().WriteToken(refreshToken);
    }

    public ClaimsPrincipal GetPrincipals(string refreshToken)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.RefreshSecretKey));

        var validation = new TokenValidationParameters
        {
            IssuerSigningKey = securityKey,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true
        };

        return new JwtSecurityTokenHandler().ValidateToken(refreshToken, validation, out _);
    }
}

// Код після рефакторингу:

public class TokenService(IJwtProvider jwtProvider) : ITokenService
{
   public Task<Tuple<string,string>> GenerateTokens(User user)
   {
      var accessToken = jwtProvider.GenerateAccessToken(user);
      var refreshToken = jwtProvider.GenerateRefreshToken(user);

      return Task.FromResult(Tuple.Create(accessToken, refreshToken));
   }
 
}

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
   private readonly JwtOptions _options = options.Value;
   
   public string GenerateAccessToken(User user)
   {
      var claims = new[]
      {
         new Claim("UserId", user.UserId.ToString()),
         new Claim("type", "access"),
         new Claim("Email", user.Email),

      }; 
      var signingCredentials = new SigningCredentials(  
         new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.AccessSecretKey)),  
         SecurityAlgorithms.HmacSha256);

      var accessToken = new JwtSecurityToken(
         claims: claims,
         signingCredentials: signingCredentials,
         expires: DateTime.Now.AddHours(_options.AccessExpiresDuration));
  
      var tokenValue = new JwtSecurityTokenHandler().WriteToken(accessToken);  
  
      return tokenValue;  
   }

   public string GenerateRefreshToken(User user)
   {
      var claims = new[]
      {
         new Claim("UserId", user.UserId.ToString()),
         new Claim("type", "refresh"),
         new Claim("Email", user.Email),
      };

      var signingCredentials = new SigningCredentials(
         new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.RefreshSecretKey)),
         SecurityAlgorithms.HmacSha256);

      var refreshToken = new JwtSecurityToken(
         claims: claims,
         signingCredentials: signingCredentials,
         expires: DateTime.Now.AddDays(_options.RefreshExpiresDuration));

      return new JwtSecurityTokenHandler().WriteToken(refreshToken);
   }

   public ClaimsPrincipal GetPrincipals(string refreshToken)
   {
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.RefreshSecretKey));  
        
      var validation = new TokenValidationParameters  
      {  
         IssuerSigningKey = securityKey,  
         ValidateIssuer = false,  
         ValidateAudience = false,  
         ValidateLifetime = false,  
         ValidateIssuerSigningKey = true  
      };  
  
      return new JwtSecurityTokenHandler().ValidateToken(refreshToken, validation, out _);  
   }
}