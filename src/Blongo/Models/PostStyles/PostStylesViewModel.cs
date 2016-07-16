namespace Blongo.Models.PostStyles
{
    public class PostStylesViewModel
    {
        public PostStylesViewModel(string styles)
        {
            Styles = styles;
        }

        public string Styles { get; }
    }
}
