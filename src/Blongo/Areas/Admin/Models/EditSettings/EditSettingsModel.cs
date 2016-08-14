using System;
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

        [Display(Name = "Company name")]
        public string CompanyName { get; set; }

        [Display(Name = "Company website URL")]
        public Uri CompanyWebsiteUrl { get; set; }

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

        [Display(Name = "SendGrid From Email Address")]
        [Required(ErrorMessage = "Please enter the SendGrid from email address ")]
        public string SendGridFromEmailAddress { get; set; }

        [Display(Name = "SendGrid Password")]
        [Required(ErrorMessage = "Please enter the SendGrid password")]
        public string SendGridPassword { get; set; }

        [Display(Name = "SendGrid Username")]
        [Required(ErrorMessage = "Please enter the SendGrid username")]
        public string SendGridUsername { get; set; }

        [DataType(DataType.Html)]
        [Display(Name = "Styles")]
        public string Styles { get; set; }
    }
}
