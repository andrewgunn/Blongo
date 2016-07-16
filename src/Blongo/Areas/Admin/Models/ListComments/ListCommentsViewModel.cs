using System.Collections.Generic;

namespace Blongo.Areas.Admin.Models.ListComments
{
    public class ListCommentsViewModel
    {
        public ListCommentsViewModel(IReadOnlyCollection<Comment> comments, Paging paging, SelectedComment selectedComment)
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
