CREATE DATABASE DVDLibrary
GO

USE DVDLibrary
GO

CREATE TABLE Movie 
(MovieID int identity (1,1) not null primary key,
Title varchar(30) NOT NULL,
ReleaseDate datetime NOT NULL,
MPAARatingID int,
DirectorName varchar(20),
Studio varchar(20),
UserRatingID int,
UserNotes varchar (max),
Actors varchar (50),
BorrowerName varchar (20),
DateBorrowed datetime,
DateReturned datetime)

CREATE TABLE MPAARating
(MPAARatingID int identity (1,1) not null primary key,
MPAARating varchar (10),
MPAADescription varchar (50))

CREATE TABLE UserRating
(UserRatingID int identity (1,1) not null primary key,
UserRating varchar (10),
UserRatingDescription varchar (50))

ALTER TABLE Movie 
add FOREIGN KEY (MPAARatingID) REFERENCES MPAARating(MPAARatingID)

ALTER TABLE Movie 
add FOREIGN KEY (UserRatingID) REFERENCES UserRating(UserRatingID)


SELECT Title, MPAARating, UserRating
from movie
	inner join MPAARating on movie.MPAARatingID = MPAARating.MPAARatingID
	inner join UserRating on movie.UserRatingID = UserRating.UserRatingID

SELECT MovieId, Title, ReleaseDate, MPAARatingId, DirectorName, Studio, UserRatingID, UserNotes, Actors, BorrowerName, DateBorrowed, DateReturned
from movie

select UserRatingID, UserRating, UserRatingDescription
from UserRating

select MPAARatingID, MPAARating, MPAADescription
from mpaarating