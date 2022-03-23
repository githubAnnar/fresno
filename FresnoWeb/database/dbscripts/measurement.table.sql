CREATE TABLE "Measurement" (
	`Id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	`Sequence`	INTEGER NOT NULL,
	`StepTestId`	INTEGER NOT NULL,
	`HeartRate`	INTEGER NOT NULL,
	`Lactate`	NUMERIC NOT NULL,
	`Load`	NUMERIC NOT NULL,
	`InCalculation`	TEXT DEFAULT 'True' CHECK(InCalculation = "True" OR InCalculation = "False"),
	FOREIGN KEY(`StepTestId`) REFERENCES `StepTest`(`Id`)
)