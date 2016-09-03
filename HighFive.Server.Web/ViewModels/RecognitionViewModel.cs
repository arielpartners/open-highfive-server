using System;

namespace HighFive.Server.Web.ViewModels
{
    public class RecognitionViewModel
    {
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string OrganizationName { get; set; }
        public string CorporateValueName { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
    }
}
