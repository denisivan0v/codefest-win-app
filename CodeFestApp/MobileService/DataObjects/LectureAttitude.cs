using Microsoft.WindowsAzure.Mobile.Service;

namespace CodeFestApp.MobileService.DataObjects
{
    public class LectureAttitude : EntityData
    {
        public int LectureId { get; set; }
        public Attitude Attitude { get; set; }
        public string DeviceIdentity { get; set; }
    }
}