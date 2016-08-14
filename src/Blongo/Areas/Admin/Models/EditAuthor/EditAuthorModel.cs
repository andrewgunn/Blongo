using System;
using System.ComponentModel.DataAnnotations;

namespace Blongo.Areas.Admin.Models.EditAuthor
{
    public class EditAuthorModel
    {
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }

        [Display(Name = "GitHub username")]
        public string GitHubUsername { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Twitter username")]
        public string TwitterUsername { get; set; }

        [Display(Name = "Website URL")]
        public Uri WebsiteUrl { get; set; }
    }
}
