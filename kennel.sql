DROP TABLE IF EXISTS Animal;

CREATE TABLE Animal (
	Id int identity primary key,
	[Name] varchar(50) not null,
	Breed varchar(40) not null,
	Age int not null,
	HasShots bit not null
)


INSERT INTO Animal ([Name], Breed, Age, HasShots)
	VALUES ('Slappy', 'Mongrel', 4, 0);

INSERT INTO Animal ([Name], Breed, Age, HasShots)
	VALUES ('Stuckup', 'some kind of cat', 10, 1);

INSERT INTO Animal ([Name], Breed, Age, HasShots)
	VALUES ('Wilson', 'Poodle', 2, 1);


	select * from animal;