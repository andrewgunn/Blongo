namespace Blongo.ViewComponents
{
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models.PostStyles;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public class PostStyles : ViewComponent
    {
        private readonly MongoClient _mongoClient;

        public PostStyles(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IViewComponentResult> InvokeAsync(ObjectId postId)
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Post>(CollectionNames.Posts);
            var post = await collection.Find(Builders<Post>.Filter.Where(p => p.Id == postId))
                .Project(p => new
                {
                    p.Styles
                })
                .SingleOrDefaultAsync();

            if (post == null)
            {
                return Content("");
            }

            var viewModel = new PostStylesViewModel(post.Styles);

            return View(viewModel);
        }
    }
}