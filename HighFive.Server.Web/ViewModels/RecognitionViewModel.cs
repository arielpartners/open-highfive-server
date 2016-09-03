using System;
using System.ComponentModel.DataAnnotations;

namespace HighFive.Server.Web.ViewModels
{
    public class RecognitionViewModel
    {
        [Required]
        public string SenderName { get; set; }
        [Required]
        public string ReceiverName { get; set; }
        [Required]
        public string OrganizationName { get; set; }
        [Required]
        public string CorporateValueName { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
    }
}
