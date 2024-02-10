CREATE TABLE [dbo].[EventNotification] (
    [EventNotificationId] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [DomainEvent]         VARCHAR (MAX)    NOT NULL,
    [IsProcessed]         BIT              NOT NULL,
    [CreatedDate]         DATETIME         NOT NULL,
    [ModifiedDate]        DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([EventNotificationId] ASC)
);

