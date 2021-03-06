﻿namespace Blongo.Areas.Admin.Models.EditPost
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using MongoDB.Bson;

    public class EditPostModel
    {
        [Display(Name = "Body")]
        [DataType(DataType.Html)]
        [Required(ErrorMessage = "Please enter the body")]
        public string Body { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Please enter the description")]
        public string Description { get; set; }

        public ObjectId Id { get; set; }

        public bool IsPublished { get; set; }

        [Display(Name = "Published date")]
        [Required(ErrorMessage = "Please enter the publish date")]
        public DateTime? PublishedAt { get; set; }

        [Display(Name = "Scripts")]
        [DataType(DataType.Html)]
        public string Scripts { get; set; }

        [Display(Name = "Styles")]
        [DataType(DataType.Html)]
        public string Styles { get; set; }

        [Display(Name = "Tags")]
        public string[] Tags { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Please enter the title")]
        public string Title { get; set; }
    }
}