USE [SolutionTemplateDb]
GO

CREATE TABLE [dbo].[ExampleTable]
(
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Key] VARCHAR(32) NOT NULL,
    [Value] VARCHAR(32) NOT NULL
)
GO
