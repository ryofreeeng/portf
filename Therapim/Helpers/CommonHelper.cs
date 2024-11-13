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
                        case "ラベル名1":
                            labelClassString = "label1";
                            break;
                        case "ラベル名2":
                            labelClassString = "label2";
                            break;
                        case "ラベル名3":
                            labelClassString = "label3";
                            break;
                        case "ラベル名4":
                            labelClassString = "label4";
                            break;
                        case "ラベル名5":
                            labelClassString = "label5";
                            break;                        
                        case "ラベル名6":
                            labelClassString = "label6";
                            break;
                        case "ラベル名7":
                            labelClassString = "label7";
                            break;                        
                        case "ラベル名8":
                            labelClassString = "label8";
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
