using System;
using System.Reflection.Emit;

namespace Therapim.Models
{
    /// <summary>
    /// レビュー用レスポンス情報
    /// </summary>
    public class CourseResponseModel
    {
        public string SystemError { get; set; } // システムエラー
        public string SqlError { get; set; } // SQLエラー
        public string SqlResult { get; set; } // SQL結果
        public List<CourseContent> SqlContent { get; set; } // コース内容リスト
    }

    public class CourseContent
    {
        public int CourseId { get; set; } // コース番号
        public string MenuId { get; set; } // メニューID
        public string CourseName { get; set; } // 簡易コース名
        public string CourseNameDisp { get; set; } // 表示コース名
        public int Price { get; set; } // 金額
        public string PriceAddInfo { get; set; } // 金額補足
        public int RequiredTimes { get; set; } // 必要回数
        public bool IsVisible { get; set; } // 表示有無
        public string Place { get; set; } // 場所
        public string Description { get; set; } // コース説明
        public string AddInfo1 { get; set; } // 補足説明1
        public string AddInfo2 { get; set; } // 補足説明2
        public string AddInfo3 { get; set; } // 補足説明3
        public string AddInfo4 { get; set; } // 補足説明4
        public string AddInfo5 { get; set; } // 補足説明5
        public string Target { get; set; } // 対象
        public string Level { get; set; } // レベル
        public string CourseImagePath1 { get; set; } // イメージ画像パス1
        public string CourseImagePath2 { get; set; } // イメージ画像パス2
        public string CourseImagePath3 { get; set; } // イメージ画像パス3
        public string CourseImagePath4 { get; set; } // イメージ画像パス4
        public string CourseImagePath5 { get; set; } // イメージ画像パス5
        public string CourseVideoPath1 { get; set; } // イメージ動画パス1
        public string CourseVideoPath2 { get; set; } // イメージ動画パス2
        public string CourseVideoPath3 { get; set; } // イメージ動画パス3
        public string Label { get; set; } // ラベル
        public string Rank { get; set; } // ランク
        public string Permission { get; set; } // 必要権限
        
    }
}
