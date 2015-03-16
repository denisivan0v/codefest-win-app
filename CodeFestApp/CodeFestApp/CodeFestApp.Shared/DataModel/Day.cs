using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFestApp.DataModel
{
    public class Day
    {
        private readonly Func<IEnumerable<Lecture>> _lecturesProvider;
        private IEnumerable<Lecture> _lectures;

        public Day(Func<IEnumerable<Lecture>> lecturesProvider)
        {
            _lecturesProvider = lecturesProvider;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<Lecture> Lectures
        {
            get { return _lectures ?? (_lectures = _lecturesProvider().Where(x => x.Day.Id == Id).ToArray()); }
        }
    }
}
