using api.CQS;
using api.database;
using api.database.entities;
using Microsoft.Data.SqlClient;
namespace api.services;

public partial class ProgramService(IDataContext context, IUserService userService) : IProgramService
{

}

// Command
public partial class ProgramService
{
    public CommandResult Execute(CreateProgramCommand command)
    {
        GetUserFromOidQuery q = new(command.Creator);
        QueryResult<UserEntity> r = userService.Execute(q);
        try
        {

            if (!r.IsSuccess)
            {
                return ICommandResult.Failure("Unknown user");
            }

            using SqlConnection conn = context.CreateConnection();


            string query = $@"
            INSERT INTO [Programs]([creator],[logo],[reward],[title],[description])
            VALUES(@Id,@Logo,@Reward,@Title,@Description)
            ";

            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Id", r.Result.Id);
            cmd.Parameters.AddWithValue("@Logo", command.Logo);
            cmd.Parameters.AddWithValue("@Reward", command.Reward);
            cmd.Parameters.AddWithValue("@Title", command.Title);
            cmd.Parameters.AddWithValue("@Description", command.Description);
            conn.Open();

            int result = cmd.ExecuteNonQuery();
            if (result < 1)
            {
                return ICommandResult.Failure("User insertion failed.");
            }

            return ICommandResult.Success();
        }

        catch (Exception)
        {
            return ICommandResult.Failure("Server error");
        }
    }
}

// Query
public partial class ProgramService
{

    public QueryResult<List<ProgramEntity>> Execute(GetUserProgramsQuery query)
    {

        try
        {
            using SqlConnection conn = context.CreateConnection();
            List<ProgramEntity> programs = [];

            string sql_query = $@"
            SELECT 
                prog.id programId,
                u.id userId,
                prog.logo,
                prog.reward,
                prog.title,
                prog.description
            FROM [Users] u
            JOIN [Programs] prog ON u.id = prog.creator
            WHERE [oid] = @Oid
            ";

            using SqlCommand cmd = new(sql_query, conn);
            cmd.Parameters.AddWithValue("@Oid", query.Oid);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ProgramEntity program = ProgramEntity.Create(
                    (int)reader["programId"],
                    (int)reader["userId"],
                    (string)reader["logo"],
                    (int)reader["reward"],
                    (string)reader["title"],
                    (string)reader["description"]
                );
                programs.Add(program);
            }

            return IQueryResult<List<ProgramEntity>>.Success(programs);
        }
        catch (Exception)
        {
            return IQueryResult<List<ProgramEntity>>.Failure("Server error");
        }
    }

    public QueryResult<List<ProgramEntity>> Execute(GetProgramsQuery query)
    {
        try
        {
            using SqlConnection conn = context.CreateConnection();
            List<ProgramEntity> programs = [];

            string sql_query = $@"
            SELECT 
                prog.id programId,
                u.id userId,
                prog.logo,
                prog.reward,
                prog.title,
                prog.description
            FROM [Users] u
            JOIN [Programs] prog ON u.id = prog.creator
            {((query.Search is null) ? "" : "WHERE prog.title LIKE " + $"'%{query.Search}%'")}
            ";

            using SqlCommand cmd = new(sql_query, conn);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ProgramEntity program = ProgramEntity.Create(
                    (int)reader["programId"],
                    (int)reader["userId"],
                    (string)reader["logo"],
                    (int)reader["reward"],
                    (string)reader["title"],
                    (string)reader["description"]
                );
                programs.Add(program);
            }

            return IQueryResult<List<ProgramEntity>>.Success(programs);
        }
        catch (Exception e)
        {
            return IQueryResult<List<ProgramEntity>>.Failure("Server error");
        }
    }

    public QueryResult<ProgramEntity> Execute(GetProgramByIdQuery query)
    {
        try
        {
            using SqlConnection conn = context.CreateConnection();

            string sql_query = $@"
            SELECT 
                prog.id programId,
                u.id userId,
                prog.logo,
                prog.reward,
                prog.title,
                prog.description
            FROM [Users] u
            JOIN [Programs] prog ON u.id = prog.creator
            WHERE prog.id = @Id
            ";

            using SqlCommand cmd = new(sql_query, conn);
            cmd.Parameters.AddWithValue("@Id", query.Id);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ProgramEntity program = ProgramEntity.Create(
                    (int)reader["programId"],
                    (int)reader["userId"],
                    (string)reader["logo"],
                    (int)reader["reward"],
                    (string)reader["title"],
                    (string)reader["description"]
                );
                return IQueryResult<ProgramEntity>.Success(program);
            }
            return IQueryResult<ProgramEntity>.Failure("Program not found");
        }
        catch (Exception )
        {
            return IQueryResult<ProgramEntity>.Failure("Server error");
        }
    }
}