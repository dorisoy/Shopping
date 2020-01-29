﻿using JacksonVeroneze.Shopping.Common;
using SQLite;
using System.IO;

namespace JacksonVeroneze.Shopping.Droid.Services
{
    public class SQLiteConnectionProvider : ISQLiteConnectionProvider
    {
        private const string FILE_NAME = "databaseShopping.db3";

        public SQLiteAsyncConnection GetConnection()
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            return new SQLiteAsyncConnection(Path.Combine(path, FILE_NAME));
        }
    }
}