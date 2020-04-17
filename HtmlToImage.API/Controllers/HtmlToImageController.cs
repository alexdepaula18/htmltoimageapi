using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using System;
using System.Text;
using System.Threading.Tasks;

namespace HtmlToImage.API.Controllers
{
    [Route("api/HtmlToImage")]
    [ApiController]
    public class HtmlToImageController : ControllerBase
    {
        private readonly ILogger<HtmlToImageController> _logger;

        public HtmlToImageController(ILogger<HtmlToImageController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post(PostHtmlToImageDTO dto)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

#if true == DEBUG
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
#endif

            ViewPortOptions defaultViewport = null;
            if (dto.Width.HasValue && dto.Height.HasValue)
                defaultViewport = new ViewPortOptions
                {
                    Width = dto.Width.Value,
                    Height = dto.Height.Value
                };

            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = new[] { "--no-sandbox" },
                DefaultViewport = defaultViewport
            }))
            using (var page = await browser.NewPageAsync())
            {
                string text = null;

                if (!string.IsNullOrWhiteSpace(dto.HtmlBase64))
                {
                    var buff = Convert.FromBase64String(dto.HtmlBase64);
                    text = UTF8Encoding.UTF8.GetString(buff);
                }
                else if (!string.IsNullOrWhiteSpace(dto.Html))
                {
                    text = dto.Html;
                }

                await page.SetContentAsync(text);
                var image = await page.ScreenshotDataAsync();

                return new FileContentResult(image, "image/png");
            }
        }
    }
}