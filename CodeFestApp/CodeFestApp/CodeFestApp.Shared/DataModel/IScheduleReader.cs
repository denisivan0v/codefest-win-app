using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeFestApp.DataModel
{
    public interface IScheduleReader
    {
        Task<IEnumerable<Day>> GetDaysAsync();
        Task<IEnumerable<Lecture>> GetCurrentLectures();
        Task<IEnumerable<Speaker>> GetSpeakers();
    }
}