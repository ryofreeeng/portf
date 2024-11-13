using System;

namespace Therapim.Models
{
    /// <summary>
    /// レビュー用レスポンス情報
    /// </summary>
    public class ReviewResponseModel
    {
        public string SystemError { get; set; } // システムエラー
        public string SqlError { get; set; } // SQLエラー
        public string SqlResult { get; set; } // SQL結果
        public List<ReviewContent> SqlContent { get; set; } // レビュー内容リスト
    }

    public class ReviewContent
    {
        public int ReviewId { get; set; } // レビュー番号
        public string Author { get; set; } // 投稿者
        public string Gender { get; set; } // 性別
        public int Age { get; set; } // 年齢
        public DateTime? PostedDateTime { get; set; } // 投稿日時
        public string Title { get; set; } // レビューテーマ
        public string Content { get; set; } // レビュー内容
        public int Rating { get; set; } // 評価
        public int? CourseId { get; set; } // コース番号
        public string UserId { get; set; } // UserId
        public bool IsVisible { get; set; } // 表示フラグ
        public DateTime? CreatedDateTime { get; set; } // 登録日時
        public string? CreatedSessionId { get; set; } // 登録セッションID
        public DateTime? UpdatedDateTime { get; set; } // 更新日時
        public string? UpdatedSessionId { get; set; } // 更新セッションID
    }
}
