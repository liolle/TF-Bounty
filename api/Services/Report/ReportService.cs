using api.CQS;
using api.database;
using api.database.entities;
using Microsoft.Data.SqlClient;

namespace api.services;


public partial class ReportService(IDataContext context, IUserService userService) : IReportService
{

}


// Command
public partial class ReportService
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
            INSERT INTO [Rapports]([title],[creator],[programId],[content])
            VALUES(@Title,@Id,@programId,@Content)
            ";

            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Title", command.Title);
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

        catch (Exception)
        {
            return ICommandResult.Failure("Server error");
        }
    }

    public CommandResult Execute(ValidateReportCommand command)
    {
        try
        {
            using SqlConnection conn = context.CreateConnection();

            string query = $@"
            UPDATE [Rapports]
            SET [status] = @Status
            WHERE [id] = @Id
            ";

            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Status", command.State);
            cmd.Parameters.AddWithValue("@Id", command.ReportId);
            conn.Open();

            int result = cmd.ExecuteNonQuery();
            if (result < 1)
            {
                return ICommandResult.Failure("Rapport Update failed.");
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
public partial class ReportService
{
    public QueryResult<List<ReportEntity>> Execute(GetUserReportQuery query)
    {
        GetUserFromOidQuery q = new(query.Oid);
        QueryResult<UserEntity> r = userService.Execute(q);
        if (!r.IsSuccess)
        {
            return IQueryResult<List<ReportEntity>>.Failure("Unknown user");
        }

        try
        {
            using SqlConnection conn = context.CreateConnection();
            List<ReportEntity> reports = [];

            string sql_query = $@"
            SELECT 
                *
            FROM [Rapports] u
            WHERE [creator] = @CreatorId
            ";

            using SqlCommand cmd = new(sql_query, conn);
            cmd.Parameters.AddWithValue("@CreatorId", r.Result.Id);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ReportEntity report = ReportEntity.Create(
                    (int)reader["id"],
                    (int)reader["creator"],
                    (int)reader["programId"],
                    (string)reader["title"],
                    (string)reader["content"],
                    (string)reader["status"],
                    (DateTime)reader["createdAt"]
                );
                reports.Add(report);
            }

            return IQueryResult<List<ReportEntity>>.Success(reports);
        }
        catch (Exception)
        {
            return IQueryResult<List<ReportEntity>>.Failure("Server error");
        }
    }

    public QueryResult<List<ReportEntity>> Execute(GetPendingReportQuery query)
    {
        try
        {
            using SqlConnection conn = context.CreateConnection();
            List<ReportEntity> reports = [];

            string sql_query = $@"
            SELECT 
                *
            FROM [Rapports] u
            WHERE [status] = 'pending'
            ";

            using SqlCommand cmd = new(sql_query, conn);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ReportEntity report = ReportEntity.Create(
                    (int)reader["id"],
                    (int)reader["creator"],
                    (int)reader["programId"],
                    (string)reader["title"],
                    (string)reader["content"],
                    (string)reader["status"],
                    (DateTime)reader["createdAt"]
                );
                reports.Add(report);
            }

            return IQueryResult<List<ReportEntity>>.Success(reports);
        }
        catch (Exception)
        {
            return IQueryResult<List<ReportEntity>>.Failure("Server error");
        }
    }
}