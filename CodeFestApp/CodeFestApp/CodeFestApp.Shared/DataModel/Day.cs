using System;
using System.Collections.Generic;

namespace CodeFestApp.DataModel
{
    public class Day
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<Lecture> Lectures { get; set; } 
    }
}
