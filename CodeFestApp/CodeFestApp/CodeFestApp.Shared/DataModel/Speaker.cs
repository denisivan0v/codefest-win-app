using System;
using System.Collections.Generic;

namespace CodeFestApp.DataModel
{
    public class Speaker
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string JobTitle { get; set; }
        public Uri Avatar { get; set; }
        public Company Company { get; set; }
        public Uri FacebookProfile { get; set; }
        public Uri MoiKrugProfile { get; set; }
        public Uri TwitterProfile { get; set; }
        public Uri LinkedInProfile { get; set; }
        public Uri VkontakteProfile { get; set; }
        public IEnumerable<Lecture> Lectures { get; set; } 
    }
}
