using System;
using System.ComponentModel.DataAnnotations;


namespace Therapim.Models
{
    /// <summary>
    /// ログイン用リクエスト情報
    /// </summary>
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Birthday is required")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Telephone number cannot be longer than 8 characters and shorter than 8 characters")]
        public string Birthday { get; set; }

        [Required(ErrorMessage = "Telephone number is required")]
        [StringLength(12, MinimumLength = 10, ErrorMessage = "Telephone number cannot be longer than 12 characters and shorter than 10 characters")]
        public string PhoneNumber { get; set; }

    }

}
