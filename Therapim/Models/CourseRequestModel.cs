using System;
using System.ComponentModel.DataAnnotations;


namespace Therapim.Models
{
    /// <summary>
    /// レビュー用リクエスト情報
    /// </summary>
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CourseRequestModel
    {
        // メニュー番号 (任意) 詳細取得を行う場合は使う
        public int? CourseId { get; set; }

        // userId なくてもよい。あればメニュー内容をカスタマイズする
        public string? UserId { get; set; }
     
    }

}
