using api.CQS;
using api.database.entities;

namespace api.services;


public interface IUserService :
    ICommandHandler<CreateUserCommand>,
    IQueryHandlerAsync<OauthMicrosoftQuery,string>,
    IQueryHandler<GetUserFromOidQuery,UserEntity>
{
    
}
