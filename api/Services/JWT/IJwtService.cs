using System.Security.Claims;

namespace api.services;

public interface IJwtService
{
    public string Generate(List<Claim> claims);
}