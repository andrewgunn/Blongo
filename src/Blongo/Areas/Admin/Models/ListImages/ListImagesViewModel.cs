﻿namespace Blongo.Areas.Admin.Models.ListImages
{
    using System;
    using System.Collections.Generic;

    public class ListImagesViewModel
    {
        public ListImagesViewModel(IReadOnlyCollection<Uri> imageUrls, Paging paging)
        {
            ImageUrls = imageUrls;
            Paging = paging;
        }

        public IReadOnlyCollection<Uri> ImageUrls { get; }

        public Paging Paging { get; }
    }
}