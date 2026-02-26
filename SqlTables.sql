CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Tasks (
    Id INT PRIMARY KEY IDENTITY,
    TaskTypeId INT NOT NULL,             
    AssignedUserId INT NOT NULL REFERENCES Users(Id),
    CurrentStatus INT NOT NULL DEFAULT 1,
    IsOpen BIT NOT NULL DEFAULT 1,
    CustomData NVARCHAR(MAX) NULL        
);
INSERT INTO Users (Name) VALUES
    ('Alice'),
    ('Bob'),
    ('Carol'),
    ('David');