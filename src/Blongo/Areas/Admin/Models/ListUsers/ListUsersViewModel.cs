namespace Blongo.Areas.Admin.Models.ListUsers
{
    using System.Collections.Generic;

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