namespace Blongo.Models.Favicon
{
    public class FaviconsViewModel
    {
        public FaviconsViewModel(string faviconsHtml)
        {
            FaviconsHtml = faviconsHtml;
        }

        public string FaviconsHtml { get; }
    }
}
