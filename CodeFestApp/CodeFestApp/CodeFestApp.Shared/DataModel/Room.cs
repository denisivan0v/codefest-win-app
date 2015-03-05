using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFestApp.DataModel
{
    public class Room
    {
        private readonly Func<IEnumerable<Lecture>> _lecturesProvider;
        private IEnumerable<Lecture> _lectures;

        public Room(Func<IEnumerable<Lecture>> lecturesProvider)
        {
            _lecturesProvider = lecturesProvider;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<Lecture> Lectures
        {
            get { return _lectures ?? (_lectures = _lecturesProvider().ToArray()); }
        }
    }
}
