using System.ComponentModel.DataAnnotations;

namespace Microsoft.Extensions.Logging.Papertrail
{
    public class PapertrailOptions
    {
        [Required]
        public string AccessToken { get; set; }
        
        [Required]
        public string Url { get; set; }
        
        [Required]
        public LogLevel LogLevel { get; set; }
    }
}