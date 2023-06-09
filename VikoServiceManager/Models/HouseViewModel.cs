﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VikoServiceManager.Models
{
    public class HouseViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? Region { get; set; }
        [Required]
        public string? PostalCode { get; set; }
        [DefaultValue(true)]
        public ResidentGroupViewModel? ResidentGroup { get; set; }
        [NotMapped]
        public string? ResidentGroupName { get; set; }
        [NotMapped]
        public string? ServiceName { get; set; }

        [NotMapped]
        public string? ResidentGroupSelected { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? ResidentGroupList { get; set; }

        [NotMapped]
        public string? ServiceSelected { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? ServiceSelectedList { get; set; }
    }
}
