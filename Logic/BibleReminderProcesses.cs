namespace Reminders.BibleCoreLibrary.Logic;
/// <summary>
/// this is for the daily bible reading.
/// for now, will use some mocks.
/// </summary>
public class BibleReminderProcesses : BasicSubReminderProcesses
{
    public static Action? ClosedBibleAction { get; set; }
    public static Action? OpenBibleProcesses { get; set; }
    private readonly IBibleReminderAccess _data;
    public BibleReminderProcesses(IBibleReminderAccess data, ISnoozeDataAccess snoozeData) : base(data, snoozeData)
    {
        _data = data;
        MainReminderProcesses.UserCompletedAction = async () =>
        {
            await _data.CompletedReadingAsync();
            await MainReminderProcesses.RecalculateRemindersAsync();
            ClosedBibleAction?.Invoke();
        };
    }
    public override async Task CloseReminderAsync(DateTime currentDate)
    {
        MainReminderProcesses.WaitingForUser = true;
        await base.CloseReminderAsync(currentDate);
        OpenBibleProcesses!.Invoke();
    }
    public override Task<ReminderModel?> GetNextReminderAsync()
    {
        return _data.GetNextBibleReadingAsync();
    }
}