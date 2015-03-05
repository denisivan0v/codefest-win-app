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
        private readonly IScheduleSource _scheduleSource;
        private readonly List<Lecture> _lectures = new List<Lecture>();

        private IEnumerable<Room> _rooms;
        private IEnumerable<Day> _days;
        private IEnumerable<Track> _tracks;
        private IEnumerable<Company> _companies;
        private IEnumerable<Speaker> _speakers;

        public ScheduleReader(IScheduleSource scheduleSource)
        {
            _scheduleSource = scheduleSource;
        }

        public async Task ReadSchedule()
        {
            var json = await _scheduleSource.ReadScheduleAsync();
            var schedule = JObject.Parse(json);

            _companies = schedule["company"]
                .Select(x => new Company
                    {
                        Id = (int)x["id"],
                        Title = (string)x["title"]
                    })
                .ToArray();

            _rooms = schedule["room"]
                .Select(x => new Room(() => _lectures)
                    {
                        Id = (int)x["id"],
                        Title = (string)x["title"]
                    })
                .ToArray();

            _days = schedule["day"]
                .Select(x => new Day(() => _lectures)
                    {
                        Id = (int)x["id"],
                        Title = (string)x["title"],
                        Date = (DateTime)x["date"]
                    })
                .ToArray();

            _tracks = schedule["section"]
                .Select(x => new Track((int)x["room_id"], () => _lectures)
                    {
                        Id = (int)x["id"],
                        Title = (string)x["title"],
                        Color = Color.FromArgb(int.Parse(x.Value<string>("color").Substring(1),
                                                         NumberStyles.HexNumber))
                    })
                .Select(x =>
                    {
                        x.SetRoom(_rooms);
                        return x;
                    })
                .ToArray();

            _speakers = schedule["speaker"]
                .Select(x => new Speaker((int)x["company_id"], () => _lectures)
                    {
                        Id = (int)x["id"],
                        Title = (string)x["title"],
                        Description = (string)x["description"],
                        JobTitle = (string)x["post"],
                        Avatar = CreateUri((string)x["avatar"]),
                        FacebookProfile = CreateUri((string)x["link_facebook"]),
                        LinkedInProfile = CreateUri((string)x["link_linkedin"]),
                        MoiKrugProfile = CreateUri((string)x["link_moikrug"]),
                        TwitterProfile = CreateUri((string)x["link_twitter"]),
                        VkontakteProfile = CreateUri((string)x["link_vkontakte"])
                    })
                .Select(x =>
                    {
                        x.SetCompany(_companies);
                        return x;
                    })
                .ToArray();

            _lectures.AddRange(schedule["lecture"]
                                   .Select(x => new Lecture((int)x["day_id"], (int)x["section_id"], (int)x["speaker_id"])
                                       {
                                           Id = (int)x["id"],
                                           Title = (string)x["title"],
                                           Description = (string)x["description"],
                                           Start = DateTime.Parse(Today + " " + x["time_start"]),
                                           End = DateTime.Parse(Today + " " + x["time_end"])
                                       })
                                   .Select(x =>
                                       {
                                           x.SetDay(_days);
                                           x.SetTrack(_tracks);
                                           x.SetSpeaker(_speakers);
                                           return x;
                                       }));
        }

        public IEnumerable<Day> GetDays()
        {
            return _days;
        }

        public IEnumerable<Lecture> GetCurrentLectures()
        {
            var now = DateTime.Now;
            return _lectures.Where(x => x.Start >= now && x.End <= now);
        }

        public IEnumerable<Speaker> GetSpeakers()
        {
            return _speakers;
        }

        public IEnumerable<Lecture> GetSpeakerLections(int speakerId)
        {
            return _lectures.Where(x => x.Speaker != null && x.Speaker.Id == speakerId);
        }

        private static Uri CreateUri(string value)
        {
            return !string.IsNullOrEmpty(value) ? new Uri(value) : null;
        }
    }
}
