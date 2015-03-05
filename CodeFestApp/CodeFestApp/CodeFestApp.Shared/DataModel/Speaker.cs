using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFestApp.DataModel
{
    public class Speaker
    {
        private readonly int _companyId;
        private readonly Func<IEnumerable<Lecture>> _lecturesProvider;

        private IEnumerable<Lecture> _lectures;

        public Speaker(int companyId, Func<IEnumerable<Lecture>> lecturesProvider)
        {
            _companyId = companyId;
            _lecturesProvider = lecturesProvider;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string JobTitle { get; set; }
        public Uri Avatar { get; set; }
        public Company Company { get; private set; }
        public Uri FacebookProfile { get; set; }
        public Uri MoiKrugProfile { get; set; }
        public Uri TwitterProfile { get; set; }
        public Uri LinkedInProfile { get; set; }
        public Uri VkontakteProfile { get; set; }
        public IEnumerable<Lecture> Lectures
        {
            get { return _lectures ?? (_lectures = _lecturesProvider().Where(x => x.Speaker != null && x.Speaker.Id == Id).ToArray()); }
        }

        public void SetCompany(IEnumerable<Company> companies)
        {
            if (Company == null)
            {
                Company = companies.SingleOrDefault(x => x.Id == _companyId);
            }
        }
    }
}
