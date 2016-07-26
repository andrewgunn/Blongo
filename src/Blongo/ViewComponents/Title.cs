using Blongo.Models.Title;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Blongo.ViewComponents
{
    public class Title : ViewComponent
    {
        public Title(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.Blog>(Data.CollectionNames.Blogs);
            var blog = await collection.Find(Builders<Data.Blog>.Filter.Empty)
                .Project(b => new Blog(b.Name))
                .SingleOrDefaultAsync();

            var viewModel = new TitleViewModel(blog);

            return View(viewModel);
        }

        private readonly MongoClient _mongoClient;
    }
}
