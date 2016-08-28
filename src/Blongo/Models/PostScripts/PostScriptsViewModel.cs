namespace Blongo.Models.PostScripts
{
    public class PostScriptsViewModel
    {
        public PostScriptsViewModel(string scripts)
        {
            Scripts = scripts;
        }

        public string Scripts { get; }
    }
}