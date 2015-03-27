using Newtonsoft.Json;

namespace CodeFestApp.Analytics
{
    public class FavoriteLecture
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "lectureId")]
        public int LectureId { get; set; }

        [JsonProperty(PropertyName = "deviceIdentity")]
        public string DeviceIdentity { get; set; }
    }
}
