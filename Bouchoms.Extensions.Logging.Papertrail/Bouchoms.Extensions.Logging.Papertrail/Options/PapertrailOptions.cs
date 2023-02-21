using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace Bouchoms.Extensions.Logging.Papertrail
{
    public class PapertrailOptions
    {
        [Required]
        public string AccessToken { get; set; }
        
        [Required]
        public string Url { get; set; }
        
        public static readonly string ConfigurationSection = "Bouchoms.Extensions.Logging.Papertrail";
    }
}