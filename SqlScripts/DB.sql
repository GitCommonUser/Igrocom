create tablespace igrocomspace;
create database Igrocom tablespace igrocomspace;
create schema igrocomschema;

create type roles as enum('admin','common');

create table User (
	Id serial primary key,
	Login VARCHAR(50) NOT NULL,
	Password VARCHAR(32) NOT NULL,
	Role Role NOT NULL
);

create table GameFiles (
	Id serial primary key,
	File bytea NOT NULL,
	FileMime VARCHAR(50) NOT NULL,
	GameId INT REFERENCES Game (Id) on update cascade on delete cascade
);

create table Game (
	Id serial primary key,
	Title VARCHAR(100) NOT NULL,
	Genre VARCHAR(50) NOT NULL,
	Description TEXT NOT NULL,
	Peculiriaties TEXT NOT NULL,
	Review TEXT NOT NULL,
	Image bytea NOT NULL,
	ImageMime VARCHAR(50) NOT NULL,
	Rating int NOT NULL CHECK (Rating > 1 and Rating <= 100),
	ReleaseDate DATE NOT NULL
);

create table Rating (
	GameId INT REFERENCES Game (Id) on update cascade on delete cascade,
	UserId INT REFERENCES User (Id) on update cascade on delete cascade,
	Value INT NOT NULL CHECK (Value > 1 and Value <= 100)
);

create table UserGame (
	GameId INT REFERENCES Game (Id) on update cascade on delete cascade,
	UserId INT REFERENCES User (Id) on update cascade on delete cascade,
);

create table ContentFiles (
	Id serial primary key,
	File bytea NOT NULL,
	FileMime VARCHAR(50) NOT NULL,
	ContentId REFERENCES Content (Id) on update cascade on delete restrict
);

create table Content (
	Id serial primary key,
	Title VARCHAR(100) NOT NULL,
	Preface TEXT NOT NULL,
	Text TEXT NOT NULL,
	Image bytea NOT NULL,
	ImageMime VARCHAR(50) NOT NULL,
	ReleaseDate DATE NOT NULL
);

create table UserContent (
	GameId INT REFERENCES Game (Id) on update cascade on delete cascade,
	UserId INT REFERENCES User (Id) on update cascade on delete cascade,
);

CREATE OR REPLACE FUNCTION return_popular_games()
RETURNS TABLE(game_id INT) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        ug."GameId"
    FROM "UserGame" ug
    GROUP BY 
        ug."GameId"
    ORDER BY 
        COUNT(ug."UserId") DESC
    LIMIT 3;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION update_game_rating()
RETURNS TRIGGER AS $$
BEGIN
    UPDATE "Game"
    SET "Rating" = (
        SELECT AVG("Value")
        FROM "Rating"
        WHERE "GameId" = NEW."GameId"
    )
    WHERE "Id" = NEW."GameId";
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_game_grade
AFTER INSERT OR UPDATE ON "Rating"
FOR EACH ROW
EXECUTE FUNCTION update_game_rating();

create index rating_index on Rating(GameId);