using System.Collections.Generic;
using System.Drawing;

namespace CodeFestApp.DataModel
{
    public class Track
    {
        public int Id { get; set; }
        public Room Room { get; set; }
        public Color Color { get; set; }
        public string Title { get; set; }
        public IEnumerable<Lecture> Lectures { get; set; } 
    }
}
