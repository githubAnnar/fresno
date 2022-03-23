CREATE TABLE "User" (
	`Id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	`FirstName`	TEXT NOT NULL,
	`LastName`	TEXT NOT NULL,
	`Email`	TEXT NOT NULL,
	`Street`	TEXT,
	`PostCode`	TEXT,
	`PostCity`	TEXT,
	`BirthDate`	TEXT,
	`Height`	INTEGER,
	`Sex`	TEXT CHECK(Sex = "M" OR Sex = "F"),
	`MaxHr`	INTEGER
)