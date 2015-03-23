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

        [Route("favorite/lectures/add", Name = "AddLectureToFavotites")]
        public async Task<IHttpActionResult> PostLectureToFavorites(FavoriteLecture favorite)
        {
            var exists = GetIsLectureInFavorites(favorite.DeviceIdentity, favorite.LectureId);
            if (exists)
            {
                return Ok();
            }

            var current = await InsertAsync(favorite);
            return CreatedAtRoute("AddFavotiteLecture", new { id = current.Id }, current);
        }

        [Route("favorite/lectures/remove/{id}", Name = "RemoveLectureFromFavorites")]
        public Task DeleteLectureFromFavorites(string id)
        {
            var exists = Query().Any(x => x.Id == id);
            if (!exists)
            {
                return Task.FromResult(true);
            }

            return DeleteAsync(id);
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            var context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<FavoriteLecture>(context, Request, Services);
        }
    }
}