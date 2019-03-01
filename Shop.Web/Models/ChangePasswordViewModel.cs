namespace Shop.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ChangePasswordViewModel
    {
        [Required]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string Confirm { get; set; }
    }
}
