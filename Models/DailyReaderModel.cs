namespace Reminders.BibleCoreLibrary.Models;
public class DailyReaderModel
{
    public string Book { get; set; } = "";
    public int Chapter { get; set; }
    public DateTime ReadDate { get; set; } //has to keep datetime just in case the information is stored in a database (?)
    public bool Completed { get; set; }
}