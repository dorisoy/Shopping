using LiteDB;

namespace JacksonVeroneze.Shopping.Common
{
    public interface IDbConnectionProvider
    {
        LiteDatabase GetConnection();
    }
}