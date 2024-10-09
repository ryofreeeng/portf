using System.Globalization;
using System.Text;

namespace Therapim.Helpers
{
    public static class CommonHelper
    {
        //数字を金額表示に変換
        public static string FormatAsCurrency(int number)
        {            
            if (number != 0)
            {
                // 金額形式に変換して表示（￥1,000- の形式）
                string formattedCurrency = string.Format(CultureInfo.CurrentCulture, "￥{0:N0}-", number);
                return formattedCurrency;
            }
            else
            {
                return "";
            }
        }

        //ラベル文字列ををカンマ区切りで分割してタグで囲う
        public static string devideLabel(string labelString)
        {
            StringBuilder resultHtml = new StringBuilder();
            if (labelString != null)
            {
                string[] labelArray = labelString.Split(",");
                List<string> labelList = new List<string>(labelArray); //現状追加や削除はしていないが可変にしておく
                
                for (int i = 0; i < labelList.Count; i++)
                {
                    string labelClassString = "";
                    switch (labelList[i])
                    {
                        case "学割":
                            labelClassString = "student";
                            break;
                        case "練習":
                            labelClassString = "practice";
                            break;
                        case "期間限定":
                            labelClassString = "limitedTime";
                            break;
                        case "モニター":
                            labelClassString = "monitor";
                            break;
                        case "リピーター限定":
                            labelClassString = "limitedRepeater";
                            break;                        
                        case "お客様限定":
                            labelClassString = "limitedCustomer";
                            break;
                        case "秘密":
                            labelClassString = "secret";
                            break;                        
                        case "オリジナル":
                            labelClassString = "original";
                            break;
                    }

                    string htmlTag = @"<span class=""course-label-a " +labelClassString +@""">" + labelList[i] + "</span>";
                    resultHtml.Append(htmlTag);
                }                

                return resultHtml.ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
