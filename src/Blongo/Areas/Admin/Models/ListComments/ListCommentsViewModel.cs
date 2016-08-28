namespace Blongo.Areas.Admin.Models.ListComments
{
    using System.Collections.Generic;

    public class ListCommentsViewModel
    {
        public ListCommentsViewModel(IReadOnlyCollection<Comment> comments, Paging paging,
            SelectedComment selectedComment)
        {
            Comments = comments;
            Paging = paging;
            SelectedComment = selectedComment;
        }

        public IReadOnlyCollection<Comment> Comments { get; }

        public Paging Paging { get; }

        public SelectedComment SelectedComment { get; }
    }
}