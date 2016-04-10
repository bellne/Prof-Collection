
INSERT INTO Movie (Title, ReleaseDate, MPAARatingID, DirectorName, Studio, UserRatingID, UserNotes)  --BorrowerName, DateBorrowed, DateReturned)
VALUES ('Matrix', '01/23/03', 4, 'WojBrothers', 'LionsGate', 5, 'This movie is a mind trip')

INSERT INTO MPAARating (MPAARating, MPAADescription)
VALUES ('G', 'Family Rating'),
('PG', 'Parental Guidance Suggested'),
('PG-13', 'Parental Guidance for children under 13'),
('R', 'Parental Guidance required for children under 17'),
('NC-17', 'No children under age of 17'),
('NR', 'Movie not rated by MPAA')

INSERT INTO UserRating (UserRating, UserRatingDescription)
VALUES (0, 'Hated it'), (1, 'Kinda hated it'), (2, 'Did not hate it'), (3, 'Neutral'), (4, 'Liked it'), (5, 'Loved it!')