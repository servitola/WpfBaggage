using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfBaggage
{
	public class PropertyDependencyAttribute : Attribute
	{
		public string PropertyToDependName { get; set; }
		public List<string> PropertyToDependValues { get; set; }
		public string PropertyToChangeName { get; set; }
		
		public PropertyDependencyAttribute(string propertyToDependName,
								   string propertyToDependValues,
								   string propertyToChangeName)
		{
			PropertyToDependName = propertyToDependName;
			PropertyToDependValues = propertyToDependValues.Split(',').ToList();
			PropertyToChangeName = propertyToChangeName;
		}
	}

	public class GetToProperties : Attribute
	{
		public int Position { get; set; }
        public bool IsEditable { get; set; }

		public GetToProperties(int position,bool isEditable = true)
		{
			Position = position;
		    IsEditable = isEditable;
		}
	}

    public class ComplexProperty : Attribute
    {
        public string[] Path;

        private static readonly string[] Separators = new[] { "~AND~"};

        public ComplexProperty(string path)
        {
            Path = path.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
