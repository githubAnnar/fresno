CREATE TABLE "SiteUserRole" (
	"RoleId"	INTEGER NOT NULL,
	"SiteUserId"	INTEGER NOT NULL,
	"CreatedAt"	TEXT NOT NULL,
	"UpdatedAt"	TEXT NOT NULL,
	PRIMARY KEY("RoleId","SiteUserId"),
	FOREIGN KEY("SiteUserId") REFERENCES "SiteUser"("Id"),
	FOREIGN KEY("RoleId") REFERENCES "Role"("Id")
)