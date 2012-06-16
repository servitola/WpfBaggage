using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using WpfBaggage.ViewModels.AuxiliaryTypes;

namespace WpfBaggage.ViewModels.Properties
{
	public class PropertyViewModelBase : ValidationPropertyViewModelBase
	{
		private bool _visibility = true;

	    public PropertyViewModelBase(FormWithPropertiesViewModelBase containingViewModel, PropertyInfo propertyInfo = null)
            : base(propertyInfo)
		{
            ContainingViewModel = containingViewModel;
			PropertyInfo = propertyInfo;
	    	ContainingObject = ((dynamic) containingViewModel).ContainingObject;

            if (propertyInfo != null)
            {
                HiddenPropertyValue = propertyInfo.GetValue(ContainingObject, null);
                IsWithSetter = propertyInfo.GetSetMethod() != null;
            }
		}

        /// <summary>
        /// Object, contains property
        /// </summary>
		public object ContainingObject { get; set; }

        /// <summary>
        /// ViewModel Conteiner
        /// </summary>
        public FormWithPropertiesViewModelBase ContainingViewModel { get; private set; }

		/// <summary>
		/// Название свойства в модели
		/// </summary>
		public string PropertyName { get { return PropertyInfo.Name; } }
		
        /// <summary>
        /// Description
        /// </summary>
		public string DisplayName
		{
            get { return PropertyInfo.ContainsAttribute<DescriptionAttribute>() 
                        ? PropertyInfo.GetAttribute<DescriptionAttribute>().Description 
                        : string.Empty; }
		}

        public virtual void ThrowOnPropertyValueChanged()
        {
            OnPropertyChanged("PropertyValue");
        }

        /// <summary>
        /// Property value from object
        /// </summary>
        public virtual object PropertyValue
        {
            get
            {
                var realValue = PropertyInfo.GetValue(ContainingObject, null);
                if (realValue == null)
                    return null;

                if (!IsWithSetter)
                    return realValue;

                if (HiddenPropertyValue == null)
                    HiddenPropertyValue = realValue;

                return HiddenPropertyValue.ToString() != realValue.ToString() 
                    ? HiddenPropertyValue 
                    : realValue;
            }
            set
            {
                CheckIsWithSetter();

                HiddenPropertyValue = value;
                
                if(PropertyDataType == typeof(string))
                {
                    PropertyInfo.SetValue(ContainingObject, value, null);
                }
                else if (PropertyDataType == typeof(double))
                {
                    double realValue;
                    if(double.TryParse((string) value, out realValue))
                        PropertyInfo.SetValue(ContainingObject, realValue, null);
                }
                else if (PropertyDataType == typeof(int))
                {
                    int realValue;
                    if(int.TryParse((string)value,out realValue))
                        PropertyInfo.SetValue(ContainingObject, realValue, null);
                }
                else if (value != null && value.GetType().GetProperty("Value") != null)
                {
                    value = value.GetType().GetProperty("Value").GetValue(value, null);
                    PropertyInfo.SetValue(ContainingObject, value, null);
                }
                else
                    PropertyInfo.SetValue(ContainingObject, value, null);
                
                OnPropertyChanged("PropertyValue");
            }
        }

        [Conditional("DEBUG")]
        private void CheckIsWithSetter()
        {
            if(!IsWithSetter)
                throw new Exception("This property have no setter");
        }

        /// <summary>
        /// Is property Visible
        /// </summary>
		public bool Visibility
		{
			get { return _visibility; }
			set
			{
				_visibility = value;
				OnPropertyChanged("Visibility");
			}
		}

        /// <summary>
        /// Is Property valid
        /// </summary>
		public bool IsValid
    	{
			get { return Visibility == false || string.IsNullOrEmpty(Error); }
    	}

        /// <summary>
        /// Template for View
        /// </summary>
        public DataTemplate EditorTemplate { get { return ((dynamic)ContainingViewModel).EditorTemplateMapper(this); } }

        public object HiddenPropertyValue { get; set; }

        /// <summary>
        /// Is property have setter
        /// </summary>
        public bool IsWithSetter { get; set; }

        /// <summary>
        /// Position for OrderBy
        /// </summary>
        public virtual int Position { get { return PropertyInfo.GetAttribute<GetToProperties>().Position; } }
	}
}
