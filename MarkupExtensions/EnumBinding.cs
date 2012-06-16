using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfBaggage.MarkupExtensions
{
    public class EnumBinding : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
        	//_innerBinding.Source = 
        	return null;
        }

    	ObjectDataProvider _objectDataProvider = new ObjectDataProvider();

        private readonly Binding _innerBinding = new Binding();

        #region Binding Wrapping

        // Summary:
        //     Gets or sets a value that indicates whether the binding ignores any System.ComponentModel.ICollectionView
        //     settings on the data source.
        //
        // Returns:
        //     true if the binding binds directly to the data source; otherwise, false.
        public bool BindsDirectlyToSource
        {
            get { return _innerBinding.BindsDirectlyToSource; }
            set { _innerBinding.BindsDirectlyToSource = value; }
        }
        //
        // Summary:
        //     Gets or sets the converter object that is called by the binding engine to
        //     modify the data as it is passed between the source and target, or vice versa.
        //
        // Returns:
        //     The System.Windows.Data.IValueConverter object that modifies the data.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Windows.Data.Binding has already been attached to a target element,
        //     and cannot be modified.
        public IValueConverter Converter
        {
            get { return _innerBinding.Converter; }
            set { _innerBinding.Converter = value; }
        }
        //
        // Summary:
        //     Gets or sets the culture to be used by the System.Windows.Data.Binding.Converter.
        //
        // Returns:
        //     The System.Globalization.CultureInfo used by the System.Windows.Data.Binding.Converter.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Windows.Data.Binding has already been attached to a target element,
        //     and cannot be modified.
        public CultureInfo ConverterCulture
        {
            get { return _innerBinding.ConverterCulture; }
            set { _innerBinding.ConverterCulture = value; }
        }
        //
        // Summary:
        //     Gets or sets a parameter that can be used in the System.Windows.Data.Binding.Converter
        //     logic.
        //
        // Returns:
        //     A parameter to be passed to the System.Windows.Data.Binding.Converter. This
        //     can be used in the conversion logic. The default is null.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Windows.Data.Binding has already been attached to a target element,
        //     and cannot be modified.
        public object ConverterParameter
        {
            get { return _innerBinding.ConverterParameter; }
            set { _innerBinding.ConverterParameter = value; }
        }
        //
        // Summary:
        //     Gets or sets the name of the element to use as the binding source object.
        //
        // Returns:
        //     The value of the System.Windows.FrameworkElement.Name property or x:Name
        //     Attribute of the element to bind to. The default is null.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Windows.Data.Binding has already been attached to a target element,
        //     and cannot be modified.-or-The System.Windows.Data.Binding.Source or System.Windows.Data.Binding.RelativeSource
        //     property has already been set.
        public string ElementName
        {
            get { return _innerBinding.ElementName; }
            set { _innerBinding.ElementName = value; }
        }
        //
        // Summary:
        //     Gets or sets a value that indicates the direction of the data flow in the
        //     binding.
        //
        // Returns:
        //     One of the System.Windows.Data.BindingMode values. The default is System.Windows.Data.BindingMode.OneWay.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Windows.Data.Binding has already been attached to a target element,
        //     and cannot be modified.
        public BindingMode Mode
        {
            get { return _innerBinding.Mode; }
            set { _innerBinding.Mode = value; }
        }
        //
        // Summary:
        //     Gets or sets a value that indicates whether the System.Windows.FrameworkElement.BindingValidationError
        //     event is raised on validation errors.
        //
        // Returns:
        //     true if the System.Windows.FrameworkElement.BindingValidationError event
        //     is raised; otherwise, false. The default is false.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Windows.Data.Binding has already been attached to a target element,
        //     and cannot be modified.
        public bool NotifyOnValidationError { get; set; }
        //
        // Summary:
        //     Gets or sets the path to the binding source property.
        //
        // Returns:
        //     The property path for the source of the binding. See System.Windows.PropertyPath
        //     or Property Path Syntax.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Windows.Data.Binding has already been attached to a target element,
        //     and cannot be modified.
        [TypeConverter(typeof(PropertyPathConverter))]
        public PropertyPath Path
        {
            get { return _innerBinding.Path; }
            set { _innerBinding.Path = value; }
        }
        //
        // Summary:
        //     Gets or sets the binding source by specifying its location relative to the
        //     position of the binding target.
        //
        // Returns:
        //     The relative location of the binding source to use. The default is null.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Windows.Data.Binding has already been attached to a target element,
        //     and cannot be modified.-or-The System.Windows.Data.Binding.ElementName or
        //     System.Windows.Data.Binding.Source property has already been set.
        public RelativeSource RelativeSource
        {
            get { return _innerBinding.RelativeSource; }
            set { _innerBinding.RelativeSource = value; }
        }
        //
        // Summary:
        //     Gets or sets the data source for the binding.
        //
        // Returns:
        //     The source object that contains the data for the binding.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Windows.Data.Binding has already been attached to a target element,
        //     and cannot be modified.-or-The System.Windows.Data.Binding.ElementName or
        //     System.Windows.Data.Binding.RelativeSource property has already been set.
        public object Source
        {
            get { return _innerBinding.Source; }
            set { _innerBinding.Source = value; }
        }
        //
        // Summary:
        //     Gets or sets a value that determines the timing of binding source updates
        //     for two-way bindings.
        //
        // Returns:
        //     A value that determines when the binding source is updated. The default is
        //     System.Windows.Data.UpdateSourceTrigger.Default.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Windows.Data.Binding has already been attached to a target element,
        //     and cannot be modified.
        public UpdateSourceTrigger UpdateSourceTrigger
        {
            get { return _innerBinding.UpdateSourceTrigger; }
            set { _innerBinding.UpdateSourceTrigger = value; }
        }
        //
        // Summary:
        //     Gets or sets a value that indicates whether the binding engine will report
        //     validation errors from an System.ComponentModel.IDataErrorInfo implementation
        //     on the bound data entity.
        //
        // Returns:
        //     true if the binding engine will report System.ComponentModel.IDataErrorInfo
        //     validation errors; otherwise, false. The default is false.
        public bool ValidatesOnDataErrors
        {
            get { return _innerBinding.ValidatesOnDataErrors; }
            set { _innerBinding.ValidatesOnDataErrors = value; }
        }
        //
        // Summary:
        //     Gets or sets a value that indicates whether the binding engine will report
        //     exception validation errors.
        //
        // Returns:
        //     true if the binding engine will report exception validation errors; otherwise,
        //     false. The default is false.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Windows.Data.Binding has already been attached to a target element,
        //     and cannot be modified.
        public bool ValidatesOnExceptions
        {
            get { return _innerBinding.ValidatesOnExceptions; }
            set { _innerBinding.ValidatesOnExceptions = value; }
        }
        //
        // Summary:
        //     Gets or sets a value that indicates whether the binding engine will report
        //     validation errors from an System.ComponentModel.INotifyDataErrorInfo implementation
        //     on the bound data entity.
        //
        // Returns:
        //     true if the binding engine will report System.ComponentModel.INotifyDataErrorInfo
        //     validation errors; otherwise, false. The default is true.
        /*public bool ValidatesOnNotifyDataErrors
        {
            get { return _innerBinding.ValidatesOnNotifyDataErrors; }
            set { _innerBinding.ValidatesOnNotifyDataErrors = value; }
        }*/

        #endregion Binding Wrapping
    }

    
}
