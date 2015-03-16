using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
            var buffer = await FileIO.ReadBufferAsync(file);
            var fileContent = new byte[buffer.Length];
            buffer.CopyTo(fileContent);
            return Encoding.UTF8.GetString(fileContent, 0, fileContent.Length);
        }
    }
}
