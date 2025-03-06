using api.CQS;

namespace api.services;


public interface IUserService :
    ICommandHandler<RegistersUserCommand>,
    IQueryHandlerAsync<OauthMicrosoftQuery,string>
{
    
}
