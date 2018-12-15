using log4net;
using System;
using System.Data.SQLite;
using System.IO;

namespace LanterneRouge.Fresno.DataLayer.Database
{
    public class Generator
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Generator));
        private const string CreateMeasurementTable = @"CREATE TABLE IF NOT EXISTS `Measurement` ( `Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `HeartRate` INTEGER NOT NULL, `Lactate` NUMERIC NOT NULL, `Load` NUMERIC NOT NULL, `StepTestId` INTEGER NOT NULL, FOREIGN KEY(`StepTestId`) REFERENCES `StepTest`(`Id`) )";
        private const string CreateStepTestTable = @"CREATE TABLE IF NOT EXISTS `StepTest` ( `Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `Sequence` INTEGER NOT NULL, `UserId` INTEGER NOT NULL, FOREIGN KEY(`UserId`) REFERENCES `User`(`Id`) )";
        private const string CreateUserTable = @"CREATE TABLE IF NOT EXISTS `User` ( `Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `FirstName` TEXT NOT NULL, `LastName` TEXT NOT NULL, `Email` TEXT NOT NULL )";

        private SQLiteConnection _connection;

        public Generator(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                Filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Fresno.sqlite");
            }

            else
            {
                Filename = filename;
            }
        }

        public string Filename { get; }

        private SQLiteConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    var connectionString = new SQLiteConnectionStringBuilder
                    {
                        DataSource = Filename,
                        ForeignKeys = true,
                        Version = 3
                    };

                    _connection = new SQLiteConnection(connectionString.ToString());
                }

                return _connection;
            }
        }

        public bool CreateDatabase()
        {
            var response = true;
            try
            {
                // create a new database connection:
                Connection.Open();
                Connection.Close();
            }

            catch (Exception ex)
            {
                Logger.Error("Error creating new database", ex);
                response = false;
            }

            return response;
        }

        public bool CreateTables()
        {
            var response = true;

            try
            {
                Connection.Open();

                var sqlite_command = Connection.CreateCommand();
                sqlite_command.CommandText = CreateUserTable;
                sqlite_command.ExecuteNonQuery();

                sqlite_command.CommandText = CreateStepTestTable;
                sqlite_command.ExecuteNonQuery();

                sqlite_command.CommandText = CreateMeasurementTable;
                sqlite_command.ExecuteNonQuery();
                sqlite_command.Dispose();

                Connection.Close();
            }

            catch (Exception ex)
            {
                Logger.Error("Error creating new tables", ex);
                response = false;
            }

            return response;
        }

        public bool RemoveDatabase()
        {
            var response = true;
            if (File.Exists(Filename))
            {
                if (Connection.State == System.Data.ConnectionState.Open)
                {
                    Connection.Close();
                }

                Connection.Dispose();

                File.Delete(Filename);
            }

            return response;
        }
    }
}
