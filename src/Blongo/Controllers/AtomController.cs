using MarkdownSharp;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;

namespace Blongo.Controllers
{
    [ResponseCache(Duration = 86400)]
    [Route("atom", Name = "Atom")]
    public class AtomController : Controller
    {
        public AtomController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IActionResult> Index([FromQuery(Name = "p")] int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                return NotFound();
            }

            const int pageSize = 10;
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var blogsCollection = database.GetCollection<Data.Blog>(Data.CollectionNames.Blogs);
            var postsCollection = database.GetCollection<Data.Post>(Data.CollectionNames.Posts);

            var blog = await blogsCollection.Find(Builders<Data.Blog>.Filter.Empty)
                .Project(b => new
                {
                    Name = SyndicationContent.CreatePlaintextContent(b.Name),
                    Author = new
                    {
                        b.Author.Name,
                        b.Author.EmailAddress,
                        b.Author.WebsiteUrl
                    },
                    Url = Url.RouteUrl("ListPosts", null, Request.Scheme)
                })
                .SingleOrDefaultAsync();

            var postFilter = Builders<Data.Post>.Filter.Where(p => p.IsPublished && p.PublishedAt <= DateTime.UtcNow);
            var totalCount = await postsCollection.CountAsync(postFilter);
            var maximumPageNumber = (int)Math.Ceiling((double)totalCount / pageSize);

            var syndicationItems = new List<SyndicationItem>();

            if (totalCount > 0)
            {
                var posts = await postsCollection.Find(postFilter)
                .Sort(Builders<Data.Post>.Sort.Descending(p => p.PublishedAt))
                .Limit(pageSize)
                .Project(p => new
                {
                    p.Title,
                    p.Description,
                    p.Body,
                    p.Tags,
                    Url = Url.RouteUrl("ViewPost", new { id = p.Id, slug = p.UrlSlug }, Request.Scheme, Request.Host.ToUriComponent()),
                    CreatedAt = p.Id.CreationTime,
                    p.LastUpdatedAt
                })
                .ToListAsync();

                foreach (var post in posts)
                {
                    var syndicationItem = new SyndicationItem
                    {
                        Id = post.Url,
                        Title = SyndicationContent.CreatePlaintextContent(post.Title),
                        Summary = SyndicationContent.CreatePlaintextContent(post.Description ?? post.Title),
                        Content = SyndicationContent.CreateHtmlContent(new Markdown().Transform(post.Body)),
                        PublishDate = new DateTimeOffset(post.CreatedAt),
                        LastUpdatedTime = new DateTimeOffset(post.LastUpdatedAt ?? post.CreatedAt),
                        Copyright = new TextSyndicationContent($"Copyright © {DateTime.UtcNow.Year} {blog?.Author?.Name}"),
                    };

                    syndicationItem.Authors.Add(new SyndicationPerson(blog?.Author.EmailAddress, blog?.Author.Name, blog?.Author.WebsiteUrl));

                    syndicationItem.Contributors.Add(new SyndicationPerson(blog?.Author.EmailAddress, blog?.Author.Name, blog?.Author.WebsiteUrl));

                    foreach (var tag in post.Tags)
                    {
                        syndicationItem.Categories.Add(new SyndicationCategory(tag.Value));
                    }

                    syndicationItem.Links.Add(new SyndicationLink(new Uri(post.Url)));

                    syndicationItems.Add(syndicationItem);
                }
            }

            var syndicationFeed = new SyndicationFeed
            {
                Id = blog?.Url,
                Title = blog?.Name,
                Items = syndicationItems
            };
            
            syndicationFeed.ElementExtensions.Add(new SyndicationElementExtension("icon", null, Url.RouteUrl("AtomIcon", null, Request.Scheme, Request.Host.ToUriComponent())));

            syndicationFeed.Links.Add(new SyndicationLink(new Uri(blog?.Url), "self", null, null, 0));

            if (maximumPageNumber > 0)
            {
                syndicationFeed.Links.Add(new SyndicationLink(new Uri(GetFeedUrl(1)), "first", null, null, 0));

                if (pageNumber > 1)
                {
                    syndicationFeed.Links.Add(new SyndicationLink(new Uri(GetFeedUrl(pageNumber - 1)), "previous", null, null, 0));
                }
                if (pageNumber < maximumPageNumber)
                {
                    syndicationFeed.Links.Add(new SyndicationLink(new Uri(GetFeedUrl(pageNumber + 1)), "next", null, null, 0));
                }

                syndicationFeed.Links.Add(new SyndicationLink(new Uri(GetFeedUrl(maximumPageNumber)), "last", null, null, 0));
            }

            return new AtomResult(syndicationFeed);
        }

        private string GetFeedUrl(int pageNumber)
        {
            return Url.RouteUrl("Atom", new { p = pageNumber }, Request.Scheme, Request.Host.ToUriComponent());
        }

        private readonly MongoClient _mongoClient;
    }
}
