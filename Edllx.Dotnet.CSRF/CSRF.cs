using System.Security.Cryptography;
using System.Text;

namespace edllx.dotnet.csrf;

public class CSRFService
{
  private readonly byte[] _secretKey;
  private readonly string _tokenName;
  private readonly string _cookieName;

  internal string CookieName {get{return _cookieName;}}
  internal string TokenName {get{return _tokenName;}}

  public CSRFService(IConfiguration config)
  {
    _secretKey = Convert.FromBase64String(config["ANTIFORGERY_SECRET_KEY"]??throw new Exception("Missing configuration ANTIFORGERY_SECRET_KEY"));
    _tokenName = config["CSRF_HEADER_NAME"]??throw new Exception("Missing configuration CSRF_HEADER_NAME");
    _cookieName = config["CSRF_COOKIE_NAME"]??throw new Exception("Missing configuration CSRF_COOKIE_NAME");
  }

  public (string CookieToken, string RequestToken) GenerateTokens()
  {
    string cookieToken = GenerateRandomToken();
    string requestToken = ComputeHmac(cookieToken);
    return (cookieToken, requestToken);
  }


  internal bool ValidateTokens(string cookieToken, string requestToken)
  {
    if (string.IsNullOrEmpty(cookieToken) || string.IsNullOrEmpty(requestToken)){return false;}

    string expectedToken = ComputeHmac(cookieToken);
    return requestToken == expectedToken;
  }

  private string GenerateRandomToken()
  {
    byte[] bytes = new byte[32];
    using RandomNumberGenerator rng = RandomNumberGenerator.Create();
    rng.GetBytes(bytes);
    return Convert.ToBase64String(bytes);
  }

  internal string ComputeHmac(string input)
  {
    using HMACSHA256 hmac = new HMACSHA256(_secretKey);
    byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
    return Convert.ToBase64String(hash);
  }
}

// Attribure
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireCSRF : Attribute { }
