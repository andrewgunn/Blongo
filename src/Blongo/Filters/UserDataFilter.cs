using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Blongo.Filters
{
    public class UserDataFilter : ActionFilterAttribute
    {
        public UserDataFilter(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var emailAddress = context.HttpContext.User.Identity.Name;
                var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
                var collection = database.GetCollection<Data.User>(Data.CollectionNames.Users);
                var user = await collection.Find(Builders<Data.User>.Filter.Where(u => u.EmailAddress == emailAddress))
                    .SingleOrDefaultAsync();

                if (user == null)
                {
                    await context.HttpContext.Authentication.SignOutAsync("Cookies");

                    // Force a refresh, otherwise the current request is still authenticated.
                    context.Result = new RedirectToRouteResult(context.RouteData.Values);
                }
                else
                {
                    ((dynamic)context.Controller).ViewBag._UserEmailAddress = user.EmailAddress;
                }
            }

            await next();
        }

        private readonly MongoClient _mongoClient;
    }
}
