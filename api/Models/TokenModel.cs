using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using api.exceptions;
using api.services;

namespace api.models;

public class MicrosoftTokenModel(
    string token_type,
    int expires_in,
    int ext_expires_in,
    string access_token,
    string refresh_token = "",
    string id_token = "",
    string scope = "")
{
    public string Token_Type { get; } = token_type;
    public int Expires_In { get; } = expires_in;
    public int Ext_Expires_In { get; } = ext_expires_in;
    public string Access_Token { get; } = access_token;
    public string Refresh_Token { get; } = refresh_token;
    public string Id_Token { get; } = id_token;
    public string Scope { get; } = scope;


    public List<Claim> GetClaims(UserService userService)
    {
        var handler = new JwtSecurityTokenHandler();

        if (Access_Token is null) { throw new MalformedInputException("Microsoft Access_Token"); }
        if (Id_Token is null) { throw new MalformedInputException("Microsoft Id_Token"); }

        JwtSecurityToken a_token = handler.ReadJwtToken(Access_Token);
        JwtSecurityToken id_token = handler.ReadJwtToken(Id_Token);

        string oid = a_token.Claims.FirstOrDefault(c => c.Type == "oid")?.Value ?? throw new MalformedInputException("Microsoft Access_Token");
        IEnumerable<Claim>roles = id_token.Claims.Where(val=>val.Type =="roles");

        List<Claim> claims = [
            new("oid",oid)
        ];

        foreach (Claim item in roles)
        {
            claims.Add(item);
        }

        return claims;
    }
}