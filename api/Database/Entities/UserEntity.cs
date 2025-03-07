namespace api.database.entities;

public class UserEntity
{
    public int Id { get; }
    public string Oid {get;}

    internal UserEntity(int id,string oid)
    {
        Id = id;
        Oid = oid;
    }

    public static UserEntity Create(int id, string oid)
    {
        return new UserEntity(id, oid);
    }
}