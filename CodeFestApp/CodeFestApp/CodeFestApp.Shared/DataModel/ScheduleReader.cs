using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace CodeFestApp.DataModel
{
    public class ScheduleReader
    {
        private readonly JObject _scheduleJson;

        public ScheduleReader(string scheduleJson)
        {
            _scheduleJson = JObject.Parse(scheduleJson);
        }

        public async Task<IEnumerable<Day>> GetDaysAsync()
        {
            return await Task.Factory.StartNew(
                () => _scheduleJson["day"]
                          .Select(x => new Day
                              {
                                  Id = x.Value<int>("id"),
                                  Title = x.Value<string>("title")
                              }));
        }
    }
}
