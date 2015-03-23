using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

using CodeFestApp.MobileService.DataObjects;
using CodeFestApp.MobileService.Models;

using Microsoft.WindowsAzure.Mobile.Service;

namespace CodeFestApp.MobileService.Controllers
{
    public class FavoriteLectureController : TableController<FavoriteLecture>
    {
        [Route("favorite/lectures/check/{deviceIdentity}/{lectureId}")]
        public bool GetIsLectureInFavorites(string deviceIdentity, int lectureId)
        {
            return Query().Any(x => x.DeviceIdentity == deviceIdentity &&
                                    x.LectureId == lectureId);
        }

        [Route("favorite/lectures/{deviceIdentity}")]
        public IQueryable<FavoriteLecture> GetFavoriteLectures(string deviceIdentity)
        {
            return Query().Where(x => x.DeviceIdentity == deviceIdentity);
        }

        [Route("favorite/lectures/add/{deviceIdentity}/{lectureId}", Name = "AddToFavotites")]
        public async Task<IHttpActionResult> PostToFavorites(string deviceIdentity, int lectureId)
        {
            var exists = GetIsLectureInFavorites(deviceIdentity, lectureId);
            if (exists)
            {
                return Ok();
            }

            var current = await InsertAsync(new FavoriteLecture
                {
                    LectureId = lectureId,
                    DeviceIdentity = deviceIdentity
                });

            return CreatedAtRoute("AddToFavotites", new { id = current.Id }, current);
        }

        [Route("favorite/lectures/remove/{deviceIdentity}/{lectureId}")]
        public Task DeleteromFavorites(string deviceIdentity, int lectureId)
        {
            var existing = Query().SingleOrDefault(x => x.DeviceIdentity == deviceIdentity &&
                                                      x.LectureId == lectureId);
            if (existing == null)
            {
                return Task.FromResult(true);
            }

            return DeleteAsync(existing.Id);
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            var context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<FavoriteLecture>(context, Request, Services);
        }
    }
}