﻿#region references

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HighFive.Server.Models;

#endregion

namespace HighFive.Server.ViewModels
{
    public class OrganizationViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        [Required]
        //public string CorporateValueName { get; set; }
        public ICollection<CorporateValue> Values { get; set; }


    }
}