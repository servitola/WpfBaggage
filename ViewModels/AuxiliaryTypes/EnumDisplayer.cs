using System;
using System.ComponentModel;
using System.Linq;

namespace WpfBaggage.ViewModels.AuxiliaryTypes
{
    public class EnumDisplayer
    {
        public object Value { get; set; }
        public string DisplayName { get; set; }

        public EnumDisplayer(object value)
        {
            Value = value;
            var values = Enum.GetValues(value.GetType());

            foreach (var getValue in values.Cast<Enum>().Where(getValue => getValue.ToString() == value.ToString()))
            {
                DisplayName = getValue.GetAttribute<DescriptionAttribute>().Description;
                break;
            }
        }
    }
}
