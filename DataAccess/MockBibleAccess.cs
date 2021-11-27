namespace Reminders.BibleCoreLibrary.DataAccess;
public class MockBibleAccess : IBibleReminderAccess, IBibleReadingContext
{
    private readonly IDatePicker _date;
    private DateTime _nextDate;
    public MockBibleAccess(IDatePicker date)
    {
        _date = date;
        _nextDate = _date.GetCurrentDate;
        var tempDate = _nextDate.Date.AddHours(6);
        if (_nextDate > tempDate)
        {
            _nextDate = tempDate.AddDays(1);
        }
        else
        {
            _nextDate = tempDate;
        }
        if (_nextDate.DayOfWeek == DayOfWeek.Saturday)
        {
            _nextDate = _nextDate.AddDays(2);
        }
        else if (_nextDate.DayOfWeek == DayOfWeek.Sunday)
        {
            _nextDate = _nextDate.AddDays(1);
        }
    }
    public Task CompletedReadingAsync()
    {
        _nextDate = _nextDate.AddDays(1);
        MainReminderProcesses.WaitingForUser = false;
        return Task.CompletedTask;
    }
    Task IBibleReadingContext.CompletedReadingAsync(DailyReaderModel reading)
    {
        return Task.CompletedTask;
    }
    Task<DailyReaderModel> IBibleReadingContext.GetCurrentReadingAsync()
    {
        DailyReaderModel model = new();
        model.Book = "Mark";
        model.Chapter = 1;
        model.ReadDate = DateTime.Now;
        return Task.FromResult(model);
    }
    Task<bool> IBibleReadingContext.HasNewReadingAsync()
    {
        return Task.FromResult(true);
    }
    Task<ReminderModel?> IBibleReminderAccess.GetNextBibleReadingAsync()
    {
        ReminderModel output = new()
        {
            Message = $"Mock Bible Message",
            Title = $"Mock Bible Title",
            NextDate = _nextDate
        };
        return Task.FromResult(output)!;
    }
    Task IProcessedReminder.ProcessedReminderAsync()
    {
        return Task.CompletedTask;
    }
}