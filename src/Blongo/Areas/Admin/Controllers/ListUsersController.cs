namespace Blongo.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ModelBinding;
    using Models.ListUsers;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using User = Data.User;

    [Area("admin")]
    [Authorize]
    [Route("admin/users", Name = "AdminListUsers")]
    public class ListUsersController : Controller
    {
        private readonly MongoClient _mongoClient;

        public ListUsersController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId? id,
            [FromQuery(Name = "p")] int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                return NotFound();
            }

            const int pageSize = 10;
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<User>(CollectionNames.Users);
            var filter = Builders<User>.Filter.Empty;
            var totalCount = await collection.CountAsync(filter);
            var maximumPageNumber = Math.Max(1, (int) Math.Ceiling((double) totalCount/pageSize));

            if (pageNumber > maximumPageNumber)
            {
                return NotFound();
            }

            var users = new List<Models.ListUsers.User>();

            if (totalCount > 0)
            {
                users = await collection.Find(filter)
                    .Sort(Builders<User>.Sort.Descending(p => p.Id))
                    .Skip((pageNumber - 1)*pageSize)
                    .Limit(pageSize)
                    .Project(u => new Models.ListUsers.User(u.Id, u.Role, u.Name, u.EmailAddress))
                    .ToListAsync();
            }

            var paging = new Paging(pageNumber, pageSize, maximumPageNumber);
            var viewModel = new ListUsersViewModel(users, paging);

            return View(viewModel);
        }
    }
}