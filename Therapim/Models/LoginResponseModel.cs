using System;
using System.ComponentModel.DataAnnotations;


namespace Therapim.Models
{
    /// <summary>
    /// ログイン用レスポンス情報
    /// </summary>
    /// 
    public class LoginResponseModel
    {
        public string SystemError { get; set; } // システムエラー
        public string SqlError { get; set; } // SQLエラー
        public string SqlResult { get; set; } // SQL結果
        public List<CustomerContent> SqlContent { get; set; } // 顧客リスト
    }

    public class CustomerContent
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public int? Rank { get; set; }
        public string? Permission { get; set; }        
        public int VisitedTimes { get; set; }

    }

}
