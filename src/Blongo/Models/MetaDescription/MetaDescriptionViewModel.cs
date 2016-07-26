namespace Blongo.Models.MetaDescription
{
    public class MetaDescriptionViewModel
    {
        public MetaDescriptionViewModel(Blog blog)
        {
            Blog = blog;
        }

        public Blog Blog { get; }
    }
}
