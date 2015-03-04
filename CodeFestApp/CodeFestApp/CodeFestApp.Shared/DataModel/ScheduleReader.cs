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

        private IEnumerable<Day> _days;
        private IEnumerable<Lecture> _lectures;
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

            _days = schedule["day"]
                .Select(x => new Day
                    {
                        Id = (int)x["id"],
                        Title = (string)x["title"],
                        Date = (DateTime)x["date"]
                    })
                .ToArray();

            _lectures = schedule["lecture"]
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
                .ToArray();

            _tracks = schedule["section"]
                .Select(x => new Track
                    {
                        Id = (int)x["id"],
                        Title = (string)x["title"],
                        Room = new Room { Id = (int)x["room_id"] },
                        Color = Color.FromArgb(int.Parse(x.Value<string>("color").Substring(1),
                                                         NumberStyles.HexNumber))
                    })
                .ToArray();

            _companies = schedule["company"]
                .Select(x => new Company
                    {
                        Id = (int)x["id"],
                        Title = (string)x["title"]
                    })
                .ToArray();

            _speakers = schedule["speaker"]
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
                .ToArray();
        }

        public IEnumerable<Day> GetDays()
        {
            return _days.Select(x => ProjectDay(x, _lectures, _speakers, _tracks));
        }

        public IEnumerable<Lecture> GetCurrentLectures()
        {
            var now = DateTime.Now;
            return _lectures.Where(x => x.Start >= now && x.End <= now)
                            .Select(x => ProjectLecture(x, _speakers, _tracks));
        }

        public IEnumerable<Speaker> GetSpeakers()
        {
            return _speakers.Select(x => ProjectSpeaker(x, _companies, _lectures));
        }

        public IEnumerable<Lecture> GetSpeakerLections(int speakerId)
        {
            return _lectures.Where(x => x.Speaker != null && x.Speaker.Id == speakerId);
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
            speaker.Lectures = lectures.Where(x => x.Speaker.Id == speaker.Id);
            return speaker;
        }
    }
}
