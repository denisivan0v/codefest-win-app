using System.Threading.Tasks;

namespace CodeFestApp.DataModel
{
    public interface IScheduleSource
    {
        Task<string> ReadScheduleAsync();
    }
}
