CREATE TABLE [dbo].[Riddle] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Titre]           NVARCHAR (MAX) NOT NULL,
    [Enonce]          VARCHAR (MAX)  NOT NULL,
    [Indice]          VARCHAR (MAX)  NOT NULL,
    [Solution]        VARCHAR (MAX)  NOT NULL,
    [DatePublication] DATETIME       NOT NULL,
    [IdCategory]      INT            NOT NULL,
    [IdUser]          INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

