namespace WpfBaggage.ViewModels.Forms
{
	public abstract class FormViewModelBase : ValidationViewModelBase
	{
		private bool _isValid;

	    /// <summary>
		/// Gets a value indicating whether the form is valid in its current state. If all properties
		/// wich validation are valid, this property returns true.
		/// </summary>
		public virtual bool IsValid
		{
			get { return _isValid; }
			protected set
			{
				_isValid = value;
				OnPropertyChanged("IsValid");
			}
		}

		protected override void PropertyChangedCompleted(string propertyName)
		{
			// test prevent infinite loop while settings IsValid 
			// (which causes an PropertyChanged to be raised)
			if (propertyName != "IsValid")
			{
				// update the isValid status
				IsValid = string.IsNullOrEmpty(Error) 
					&& ValidPropertiesCount == TotalPropertiesWithValidationCount;
			}
		}
	}
}
