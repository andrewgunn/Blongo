using System.ComponentModel.DataAnnotations;

namespace Blongo.Areas.Admin.Models.EditSettings
{
    public class EditSettingsModel
    {
        [Display(Name = "Akismet API Key")]
        public string AkismetApiKey { get; set; }

        [Display(Name = "Azure storage connection string")]
        [Required(ErrorMessage = "Please enter the Azure storage connection string")]
        public string AzureStorageConnectionString { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Feed URL")]
        public string FeedUrl { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter the name")]
        public string Name { get; set; }

        [Display(Name = "Real Favicon Generator API Key")]
        public string RealFaviconGeneratorApiKey { get; set; }

        [DataType(DataType.Html)]
        [Display(Name = "Scripts")]
        public string Scripts { get; set; }

        [DataType(DataType.Html)]
        [Display(Name = "Styles")]
        public string Styles { get; set; }
    }
}
