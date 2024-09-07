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
        [Required(ErrorMessage = "投稿者名は必須項目です。")]
        [StringLength(30, ErrorMessage = "投稿者名は30文字以内で入力してください。")]
        public string Author { get; set; }

        // 性別 (必須、文字列長最大10)
        [Required(ErrorMessage = "性別は必須項目です。")]
        [StringLength(10, ErrorMessage = "性別は10文字以内で入力してください。")]
        public string Gender { get; set; }

        // 年齢 (必須、範囲1〜80)
        //[Required(ErrorMessage = "年齢は必須項目です。")]
        [Range(1, 80, ErrorMessage = "年齢を選択してください。")]
        public int Age { get; set; }

        // 投稿日時 (任意)
        public DateTime? PostedDateTime { get; set; }

        // レビュータイトル (必須、文字列長最大200)
        [Required(ErrorMessage = "タイトルは必須項目です。")]
        [StringLength(50, ErrorMessage = "タイトルは50文字以内で入力してください。")]
        public string Title { get; set; }

        // レビュー内容 (必須、文字列長最大1000)
        [Required(ErrorMessage = "内容は必須項目です。")]
        [StringLength(1000, ErrorMessage = "内容は1000文字以内で入力してください。")]
        public string Content { get; set; }

        // 評価 (必須、範囲1〜5)
        //[Required(ErrorMessage = "評価は必須項目です。")]
        [Range(1, 5, ErrorMessage = "評価を選択してください。")]
        public int Rating { get; set; }

        // コース番号 (任意)
        public int? CourseId { get; set; }

        // userId　画面にない項目のため、Required属性はつけずにアクションメソッド内で手動バリデーション実行する        
        [StringLength(100, ErrorMessage = "入力項目ではありませんが、ユーザIDの桁数が大きすぎます。もう一度ログインからやり直してください")]
        public string? UserId { get; set; }

        // 表示フラグ (任意)
        public bool? IsVisible { get; set; }


        //登録セッションID
        public string? CreatedSessionId {  get; set; }

        //更新セッションID
        public string? UpdatedSessionId { get; set; }
    }

}
