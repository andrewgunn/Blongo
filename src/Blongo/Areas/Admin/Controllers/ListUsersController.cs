using Blongo.Areas.Admin.Models.ListUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Blongo.ModelBinding;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    [Route("admin/users", Name = "AdminListUsers")]
    public class ListUsersController : Controller
    {
        public ListUsersController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId? id, [FromQuery(Name = "p")] int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                return NotFound();
            }

            const int pageSize = 10;
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.User>(Data.CollectionNames.Users);
            var filter = Builders<Data.User>.Filter.Empty;
            var totalCount = await collection.CountAsync(filter);
            var maximumPageNumber = Math.Max(1, (int)Math.Ceiling((double)totalCount / pageSize));

            if (pageNumber > maximumPageNumber)
            {
                return NotFound();
            }

            var users = new List<User>();

            if (totalCount > 0)
            {
                users = await collection.Find(filter)
                    .Sort(Builders<Data.User>.Sort.Descending(p => p.Id))
                    .Skip((pageNumber - 1) * pageSize)
                    .Limit(pageSize)
                    .Project(u => new User(u.Id, u.Role.ToString() /* // TODO Convert to a propert "string" */, u.Name, u.EmailAddress))
                    .ToListAsync();
            }

            var paging = new Paging(pageNumber, pageSize, maximumPageNumber);
            var viewModel = new ListUsersViewModel(users, paging);

            return View(viewModel);
        }

        private readonly MongoClient _mongoClient;
    }
}
