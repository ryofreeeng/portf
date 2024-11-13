using System;
using System.ComponentModel.DataAnnotations;


namespace Therapim.Models
{
    /// <summary>
    /// レビュー用リクエスト情報
    /// </summary>
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ReviewRequestModel
    {
        // レビュー番号 (nullable)
        public int? ReviewId { get; set; }

        // 投稿者 (必須、文字列長最大30)
        [Required(ErrorMessage = "投稿者名をご入力ください🍊")]
        [StringLength(30, ErrorMessage = "投稿者名は30文字以内で入力してください🍊")]
        public string Author { get; set; }

        // 性別 (必須、文字列長最大10)
        [Required(ErrorMessage = "性別をご入力ください🍊")]
        [RegularExpression("^(女性)$", ErrorMessage = "性別は「女性」のみ選択できます。")]
        public string Gender { get; set; }

        // 年齢 (必須、範囲1〜80)
        //[Required(ErrorMessage = "年齢は必須項目です🍊")]
        [Range(1, 80, ErrorMessage = "年齢をご選択ください🍊")]
        public int Age { get; set; }

        // 投稿日時 (任意)
        public DateTime? PostedDateTime { get; set; }

        // レビュータイトル (必須、文字列長最大200)
        [Required(ErrorMessage = "タイトルをご入力ください🍊")]
        [StringLength(50, ErrorMessage = "タイトルは50文字以内で入力してください🍊")]
        public string Title { get; set; }

        // レビュー内容 (必須、文字列長最大1000)
        [Required(ErrorMessage = "内容をご入力ください🍊")]
        [StringLength(1000, ErrorMessage = "内容は1000文字以内で入力してください🍊")]
        public string Content { get; set; }

        // 満足度 (必須、範囲1〜5)
        //[Required(ErrorMessage = "満足度は必須項目です🍊")]
        [Range(1, 5, ErrorMessage = "満足度を★～★★★★★からご選択ください🍊")]
        public int Rating { get; set; }

        // コース番号 (任意)
        public int? CourseId { get; set; }

        // userId　画面にない項目のため、Required属性はつけずにアクションメソッド内で手動バリデーション実行する        
        [StringLength(100, ErrorMessage = "ユーザIDの桁数が超過しています🍊こちらは入力項目ではないため、お手数ですがもう一度ログインからやり直してください")]
        public string? UserId { get; set; }

        // 表示フラグ (任意)
        public bool? IsVisible { get; set; }


        //登録セッションID
        public string? CreatedSessionId {  get; set; }

        //更新セッションID
        public string? UpdatedSessionId { get; set; }
    }

}
