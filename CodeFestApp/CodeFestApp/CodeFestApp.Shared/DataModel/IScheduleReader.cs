using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeFestApp.DataModel
{
    public interface IScheduleReader
    {
        Task ReadSchedule();

        IEnumerable<Day> GetDays();
        IEnumerable<Lecture> GetCurrentLectures();
        IEnumerable<Speaker> GetSpeakers();
    }
}