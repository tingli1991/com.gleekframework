using System;
using System.ComponentModel.DataAnnotations;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>
    /// 不等于模型验证
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class NotEqualAttribute : ValidationAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly object _value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public NotEqualAttribute(int value)
        {
            _value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (_value.Equals(value))
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}