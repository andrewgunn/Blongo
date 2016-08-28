namespace RealFaviconGeneratorSdk
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    public class RealFaviconGenerator
    {
        private readonly HttpClient _httpClient;
        private readonly RealFaviconGeneratorSettings _realFaviconGeneratorSettings;

        public RealFaviconGenerator(RealFaviconGeneratorSettings realFaviconGeneratorSettings, HttpClient httpClient)
        {
            _realFaviconGeneratorSettings = realFaviconGeneratorSettings;
            _httpClient = httpClient;
        }

        public async Task<GenerateFaviconsResult> GenerateFaviconsAsync(Image image, string version)
        {
            if (string.IsNullOrWhiteSpace(_realFaviconGeneratorSettings.ApiKey))
            {
                throw new RealFaviconGeneratorApiKeyCannotBeNullOrWhitespaceException(
                    _realFaviconGeneratorSettings.ApiKey);
            }

            var requestUri = "https://realfavicongenerator.net/api/favicon";
            var requestContent =
                new StringContent(
                    $@"
            {{
                ""favicon_generation"": {{
                        ""api_key"": ""{_realFaviconGeneratorSettings
                        .ApiKey}"",
                    ""master_picture"": {{
                        ""type"": ""inline"",
                        ""content"": ""{ImageToBase64
                            (image, ImageFormat.Png)}""
                    }},
                    ""favicon_design"": {{
                        ""ios"": {{
                            ""picture_aspect"": ""no_change""
                        }},
                        ""desktop_browser"": [],
                        ""windows"": {{
                            ""picture_aspect"": ""no_change"",
                            ""background_color"": ""#0275d8"",
                            ""on_conflict"": ""override""
                        }},
                        ""android_chrome"": {{
                            ""picture_aspect"": ""no_change"",
                            ""theme_color"": ""#0275d8"",
                            ""manifest"": {{
                                ""name"": ""Andrew Gunn's Blog"",
                                ""display"": ""browser"",
                                ""orientation"": ""not_set"",
                                ""on_conflict"": ""override"",
                                ""declared"": true
                            }}
                        }},
                        ""safari_pinned_tab"": {{
                            ""picture_aspect"": ""black_and_white"",
                            ""threshold"": 50,
                            ""theme_color"": ""#0275d8""
                        }}
                    }},
                    ""settings"": {{
                        ""compression"": 3,
                        ""scaling_algorithm"": ""Mitchell"",
                        ""error_on_image_too_small"": false
                    }},
                    ""versioning"": {{
                        ""param_name"": ""v"",
                        ""param_value"": ""{version}""
                    }}
                }}
            }}",
                    Encoding.UTF8, "application/json");
            var responseMessage = await _httpClient.PostAsync(requestUri, requestContent);

            responseMessage.EnsureSuccessStatusCode();

            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            var favicon = JObject.Parse(responseContent)
                .SelectToken("favicon_generation_result.favicon");
            var fileUrls = favicon.SelectToken("files_urls")
                .Select(x => (string) x)
                .Select(x => new Uri(x))
                .ToList();
            var html = favicon.SelectToken("html_code")
                .ToString();

            return new GenerateFaviconsResult(fileUrls, html);
        }

        public string ImageToBase64(Image image, ImageFormat format)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, format);
                var imageBytes = memoryStream.ToArray();

                return Convert.ToBase64String(imageBytes);
            }
        }
    }
}