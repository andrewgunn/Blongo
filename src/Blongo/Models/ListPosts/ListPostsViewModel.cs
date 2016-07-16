using System.Collections.Generic;

namespace Blongo.Models.ListPosts
{
    public class ListPostsViewModel
    {
        public ListPostsViewModel(IReadOnlyCollection<Post> posts, Paging paging)
        {
            Posts = posts;
            Paging = paging;
        }

        public Paging Paging { get; }

        public IReadOnlyCollection<Post> Posts { get; }
    }
}
