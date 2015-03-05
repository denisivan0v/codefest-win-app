using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFestApp.DataModel
{
    public class Lecture
    {
        private readonly int _dayId;
        private readonly int _trackId;
        private readonly int _speakerId;

        public Lecture(int dayId, int trackId, int speakerId)
        {
            _dayId = dayId;
            _trackId = trackId;
            _speakerId = speakerId;
        } 

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Day Day { get; private set; }
        public Track Track { get; private set; }
        public Speaker Speaker { get; private set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public void SetDay(IEnumerable<Day> days)
        {
            if (Day == null)
            {
                Day = days.SingleOrDefault(x => x.Id == _dayId);
            }
        }

        public void SetTrack(IEnumerable<Track> tracks)
        {
            if (Track == null)
            {
                Track = tracks.SingleOrDefault(x => x.Id == _trackId);
            }
        }

        public void SetSpeaker(IEnumerable<Speaker> speakers)
        {
            if (Speaker == null)
            {
                Speaker = speakers.FirstOrDefault(x => x.Id == _speakerId);
            }
        }
    }
}
