namespace Reminders.BibleCoreLibrary.DataAccess;
public interface IBibleReadingContext
{
    Task CompletedReadingAsync(DailyReaderModel reading);
    Task<bool> HasNewReadingAsync();
    Task<DailyReaderModel> GetCurrentReadingAsync();
}