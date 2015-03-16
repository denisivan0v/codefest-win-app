using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFestApp.DataModel
{
    public class Track
    {
        private readonly int _roomId;
        private readonly Func<IEnumerable<Lecture>> _lecturesProvider;

        private IEnumerable<Lecture> _lectures;

        public Track(int roomId, Func<IEnumerable<Lecture>> lecturesProvider)
        {
            _roomId = roomId;
            _lecturesProvider = lecturesProvider ;
        }

        public int Id { get; set; }
        public Room Room { get; private set; }
        public string Color { get; set; }
        public string Title { get; set; }
        public IEnumerable<Lecture> Lectures
        {
            get { return _lectures ?? (_lectures = _lecturesProvider().Where(x => x.Track.Id == Id).ToArray()); }
        }

        public void SetRoom(IEnumerable<Room> rooms)
        {
            if (Room == null)
            {
                Room = rooms.SingleOrDefault(x => x.Id == _roomId);
            }
        }
    }
}
