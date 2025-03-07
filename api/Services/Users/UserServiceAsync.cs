using System.Security.Claims;
using api.CQS;
using api.exceptions;
using api.models;

namespace api.services;


// Async Commands

public partial class UserService
{

}


// Async Queries
public partial class UserService
{

    public async Task<QueryResult<string>> Execute(OauthMicrosoftQuery query)
    {

        try
        {
            string client_id = configuration["CLIENT_ID"] ?? throw new MissingConfigurationException("CLIENT_ID");
            string client_secret = configuration["CLIENT_SECRET"] ?? throw new MissingConfigurationException("CLIENT_SECRET");
            string tenant_id = configuration["TENANT_ID"] ?? throw new MissingConfigurationException("TENANT_ID");

            var tokenResponse = await httpClient.PostAsync(
                $"https://login.microsoftonline.com/{tenant_id}/oauth2/v2.0/token",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "client_id",  client_id},
                    { "client_secret", client_secret },
                    { "code", query.Code },
                    { "redirect_uri", query.Redirect_Success_Uri },
                    { "scope", "openid profile email" },
                    { "grant_type", "authorization_code" }
                })
            );


            MicrosoftTokenModel? res = await tokenResponse.Content.ReadFromJsonAsync<MicrosoftTokenModel>();


            if (res is null)
            {
                return IQueryResult<string>.Failure("Authentication failed");
            }

            List<Claim> claims =  res.GetClaims(this);
            string oid = claims.FirstOrDefault(c => c.Type == "oid")?.Value ?? throw new MalformedInputException("Auth generate claims");
            Execute(new CreateUserCommand(oid));
            return IQueryResult<string>.Success(jwt.Generate(claims));
        }
        catch (Exception )
        {
            return IQueryResult<string>.Failure("Server error");
        }
    }

}