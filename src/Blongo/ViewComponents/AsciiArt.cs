using Blongo.Models.AsciiArt;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blongo.ViewComponents
{
    public class AsciiArt : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var text = WebUtility.UrlEncode("Blongo");

            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync($"http://artii.herokuapp.com/make?text={text}");
                httpResponseMessage.EnsureSuccessStatusCode();

                var viewModel = new AsciiArtModel(await httpResponseMessage.Content.ReadAsStringAsync());

                return View(viewModel);
            }
        }
    }
}
