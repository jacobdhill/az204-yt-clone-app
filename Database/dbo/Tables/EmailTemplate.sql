CREATE TABLE [dbo].[EmailTemplate] (
    [EmailTemplateId]  UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Code]             VARCHAR (255)    NOT NULL,
    [Content]          VARCHAR (MAX)    NOT NULL,
    [CreatedDate]      DATETIME         NOT NULL,
    [ModifiedDate]     DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([EmailTemplateId] ASC)
);

