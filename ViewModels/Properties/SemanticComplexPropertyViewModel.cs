using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WpfBaggage.ViewModels.AuxiliaryTypes;

namespace WpfBaggage.ViewModels.Properties
{
	public class SemanticComplexPropertyViewModel : PropertyViewModelBase
	{
        private int _position;
        
        public SemanticComplexPropertyViewModel(FormWithPropertiesViewModelBase containingViewModel,int position) : base(containingViewModel)
		{
            Properties = new List<PropertyViewModelBase>();
            _position = position;
		}

		public List<PropertyViewModelBase> Properties { get; set; }

	    /// <summary>
	    /// Position for OrderBy
	    /// </summary>
        public override int Position
	    {
	        get { return _position; }
	    }

	    public new bool IsValid { get { return Properties.All(property => property.IsValid); } }

        /// <summary>
        /// Description
        /// </summary>
        public new string DisplayName
        {
            get { return string.Empty; }
        }
	}
}
