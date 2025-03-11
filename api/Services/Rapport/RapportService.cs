using api.CQS;
using api.database;
using api.database.entities;
using Microsoft.Data.SqlClient;

namespace api.services;


public partial class RapportService(IDataContext context, IUserService userService) : IRapportService
{

}


// Command
public partial class RapportService
{
    public CommandResult Execute(CreateRapportCommand command)
    {
        GetUserFromOidQuery q = new(command.Creator);
        QueryResult<UserEntity> r = userService.Execute(q);
        if (!r.IsSuccess)
        {
            return ICommandResult.Failure("Unknown user");
        }

        try
        {
            using SqlConnection conn = context.CreateConnection();

            string query = $@"
            INSERT INTO [Rapports]([creator],[programId],[content])
            VALUES(@Id,@programId,@Content)
            ";

            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Id", r.Result.Id);
            cmd.Parameters.AddWithValue("@ProgramId", command.ProgramId);
            cmd.Parameters.AddWithValue("@Content", command.Content);
            conn.Open();

            int result = cmd.ExecuteNonQuery();
            if (result < 1)
            {
                return ICommandResult.Failure("Rapport insertion failed.");
            }

            return ICommandResult.Success();
        }

        catch (Exception e)
        {
            return ICommandResult.Failure("Server error");
        }
    }
}


// Query
public partial class RapportService
{

}