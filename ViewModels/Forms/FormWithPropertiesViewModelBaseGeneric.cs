using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using WpfBaggage.Behavior;
using WpfBaggage.ViewModels.AuxiliaryTypes;
using WpfBaggage.ViewModels.Properties;

namespace WpfBaggage.ViewModels.Forms
{
	public class FormWithPropertiesViewModelBase<TModel> : FormWithPropertiesViewModelBase
    {

        #region PrivateFields

        private List<PropertyViewModelBase> _properties;
        private readonly Func<FormWithPropertiesViewModelBase<TModel>, PropertyInfo, PropertyViewModelBase> _propertiesFactoryMethod;

        #endregion PrivateFields

        #region Constructors

        public FormWithPropertiesViewModelBase(TModel containingObject, 
                                               Func<PropertyViewModelBase, DataTemplate> editorTemplatesMapper,
                        Func<FormWithPropertiesViewModelBase<TModel>, PropertyInfo, PropertyViewModelBase> propertiesFactoryMethod = null,
							bool isToInlinePropertiesFactoryToBaseFactory = true)
        {
            ContainingObject = containingObject;
            if (editorTemplatesMapper != null)
                EditorTemplateMapper = editorTemplatesMapper;

            if(propertiesFactoryMethod == null || isToInlinePropertiesFactoryToBaseFactory)
                #region Own Property Fabric
                _propertiesFactoryMethod = (obj, propInfo) =>
                {
					if (propertiesFactoryMethod != null && isToInlinePropertiesFactoryToBaseFactory)
                	{
                		var viewModel = propertiesFactoryMethod(obj, propInfo);
						if (viewModel != null)
							return viewModel;
                	}

                	ConstructorInfo constructor;

                    if (!propInfo.PropertyType.IsEnum)
                    {
                        constructor =
                            typeof (PropertyViewModelBase).GetConstructor(new[]
                                                        {
                                                            typeof (FormWithPropertiesViewModelBase<TModel>),
                                                            typeof (PropertyInfo)
                                                        });

                    }
                    else
                    {
                        constructor =
                            typeof(EnumPropertyViewModelBase).GetConstructor(new[]
                                                        {
                                                            typeof (FormWithPropertiesViewModelBase<TModel>),
                                                            typeof (PropertyInfo)
                                                        });
                    }

                    Debug.Assert(constructor != null, "constructor != null");
                    return (PropertyViewModelBase)constructor.Invoke(new object[] { obj, propInfo });
                
                };
            #endregion Own Property Fabric
            else
                _propertiesFactoryMethod = propertiesFactoryMethod;
        }

        #endregion Constructors

		#region Properties

		public virtual List<PropertyViewModelBase> Properties
		{
			get { return _properties ?? (_properties = CreateProperties()); }
		}

		private List<PropertyViewModelBase> CreateProperties()
		{
			var properties =  typeof(TModel).GetProperties()
								   .Where(item => item.ContainsAttribute<GetToProperties>())
								   .Select(property => _propertiesFactoryMethod(this,property))
								   .ToList();

			SetDependencies(properties);

		    SetIsValidDependece(properties);

		    GenerateSemanticComplexProperties(properties);

            CreatingPropertiesCompleted(properties);

		    properties = properties.OrderBy(item => item.Position).ToList();

			return properties;
		}

	    private void GenerateSemanticComplexProperties(List<PropertyViewModelBase> properties)
	    {
	        var complexGroups = new List<string[]>();
	        var propertiesWithComplexAttribute = properties.Where(item => item.PropertyInfo.ContainsAttribute<ComplexProperty>()).ToList();

            //берём комплексные группы
	        foreach (var complexPath in propertiesWithComplexAttribute
                                                  .Select(property => property.PropertyInfo.GetAttribute<ComplexProperty>().Path)
                                                  .Where(complexPath => !complexGroups.Contains(complexPath, StringArrayComparer.Instance)))
	            complexGroups.Add(complexPath);

	        foreach (var complexGroup in complexGroups)
	        {
	            var thisComplextGroupProperties = propertiesWithComplexAttribute.Where(item => StringArrayComparer.Instance.Equals(item.PropertyInfo.GetAttribute<ComplexProperty>().Path, complexGroup)).ToList();
                var semanticProperty = new SemanticComplexPropertyViewModel(this, thisComplextGroupProperties.Min(item => item.Position));

	            semanticProperty.Properties = thisComplextGroupProperties;

	            foreach (var property in thisComplextGroupProperties)
	                properties.Remove(property);

	            properties.Add(semanticProperty);
	        }
	    }

	    private void SetIsValidDependece(List<PropertyViewModelBase> properties)
	    {
	        foreach (var propertyViewModelBase in properties)
	        {
	            propertyViewModelBase.PropertyChanged += (o, e) =>
	                                                         {
                                                                 if (e.PropertyName != "PropertyValue") return;
                                                                 OnPropertyChanged("IsValid");
	                                                         };
	        }
	    }

	    private static void SetDependencies(List<PropertyViewModelBase> properties)
		{
			foreach (var propertyViewModel in properties.Where(item => item.PropertyInfo.ContainsAttribute<PropertyDependencyAttribute>()))
			{
				var propertyToDepence =	properties.First(prop => prop.PropertyName == propertyViewModel.PropertyInfo.GetAttribute<PropertyDependencyAttribute>().PropertyToDependName);
				var valuesToDetect = propertyViewModel.PropertyInfo.GetAttribute<PropertyDependencyAttribute>().PropertyToDependValues;
				var propertyToChange = propertyViewModel.ReflectOnPath(propertyViewModel.PropertyInfo.GetAttribute<PropertyDependencyAttribute>().PropertyToChangeName);

			    var model = propertyViewModel;
			    propertyToDepence.PropertyChanged += (o, e) =>
				        {
							if (e.PropertyName != "PropertyValue") return;

                            if (propertyToChange.PropertyType == typeof(bool))
                                propertyToChange.SetValue(model, valuesToDetect.Contains(((PropertyViewModelBase) o).PropertyValue.ToString()), null);
				        };

                if (propertyToChange.PropertyType == typeof(bool))
                    propertyToChange.SetValue(model, valuesToDetect.Contains(((PropertyViewModelBase)propertyToDepence).PropertyValue.ToString()), null);
			}
		}

		protected virtual void CreatingPropertiesCompleted(List<PropertyViewModelBase> properties)
		{
		}

		#endregion Properties

	    public virtual PropertyViewModelBase GetProperty(string name)
	    {
	        return Properties.First(item => item.PropertyName == name);
	    }

		public TModel ContainingObject { get; set; }

		public Func<PropertyViewModelBase, DataTemplate> EditorTemplateMapper;

		public override bool IsValid
        {
            get { return Properties.All(propertyViewModelBase => propertyViewModelBase.IsValid); }
        }
	}
}
