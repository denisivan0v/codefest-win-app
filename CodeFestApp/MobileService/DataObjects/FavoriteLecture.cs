using Microsoft.WindowsAzure.Mobile.Service;

namespace CodeFestApp.MobileService.DataObjects
{
    public class FavoriteLecture : EntityData
    {
        public int LectureId { get; set; }
        public string DeviceIdentity { get; set; }
    }
}