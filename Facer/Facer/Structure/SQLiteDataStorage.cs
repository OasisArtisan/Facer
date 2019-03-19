using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SQLite;

namespace Facer.Structure
{
    class SQLiteDataStorage : IDataStorage
    {
        private SQLiteAsyncConnection database;

        public SQLiteDataStorage(string dbPath, string dbName)
        {
            database = new SQLiteAsyncConnection(Path.Combine(dbPath,dbName + "db3"));
            
        }

        public Data LoadData()
        {
            throw new NotImplementedException();
        }

        public bool SaveData(Data d)
        {
            throw new NotImplementedException();
        }
    }
}
