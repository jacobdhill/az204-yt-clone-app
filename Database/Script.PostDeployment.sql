IF NOT EXISTS (
	SELECT *
	FROM [dbo].[EmailTemplate]
)
BEGIN
	
	INSERT INTO [dbo].[EmailTemplate]
	(
		[EmailTemplateId],
		[Code],
		[Content],
		[CreatedDate],
		[ModifiedDate]
	)
	VALUES
	(
		newid(),
		'ReminderCreatedEvent',
		'<p>Hello,</p><p>Your new reminder <b>{{Description}}</b> has been created and will notify you starting from {{NotifyDaysBefore}} days out until the {{DayOfMonth}} day of the month.</p>',
		getdate(),
		null
	),
	(
		newid(),
		'ReminderUpdatedEvent',
		'<p>Hello,</p><p>Your reminder <b>{{Description}}</b> has been updated from:</p><p>Notifying starting from {{OldNotifyDaysBefore}} days out until the {{OldDayOfMonth}} day of the month.</p><p>to:</p><p>Notifying starting from {{NotifyDaysBefore}} days out until the {{DayOfMonth}} day of the month.</p>',
		getdate(),
		null
	),
	(
		newid(),
		'ReminderDeletedEvent',
		'<p>Hello,</p><p>Your reminder <b>{{Description}}</b> has been deleted.</p>',
		getdate(),
		null
	);

END