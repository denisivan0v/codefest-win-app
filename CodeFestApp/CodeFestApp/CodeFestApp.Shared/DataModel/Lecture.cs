using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFestApp.DataModel
{
    public class Lecture
    {
        private readonly int _dayId;
        private readonly int _trackId;
        private readonly IEnumerable<int> _speakerIds;
        private readonly Func<IEnumerable<Speaker>> _speakersProvider;

        private IEnumerable<Speaker> _speakers;

        public Lecture(int dayId, int trackId, Func<IEnumerable<Speaker>> speakersProvider, int speakerId, params int[] otherSpeakerIds)
        {
            _dayId = dayId;
            _trackId = trackId;
            _speakersProvider = speakersProvider;
            _speakerIds = new[] { speakerId }.Union(otherSpeakerIds);
        } 

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Day Day { get; private set; }
        public Track Track { get; private set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public IEnumerable<Speaker> Speakers
        {
            get { return _speakers ?? (_speakers = _speakersProvider().Where(x => _speakerIds.Contains(x.Id)).ToArray()); }
        }

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
    }
}
