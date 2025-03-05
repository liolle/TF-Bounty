using Microsoft.Data.SqlClient;

namespace api.database;

public interface IDataContext {
    SqlConnection CreateConnection();
}