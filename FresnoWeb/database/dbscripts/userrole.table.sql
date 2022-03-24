CREATE TABLE "UserRole" (
	"RoleId"	INTEGER NOT NULL,
	"UserId"	INTEGER NOT NULL,
	"CreatedAt"	TEXT NOT NULL,
	"UpdatedAt"	TEXT NOT NULL,
	PRIMARY KEY("RoleId","UserId"),
	FOREIGN KEY("UserId") REFERENCES "User"("Id"),
	FOREIGN KEY("RoleId") REFERENCES "Role"("Id")
)