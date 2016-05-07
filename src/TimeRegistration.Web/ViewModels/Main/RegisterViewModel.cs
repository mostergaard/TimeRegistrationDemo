using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TimeRegistration.Web.Models;

namespace TimeRegistration.Web.ViewModels.Main
{
    public class RegisterViewModel
    {
        public IList<Customer> CustomersAndProjects { get; set; }

        public IList<string> AllCustomers { get; set; }

        public IList<string> AllProjects { get; set; }

        [Required]
        [Display(Name = "Select Customer")]
        public string Customer { get; set; }

        [Required]
        [Display(Name = "Select Project")]
        public string Project { get; set; }

        [Required]
        [Display(Name = "Choose Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Number of hours")]
        public double Hours { get; set; }

        [Display(Name = "Notes")]
        [StringLength(200, ErrorMessage = "The {0} must be maximum {1} characters long.")]

        public string Notes { get; set; }
    }
}
