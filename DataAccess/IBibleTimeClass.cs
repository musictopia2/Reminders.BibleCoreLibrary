namespace Reminders.BibleCoreLibrary.DataAccess;
public interface IBibleTimeClass
{
    Task<DateTime> GetNextReadingAsync(DailyReaderModel reading); //i think we still need datetime (?)
}