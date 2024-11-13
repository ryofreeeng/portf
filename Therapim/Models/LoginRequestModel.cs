using System;
using System.ComponentModel.DataAnnotations;


namespace Therapim.Models
{
    /// <summary>
    /// ログイン用リクエスト情報
    /// </summary>
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "お名前をご入力ください🍊")]
        [StringLength(20, ErrorMessage = "お名前は20文字以下でご入力ください🍊")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "生年月日をご入力ください🍊")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "生年月日は西暦を含めて8桁でご入力ください🍊")]
        public string Birthday { get; set; }

        [Required(ErrorMessage = "電話番号をご入力ください🍊")]
        [StringLength(12, MinimumLength = 10, ErrorMessage = "電話番号は10～12桁でご入力ください🍊")]
        public string PhoneNumber { get; set; }

        // リダイレクトURL
        [Required(ErrorMessage = "不正な遷移です🍊")]
        public string ReturnUrl { get; set; }

    }

}
