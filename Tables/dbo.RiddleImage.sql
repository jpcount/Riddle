CREATE TABLE [dbo].[RiddleImage] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Url]      NVARCHAR (MAX) NOT NULL,
    [idRiddle] INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

