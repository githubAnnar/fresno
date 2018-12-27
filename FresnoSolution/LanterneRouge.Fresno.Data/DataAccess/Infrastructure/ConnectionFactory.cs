using LanterneRouge.Fresno.DataLayer.Database;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.Infrastructure
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;

        public ConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

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
                CheckExistingFile();
                //var table = DbProviderFactories.GetFactoryClasses();
                //var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
                var factory = SQLiteFactory.Instance;
                var conn = factory.CreateConnection();
                conn.ConnectionString = _connectionString;
                conn.Open();
                return conn;
            }
        }

        private void CheckExistingFile()
        {
            var builder = new SQLiteConnectionStringBuilder(_connectionString);

            if (!File.Exists(builder.DataSource))
            {
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
