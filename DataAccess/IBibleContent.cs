namespace Reminders.BibleCoreLibrary.DataAccess;
public interface IBibleContent
{
    //for now, its only one being done.
    //we may eventually allow a list of items to be sent.
    //for now, only one.
    //can use the bible database to get this.  but does not care how its done.
    //most likely the wpf library will have reference to the actual bible database.
    Task<BasicList<string>> GetBookChaperDataAsync(string bookName, int chapter);
}