namespace Reminders.BibleCoreLibrary.DataAccess;
//this implementation depends on accessing files on hard drive.  a person has to implement interface to get location (they can even use iconfiguration if they chose to).
public class BibleReadingFileAccess : IBibleReminderAccess, IBibleReadingContext
{
    private readonly IReadingPath _path;
    private readonly IBibleTimeClass _time;
    private DailyReaderModel? _reading;
    private readonly BasicList<DailyReaderModel> _completeList = new();
    public BibleReadingFileAccess(IReadingPath path, IBibleTimeClass time)
    {
        _path = path;
        _time = time;
        _completeList = js.RetrieveSavedObject<BasicList<DailyReaderModel>>(_path.ReadingPath);
    }
    public Task CompletedReadingAsync(DailyReaderModel reading)
    {
        _reading = reading;
        return CompletedReadingAsync();
    }
    public Task<DailyReaderModel> GetCurrentReadingAsync()
    {
        DailyReaderModel daily = _completeList.Where(item => item.Completed == false).OrderBy(item => item.ReadDate).Take(1).Single();
        return Task.FromResult(daily);
    }
    public Task<bool> HasNewReadingAsync()
    {
        if (_completeList.Count == 0)
        {
            throw new CustomBasicException("Never loaded the bible reading list.  Rethink");
        }
        return Task.FromResult(_completeList.Any(item => item.Completed == false));
    }
    private async Task CompletedReadingAsync()
    {
        if (_reading == null)
        {
            throw new CustomBasicException("No reading was found.  Rethink");
        }
        DailyReaderModel newReading = _completeList.Single(item => item.ReadDate == _reading.ReadDate);
        _completeList.ForConditionalItems(item => item.ReadDate == _reading.ReadDate, item => item.Completed = true);
        await js.SaveObjectAsync(_path.ReadingPath, _completeList);
        MainReminderProcesses.WaitingForUser = false;
    }
    Task IBibleReminderAccess.CompletedReadingAsync()
    {
        return CompletedReadingAsync();
    }
    async Task<ReminderModel?> IBibleReminderAccess.GetNextBibleReadingAsync()
    {
        await Task.CompletedTask;
        if (_completeList.Count == 0)
        {
            return null;
        }
        if (await HasNewReadingAsync() == false)
        {
            return null;
        }
        _reading = await GetCurrentReadingAsync();
        DateTime nextDate = await _time.GetNextReadingAsync(_reading);
        ReminderModel output = new()
        {
            Message = "This is a reminder to do your daily reading.",
            Title = "Bible Reader Reminder",
            NextDate = nextDate
        };
        return output;
    }
    Task IProcessedReminder.ProcessedReminderAsync()
    {
        return Task.CompletedTask;
    }
}