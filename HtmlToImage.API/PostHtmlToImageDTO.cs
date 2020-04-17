using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HtmlToImage.API
{
    public class PostHtmlToImageDTO
    {
        public string HtmlBase64 { get; set; }
        public string Html { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
    }
}
