using System.Collections.Generic;

namespace Blongo.Areas.Admin.Models.ListPosts
{
    public class ListPostsViewModel
    {
        public ListPostsViewModel(IReadOnlyCollection<Post> posts, Paging paging, SelectedPost selectedPost)
        {
            Posts = posts;
            Paging = paging;
            SelectedPost = selectedPost;
        }

        public Paging Paging { get; }

        public IReadOnlyCollection<Post> Posts { get; }

        public SelectedPost SelectedPost { get; }
    }
}
