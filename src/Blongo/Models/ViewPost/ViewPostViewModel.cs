namespace Blongo.Models.ViewPost
{
    using System.Collections.Generic;

    public class ViewPostViewModel
    {
        public ViewPostViewModel(Post post, IReadOnlyCollection<Comment> comments, CreateCommentModel createCommentModel,
            SiblingPost previousPost, SiblingPost nextPost)
        {
            Post = post;
            Comments = comments;
            CreateCommentModel = createCommentModel;
            PreviousPost = previousPost;
            NextPost = nextPost;
        }

        public IReadOnlyCollection<Comment> Comments { get; }

        public CreateCommentModel CreateCommentModel { get; }

        public SiblingPost NextPost { get; }

        public Post Post { get; }

        public SiblingPost PreviousPost { get; }
    }
}