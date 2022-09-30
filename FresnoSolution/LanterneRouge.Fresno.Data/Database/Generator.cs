using LanterneRouge.Fresno.Database.SQLite;
using LanterneRouge.Fresno.Database.SQLite.Clauses;
using LanterneRouge.Fresno.Database.SQLite.Constraints;
using LanterneRouge.Fresno.Database.SQLite.Statements;
using LanterneRouge.Fresno.Database.SQLite.Types;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace LanterneRouge.Fresno.DataLayer.Database
{
    public class Generator
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Generator));
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

        /// <summary>
        /// CREATE TABLE `User` (`Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `FirstName` TEXT NOT NULL, `LastName`	TEXT NOT NULL, `Email`	TEXT NOT NULL, `Street` TEXT, `PostCode` TEXT, `PostCity` TEXT, `BirthDate` TEXT, `Height` INTEGER, `Sex` TEXT CHECK(Sex = ""M"" OR Sex = ""F""), `MaxHr` INTEGER );
        /// </summary>
        private TableStatement UserTable
        {
            get
            {
                var columns = new List<ColumnStatement>
                {
                    new ColumnStatement("Id")
                    {
                        Affinity = AffinityType.INTEGER,
                        ColumnConstraints = new List<IConstraint>
                        {
                            new NotNullConstraint(),
                            new ColumnPrimaryKeyConstraint(new ConflictClause())
                            {
                                IsAutoIncrement=true
                            },
                        }
                    },
                    new ColumnStatement("FirstName")
                    {
                        Affinity = AffinityType.TEXT,
                        ColumnConstraints = new List<IConstraint>
                        {
                            new NotNullConstraint()
                        }
                    },
                    new ColumnStatement("LastName")
                    {
                        Affinity = AffinityType.TEXT,
                        ColumnConstraints = new List<IConstraint>
                        {
                            new NotNullConstraint()
                        }
                    },
                    new ColumnStatement("Email")
                    {
                        Affinity = AffinityType.TEXT,
                        ColumnConstraints = new List<IConstraint>
                        {
                            new NotNullConstraint()
                        }
                    },
                    new ColumnStatement("Street")
                    {
                        Affinity = AffinityType.TEXT
                    },
                    new ColumnStatement("PostCode")
                    {
                        Affinity = AffinityType.TEXT
                    },
                    new ColumnStatement("PostCity")
                    {
                        Affinity = AffinityType.TEXT
                    },
                    new ColumnStatement("BirthDate")
                    {
                        Affinity = AffinityType.TEXT
                    },
                    new ColumnStatement("Height")
                    {
                        Affinity = AffinityType.INTEGER
                    },
                    new ColumnStatement("Sex")
                    {
                        Affinity = AffinityType.TEXT,
                        ColumnConstraints = new List<IConstraint>
                        {
                            new CheckConstraint("Sex='M' OR Sex='F'")
                        }
                    },
                    new ColumnStatement("MaxHr")
                    {
                        Affinity = AffinityType.INTEGER
                    }
                };

                var userTable = new TableStatement("User")
                {
                    Columns = columns
                };

                return userTable;
            }
        }

        /// <summary>
        /// CREATE TABLE `StepTest` (`Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `UserId` INTEGER NOT NULL, `TestType`	REAL NOT NULL DEFAULT 'Bike' CHECK(TestType = ""Bike"" OR TestType = ""Run""), `EffortUnit` TEXT NOT NULL DEFAULT 'W' CHECK(EffortUnit = ""W"" OR EffortUnit = ""m-s""), `StepDuration` INTEGER NOT NULL, `LoadPreset` NUMERIC NOT NULL, `Increase`	NUMERIC NOT NULL, `Temperature`	NUMERIC, `Weight` NUMERIC, `TestDate` TEXT, FOREIGN KEY(`UserId`) REFERENCES `User`(`Id`));
        /// </summary>
        private TableStatement StepTestTable
        {
            get
            {
                var columns = new List<ColumnStatement>
                {
                    new ColumnStatement("Id")
                    {
                        Affinity = AffinityType.INTEGER,
                        ColumnConstraints = new List<IConstraint>
                        {
                            new NotNullConstraint(),
                            new ColumnPrimaryKeyConstraint(new ConflictClause())
                            {
                                IsAutoIncrement=true
                            },
                        }
                    },
                    new ColumnStatement("UserId")
                    {
                        Affinity = AffinityType.INTEGER,
                        ColumnConstraints=new List<IConstraint>
                        {
                            new NotNullConstraint()
                        }
                    },
                    new ColumnStatement("TestType")
                    {
                        Affinity = AffinityType.TEXT,
                        ColumnConstraints = new List<IConstraint>
                        {
                            new NotNullConstraint(),
                            new DefaultConstraint("'Bike'"),
                            new CheckConstraint("TestType = 'Bike' OR TestType = 'Run'")
                        }
                    },
                    new ColumnStatement("EffortUnit")
                    {
                        Affinity = AffinityType.TEXT,
                        ColumnConstraints = new List<IConstraint>
                        {
                            new NotNullConstraint(),
                            new DefaultConstraint("'W'"),
                            new CheckConstraint("EffortUnit = 'W' OR EffortUnit = 'm-s'")
                        }
                    },
                    new ColumnStatement("StepDuration")
                    {
                        Affinity = AffinityType.INTEGER,
                        ColumnConstraints = new List<IConstraint>
                        {
                            new NotNullConstraint()
                        }
                    },
                    new ColumnStatement("LoadPreset")
                    {
                        Affinity = AffinityType.NUMERIC,
                        ColumnConstraints = new List<IConstraint>
                        {
                            new NotNullConstraint()
                        }
                    },
                    new ColumnStatement("Increase")
                    {
                        Affinity = AffinityType.NUMERIC,
                        ColumnConstraints = new List<IConstraint>
                        {
                            new NotNullConstraint()
                        }
                    },
                    new ColumnStatement("Temperature")
                    {
                        Affinity = AffinityType.NUMERIC
                    },
                    new ColumnStatement("Weight")
                    {
                        Affinity = AffinityType.NUMERIC
                    },
                    new ColumnStatement("TestDate")
                    {
                        Affinity = AffinityType.TEXT
                    },
                };

                var tableFkClause = new ForeignKeyClause(UserTable, new[] { "Id" });

                var stepTestTable = new TableStatement("StepTest")
                {
                    Columns = columns,
                    TableConstraints = new List<IConstraint>
                    {
                        new TableForeignKeyConstraint(new[]{"UserId"}, tableFkClause)
                    }
                };

                return stepTestTable;
            }
        }

        /// <summary>
        /// CREATE TABLE `Measurement` (`Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `Sequence` INTEGER NOT NULL, `StepTestId` INTEGER NOT NULL, `HeartRate` INTEGER NOT NULL, `Lactate` NUMERIC NOT NULL, `Load` NUMERIC NOT NULL, `InCalculation` TEXT DEFAULT 'True' CHECK(InCalculation = ""True"" OR InCalculation = ""False""), FOREIGN KEY(`StepTestId`) REFERENCES `StepTest`(`Id`));
        /// </summary>
        private TableStatement MeasurementTable
        {
            get
            {
                var columns = new List<ColumnStatement>
                {
                    new ColumnStatement("Id")
                    {
                        Affinity = AffinityType.INTEGER,
                        ColumnConstraints = new List<IConstraint>
                        {
                            new NotNullConstraint(),
                            new ColumnPrimaryKeyConstraint(new ConflictClause())
                            {
                                IsAutoIncrement = true
                            }
                        }
                    },
                    new ColumnStatement("Sequence")
                    {
                        Affinity = AffinityType.INTEGER,
                        ColumnConstraints = new List<IConstraint>
                        {
                            new NotNullConstraint()
                        }
                    },
                    new ColumnStatement("StepTestId")
                    {
                        Affinity = AffinityType.INTEGER,
                        ColumnConstraints = new List<IConstraint>
                        {
                            new NotNullConstraint()
                        }
                    },
                    new ColumnStatement("HeartRate")
                    {
                        Affinity = AffinityType.INTEGER,
                        ColumnConstraints = new List<IConstraint>
                        {
                            new NotNullConstraint()
                        }
                    },
                    new ColumnStatement("Lactate")
                    {
                        Affinity = AffinityType.NUMERIC,
                        ColumnConstraints = new List<IConstraint>
                        {
                            new NotNullConstraint()
                        }
                    },
                    new ColumnStatement("Load")
                    {
                        Affinity = AffinityType.NUMERIC,
                        ColumnConstraints = new List<IConstraint>
                        {
                            new NotNullConstraint()
                        }
                    },
                    new ColumnStatement("InCalculation")
                    {
                        Affinity = AffinityType.TEXT,
                        ColumnConstraints = new List<IConstraint>
                        {
                            new DefaultConstraint("'True'"),
                            new CheckConstraint("InCalculation = 'True' OR InCalculation = 'False'")
                        }
                    }
                };

                var tableFkClause = new ForeignKeyClause(StepTestTable, new[] { "Id" });
                var measurementTable = new TableStatement("Measurement")
                {
                    Columns = columns,
                    TableConstraints = new List<IConstraint>
                    {
                        new TableForeignKeyConstraint(new[]{"StepTestId"}, tableFkClause)
                    }
                };

                return measurementTable;
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
                sqlite_command.CommandText = UserTable.GenerateStatement();
                sqlite_command.ExecuteNonQuery();
                Logger.Info("User table created");

                sqlite_command.CommandText = StepTestTable.GenerateStatement();
                sqlite_command.ExecuteNonQuery();
                Logger.Info("StepTest table created");

                sqlite_command.CommandText = MeasurementTable.GenerateStatement();
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
