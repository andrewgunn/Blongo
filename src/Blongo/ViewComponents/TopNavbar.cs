using Blongo.Models.TopNavbar;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Blongo.ViewComponents
{
    public class TopNavbar : ViewComponent
    {
        public TopNavbar(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Content("");
            }

            var emailAddress = HttpContext.User.Identity.Name;
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.User>(Data.CollectionNames.Users);
            var user = await collection.Find(Builders<Data.User>.Filter.Where(u => u.EmailAddress == emailAddress))
                .Project(u => new User(u.Name, u.EmailAddress))
                .SingleOrDefaultAsync();

            var viewModel = new TopNavbarViewModel(user);

            return View(viewModel);
        }

        private readonly MongoClient _mongoClient;
    }
}
