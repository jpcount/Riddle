CREATE TABLE [dbo].[User2] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (50) NULL,
    [FirstName] VARCHAR (50) NULL,
    [UserName]  VARCHAR (50) NULL,
    [Email]     VARCHAR (50) NULL,
    [Phone]     VARCHAR (50) NULL,
    [Password]  VARCHAR (50) NULL,
    [admin]     VARCHAR (10) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

