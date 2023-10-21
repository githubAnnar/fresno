﻿using LanterneRouge.Fresno.Database.SQLite.Tool;
using LanterneRouge.Fresno.Repository.Contracts;
using log4net;
using System.Data;
using System.Data.SQLite;

namespace LanterneRouge.Fresno.Repository.Infrastructure
{
    public class ConnectionFactory : IConnectionFactory
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConnectionFactory));
        private readonly string _connectionString;
        private IDbConnection? _connection = null;

        public ConnectionFactory(SQLiteConnectionStringBuilder connectionStringBuilder)
        {
            _connectionString = connectionStringBuilder.ToString();
            Logger.Debug($"Connectionstring: {_connectionString}");
        }

        public ConnectionFactory(string filename) : this(new SQLiteConnectionStringBuilder { DataSource = filename, ForeignKeys = true, Version = 3 })
        { }

        /// <summary>
        /// 
        /// </summary>
        /// Remarks:
        /// https://stackoverflow.com/questions/1117683/add-a-dbproviderfactory-without-an-app-config
        /// https://weblog.west-wind.com/posts/2017/Nov/27/Working-around-the-lack-of-dynamic-DbProviderFactory-loading-in-NET-Core
        public IDbConnection GetConnection
        {
            get
            {
                if (_connection == null)
                {
                    CheckExistingFile();
                    //var table = DbProviderFactories.GetFactoryClasses();
                    //var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
                    SQLiteFactory factory = SQLiteFactory.Instance;
                    _connection = factory.CreateConnection();
                    _connection.ConnectionString = _connectionString;
                    _connection.Open();
                    Logger.Debug($"Connection '{_connectionString}' is open!");
                }

                return _connection;
            }
        }

        private void CheckExistingFile()
        {
            var builder = new SQLiteConnectionStringBuilder(_connectionString);

            if (!File.Exists(builder.DataSource))
            {
                Logger.Debug($"{builder.DataSource} is not open, creating new and creating database and tables!");
                var generator = new Generator(builder.DataSource);
                generator.CreateDatabase();
                generator.CreateTables();
            }
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ConnectionFactory() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
