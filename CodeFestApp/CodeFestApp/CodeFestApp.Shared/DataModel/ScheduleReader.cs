using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace CodeFestApp.DataModel
{
    public class ScheduleReader
    {
        private static readonly string Today = DateTime.Today.ToString("d");

        private readonly JObject _scheduleJson;
        private readonly Lazy<IEnumerable<Day>> _days;
        private readonly Lazy<IEnumerable<Lecture>> _lectures;
        private readonly Lazy<IEnumerable<Track>> _sections;

        public ScheduleReader(string scheduleJson)
        {
            _scheduleJson = JObject.Parse(scheduleJson);
            _days = new Lazy<IEnumerable<Day>>(
                () => _scheduleJson["day"]
                          .Select(x => new Day
                              {
                                  Id = (int)x["id"],
                                  Title = (string)x["title"],
                                  Date = (DateTime)x["date"]
                              })
                          .ToArray());

            _lectures = new Lazy<IEnumerable<Lecture>>(
                () => _scheduleJson["lecture"]
                          .Select(x => new Lecture
                              {
                                  Id = (int)x["id"],
                                  Title = (string)x["title"],
                                  Description = (string)x["description"],
                                  Day = new Day { Id = (int)x["day_id"] },
                                  Track = new Track { Id = (int)x["section_id"] },
                                  Start = DateTime.Parse(Today + " " + x["time_start"]),
                                  End = DateTime.Parse(Today + " " + x["time_end"])
                              })
                          .ToArray());

            _sections = new Lazy<IEnumerable<Track>>(
                () => _scheduleJson["section"]
                          .Select(x => new Track
                              {
                                  Id = (int)x["id"],
                                  Title = (string)x["title"],
                                  Room = new Room { Id = (int)x["room_id"] },
                                  Color = Color.FromArgb(int.Parse(x.Value<string>("color").Substring(1),
                                                                   NumberStyles.HexNumber))
                              })
                          .ToArray());

        }

        public async Task<IEnumerable<Day>> GetDaysAsync()
        {
            return await Task.Factory.StartNew(
                () =>
                    {
                        foreach (var day in _days.Value)
                        {
                            if (day.Lectures == null || !day.Lectures.Any())
                            {
                                day.Lectures = _lectures.Value
                                    .Where(x => x.Day.Id == day.Id)
                                    .Select(x =>
                                        {
                                            x.Track = _sections.Value.SingleOrDefault(s => s.Id == x.Track.Id);
                                            return x;
                                        })
                                    .ToArray();
                            }
                        }

                       return _days.Value;
                    });
        }
    }
}
