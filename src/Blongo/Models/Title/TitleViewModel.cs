namespace Blongo.Models.Title
{
    public class TitleViewModel
    {
        public TitleViewModel(Blog blog)
        {
            Blog = blog;
        }

        public Blog Blog { get; }
    }
}