﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TimeRegistration.BusinessLogic.Models;

namespace TimeRegistration.Web.ViewModels.Main
{
    public class RegisterViewModel
    {
        public IList<Customer> PossibleCustomers { get; set; }

        [Required]
        [Display(Name = "Select Customer")]
        public Guid CustomerId { get; set; }

        [Required]
        [Display(Name = "Select Project")]
        public Guid ProjectId { get; set; }

        [Required]
        [Display(Name = "Choose Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        [Range(0.01, 24)]
        [Display(Name = "Number of hours")]
        [DisplayFormat(DataFormatString = "{0:F1}", ApplyFormatInEditMode = true)]
        public double Hours { get; set; }

        [Display(Name = "Notes")]
        [StringLength(200, ErrorMessage = "The {0} must be maximum {1} characters long.")]
        public string Notes { get; set; }
    }
}
