BEGIN TRANSACTION;
INSERT INTO "Role" ("Id","Name","CreatedAt","UpdatedAt") VALUES (1,'User','2022-03-24 16:00:00','2022-03-24 16:00:00'),
 (2,'Moderator','2022-03-24 16:00:00','2022-03-24 16:00:00'),
 (3,'Admin','2022-03-24 16:00:00','2022-03-24 16:00:00');
COMMIT;
