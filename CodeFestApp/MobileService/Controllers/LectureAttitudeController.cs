using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

using CodeFestApp.MobileService.DataObjects;
using CodeFestApp.MobileService.Models;

using Microsoft.WindowsAzure.Mobile.Service;

namespace CodeFestApp.MobileService.Controllers
{
    public class LectureAttitudeController : TableController<LectureAttitude>
    {
        [Route("attitude/{deviceIdentity}/{lectureId}")]
        public IQueryable<LectureAttitude> GetAttitudesForDevice(string deviceIdentity, int lectureId)
        {
            return Query().Where(x => x.DeviceIdentity == deviceIdentity &&
                                      x.LectureId == lectureId);
        }

        [Route("like/{deviceIdentity}/{lectureId}", Name = "Like")]
        public async Task<IHttpActionResult> PostLike(string deviceIdentity, int lectureId)
        {
            var current = await InsertAsync(new LectureAttitude
                {
                    DeviceIdentity = deviceIdentity,
                    LectureId = lectureId,
                    Attitude = Attitude.Like,
                });
            return CreatedAtRoute("Like", new { id = current.Id }, current);
        }

        [Route("dislike/{deviceIdentity}/{lectureId}", Name = "Dislike")]
        public async Task<IHttpActionResult> PostDislike(string deviceIdentity, int lectureId)
        {
            var current = await InsertAsync(new LectureAttitude
                {
                    DeviceIdentity = deviceIdentity,
                    LectureId = lectureId,
                    Attitude = Attitude.Dislike,
                });
            return CreatedAtRoute("Dislike", new { id = current.Id }, current);
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            var context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<LectureAttitude>(context, Request, Services);
        }
    }
}