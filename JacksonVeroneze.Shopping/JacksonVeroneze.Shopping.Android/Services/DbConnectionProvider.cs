using JacksonVeroneze.Shopping.Common;
using LiteDB;
using System.IO;

namespace JacksonVeroneze.Shopping.Droid.Services
{
    public class DbConnectionProvider : IDbConnectionProvider
    {
        private const string FILE_NAME = "database_shopping.dbx";

        public LiteDatabase GetConnection()
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            return new LiteDatabase(Path.Combine(path, FILE_NAME));
        }
    }
}