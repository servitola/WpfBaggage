using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WpfBaggage.ViewModels.AuxiliaryTypes;

namespace WpfBaggage.ViewModels.Properties
{
    class EnumPropertyViewModelBase : PropertyViewModelBase
    {
        public EnumPropertyViewModelBase(FormWithPropertiesViewModelBase containingViewModel, PropertyInfo propertyInfo)
            : base(containingViewModel, propertyInfo) { }

        private List<EnumDisplayer> _availableValues;

        public List<EnumDisplayer> AvailableValues 
        { 
            get
            {
                if(_availableValues == null)
                {
                    _availableValues = new List<EnumDisplayer>();
                    foreach (var value in Enum.GetValues(PropertyInfo.PropertyType))
                        _availableValues.Add(new EnumDisplayer(value));
                }
                return _availableValues;
            }
        }

        public new EnumDisplayer PropertyValue
        {
            get { return AvailableValues.First(item => item.Value.ToString() == PropertyInfo.GetValue(ContainingObject, null).ToString()); }
            set { base.PropertyValue = value.Value; }
        }
    }
}
