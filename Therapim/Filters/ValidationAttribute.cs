using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Therapim.Filters
{    
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 他のある項目がbool型でfalseの場合にチェックを行うバリデーション属性
    /// </summary>
    public class RequiredIfFalseAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public RequiredIfFalseAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var comparisonValue = validationContext.ObjectType.GetProperty(_comparisonProperty)
                ?.GetValue(validationContext.ObjectInstance);

            // 前提条件となる項目がfalseの場合に、対象項目がnullや空文字であるならエラーにする
            if (comparisonValue is bool isFalse && !isFalse && string.IsNullOrEmpty(value as string))
            {
                return new ValidationResult($"{validationContext.DisplayName} は必須です。");
            }

            return ValidationResult.Success;
        }
    }


    /// <summary>
    /// 他のある項目がbool型でtrueの場合にチェックを行うバリデーション属性
    /// </summary>
    public class RequiredIfTrueAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public RequiredIfTrueAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var comparisonValue = validationContext.ObjectType.GetProperty(_comparisonProperty)
                ?.GetValue(validationContext.ObjectInstance);

            // 前提条件となる項目がtrueの場合に、対象項目がnullや空文字であるならエラーにする
            if (comparisonValue is bool isTrure && isTrure && string.IsNullOrEmpty(value as string))
            {
                return new ValidationResult($"{validationContext.DisplayName} は必須です。");
            }

            return ValidationResult.Success;
        }
    }

}
