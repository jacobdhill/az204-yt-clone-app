CREATE TABLE [dbo].[Reminder] (
    [ReminderId]       UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Description]      VARCHAR (255)    NOT NULL,
    [NotifyDaysBefore] INT              NOT NULL,
    [DayOfMonth]       INT              NOT NULL,
    [CreatedDate]      DATETIME         NOT NULL,
    [ModifiedDate]     DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([ReminderId] ASC)
);

