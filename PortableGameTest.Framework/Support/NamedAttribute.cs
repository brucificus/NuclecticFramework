using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PortableGameTest.Framework.Support
{
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
	public class NamedAttribute
		: Attribute
	{
		public string Name { get; private set; }

		public NamedAttribute(string name)
		{
			Name = name;
		}

		public static string ValueOf(ParameterInfo parameterInfo)
		{
			var namedAttribute = parameterInfo.GetCustomAttribute<NamedAttribute>();
			if (namedAttribute != null)
				return namedAttribute.Name;
			return null;
		}
	}
}
