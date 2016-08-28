namespace Blongo.Models.BlogScripts
{
    public class BlogScriptsViewModel
    {
        public BlogScriptsViewModel(string scripts)
        {
            Scripts = scripts;
        }

        public string Scripts { get; }
    }
}