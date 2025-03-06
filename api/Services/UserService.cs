using api.CQS;
using api.database;
using Microsoft.Data.SqlClient;

namespace api.services;

public partial class UserService(IDataContext context, IJwtService jwt, HttpClient httpClient, IConfiguration configuration) : IUserService
{
    public CommandResult Execute(RegistersUserCommand command)
    {
        try
        {
            using SqlConnection conn = context.CreateConnection();
            return ICommandResult.Success();
        }
        catch (Exception e)
        {
            return ICommandResult.Failure("Server error");
        }
    }

}