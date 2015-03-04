using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace CodeFestApp.DataModel
{
    public class ScheduleReader : IScheduleReader
    {
        private static readonly string Today = DateTime.Today.ToString("d");

        private readonly JObject _scheduleJson;
        private readonly Lazy<IEnumerable<Day>> _days;
        private readonly Lazy<IEnumerable<Lecture>> _lectures;
        private readonly Lazy<IEnumerable<Track>> _tracks;
        private readonly Lazy<IEnumerable<Company>> _companies;
        private readonly Lazy<IEnumerable<Speaker>> _speakers;

        public ScheduleReader(IScheduleSource scheduleSource)
        {
            var json = scheduleSource.ReadScheduleAsync();
            json.Wait();

            _scheduleJson = JObject.Parse(json.Result);
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
                                  Speaker = new Speaker { Id = (int)x["speaker_id"] },
                                  Start = DateTime.Parse(Today + " " + x["time_start"]),
                                  End = DateTime.Parse(Today + " " + x["time_end"])
                              })
                          .ToArray());

            _tracks = new Lazy<IEnumerable<Track>>(
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

            _companies = new Lazy<IEnumerable<Company>>(
                () => _scheduleJson["company"]
                          .Select(x => new Company
                              {
                                  Id = (int)x["id"],
                                  Title = (string)x["title"]
                              })
                          .ToArray());

            _speakers = new Lazy<IEnumerable<Speaker>>(
                () => _scheduleJson["speaker"]
                          .Select(x => new Speaker
                              {
                                  Id = (int)x["id"],
                                  Title = (string)x["title"],
                                  Description = (string)x["description"],
                                  JobTitle = (string)x["post"],
                                  Avatar = CreateUri((string)x["avatar"]),
                                  Company = new Company { Id = (int)x["company_id"] },
                                  FacebookProfile = CreateUri((string)x["link_facebook"]),
                                  LinkedInProfile = CreateUri((string)x["link_linkedin"]),
                                  MoiKrugProfile = CreateUri((string)x["link_moikrug"]),
                                  TwitterProfile = CreateUri((string)x["link_twitter"]),
                                  VkontakteProfile = CreateUri((string)x["link_vkontakte"])
                              })
                          .ToArray());
        }

        public async Task<IEnumerable<Day>> GetDaysAsync()
        {
            return await Task.Factory.StartNew(
                () => _days.Value.Select(x => ProjectDay(x, _lectures.Value, _speakers.Value, _tracks.Value)));
        }

        public async Task<IEnumerable<Lecture>> GetCurrentLectures()
        {
            var now = DateTime.Now;
            return await Task.Factory.StartNew(
                () => _lectures.Value
                               .Where(x => x.Start >= now && x.End <= now)
                               .Select(x => ProjectLecture(x, _speakers.Value, _tracks.Value)));
        }

        public async Task<IEnumerable<Speaker>> GetSpeakers()
        {
            return await Task.Factory.StartNew(
                () => _speakers.Value
                               .Select(x => ProjectSpeaker(x, _companies.Value, _lectures.Value)));
        }

        private static Uri CreateUri(string value)
        {
            return !string.IsNullOrEmpty(value) ? new Uri(value) : null;
        }

        private static Day ProjectDay(Day day,
                                      IEnumerable<Lecture> lectures,
                                      IEnumerable<Speaker> speakers,
                                      IEnumerable<Track> tracks)
        {
            if (day.Lectures == null || !day.Lectures.Any())
            {
                day.Lectures = lectures.Where(x => x.Day.Id == day.Id)
                                       .Select(x => ProjectLecture(x, speakers, tracks));
            }

            return day;
        }

        private static Lecture ProjectLecture(Lecture lecture, IEnumerable<Speaker> speakers, IEnumerable<Track> tracks)
        {
            lecture.Speaker = speakers.SingleOrDefault(s => s.Id == lecture.Speaker.Id);
            lecture.Track = tracks.SingleOrDefault(t => t.Id == lecture.Track.Id);
            return lecture;
        }

        private static Speaker ProjectSpeaker(Speaker speaker, IEnumerable<Company> companies, IEnumerable<Lecture> lectures)
        {
            speaker.Company = companies.SingleOrDefault(x => x.Id == speaker.Company.Id);
            speaker.Lectures = lectures.Where(x => x.Speaker.Id == speaker.Id).ToArray();
            return speaker;
        }
    }
}
