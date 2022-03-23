CREATE TABLE "StepTest" (
	`Id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	`UserId`	INTEGER NOT NULL,
	`TestType`	TEXT NOT NULL DEFAULT 'Bike' CHECK(TestType = "Bike" OR TestType = "Run"),
	`EffortUnit`	TEXT NOT NULL DEFAULT 'W' CHECK(EffortUnit = "W" OR EffortUnit = "m-s"),
	`StepDuration`	INTEGER NOT NULL,
	`LoadPreset`	NUMERIC NOT NULL,
	`Increase`	NUMERIC NOT NULL,
	`Temperature`	NUMERIC,
	`Weight`	NUMERIC,
	`TestDate`	TEXT,
	FOREIGN KEY(`UserId`) REFERENCES `User`(`Id`)
)