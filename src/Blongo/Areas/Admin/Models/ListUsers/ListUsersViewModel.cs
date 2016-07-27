using System.Collections.Generic;

namespace Blongo.Areas.Admin.Models.ListUsers
{
    public class ListUsersViewModel
    {
        public ListUsersViewModel(IReadOnlyCollection<User> users, Paging paging)
        {
            Users = users;
            Paging = paging;
        }

        public Paging Paging { get; }

        public IReadOnlyCollection<User> Users { get; }
    }
}
