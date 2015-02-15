using System;

namespace CodeFestApp.DataModel
{
    public class Lecture
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Day Day { get; set; }
        public Track Track { get; set; }
        public Speaker Speaker { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
