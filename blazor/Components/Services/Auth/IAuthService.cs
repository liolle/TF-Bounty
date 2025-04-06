namespace blazor.services;

public interface IAuthService
{
    Task AzureLogin(string code,string redirect_success_uri,string redirect_failure_uri);
    Task Logout();
}
