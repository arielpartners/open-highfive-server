using System;
using System.ComponentModel.DataAnnotations;

namespace HighFive.Server.Web.ViewModels
{
    public class RecognitionViewModel
    {
        public int Id { get; set; }
        [Required]
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        [Required]
        public string SenderEmail { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverEmail { get; set; }
        [Required]
        public string ReceiverEmail { get; set; }
        [Required]
        public string OrganizationName { get; set; }
        [Required]
        public string CorporateValueName { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
    }
}
