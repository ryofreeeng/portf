using System;

namespace Therapim.Models
{
    /// <summary>
    /// レビュー用レスポンス情報
    /// </summary>
    public class ReservationResponseModel
    {
        public string SystemError { get; set; } // システムエラー
        public string SqlError { get; set; } // SQLエラー
        public string SqlResult { get; set; } // SQL結果
        public List<ReviewContent> SqlContent { get; set; } // レビュー内容リスト
    }

    public class ReservationContent
    {
        // 予約ID
        public int ReservationId { get; set; }

        // ユーザID
        public string? UserId { get; set; }

        // お名前
        public string FullName { get; set; }

        // 電話番号
        public string PhoneNumber { get; set; }

        // 生年月日
        public string Birthday { get; set; }

        // メールアドレス
        public string MailAddress { get; set; }

        // 性別
        public string Gender { get; set; }

        // SNS名
        public string? SnsName { get; set; }

        // メニューID
        public int MenuId { get; set; }

        // コース表示名
        public string? CourseNameDisp { get; set; }

        // コースオプションID
        public int? CourseOptionId { get; set; }

        // コースオプション
        public string? CourseOption { get; set; }

        // ペーパーブラ選択
        public string PaperBraSelect { get; set; }

        // ペーパーパンツ選択
        public string PaperPantsSelect { get; set; }

        // アロマオイル選択
        public string AromaOilSelect { get; set; }

        // 出張希望
        public bool IsVisit { get; set; }

        // 希望出張先概要
        public string? DesiredVisitPlace { get; set; }

        // 出張先住所
        public string? Address { get; set; }

        // 希望サロン
        public string? DesiredSalon { get; set; }

        // 第1希望日時 開始・終了
        public DateTime DesiredDateTimeStart1 { get; set; }
        public DateTime DesiredDateTimeEnd1 { get; set; }

        // 第2希望日時 開始・終了 (任意)
        public DateTime? DesiredDateTimeStart2 { get; set; }
        public DateTime? DesiredDateTimeEnd2 { get; set; }

        // 第3希望日時 開始・終了 (任意)
        public DateTime? DesiredDateTimeStart3 { get; set; }
        public DateTime? DesiredDateTimeEnd3 { get; set; }

        // 会話希望
        public string? Conversation { get; set; }

        // 施術NGな部位
        public string? NotTouchParts { get; set; }

        // その他要望
        public string? OtherRequest { get; set; }

        // 補足事項
        public string? OtherThings { get; set; }

        // 登録日時
        public DateTime CreatedAt { get; set; }

        // 登録セッションID
        public string? CreatedSessionId { get; set; }
    }
}
