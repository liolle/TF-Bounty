using api.CQS;
using api.database.entities;

namespace api.services;


public interface IUserService :
    ICommandHandler<CreateUserCommand>,
    IQueryHandler<GetUserFromOidQuery,UserEntity>,
    IQueryHandlerAsync<OauthMicrosoftQuery,string>
{
    
}
