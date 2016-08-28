namespace Blongo.Models.BlogStyles
{
    public class BlogStylesViewModel
    {
        public BlogStylesViewModel(string styles)
        {
            Styles = styles;
        }

        public string Styles { get; }
    }
}