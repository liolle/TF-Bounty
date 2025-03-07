using api.CQS;
using api.database;
using api.database.entities;
using Microsoft.Data.SqlClient;

namespace api.services;

public partial class UserService(IDataContext context, IJwtService jwt, HttpClient httpClient, IConfiguration configuration) : IUserService
{

}

// Commands 
public partial class UserService
{
    public CommandResult Execute(CreateUserCommand command)
    {
        try
        {
            using SqlConnection conn = context.CreateConnection();

            string query = $@"
            INSERT INTO [Users]([oid])
            VALUES(@Oid)
            ";

            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Oid", command.Oid);
            conn.Open();

            int result = cmd.ExecuteNonQuery();
            if (result < 1)
            {
                return ICommandResult.Failure("User insertion failed.");
            }

            return ICommandResult.Success();
        }
        catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
        {
            return ICommandResult.Failure("User already exists."); 
        }
        catch (Exception)
        {
            return ICommandResult.Failure("Server error");
        }
    }
}


// Queries
public partial class UserService
{
    QueryResult<UserEntity> IQueryHandler<GetUserFromOidQuery, UserEntity>.Execute(GetUserFromOidQuery query)
    {
        using SqlConnection conn = context.CreateConnection();

        string sql_query = $@"
        SELECT * FROM [Users]
        WHERE [oid] = @Oid
        ";

        using SqlCommand cmd = new(sql_query, conn);
        cmd.Parameters.AddWithValue("@Oid", query.Oid);

        using SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            UserEntity user = UserEntity.Create(
                (int)reader["id"],
                (string)reader["oid"]
            );
            return IQueryResult<UserEntity>.Success(user);
        }

        return IQueryResult<UserEntity>.Failure("Unknown user oid");
    }
}