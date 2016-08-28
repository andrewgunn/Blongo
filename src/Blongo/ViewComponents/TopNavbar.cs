namespace Blongo.ViewComponents
{
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models.TopNavbar;
    using MongoDB.Driver;
    using User = Data.User;

    public class TopNavbar : ViewComponent
    {
        private readonly MongoClient _mongoClient;

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
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<User>(CollectionNames.Users);
            var user = await collection.Find(Builders<User>.Filter.Where(u => u.EmailAddress == emailAddress))
                .Project(u => new Models.TopNavbar.User(u.Name, u.EmailAddress))
                .SingleOrDefaultAsync();

            var viewModel = new TopNavbarViewModel(user);

            return View(viewModel);
        }
    }
}