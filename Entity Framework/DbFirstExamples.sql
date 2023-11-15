IF NOT EXISTS 
(
    SELECT name
	FROM master.dbo.sysdatabases
	WHERE name = N'DbFirstExamples'
)
CREATE DATABASE DbFirstExamples

GO
USE DbFirstExamples

DROP TABLE IF EXISTS ArticleTag
DROP TABLE IF EXISTS Articles
DROP TABLE IF EXISTS Tags

-- --- Create Tables ---
CREATE TABLE Tags
(
    _id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50) NOT NULL
);


CREATE TABLE Articles
(
    _id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Date DATETIME,
    Title VARCHAR(255) NOT NULL,
    Viewed INT,
);


CREATE TABLE ArticleTag
(
    _id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    ArticleID int FOREIGN KEY 
        REFERENCES Articles(_id) 
        ON UPDATE CASCADE
        ON DELETE NO ACTION,
    TagID int FOREIGN KEY 
        REFERENCES Tags(_id)
        ON UPDATE CASCADE
        ON DELETE NO ACTION,
);


-- --- Insert Data ---
INSERT INTO Articles
    (date,title,viewed)
VALUES
    ('2022-01-01', 'NoSQL Review', '2');
INSERT INTO Articles
    (date,title,viewed)
VALUES
    ('2022-01-02', 'Python Review', '5');
INSERT INTO Articles
    (date,title,viewed)
VALUES
    ('2022-01-02', 'Financial Analysis', '10');


INSERT INTO Tags
    (name)
VALUES
    ('Database')
INSERT INTO Tags
    (name)
VALUES
    ('MongoDB')
INSERT INTO Tags
    (name)
VALUES
    ('Python')
INSERT INTO Tags
    (name)
VALUES
    ('Finance')


INSERT INTO ArticleTag
    (ArticleID, TagID)
VALUES
    (1, 1)
INSERT INTO ArticleTag
    (ArticleID, TagID)
VALUES
    (1, 2)
INSERT INTO ArticleTag
    (ArticleID, TagID)
VALUES
    (2, 3)
INSERT INTO ArticleTag
    (ArticleID, TagID)
VALUES
    (3, 2)
INSERT INTO ArticleTag
    (ArticleID, TagID)
VALUES
    (3, 3)
INSERT INTO ArticleTag
    (ArticleID, TagID)
VALUES
    (3, 4)

Go

-- Create View --
CREATE OR ALTER VIEW Articles_With_Tag
AS
    SELECT Date, Title, Name
    FROM
        ((Articles as A
        INNER JOIN ArticleTag  as  B
        ON A._id = B.ArticleId)
        INNER JOIN Tags as C
        ON B.TagId = C._id)