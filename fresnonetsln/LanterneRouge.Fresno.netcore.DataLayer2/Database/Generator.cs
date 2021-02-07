using log4net;
using System;
using System.Data.SQLite;
using System.IO;

namespace LanterneRouge.Fresno.netcore.DataLayer2.Database
{
    public class Generator
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Generator));
        private const string CreateMeasurementTable = @"CREATE TABLE `Measurement` (`Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `Sequence` INTEGER NOT NULL, `StepTestId` INTEGER NOT NULL, `HeartRate` INTEGER NOT NULL, `Lactate` NUMERIC NOT NULL, `Load` NUMERIC NOT NULL, `InCalculation` TEXT DEFAULT 'True' CHECK(InCalculation = ""True"" OR InCalculation = ""False""), FOREIGN KEY(`StepTestId`) REFERENCES `StepTest`(`Id`));";

        private const string CreateStepTestTable = @"CREATE TABLE `StepTest` (`Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `UserId` INTEGER NOT NULL, `TestType`	REAL NOT NULL DEFAULT 'Bike' CHECK(TestType = ""Bike"" OR TestType = ""Run""), `EffortUnit` TEXT NOT NULL DEFAULT 'W' CHECK(EffortUnit = ""W"" OR EffortUnit = ""m-s""), `StepDuration` INTEGER NOT NULL, `LoadPreset` NUMERIC NOT NULL, `Increase`	NUMERIC NOT NULL, `Temperature`	NUMERIC, `Weight` NUMERIC, `TestDate` TEXT, FOREIGN KEY(`UserId`) REFERENCES `User`(`Id`));";

        private const string CreateUserTable = @"CREATE TABLE `User` (`Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `FirstName` TEXT NOT NULL, `LastName`	TEXT NOT NULL, `Email`	TEXT NOT NULL, `Street` TEXT, `PostCode` TEXT, `PostCity` TEXT, `BirthDate` TEXT, `Height` INTEGER, `Sex` TEXT CHECK(Sex = ""M"" OR Sex = ""F""), `MaxHr` INTEGER );";

        private SQLiteConnection _connection;

        public Generator(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                Filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Fresno.sqlite");
            }

            else
            {
                Filename = filename;
            }

            Logger.Debug($"Filename set to '{Filename}'");
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
                Logger.Info("New database created");
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
                Logger.Info("User table created");

                sqlite_command.CommandText = CreateStepTestTable;
                sqlite_command.ExecuteNonQuery();
                Logger.Info("StepTest table created");

                sqlite_command.CommandText = CreateMeasurementTable;
                sqlite_command.ExecuteNonQuery();
                Logger.Info("Measurement table created");
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
                Logger.Info($"Database {Filename} deleted!");
            }

            return response;
        }
    }
}
