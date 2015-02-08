using System;
using System.Threading.Tasks;

using Windows.Storage;

namespace CodeFestApp.DataModel
{
    public class ScheduleSource : IScheduleSource
    {
        private readonly Uri _dataUri;

        public ScheduleSource()
        {
            _dataUri = new Uri("ms-appx:///DataModel/schedule.json");
        }

        public async Task<string> ReadScheduleAsync()
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(_dataUri);
            return await FileIO.ReadTextAsync(file);
        }
    }
}
