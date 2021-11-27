namespace Reminders.BibleCoreLibrary.DataAccess;
public interface IBibleReminderAccess : IProcessedReminder
{
    Task<ReminderModel?> GetNextBibleReadingAsync();
    Task CompletedReadingAsync();
}