namespace Blongo.Models.TopNavbar
{
    public class TopNavbarViewModel
    {
        public TopNavbarViewModel(User user)
        {
            User = user;
        }

        public User User { get; }
    }
}
