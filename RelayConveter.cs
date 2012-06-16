using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfBaggage
{
    public class RelayConverter<TSource, TDestination> : IValueConverter
    {
        private readonly IValueConverter _originConverter;
        private readonly Func<TSource, TDestination> _converterFunc;
        private readonly Func<TDestination, TSource> _converterBackFunc;
        private readonly Action _afterConvertingFunc;
        private readonly Action _afterConvertingBackFunc;
        /*private readonly object _converterParameter;*/

        public RelayConverter(IValueConverter originConverter = null,
            Func<TSource, TDestination> converterFunc = null, /*object converterParameter = null,*/ Action afterConvertingFunc = null,
            Func<TDestination, TSource> converterBackFunc = null, Action afterConvertingBackFunc = null)
        {
            _originConverter = originConverter;
            _converterFunc = converterFunc;
            _afterConvertingFunc = afterConvertingFunc;
            _converterBackFunc = converterBackFunc;
            _afterConvertingBackFunc = afterConvertingBackFunc;
           /* _converterParameter = converterParameter;*/
        }

        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (_originConverter != null)
                value = _originConverter.Convert(value, targetType, parameter, culture);

            var result = !typeof(TSource).IsClass && value == null
                            ? null
                            : (_converterFunc != null ? _converterFunc((TSource)value) : value);

            if (_afterConvertingFunc != null)
                _afterConvertingFunc();
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (_originConverter != null)
                value = _originConverter.ConvertBack(value, targetType, parameter, culture);

            var result = _converterBackFunc != null ? _converterBackFunc((TDestination)value) : value;
            if (_afterConvertingBackFunc != null)
                _afterConvertingBackFunc();
            return result;
        }

        #endregion
    }
}
