using System;

namespace WebUI.MVC.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public string Message { get; set; }
        public string Trace { get; set; }
        public bool ShowAdditionalInfo { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
