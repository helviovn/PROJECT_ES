CREATE TABLE [dbo].[Table]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(255) NOT NULL, 
    [data_inicio] DATETIME NOT NULL, 
    [data_fim] DATETIME NOT NULL, 
    [n_participantes] INT NULL 
)
