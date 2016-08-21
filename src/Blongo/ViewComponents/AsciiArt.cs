﻿using Blongo.Models.AsciiArt;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blongo.ViewComponents
{
    public class AsciiArt : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Content("");
            }

            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync($"http://artii.herokuapp.com/make?text={WebUtility.UrlEncode(text)}");
                httpResponseMessage.EnsureSuccessStatusCode();

                var viewModel = new AsciiArtModel(await httpResponseMessage.Content.ReadAsStringAsync());

                return View(viewModel);
            }
        }
    }
}
