using System;
using System.Linq;
using System.Reflection;

namespace WpfBaggage
{
	public static class Tools
	{
		public static T GetAttribute<T>(this MemberInfo info) where T : Attribute
		{
            return info.GetCustomAttributes(typeof(T), false).Cast<T>().SingleOrDefault();
		}

        public static T GetAttribute<T>(this Enum info) where T : Attribute
        {
            return info.GetType().GetMember(info.ToString())[0].GetCustomAttributes(typeof (T), false).Cast<T>().SingleOrDefault();
        }

        public static bool ContainsAttribute<TExpected>(this MemberInfo memberInfo) where TExpected : Attribute
        {
            var attribute = memberInfo.GetCustomAttributes(typeof(TExpected),false).Cast<TExpected>().FirstOrDefault();
            return attribute != null;
        }

	    public static PropertyInfo ReflectOnPath(this object o, string path)
		{
			var value = o;
			var pathComponents = path.Split('.');
			for (var i = 0; i < pathComponents.Count() - 1; i++ )
				value = value.GetType().GetProperty(pathComponents[i]).GetValue(value, null);

			var propertyInfo = value.GetType().GetProperty(pathComponents[pathComponents.Count() - 1]);
			
			return propertyInfo;
		}
	}
}