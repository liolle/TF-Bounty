namespace api.CQS;

public class OauthMicrosoftQuery(string code, string redirect_success_uri,string redirect_failure_uri ) : IQueryDefinition<Task<string>>
{
    public string Code { get; } = code;
    public string Redirect_Success_Uri { get; } = redirect_success_uri;
    public string Redirect_Failure_Uri { get; } = redirect_failure_uri;
}