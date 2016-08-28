namespace Blongo.Models.ListPosts
{
    using System.Collections.Generic;

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