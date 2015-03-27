using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeFestApp.DataModel
{
    public interface IScheduleReader
    {
        Task ReadSchedule();

        IEnumerable<Day> GetDays();
        IEnumerable<Speaker> GetSpeakers();
        IEnumerable<Track> GetTracks();
        IEnumerable<Lecture> GetLectures();
        IEnumerable<Lecture> GetCurrentLectures();
    }
}