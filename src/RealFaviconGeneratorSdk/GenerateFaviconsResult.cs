namespace RealFaviconGeneratorSdk
{
    using System;
    using System.Collections.Generic;

    public class GenerateFaviconsResult
    {
        public GenerateFaviconsResult(IEnumerable<Uri> fileUrls, string html)
        {
            FileUrls = fileUrls;
            Html = html;
        }

        public IEnumerable<Uri> FileUrls { get; }

        public string Html { get; }
    }
}