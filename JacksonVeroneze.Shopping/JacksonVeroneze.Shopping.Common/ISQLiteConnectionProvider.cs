using SQLite;

namespace JacksonVeroneze.Shopping.Common
{
    public interface ISQLiteConnectionProvider
    {
        SQLiteAsyncConnection GetConnection();
    }
}