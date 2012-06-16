using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using WpfBaggage.ViewModels.Forms;
using WpfBaggage.ViewModels.Properties;

namespace WpfBaggage.ViewModels.AuxiliaryTypes
{
    public abstract class ValidationPropertyViewModelBase : ViewModelBaseThreadSafe, IDataErrorInfo
    {

        #region Private Fields

        private static readonly Func<ValidationPropertyViewModelBase, object> HiddenPropertyGetter;
        private const string Value = "PropertyValue";
        private const string HiddenValue = "HiddenPropertyValue";
		private readonly List<ValidationAttribute> _validators;
        
        private struct TypeValidationData
        {
            public readonly string ErrorMessage;
            public readonly string Pattern;
            public TypeValidationData(string pattern,string errorMessage)
            {
                Pattern = pattern;
                ErrorMessage = errorMessage;
            }
        }

        private static readonly Dictionary<Type, TypeValidationData> TypeValidationDictionary;
        
        #endregion PrivateFields

        #region Constructors

        static ValidationPropertyViewModelBase()
        {
            var hiddenProperty = typeof(PropertyViewModelBase).GetProperty(HiddenValue);

            HiddenPropertyGetter = viewModel => hiddenProperty.GetValue(viewModel, null);

            TypeValidationDictionary = new Dictionary<Type, TypeValidationData>
                                  {
                                        {typeof (double), new TypeValidationData("^(-?[0-9]*[,.]?[0-9]{0,})$","Значение должно быть числом с плавающей точкой")},
                                        {typeof (int), new TypeValidationData("^(-?[0-9]*)$","Значение должно быть числом")},
                                  };
        }

        protected ValidationPropertyViewModelBase(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            _validators = GetValidations(propertyInfo);
        }

        public PropertyInfo PropertyInfo;

        /// <summary>
        /// Тип свойства
        /// </summary>
        public virtual Type PropertyType { get { return PropertyInfo.PropertyType; } }

        /// <summary>
        /// Тип данных свойства (независимо от того, Nullable оно или нет)
        /// </summary>
        public virtual Type PropertyDataType
        {
            get { return !PropertyType.IsGenericType ? PropertyType : PropertyType.GetGenericArguments().First(); }
        }

        #endregion Constructors

        #region Validation

        /// <summary>
		/// Gets the error message for the property with the given name.
		/// </summary>
		/// <param name="propertyName">Name of the property</param>
        public string this[string propertyName]
		{
			get
			{
                //check if it's "PropertyValue" property
                if (propertyName != Value) return string.Empty;

                var propertyValue = HiddenPropertyGetter(this);

                //проаеряем на валидность типа double или int
                if (TypeValidationDictionary.ContainsKey(PropertyDataType))
                {
                    var data = TypeValidationDictionary[PropertyDataType];

                    if(!Regex.IsMatch(propertyValue.ToString(), data.Pattern))
                        return data.ErrorMessage;
                }

                var errorMessages = _validators.Where(v => !v.IsValid(propertyValue))
                                               .Select(v => v.ErrorMessage)
                                               .ToArray();

                return string.Join(Environment.NewLine, errorMessages);    
			}
		}

        /// <summary>
		/// Gets an error message indicating what is wrong with this object.
		/// </summary>
        public string Error
        {
            get
            {
                var errors = from attribute in _validators
                             where !attribute.IsValid(HiddenPropertyGetter(this))
                             select attribute.ErrorMessage;

                return string.Join(Environment.NewLine, errors.ToArray());
            }
        }

    	private static List<ValidationAttribute> GetValidations(PropertyInfo property)
    	{
    	    return property == null 
                ? new List<ValidationAttribute>()
                : property.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>().ToList();
    	}

        #endregion Validation
    }
}
