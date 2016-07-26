namespace Blongo.Models.Sidebar
{
    public class SidebarViewModel
    {
        public SidebarViewModel(Blog blog)
        {
            Blog = blog;
        }

        public Blog Blog { get; }
    }
}
